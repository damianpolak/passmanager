using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace PasswordManager
{
    class Management
    {
        private Settings appSettings;
        private Variables appVariables;
        private Logs appLogs;
        private bool bSettingsAvailable;
        public string sInputKey { get; set; }

        public Management()
        {
            appSettings = new Settings();
            appVariables = new Variables();
            appLogs = new Logs();
            
            if (File.Exists(appVariables.sSettingsFilePath))
            {
                appSettings = LoadSettingsFromFile(appVariables.sSettingsFilePath);
                bSettingsAvailable = true;
            }
            else
            {
                try
                {
                    Directory.CreateDirectory(appVariables.sSettingsFileDirectory);
                    bSettingsAvailable = false;
                }
                catch(Exception ex)
                {
                    bSettingsAvailable = false;
                    appLogs.Message(ex.Message.ToString());
                }
                
            }
        }

        public Settings GetSettings()
        {
            if(bSettingsAvailable == true)
            {
                return appSettings;
            } else
            {
                return null;
            }
            
        }

        private Settings LoadSettingsFromFile(string filePath)
        {
            try
            {
                using (Stream stream = File.Open(filePath, FileMode.Open))
                {
                    BinaryFormatter bin = new BinaryFormatter();
                    return (Settings)bin.Deserialize(stream);
                }
            }
            catch (IOException e)
            {
                appLogs.Message(e.Message);
                return null;
            }
        }

        public void SaveSettingsToFile(Settings lastSettings)
        {
            try
            {
                using (Stream stream = File.Open(appVariables.sSettingsFilePath, FileMode.Create))
                {
                    BinaryFormatter bin = new BinaryFormatter();
                    bin.Serialize(stream, lastSettings);

                }
            }
            catch (IOException e)
            {
                appLogs.Message(e.Message);
            }
        }

        // Rijandael
        public string EncryptRijndael(string text, string salt)
        {
            if (string.IsNullOrEmpty(text))
                throw new ArgumentNullException("text");

            var aesAlg = NewRijndaelManaged(salt);

            var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
            var msEncrypt = new MemoryStream();
            using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
            using (var swEncrypt = new StreamWriter(csEncrypt))
            {
                swEncrypt.Write(text);
            }

            return Convert.ToBase64String(msEncrypt.ToArray());
        }

        public static bool IsBase64String(string base64String)
        {
            base64String = base64String.Trim();
            return (base64String.Length % 4 == 0) &&
                   Regex.IsMatch(base64String, @"^[a-zA-Z0-9\+/]*={0,3}$", RegexOptions.None);

        }

        public string DecryptRijndael(string cipherText, string salt)
        {
            if (string.IsNullOrEmpty(cipherText))
                throw new ArgumentNullException("cipherText");

            if (!IsBase64String(cipherText))
                throw new Exception("The cipherText input parameter is not base64 encoded");

            string text;

            var aesAlg = NewRijndaelManaged(salt);
            var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
            var cipher = Convert.FromBase64String(cipherText);

            using (var msDecrypt = new MemoryStream(cipher))
            {
                using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (var srDecrypt = new StreamReader(csDecrypt))
                    {
                        text = srDecrypt.ReadToEnd();
                    }
                }
            }
            return text;
        }

        private RijndaelManaged NewRijndaelManaged(string salt)
        {
            if (salt == null) throw new ArgumentNullException("salt");
            var saltBytes = Encoding.ASCII.GetBytes(salt);
            var key = new Rfc2898DeriveBytes(sInputKey, saltBytes);

            var aesAlg = new RijndaelManaged();
            aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);
            aesAlg.IV = key.GetBytes(aesAlg.BlockSize / 8);

            return aesAlg;
        }
    }
}
