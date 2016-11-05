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
        public double columnLastChangesWidth;
        public double columnCategoryWidth;
        public double columnCreatedDateTimeWidth;
        public double columnUIDWidth;

        // Show: category, preview and toolbar
        public double gcd1;
        public double grd4;
        public bool bShowCategory;
        public bool bShowPreviewEntries;
        public bool bShowToolBar;
        public double marginTopCategories;
        public double marginTopListPasswords;

        public bool bAscendingSort;
        public bool bNoSort;
        public bool bSortByTitle;
        public bool bSortByUserName;
        public bool bSortByPassword;
        public bool bSortByURL;
    }
}
