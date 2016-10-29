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
    /// Interaction logic for AddWindow.xaml
    /// </summary>
    public partial class AddWindow : MetroWindow
    {
        public string[] values;
        private Logs appLogs;
        public AddWindow()
        {
            InitializeComponent();

            appLogs = new Logs();
        }

        private void btCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void btAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                values = new string[] { tbName.Text, tbLogin.Text, tbPassword.Text, tbLink.Text, tbDescription.Text};
                DialogResult = true;
            }
            catch (Exception ex)
            {
                appLogs.Message(ex.Message);
            }
        }
    }
}
