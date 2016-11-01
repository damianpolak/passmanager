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


        public static List<DataBase> NoSort(List<DataBase> list)
        {
            return list;
        }

        public static List<DataBase> SortByTitle(List<DataBase> list, bool ascending)
        {
            if(ascending == true)
            {
                return list.OrderBy(x => x.Name).ToList();
            } else
            {
                return list.OrderByDescending(x => x.Name).ToList();
            }
        }

        public static List<DataBase> SortByUserName(List<DataBase> list, bool ascending)
        {
            if(ascending == true)
            {
                return list.OrderBy(x => x.Login).ToList();
            } else
            {
                return list.OrderByDescending(x => x.Login).ToList();
            }
            
        }

        public static List<DataBase> SortByPassword(List<DataBase> list, bool ascending)
        {
            if(ascending == true)
            {
                return list.OrderBy(x => x.Password).ToList();
            } else
            {
                return list.OrderByDescending(x => x.Password).ToList();
            }
            
        }

        public static List<DataBase> SortByURL(List<DataBase> list, bool ascending)
        {
            if(ascending == true)
            {
                return list.OrderBy(x => x.Link).ToList();
            } else
            {
                return list.OrderByDescending(x => x.Link).ToList();
            }
            
        }

        public static List<DataBase> SortAscending(List<DataBase> list)
        {
            return (list.OrderBy(x => x)).ToList();
        }

        public static List<DataBase> SortDescending(List<DataBase> list)
        {
            return (list.OrderByDescending(x => x)).ToList();
        }
    }
}
