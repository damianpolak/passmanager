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
        private bool bAscendingSort = true;
        private bool bMadeChanges = false;

        // TEST
        //TestConsole tc;
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
            miClose.IsEnabled = true;
            miDuplicate.IsEnabled = true;

            // ContextMenu
            cmAdd.IsEnabled = true;
            cmCopyPassword.IsEnabled = true;
            cmCopyUserName.IsEnabled = true;
            cmDuplicate.IsEnabled = true;
            cmEdit.IsEnabled = true;
            cmRemove.IsEnabled = true;
            cmSelectAll.IsEnabled = true;
        }

        private void DisableControls()
        {
            miAdd.IsEnabled = false;
            miEdit.IsEnabled = false;
            miRemove.IsEnabled = false;
            miSaveAs.IsEnabled = false;
            miSave.IsEnabled = false;
            miClose.IsEnabled = false;
            miDuplicate.IsEnabled = false;

            // ContextMenu
            cmAdd.IsEnabled = false;
            cmCopyPassword.IsEnabled = false;
            cmCopyUserName.IsEnabled = false;
            cmDuplicate.IsEnabled = false;
            cmEdit.IsEnabled = false;
            cmRemove.IsEnabled = false;
            cmSelectAll.IsEnabled = false;
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
            } else
            {
                bIsActiveDocument = false;
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
            appSettings.gcd1 = gcd1.Width.Value;
            appSettings.grd4 = grd4.Height.Value;
            appSettings.bNoSort = mivNoSort.IsChecked;
            appSettings.bSortByTitle = mivTitleSort.IsChecked;
            appSettings.bSortByUserName = mivLoginSort.IsChecked;
            appSettings.bSortByPassword = mivPasswordSort.IsChecked;
            appSettings.bSortByURL = mivLinkSort.IsChecked;
            appSettings.bAscendingSort = bAscendingSort;
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
                gcd1.Width = new GridLength(appSettings.gcd1, GridUnitType.Pixel);
                grd4.Height = new GridLength(appSettings.grd4, GridUnitType.Star);
                mivNoSort.IsChecked = appSettings.bNoSort;
                mivTitleSort.IsChecked = appSettings.bSortByTitle;
                mivLoginSort.IsChecked = appSettings.bSortByUserName;
                mivPasswordSort.IsChecked = appSettings.bSortByPassword;
                mivLinkSort.IsChecked = appSettings.bSortByURL;
                bAscendingSort = appSettings.bAscendingSort;
            }
        }
      
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
        private void miEdit_Click(object sender, RoutedEventArgs e)
        {
            if(listViewPasswords.SelectedItem != null)
            {
                AddWindow editWindow = new AddWindow();
                editWindow.btAdd.Content = "Save";
                editWindow.Owner = this;
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

                    ViewAndSortListView();
                    bMadeChanges = true;
                }

            }
        }

        private void miRemoveItem_Click(object sender, RoutedEventArgs e)
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

            if(bIsActiveDocument == true)
            {
                if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.S))
                    miSave_Click(sender, e);
                if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.I))
                    miAdd_Click(sender, e);
                if (Keyboard.IsKeyDown(Key.Delete))
                    miRemoveItem_Click(sender, e);
                if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.B))
                    cmCopyUserName_Click(sender, e);
                if (Keyboard.IsKeyDown(Key.Enter))
                    miEdit_Click(sender, e);
                if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.K))
                    miDuplicate_Click(sender, e);

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
                        bIsActiveDocument = true;
                        passwordToEncode = of.tbPassword.Password;
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
                                ViewAndSortListView();
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

        public void TurnOffVisibility()
        {
            listViewPasswords.Visibility = Visibility.Hidden;
            lbxCategories.Visibility = Visibility.Hidden;
            tbEntryView.Visibility = Visibility.Hidden;
            gSplitter.Visibility = Visibility.Hidden;
            gSplitter2.Visibility = Visibility.Hidden;
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
            pg.Owner = this;
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

        private void miAdd_Click(object sender, RoutedEventArgs e)
        {
            AddWindow addWindow = new AddWindow();
            addWindow.Owner = this;
            addWindow.cbCategory.ItemsSource = listCategories;
            if (addWindow.ShowDialog() == true)
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

                if (listDataBase.Last<DataBase>().Category != null)
                    listCategories.Add(listDataBase.Last<DataBase>().Category);
                UpdateCategoryList();
                ViewAndSortListView();
                bMadeChanges = true;
            }
        }

        private void mivShowPreview_Click(object sender, RoutedEventArgs e)
        {
            if (mivShowPreview.IsChecked == true)
            {
                mivShowPreview.IsChecked = false;
                tbEntryView.Visibility = Visibility.Hidden;
                grd4.Height = new GridLength(0, GridUnitType.Star);
                
            } else
            {
                mivShowPreview.IsChecked = true;
                tbEntryView.Visibility = Visibility.Visible;
                grd4.Height = new GridLength(51, GridUnitType.Star);
            }
        }

        private void mivShowCat_Click(object sender, RoutedEventArgs e)
        {
            if(mivShowCat.IsChecked == true)
            {
                mivShowCat.IsChecked = false;
                lbxCategories.Visibility = Visibility.Hidden;
                gcd1.Width = new GridLength(0, GridUnitType.Star);
            } else
            {
                mivShowCat.IsChecked = true;
                lbxCategories.Visibility = Visibility.Visible;
                gcd1.Width = new GridLength(140, GridUnitType.Pixel);
            }
        }

        private void cmCopyUserName_Click(object sender, RoutedEventArgs e)
        {
            if (listViewPasswords.SelectedItem != null)
            {
                DataBase db = (DataBase)listViewPasswords.SelectedItem;
                Clipboard.SetText(db.Login);
            }
        }

        private void cmCopyPassword_Click(object sender, RoutedEventArgs e)
        {
            if(listViewPasswords.SelectedItem != null)
            {
                DataBase db = (DataBase)listViewPasswords.SelectedItem;
                string result = (from DataBase d in listDataBase
                              where d.Name == db.Name
                              select d.Password).First().ToString();
                Clipboard.SetText(result);
            }
        }

        private void miDuplicate_Click(object sender, RoutedEventArgs e)
        {
            if(listViewPasswords.SelectedItem != null)
            {
                DataBase db = (DataBase)listViewPasswords.SelectedItem;

                var result = (from DataBase d in listDataBase
                              where d.Name == db.Name
                              select db).ToList();
                
                listDataBase.Add(new DataBase()
                {
                    Name = result.First().Name,
                    Login = result.First().Login,
                    Password = result.First().Password,
                    Link = result.First().Link,
                    Description = result.First().Description,
                    Category = result.First().Category,
                    DateAndTime = DateTime.Now.ToString()
                });

                if (listDataBase.Last<DataBase>().Category != null)
                    listCategories.Add(listDataBase.Last<DataBase>().Category);
                UpdateCategoryList();
                listViewPasswords.Items.Add(listDataBase.Last<DataBase>());
                bMadeChanges = true;
            }
        }


        private void mivTitleSort_Click(object sender, RoutedEventArgs e)
        {
            if(mivTitleSort.IsChecked == false)
            {
                mivTitleSort.IsChecked = true;

                // Other sort options false
                mivNoSort.IsChecked = false;
                mivLoginSort.IsChecked = false;
                mivPasswordSort.IsChecked = false;
                mivLinkSort.IsChecked = false;
            }
            ShowItemsInListView(DBQueries.SortByTitle(listDataBase, bAscendingSort));
        }

        private void mivNoSort_Click(object sender, RoutedEventArgs e)
        {
            if(mivNoSort.IsChecked == false)
            {
                mivNoSort.IsChecked = true;

                // Other sort options false
                mivTitleSort.IsChecked = false;
                mivLoginSort.IsChecked = false;
                mivPasswordSort.IsChecked = false;
                mivLinkSort.IsChecked = false;
            }
            ShowItemsInListView(DBQueries.NoSort(listDataBase));

        }

        private void mivLoginSort_Click(object sender, RoutedEventArgs e)
        {
            if (mivLoginSort.IsChecked == false)
            {
                mivLoginSort.IsChecked = true;

                // Other sort options false
                mivTitleSort.IsChecked = false;
                mivNoSort.IsChecked = false;
                mivPasswordSort.IsChecked = false;
                mivLinkSort.IsChecked = false;
            }
            ShowItemsInListView(DBQueries.SortByUserName(listDataBase, bAscendingSort));
        }

        private void mivPasswordSort_Click(object sender, RoutedEventArgs e)
        {
            if (mivPasswordSort.IsChecked == false)
            {
                mivPasswordSort.IsChecked = true;

                // Other sort options false
                mivTitleSort.IsChecked = false;
                mivLoginSort.IsChecked = false;
                mivNoSort.IsChecked = false;
                mivLinkSort.IsChecked = false;
            }
            ShowItemsInListView(DBQueries.SortByPassword(listDataBase, bAscendingSort));
        }

        private void mivLinkSort_Click(object sender, RoutedEventArgs e)
        {
            if (mivLinkSort.IsChecked == false)
            {
                mivLinkSort.IsChecked = true;

                // Other sort options false
                mivTitleSort.IsChecked = false;
                mivLoginSort.IsChecked = false;
                mivPasswordSort.IsChecked = false;
                mivNoSort.IsChecked = false;
            }
            ShowItemsInListView(DBQueries.SortByURL(listDataBase, bAscendingSort));
        }

        private void mivAscending_Click(object sender, RoutedEventArgs e)
        {
            if(mivAscending.IsChecked == false)
            {
                mivAscending.IsChecked = true;
                mivDescending.IsChecked = false;
                bAscendingSort = true;
            }
            ViewAndSortListView();
        }

        private void mivDescending_Click(object sender, RoutedEventArgs e)
        {
            if(mivDescending.IsChecked == false)
            {
                mivDescending.IsChecked = true;
                mivAscending.IsChecked = false;
                bAscendingSort = false;
            }
            ViewAndSortListView();
        }

        public void ViewAndSortListView()
        {
            if (mivNoSort.IsChecked == true)
                ShowItemsInListView(DBQueries.NoSort(listDataBase));
            if (mivTitleSort.IsChecked == true)
                ShowItemsInListView(DBQueries.SortByTitle(listDataBase, bAscendingSort));
            if (mivLoginSort.IsChecked == true)
                ShowItemsInListView(DBQueries.SortByUserName(listDataBase, bAscendingSort));
            if (mivPasswordSort.IsChecked == true)
                ShowItemsInListView(DBQueries.SortByPassword(listDataBase, bAscendingSort));
            if (mivLinkSort.IsChecked == true)
                ShowItemsInListView(DBQueries.SortByURL(listDataBase, bAscendingSort));
        }

        private void miClose_Click(object sender, RoutedEventArgs e)
        {
            if (bMadeChanges == true)
            {
                SaveToFileDialog();
            }
            else
            {
                appManagement.RemoveEmptyFile();
            }

            ClearAll();
            TurnOffVisibility();
            bMadeChanges = false;
        }
    }
}
