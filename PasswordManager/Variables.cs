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
        public string sActualFilePath;
        public string sSalt = "*sh1%dmc&84J6&4c";

        public Variables()
        {
            sSettingsFilePath = sSettingsFileDirectory + "settings.bin";
        }
    }
}
