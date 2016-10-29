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
            listDataBase = new List<DataBase>();

            DataBase db1 = new DataBase();
            db1.Name = "test1";
            db1.Login = "login1";
            db1.Password = "password1";

            DataBase db2 = new DataBase();
            db2.Name = "test2";
            db2.Login = "logitest2";
            db2.Password = "password2";

            listViewPasswords.Items.Add(db1);
            listViewPasswords.Items.Add(db2);
            listDataBase.Add(db1);
            listDataBase.Add(db2);

            


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
            NewWindow newWindow = new NewWindow();
            
            
            if(newWindow.ShowDialog() == true)
            {
                
            } 
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
            }
            
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            if(listViewPasswords.SelectedItem != null)
            {
                AddWindow editWindow = new AddWindow();
                editWindow.btAdd.Content = "Save";
                DataBase db = (DataBase)listViewPasswords.SelectedItem;
                editWindow.Title = "Edit [" + db.Name + "]";
                editWindow.tbName.Text = db.Name;
                editWindow.tbLogin.Text = db.Login;
                editWindow.tbPassword.Text = db.Password;
                editWindow.tbLink.Text = db.Link;
                editWindow.tbDescription.Text = db.Description;

                if (editWindow.ShowDialog() == true)
                {
                    //MessageBox.Show(listDataBase.Find(x => x.Name == db.Name).Name);
                    listDataBase[listDataBase.FindIndex(x => x.Name == db.Name)].Name = editWindow.tbName.Text;
                    listDataBase[listDataBase.FindIndex(x => x.Name == db.Name)].Password = editWindow.tbPassword.Text;
                    listDataBase[listDataBase.FindIndex(x => x.Name == db.Name)].Login = editWindow.tbLogin.Text;
                    listDataBase[listDataBase.FindIndex(x => x.Name == db.Name)].Link = editWindow.tbLink.Text;
                    listDataBase[listDataBase.FindIndex(x => x.Name == db.Name)].Description = editWindow.tbDescription.Text;

                    ((DataBase)listViewPasswords.Items[listViewPasswords.SelectedIndex]).Name = editWindow.tbName.Text;
                    ((DataBase)listViewPasswords.Items[listViewPasswords.SelectedIndex]).Login = editWindow.tbLogin.Text;
                    ((DataBase)listViewPasswords.Items[listViewPasswords.SelectedIndex]).Password = editWindow.tbPassword.Text;
                    ((DataBase)listViewPasswords.Items[listViewPasswords.SelectedIndex]).Link = editWindow.tbLink.Text;
                    ((DataBase)listViewPasswords.Items[listViewPasswords.SelectedIndex]).Description = editWindow.tbDescription.Text;

                    listViewPasswords.Items.Refresh();
                }

            }
        }

        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            if (listViewPasswords.SelectedItem != null)
            {
                DataBase db = (DataBase)listViewPasswords.SelectedItem;
                listViewPasswords.Items.RemoveAt(listViewPasswords.SelectedIndex);
                listDataBase.RemoveAt(listDataBase.FindIndex(x => x.Name == db.Name));
            }
        }
    }
}
