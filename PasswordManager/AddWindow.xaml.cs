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
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using System.Security.Cryptography;

namespace PasswordManager
{
    /// <summary>
    /// Interaction logic for AddWindow.xaml
    /// </summary>
    public partial class AddWindow : MetroWindow
    {
        private Logs appLogs;
        public string UID;

        public AddWindow()
        {
            InitializeComponent();

            appLogs = new Logs();
            tbName.Focus();
        }

        private void btCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void btAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(cbCategory.Text.Length > 0)
                    if (cbCategory.Text[0] == ' ')
                    {
                        MessageBox.Show("popraw");
                        return;
                    }
                using (MD5 md5hash = MD5.Create())
                {
                    UID = Management.GetMd5Hash(md5hash, tbName.Text + tbLogin.Text + tbPassword + DateTime.Now.ToString());
                }
                DialogResult = true;  
                
            }
            catch (Exception ex)
            {
                Logs.Message(ex.Message);
            }
        }

        private void MetroWindow_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                if(tbDescription.Focusable != true)
                {
                    btAdd_Click(sender, e);
                }
                
            }
        }
    }
}
