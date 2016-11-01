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
using System.IO;

namespace PasswordManager
{
    /// <summary>
    /// Interaction logic for TestConsole.xaml
    /// </summary>
    public partial class TestConsole : Window
    {
        public TestConsole()
        {
            InitializeComponent();
        }

        private void btEncrypt_Click(object sender, RoutedEventArgs e)
        {
            Management man = new Management();
            man.sInputKey = "mojehaslo";
            tbConsole.Text = man.EncryptRijndael(File.ReadAllText(@"C:\Users\dpolak\Desktop\settings.bin"), "1234567890");
            StreamReader sr = new StreamReader(@"C:\Users\dpolak\Desktop\file.xml");
            tbConsole.Text = man.EncryptRijndael(sr.ReadToEnd(), "1234567890");
            sr.Close();
        }

        private void btDecrypt_Click(object sender, RoutedEventArgs e)
        {
            Management man = new Management();
            man.sInputKey = "mojehaslo";
            tbConsole2.Text = man.DecryptRijndael(tbConsole.Text, "1234567890");
            StreamWriter sw = new StreamWriter(@"C:\Users\dpolak\Desktop\file.xml2");
            sw.Write(tbConsole2.Text);
            sw.Close();

        }
    }
}
