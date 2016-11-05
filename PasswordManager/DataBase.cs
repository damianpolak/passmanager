using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PasswordManager
{
    [Serializable()]
    public class DataBase
    {
        public string Name { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Link { get; set; }
        public string Description { get; set; }
        public string DateAndTime { get; set; } // Last modification
        public string CreatedDateAndTime { get; set; }
        public string Category { get; set; }
        public string UID { get; set; }
    }
}
