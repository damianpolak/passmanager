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
using System.Security.Cryptography;
using System.IO;
using Microsoft.Win32;

using MahApps.Metro.Controls;

namespace PasswordManager
{
    /// <summary>
    /// Interaction logic for NewWindow.xaml
    /// </summary>
    public partial class NewWindow : MetroWindow
    {
        private Logs appLogs;

        public string sPassword;
        public string sInputDir;
        public NewWindow()
        {
            InitializeComponent();
            appLogs = new Logs();
          

        }

        private void btDone_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DialogResult = true;
            } catch(Exception ex)
            {
                appLogs.Message(ex.Message);
            }
        }

        private void btCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sv = new SaveFileDialog();
            sv.Filter = "Password Manager Files (*.psm)|*.psm";
            sv.DefaultExt = "psm";
            sv.AddExtension = true;
            
            try
            {
                if(sv.ShowDialog() == true)
                {
                    sInputDir = sv.FileName;
                    File.WriteAllText(sv.FileName, "");
                } else
                {
                    DialogResult = false;
                }
            } catch(Exception ex)
            {
                appLogs.Message(ex.Message);
            }

        }
    }
}
