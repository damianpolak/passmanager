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
using System.Security.Cryptography;

namespace PasswordManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindowPart2 : MetroWindow
    {
        private Management appManagement;
        private List<DataBase> listDataBase;
        private List<string> listCategories;
        private string passwordToEncode;
        
        private bool bIsActiveDocument = false;
        private bool bAscendingSort = true;
        private bool bMadeChanges = false;

        private string sCategory = "General";
        // TEST
        //TestConsole tc;
        public MainWindowPart2()
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
            lbxCategories.SelectedIndex = 0;
            
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

            // View
            mivShowCat.IsEnabled = true;
            mivShowPreview.IsEnabled = true;
            mivSortBy.IsEnabled = true;

            // ContextMenu
            cmAdd.IsEnabled = true;
            cmCopyPassword.IsEnabled = true;
            cmCopyUserName.IsEnabled = true;
            cmDuplicate.IsEnabled = true;
            cmEdit.IsEnabled = true;
            cmRemove.IsEnabled = true;
            cmSelectAll.IsEnabled = true;

            // toolbar
            tbtSave.IsEnabled = true;
            tbtSaveAs.IsEnabled = true;
            tbtCopyUserName.IsEnabled = true;
            tbtCopyPassword.IsEnabled = true;
            tbtFind.IsEnabled = true;
            tbtLockWorkSpace.IsEnabled = true;
            
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

            // View
            mivShowCat.IsEnabled = false;
            mivShowPreview.IsEnabled = false;
            mivSortBy.IsEnabled = false;

            // ContextMenu
            cmAdd.IsEnabled = false;
            cmCopyPassword.IsEnabled = false;
            cmCopyUserName.IsEnabled = false;
            cmDuplicate.IsEnabled = false;
            cmEdit.IsEnabled = false;
            cmRemove.IsEnabled = false;
            cmSelectAll.IsEnabled = false;

            // toolbar
            tbtSave.IsEnabled = false;
            tbtSaveAs.IsEnabled = false;
            tbtCopyUserName.IsEnabled = false;
            tbtCopyPassword.IsEnabled = false;
            tbtFind.IsEnabled = false;
            tbtLockWorkSpace.IsEnabled = false;
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
                appManagement.RemoveEmptyFile();
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
            appSettings.columnCreatedDateTimeWidth = columnCreationTime.Width;
            appSettings.columnUIDWidth = columnUID.Width;
            appSettings.gcd1 = gcd1.Width.Value;
            appSettings.grd4 = grd4.Height.Value;
            appSettings.bShowCategory = mivShowCat.IsChecked;
            appSettings.bShowPreviewEntries = mivShowPreview.IsChecked;
            appSettings.bShowToolBar = mivShowToolBar.IsChecked;
            appSettings.marginTopCategories = lbxCategories.Margin.Top;
            appSettings.marginTopListPasswords = listViewPasswords.Margin.Top;

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
                columnUID.Width = appSettings.columnUIDWidth;
                columnCreationTime.Width = appSettings.columnCreatedDateTimeWidth;
                
                gcd1.Width = new GridLength(appSettings.gcd1, GridUnitType.Pixel);
                mivShowCat.IsChecked = appSettings.bShowCategory;
                grd4.Height = new GridLength(appSettings.grd4, GridUnitType.Star);
                mivShowPreview.IsChecked = appSettings.bShowPreviewEntries;
                mivShowToolBar.IsChecked = appSettings.bShowToolBar;
                lbxCategories.Margin = new Thickness { Top = appSettings.marginTopCategories };
                listViewPasswords.Margin = new Thickness { Top = appSettings.marginTopListPasswords };

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
            foreach(String item in listDataBase.Select(x => x.Category).Distinct().ToList())
            {
                if((item != null) && (item != ""))
                if(!listCategories.Exists(x=>x == item))
                    listCategories.Add(item);
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
                editWindow.cbCategory.Text = db.Category;

                if (editWindow.ShowDialog() == true)
                {
                    listDataBase[listDataBase.FindIndex(x => x.UID == db.UID)].Name = editWindow.tbName.Text;
                    listDataBase[listDataBase.FindIndex(x => x.UID == db.UID)].Password = editWindow.tbPassword.Text;
                    listDataBase[listDataBase.FindIndex(x => x.UID == db.UID)].Login = editWindow.tbLogin.Text;
                    listDataBase[listDataBase.FindIndex(x => x.UID == db.UID)].Link = editWindow.tbLink.Text;
                    listDataBase[listDataBase.FindIndex(x => x.UID == db.UID)].Description = editWindow.tbDescription.Text;
                    listDataBase[listDataBase.FindIndex(x => x.UID == db.UID)].Category = editWindow.cbCategory.Text;
                    listDataBase[listDataBase.FindIndex(x => x.UID == db.UID)].DateAndTime = DateTime.Now.ToString();
                    /*
                    ((DataBase)listViewPasswords.Items[listViewPasswords.SelectedIndex]).Name = editWindow.tbName.Text;
                    ((DataBase)listViewPasswords.Items[listViewPasswords.SelectedIndex]).Login = editWindow.tbLogin.Text;
                    ((DataBase)listViewPasswords.Items[listViewPasswords.SelectedIndex]).Password = editWindow.tbPassword.Text;
                    ((DataBase)listViewPasswords.Items[listViewPasswords.SelectedIndex]).Link = editWindow.tbLink.Text;
                    ((DataBase)listViewPasswords.Items[listViewPasswords.SelectedIndex]).Description = editWindow.tbDescription.Text;
                    ((DataBase)listViewPasswords.Items[listViewPasswords.SelectedIndex]).Category = editWindow.cbCategory.Text;
                    ((DataBase)listViewPasswords.Items[listViewPasswords.SelectedIndex]).DateAndTime = DateTime.Now.ToString();*/

                    _AddItemsToListView(listDataBase, sCategory);
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
                listDataBase.RemoveAt(listDataBase.FindIndex(x => x.UID == db.UID));
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
            About about = new About();
            about.Owner = this;
            about.ShowDialog();
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
                                UpdateCategoryList();
                                _AddItemsToListView(listDataBase, sCategory);

                                break;
                        }
                    } 
                }


            } else
            {
                SaveToFileDialog();
            }
        }

        public void UpdateCategoryList()
        {
            listCategories = (GetCategoryList().Where(x => x != "")).ToList();
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
                Logs.Message(ex.Message);
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

        private void miAdd_Click(object sender, RoutedEventArgs e)
        {
            AddWindow addWindow = new AddWindow();
            addWindow.Owner = this;
            addWindow.cbCategory.ItemsSource = listCategories.Where(x => x != "General");
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
                    UID = addWindow.UID,
                    CreatedDateAndTime = DateTime.Now.ToString(),
                    DateAndTime = DateTime.Now.ToString()
                });

                if (listDataBase.Last<DataBase>().Category != null)
                    listCategories.Add(listDataBase.Last<DataBase>().Category);
                UpdateCategoryList();
                _AddItemsToListView(listDataBase, sCategory);
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

                Clipboard.SetText(listDataBase.Find(x => x.UID == db.UID).Login);
            }
        }

        private void cmCopyPassword_Click(object sender, RoutedEventArgs e)
        {
            if(listViewPasswords.SelectedItem != null)
            {
                DataBase db = (DataBase)listViewPasswords.SelectedItem;

                Clipboard.SetText(listDataBase.Find(x=>x.UID==db.UID).Password);
                //MessageBox.Show(listDataBase[0].Password);
            }
        }

        private void miDuplicate_Click(object sender, RoutedEventArgs e)
        {
            if(listViewPasswords.SelectedItem != null)
            {
                DataBase db = (DataBase)listViewPasswords.SelectedItem;

                var result = (from DataBase d in listDataBase
                              where d.UID == db.UID
                              select d).ToList();

                string UID;
                using (MD5 md5hash = MD5.Create())
                {
                    UID = Management.GetMd5Hash(md5hash, result.First().Name + result.First().Login + result.First().Password + DateTime.Now.ToString());
                }
                listDataBase.Add(new DataBase()
                {
                    Name = result.First().Name,
                    Login = result.First().Login,
                    Password = result.First().Password,
                    Link = result.First().Link,
                    Description = result.First().Description,
                    Category = result.First().Category,
                    CreatedDateAndTime = DateTime.Now.ToString(),
                    UID = UID,
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
                mivNoSort.IsChecked = false;
                mivLoginSort.IsChecked = false;
                mivPasswordSort.IsChecked = false;
                mivLinkSort.IsChecked = false;
            } else
            {
                AscDescChangeState();
                return;
            }

            mivTitleSort.IsChecked = true;
            _AddItemsToListView(listDataBase, sCategory);
        }

        public void AscDescChangeState()
        {
            if (bAscendingSort == true)
            {
                mivDescending_Click(null, null);
            }
            else
            {
                mivAscending_Click(null, null);
            }

        }
        private void mivNoSort_Click(object sender, RoutedEventArgs e)
        {
            if(mivNoSort.IsChecked == false)
            {
                // Other sort options false
                mivTitleSort.IsChecked = false;
                mivLoginSort.IsChecked = false;
                mivPasswordSort.IsChecked = false;
                mivLinkSort.IsChecked = false;
            }
            mivNoSort.IsChecked = true;
            _AddItemsToListView(listDataBase, sCategory);
        }

        private void mivLoginSort_Click(object sender, RoutedEventArgs e)
        {
            if (mivLoginSort.IsChecked == false)
            {

                // Other sort options false
                mivTitleSort.IsChecked = false;
                mivNoSort.IsChecked = false;
                mivPasswordSort.IsChecked = false;
                mivLinkSort.IsChecked = false;
            } else
            {
                AscDescChangeState();
                return;
            }

            mivLoginSort.IsChecked = true;
            _AddItemsToListView(listDataBase, sCategory);
        }

        private void mivPasswordSort_Click(object sender, RoutedEventArgs e)
        {
            if (mivPasswordSort.IsChecked == false)
            {
                mivTitleSort.IsChecked = false;
                mivLoginSort.IsChecked = false;
                mivNoSort.IsChecked = false;
                mivLinkSort.IsChecked = false;
            } else
            {
                AscDescChangeState();
                return;
            }

            mivPasswordSort.IsChecked = true;
            _AddItemsToListView(listDataBase, sCategory);
        }

        private void mivLinkSort_Click(object sender, RoutedEventArgs e)
        {
            if (mivLinkSort.IsChecked == false)
            {
                mivTitleSort.IsChecked = false;
                mivLoginSort.IsChecked = false;
                mivPasswordSort.IsChecked = false;
                mivNoSort.IsChecked = false;
            } else
            {
                AscDescChangeState();
                return;
            }

            mivLinkSort.IsChecked = true;
            _AddItemsToListView(listDataBase, sCategory);
        }

        private void mivAscending_Click(object sender, RoutedEventArgs e)
        {
            if (mivNoSort.IsChecked != true)
            {
                if (mivAscending.IsChecked == false)
                {
                    mivDescending.IsChecked = false;
                    mivAscending.IsChecked = true;
                    bAscendingSort = true;
                }
                else
                {
                    mivDescending.IsChecked = true;
                    mivAscending.IsChecked = false;
                    bAscendingSort = false;
                }

                _AddItemsToListView(listDataBase, sCategory);
            }


        }

        private void mivDescending_Click(object sender, RoutedEventArgs e)
        {
            if(mivNoSort.IsChecked != true)
            {
                if (mivDescending.IsChecked == false)
                {
                    mivDescending.IsChecked = true;
                    mivAscending.IsChecked = false;
                    bAscendingSort = false;
                }
                else
                {
                    mivDescending.IsChecked = false;
                    mivAscending.IsChecked = true;
                    bAscendingSort = true;
                }

                _AddItemsToListView(listDataBase, sCategory);
            }

        }

        public void _AddItemsToListView(List<DataBase> db, string category)
        {
            List<DataBase> _db;
            switch(sCategory)
            {
                case "General":
                    _db = _SortListView(DBQueries.GetAllItems(db));
                    break;

                default:
                    _db = _SortListView(DBQueries.GetItemsByCategory(db, sCategory));
                    break;
            }

            listViewPasswords.Items.Clear();
            foreach (DataBase item in _db)
            {
                listViewPasswords.Items.Add(item);
            }

            listViewPasswords.Items.Refresh();
        }

        public List<DataBase> _SortListView(List<DataBase> db)
        {
            if (mivNoSort.IsChecked == true)
                return DBQueries.NoSort(db);
            if (mivTitleSort.IsChecked == true)
                return DBQueries.SortByTitle(db, bAscendingSort);
            if (mivLoginSort.IsChecked == true)
                return DBQueries.SortByUserName(db, bAscendingSort);
            if (mivPasswordSort.IsChecked == true)
                return DBQueries.SortByPassword(db, bAscendingSort);
            if (mivLinkSort.IsChecked == true)
                return DBQueries.SortByURL(db, bAscendingSort);
            return null;
        }

        private void lbxCategories_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbxCategories.SelectedItem != null)
            {
                sCategory = (String)lbxCategories.SelectedItem;
                _AddItemsToListView(listDataBase, sCategory);
            }
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
            DisableControls();
            bMadeChanges = false;
        }

        private void mivShowToolBar_Click(object sender, RoutedEventArgs e)
        {
            if (mivShowToolBar.IsChecked == true)
            {
                mivShowToolBar.IsChecked = false;
                tbtMenu.Visibility = Visibility.Hidden;
                lbxCategories.Margin = new Thickness { Top = 27 };
                listViewPasswords.Margin = new Thickness { Top = 27 };
            }
            else
            {
                mivShowToolBar.IsChecked = true;
                tbtMenu.Visibility = Visibility.Visible;
                lbxCategories.Margin = new Thickness { Top = 57 };
                listViewPasswords.Margin = new Thickness { Top = 57 };
            }
        }

        private void mivConfigureColumns_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
