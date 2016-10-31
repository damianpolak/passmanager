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

namespace PasswordManager
{
    /// <summary>
    /// Interaction logic for OpenFile.xaml
    /// </summary>
    public partial class OpenFile : MetroWindow
    {
        public OpenFile()
        {
            InitializeComponent();
            tbPassword.Focus();
        }
        
        private void btDecrypt_Click(object sender, RoutedEventArgs e)
        {
            if(tbPassword.Text.Length > 7)
            {
                DialogResult = true;
            } else
            {
                MyOwnDialog own = new MyOwnDialog();
                own.Owner = this;
                own.Title = "Information";
                own.btnFirst.Content = "Ok";
                own.btnSecond.Content = "Cancel";
                own.mainLabel.Text = "Password is too short! Minimum length is 8 characters.";
                own.ShowDialog();
            }
            
        }

        private void btCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void tbPassword_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                btDecrypt_Click(sender, e);
            }
        }
    }
}
