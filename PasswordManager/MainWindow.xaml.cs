using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro.Controls;

namespace PasswordManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private Management appManagement;
        private DataBase dataBase;
        private List<DataBase> listDataBase;

        public MainWindow()
        {
            InitializeComponent();
            appManagement = new Management();
            //dataBase = new DataBase();
            listDataBase = new List<DataBase>();

            /*
            List<Item> items = new List<Item>();
            items.Add(new Item() { Name = "John Doe", Login = "login1", Password = "john@doe-family.com" });
            items.Add(new Item() { Name = "Jane Doe", Login = "login2", Password = "jane@doe-family.com" });
            items.Add(new Item() { Name = "Sammy Doe", Login = "login3", Password = "sammy.doe@gmail.com" });
            listViewPasswords.ItemsSource = items;
            */

        }

        public class Item
        {
            public string Name { get; set; }

            public string Login { get; set; }

            public string Password { get; set; }
            public string Link { get; set; }
            public string Description { get; set; }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Settings appSettings = new Settings();
            appSettings.mainTop = this.Top;
            appSettings.mainLeft = this.Left;
            appSettings.mainHeight = this.Height;
            appSettings.mainWidth = this.Width;
            appSettings.columnNameWidth = columnName.Width;
            appSettings.columnLoginWidth = columnLogin.Width;
            appSettings.columnPasswordWidth = columnPassword.Width;
            appSettings.columnLinkWidth = columnLink.Width;
            appSettings.columnDescriptionWidth = columnDescription.Width;
            appManagement.SaveSettingsToFile(appSettings);
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Settings appSettings = new Settings();
            if(appManagement.GetSettings() != null)
            {
                appSettings = appManagement.GetSettings();
                this.Top = appSettings.mainTop;
                this.Left = appSettings.mainLeft;
                this.Width = appSettings.mainWidth;
                this.Height = appSettings.mainHeight;
                columnName.Width = appSettings.columnNameWidth;
                columnLogin.Width = appSettings.columnLoginWidth;
                columnPassword.Width = appSettings.columnPasswordWidth;
                columnLink.Width = appSettings.columnLinkWidth;
                columnDescription.Width = appSettings.columnDescriptionWidth;

            }
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            AddWindow addWindow = new AddWindow();
            
            if(addWindow.ShowDialog() == true)
            {

                listDataBase.Add(new DataBase()
                {
                    Name = addWindow.tbName.Text,
                    Login = addWindow.tbLogin.Text,
                    Password = addWindow.tbPassword.Text,
                    Link = addWindow.tbLink.Text,
                    Description = addWindow.tbDescription.Text
                });
                listViewPasswords.Items.Add(listDataBase.Last<DataBase>());
                //listViewPasswords.Items.Add(new Item { Name = addWindow.tbName.Text, Login = addWindow.tbLogin.Text });
            }
            
        }
    }
}
