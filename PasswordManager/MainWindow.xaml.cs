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
using Microsoft.Win32;
using MahApps.Metro.Controls;
using System.IO;

namespace PasswordManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private Management appManagement;
        private List<DataBase> listDataBase;
        private string passwordToEncode;
        
        private bool bIsActiveDocument = false;
        private bool bMadeChanges = false;

        // TEST
        TestConsole tc;
        public MainWindow()
        {
            InitializeComponent();
            appManagement = new Management();
            listDataBase = new List<DataBase>();

            //tc = new TestConsole();
            //tc.Show();
            /*
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
            */

            System.Windows.Forms.NotifyIcon tray = new System.Windows.Forms.NotifyIcon();
            tray.Visible = true;


        }

        private void ClearAll()
        {
            listViewPasswords.Items.Clear();
            listDataBase.Clear();
        }

        private void EnableControls()
        {
            miAdd.IsEnabled = true;
            miEdit.IsEnabled = true;
            miRemove.IsEnabled = true;
            miSave.IsEnabled = true;
            miSaveAs.IsEnabled = true;
        }

        private void DisableControls()
        {
            miAdd.IsEnabled = false;
            miEdit.IsEnabled = false;
            miRemove.IsEnabled = false;
            miSaveAs.IsEnabled = false;
            miSave.IsEnabled = false;
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
            newWindow.Owner = this;
            
            if(newWindow.ShowDialog() == true)
            {
                bIsActiveDocument = true;
                
                passwordToEncode = newWindow.tbPassword.Text;
                listViewPasswords.Visibility = Visibility.Visible;
                ClearAll();
                EnableControls();
                appManagement.appVariables.sActualFilePath = newWindow.sInputDir;
            } 
        }

        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(bMadeChanges == true)
            {
                SaveToFileDialog();
            } else
            {
                appManagement.RemoveEmptyFile();
            }
            
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
            addWindow.Owner = this;
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
                //listViewPasswords.c
                listViewPasswords.Items.Add(listDataBase.Last<DataBase>());
                
                bMadeChanges = true;
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
                    bMadeChanges = true;
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
                bMadeChanges = true;
            }
        }

        private void miExit_Click(object sender, RoutedEventArgs e)
        {

            if(bMadeChanges == false)
            {
                Application.Current.Shutdown();
            } else
            {
                SaveToFileDialog();
                Application.Current.Shutdown();
            }
            
        }

        private void MenuItem_Click_4(object sender, RoutedEventArgs e)
        {
            //Management.ShowDialog(this);
        }

        private void miSave_Click(object sender, RoutedEventArgs e)
        {
            SaveToFile();

        }

        private void miOpen_Click(object sender, RoutedEventArgs e)
        {
            if(bMadeChanges != true)
            {
                Management man = new Management();

                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "Password Manager Files (*.psm)|*.psm";
                ofd.DefaultExt = "psm";
                ofd.AddExtension = true;

                if (ofd.ShowDialog() == true)
                {
                    appManagement.appVariables.sActualFilePath = ofd.FileName;

                    OpenFile of = new OpenFile();
                    of.Owner = this;
                    if(of.ShowDialog() == true)
                    {
                        passwordToEncode = of.tbPassword.Text;
                        appManagement.sInputKey = passwordToEncode;

                        string sReadFile = appManagement.DecryptRijndael(File.ReadAllText(
                        appManagement.appVariables.sActualFilePath), appManagement.appVariables.sSalt);
                        switch(sReadFile)
                        {
                            case "KeyIsInvalid":
                                Management.ShowDialog(this, "Information", "Ok", "Cancel",
                                    "Key file is invalid! Please try again!");
                                break;
                            case "FileIsCorrupt":
                                Management.ShowDialog(this, "Information", "Ok", "Cancel",
                                    "File is corrupted!");
                                break;

                            default:
                                File.WriteAllText(appManagement.appVariables.sActualFilePath + ".temp", sReadFile);

                                listDataBase = man.Deserialize(appManagement.appVariables.sActualFilePath + ".temp");
                                File.Delete(appManagement.appVariables.sActualFilePath + ".temp");
                                listViewPasswords.Visibility = Visibility.Visible;
                                EnableControls();
                                listViewPasswords.Items.Clear();
                                foreach (DataBase item in listDataBase)
                                {
                                    listViewPasswords.Items.Add(item);
                                }
                                listViewPasswords.Items.Refresh();
                                break;
                        }

                    }

                }


            } else
            {
                SaveToFileDialog();
            }

            
        }

        public void SaveToFileDialog()
        {

            if (Management.ShowDialog(this, "Question", "Yes", "No", 
                "Do you want to save changes?") == true)
            {
                SaveToFile();
            }
        }
        public void SaveToFile()
        {
            Management man = new Management();
            man.Serialize(listDataBase, appManagement.appVariables.sActualFilePath);
            man.sInputKey = passwordToEncode;
            string sReadFile = man.EncryptRijndael(File.ReadAllText(appManagement.appVariables.sActualFilePath), appManagement.appVariables.sSalt);
            File.Delete(appManagement.appVariables.sActualFilePath);
            File.WriteAllText(appManagement.appVariables.sActualFilePath, sReadFile);
            bMadeChanges = false;
        }

        private void miSaveAs_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sv = new SaveFileDialog();
            sv.Filter = "Password Manager Files (*.psm)|*.psm";
            sv.DefaultExt = "psm";
            sv.AddExtension = true;

            try
            {
                if (sv.ShowDialog() == true)
                {
                    appManagement.RemoveEmptyFile();
                    appManagement.appVariables.sActualFilePath = sv.FileName;
                    SaveToFile();
                }
                else
                {
                    DialogResult = false;
                }
            }
            catch (Exception ex)
            {
                appManagement.appLogs.Message(ex.Message);
            }
        }
    }
}
