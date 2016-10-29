using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PasswordManager
{
    class Variables
    {
        public string sSettingsFileDirectory = System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData) + "\\PasswordManager\\";
        public string sSettingsFilePath;

        public Variables()
        {
            sSettingsFilePath = sSettingsFileDirectory + "settings.bin";
        }
    }
}
