using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace PasswordManager
{
    class Management
    {
        private Settings appSettings;
        private Variables appVariables;
        private Logs appLogs;
        private bool bSettingsAvailable;

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
    }
}
