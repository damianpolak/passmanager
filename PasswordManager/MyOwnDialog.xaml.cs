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
    /// Interaction logic for MyOwnDialog.xaml
    /// </summary>
    public partial class MyOwnDialog : MetroWindow
    {
        public MyOwnDialog()
        {
            InitializeComponent();
        }

        private void btnFirst_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void btnSecond_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void MetroWindow_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                btnFirst_Click(sender, e);
            }
        }
    }
}
