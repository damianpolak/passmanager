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
            tbConsole.Text = man.EncryptRijndael(tbConsole2.Text, "1234567890");
        }

        private void btDecrypt_Click(object sender, RoutedEventArgs e)
        {
            Management man = new Management();
            man.sInputKey = "mojehaslo";
            //tbConsole2.Text = man.DecryptRijndael(tbConsole.Text, "1234567890");
            MessageBox.Show(man.DecryptRijndael(tbConsole.Text, "1234567890"));
        }
    }
}
