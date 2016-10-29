using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PasswordManager
{
    [Serializable()]
    class Settings
    {
        public double mainTop;
        public double mainLeft;
        public double mainHeight;
        public double mainWidth;

        public double columnNameWidth;
        public double columnLoginWidth;
        public double columnPasswordWidth;
        public double columnLinkWidth;
        public double columnDescriptionWidth;
    }
}
