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
        private List<string> listCategories;
        private string passwordToEncode;
        
        private bool bIsActiveDocument = false;
        private bool bMadeChanges = false;

        // TEST
        TestConsole tc;
        public MainWindow()
        {
            InitializeComponent();
            KeyDown += HandleKeyDown;
            appManagement = new Management();
            listDataBase = new List<DataBase>();
            listCategories = new List<string>();

            //tc = new TestConsole();
            //tc.Show();

            // Clean and add default groups
            ListCategoriesClear();
            
        }

        private void ClearAll()
        {
            listViewPasswords.Items.Clear();
            listDataBase.Clear();
            ListCategoriesClear();
        }

        public void ListCategoriesClear()
        {
            listCategories.Clear();
            lbxCategories.ItemsSource = null;
            lbxCategories.Items.Clear();
            listCategories.Add("General");
        }
        private void EnableControls()
        {
            miAdd.IsEnabled = true;
            miEdit.IsEnabled = true;
            miRemove.IsEnabled = true;
            miSave.IsEnabled = true;
            miSaveAs.IsEnabled = true;
            miDuplicate.IsEnabled = true;
            
        }

        private void DisableControls()
        {
            miAdd.IsEnabled = false;
            miEdit.IsEnabled = false;
            miRemove.IsEnabled = false;
            miSaveAs.IsEnabled = false;
            miSave.IsEnabled = false;
            miDuplicate.IsEnabled = false;
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

                passwordToEncode = newWindow.tbPassword.Password;
                TurnOnVisibility();
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
            appSettings.columnCategoryWidth = columnCategory.Width;
            appSettings.columnLastChangesWidth = columnLastChanges.Width;

            /*
            appSettings.gcd1 = gcd1.Width.Value;
            appSettings.gcd2 = gcd2.Width.Value;
            appSettings.gcd3 = gcd3.Width.Value;

            appSettings.grd1 = grd1.Height.Value;
            appSettings.grd2 = grd2.Height.Value;
            appSettings.grd3 = grd3.Height.Value;
            appSettings.grd4 = grd4.Height.Value;*/
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
                columnLastChanges.Width = appSettings.columnLastChangesWidth;
                columnCategory.Width = appSettings.columnCategoryWidth;
                /*
                gcd1.Width = new GridLength(appSettings.gcd1);
                gcd2.Width = new GridLength(appSettings.gcd2);
                gcd3.Width = new GridLength(appSettings.gcd3);

                grd1.Height = new GridLength(appSettings.grd1);
                grd2.Height = new GridLength(appSettings.grd2);
                grd3.Height = new GridLength(appSettings.grd3);
                grd4.Height = new GridLength(appSettings.grd4);*/

            }
        }

        #region ---- ADD NEW ITEM ----
        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            AddWindow addWindow = new AddWindow();
            addWindow.Owner = this;
            addWindow.cbCategory.ItemsSource = listCategories;
            if(addWindow.ShowDialog() == true)
            {
                listDataBase.Add(new DataBase()
                {
                    Name = addWindow.tbName.Text,
                    Login = addWindow.tbLogin.Text,
                    Password = addWindow.tbPassword.Text,
                    Link = addWindow.tbLink.Text,
                    Description = addWindow.tbDescription.Text,
                    Category = addWindow.cbCategory.Text,
                    DateAndTime = DateTime.Now.ToString()
                    
                });

                if(listDataBase.Last<DataBase>().Category != null)
                    listCategories.Add(listDataBase.Last<DataBase>().Category);
                UpdateCategoryList();
                listViewPasswords.Items.Add(listDataBase.Last<DataBase>());
                bMadeChanges = true;
            }

        }
        #endregion ADD NEW ITEM END

        
        public List<string> GetCategoryList()
        {
            //List<string> lCategory = new List<string>();
            foreach(DataBase item in listDataBase)
            {
                if((item.Category != null) && (item.Category != "") && (item.Category != " "))
                    if(!listCategories.Exists(x => x == item.Category))
                    {
                        listCategories.Add(item.Category);
                    }
                       
            }
            return listCategories;

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
                editWindow.cbCategory.ItemsSource = listCategories;

                
                if (editWindow.ShowDialog() == true)
                {
                    listDataBase[listDataBase.FindIndex(x => x.Name == db.Name)].Name = editWindow.tbName.Text;
                    listDataBase[listDataBase.FindIndex(x => x.Name == db.Name)].Password = editWindow.tbPassword.Text;
                    listDataBase[listDataBase.FindIndex(x => x.Name == db.Name)].Login = editWindow.tbLogin.Text;
                    listDataBase[listDataBase.FindIndex(x => x.Name == db.Name)].Link = editWindow.tbLink.Text;
                    listDataBase[listDataBase.FindIndex(x => x.Name == db.Name)].Description = editWindow.tbDescription.Text;
                    listDataBase[listDataBase.FindIndex(x => x.Name == db.Name)].Category = editWindow.cbCategory.Text;
                    listDataBase[listDataBase.FindIndex(x => x.Name == db.Name)].DateAndTime = DateTime.Now.ToString();

                    ((DataBase)listViewPasswords.Items[listViewPasswords.SelectedIndex]).Name = editWindow.tbName.Text;
                    ((DataBase)listViewPasswords.Items[listViewPasswords.SelectedIndex]).Login = editWindow.tbLogin.Text;
                    ((DataBase)listViewPasswords.Items[listViewPasswords.SelectedIndex]).Password = editWindow.tbPassword.Text;
                    ((DataBase)listViewPasswords.Items[listViewPasswords.SelectedIndex]).Link = editWindow.tbLink.Text;
                    ((DataBase)listViewPasswords.Items[listViewPasswords.SelectedIndex]).Description = editWindow.tbDescription.Text;
                    ((DataBase)listViewPasswords.Items[listViewPasswords.SelectedIndex]).Category = editWindow.cbCategory.Text;
                    ((DataBase)listViewPasswords.Items[listViewPasswords.SelectedIndex]).DateAndTime = DateTime.Now.ToString();

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

        private void HandleKeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.N))
                MenuItem_Click(sender, e);
            if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.O))
                miOpen_Click(sender, e);
            if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.S))
                miSave_Click(sender, e);
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
                                TurnOnVisibility();
                                EnableControls();
                                ShowItemsInListView(listDataBase);
                                UpdateCategoryList();


                                break;
                        }

                    }

                }


            } else
            {
                SaveToFileDialog();
            }

            
        }

        public void ShowItemsInListView(List<DataBase> db)
        {
            listViewPasswords.Items.Clear();
            foreach (DataBase item in db)
            {
                listViewPasswords.Items.Add(item);
            }
            listViewPasswords.Items.Refresh();
        }

        public void UpdateCategoryList()
        {
            listCategories = GetCategoryList();

            lbxCategories.ItemsSource = listCategories;
            lbxCategories.Items.Refresh();
        }
        public void TurnOnVisibility()
        {
            listViewPasswords.Visibility = Visibility.Visible;
            lbxCategories.Visibility = Visibility.Visible;
            tbEntryView.Visibility = Visibility.Visible;
            gSplitter.Visibility = Visibility.Visible;
            gSplitter2.Visibility = Visibility.Visible;
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

        private void miPasswordGenerator_Click(object sender, RoutedEventArgs e)
        {
            Generator pg = new Generator();
            pg.Show();
        }

        private void listViewPasswords_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listViewPasswords.SelectedItem != null)
            { 
                DataBase db = (DataBase)listViewPasswords.SelectedItem;
                tbEntryView.Text = "Category: [ " + db.Category + " ]" +
                                    " Title: [ " + db.Name + " ]" +
                                    " Password: [ " + db.Password + " ]" +
                                    " URL: [ " + db.Link + " ]" +
                                    " Last Modification Time: [ " + db.DateAndTime + " ]" +
                                    Environment.NewLine + Environment.NewLine +
                                    "Description: " + db.Description;
            }
        }

        private void lbxCategories_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(lbxCategories.SelectedItem != null)
            {
                
                string sCategory = (String)lbxCategories.SelectedItem;

                switch(sCategory)
                {
                    case "General":
                        ShowItemsInListView(DBQueries.GetAllItems(listDataBase));
                        break;
                    default:
                        ShowItemsInListView(DBQueries.GetItemsByCategory(listDataBase, sCategory));
                        
                        break;
                }

            }
        }
    }
}
