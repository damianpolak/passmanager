using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PasswordManager
{
    class DBQueries
    {
        public static List<DataBase> GetItemsByCategory(List<DataBase> list, string sCategory)
        {
            return (from DataBase db in list
                    where db.Category == sCategory
                    select db).ToList();
        }

        public static List<DataBase> GetAllItems(List<DataBase> list)
        {
            return list;
        }
    }
}
