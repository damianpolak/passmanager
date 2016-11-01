using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace PasswordManager
{
    class Management
    {
        private Settings appSettings;
        public Variables appVariables;
        public Logs appLogs;
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

        public void RemoveEmptyFile()
        {
            if(appVariables.sActualFilePath != null)
            {
                if (new FileInfo(appVariables.sActualFilePath).Length == 0)
                {
                    File.Delete(appVariables.sActualFilePath);
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

        public void Serialize(List<DataBase> list, string sFilePath)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<DataBase>));
                using (TextWriter writer = new StreamWriter(sFilePath))
                {
                    serializer.Serialize(writer, list);
                }
            }
            catch(Exception ex)
            {
                appLogs.Message(ex.Message);
            }

        }

        public List<DataBase> Deserialize(string sFilePath)
        {
            try
            {
                XmlSerializer deserializer = new XmlSerializer(typeof(List<DataBase>));
                TextReader reader = new StreamReader(sFilePath);
                object obj = deserializer.Deserialize(reader);
                List<DataBase> XmlData = (List<DataBase>)obj;
                reader.Close();
                return XmlData;
            }
            catch(Exception ex)
            {
                appLogs.Message(ex.Message + " [DESERIALIZATION ERROR]");
                return null;
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
            {
                appLogs.Message("The cipherText input parameter is not base64 encoded");
                return "FileIsCorrupt";
            }
                

            string text;
            try
            {
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
            }
            catch(Exception ex)
            {
                appLogs.Message(ex.Message + "[KEY IS INVALID]");
                return "KeyIsInvalid";
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

        public static bool ShowDialog(MainWindow mainWindow, string sTitle, string sBtnFirst, string sBtnSecond, string sLabel)
        {
            MyOwnDialog dialog = new MyOwnDialog();
            dialog.Owner = mainWindow;
            dialog.Title = sTitle;
            dialog.btnFirst.Content = sBtnFirst;
            dialog.btnSecond.Content = sBtnSecond;
            dialog.mainLabel.Text = sLabel;

            if (dialog.ShowDialog() == true)
            {
                return true;
            }

            return false;
        }

        public static bool ShowDialog(NewWindow newWindow, string sTitle, string sBtnFirst, string sBtnSecond, string sLabel)
        {
            MyOwnDialog dialog = new MyOwnDialog();
            dialog.Owner = newWindow;
            dialog.Title = sTitle;
            dialog.btnFirst.Content = sBtnFirst;
            dialog.btnSecond.Content = sBtnSecond;
            dialog.mainLabel.Text = sLabel;

            if (dialog.ShowDialog() == true)
            {
                return true;
            }

            return false;
        }

    }
}
