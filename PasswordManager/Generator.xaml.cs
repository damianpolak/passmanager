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
using MahApps.Metro.Controls;
using System.Threading;

namespace PasswordManager
{
    /// <summary>
    /// Interaction logic for Generator.xaml
    /// </summary>
    public partial class Generator : MetroWindow
    {
        private string sAlphabet = "abcdefghijklmnoprstuqvwxyz";
        private string sSpecialChars = "`~!@#$%^&*()_+-=[]\\{}|;':\\<>,./";
        private Random random;

        public Generator()
        {
            InitializeComponent();
        }

        private string GetMd5Hash(MD5 md5Hash, string input)
        {
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }

        private string GeneratePassword()
        {
            random = new Random();
            string pass = "";

            try
            {
                while (pass.Length < int.Parse(tbLength.Text))
                {
                    switch (random.Next(0, 3))
                    {
                        case 0:
                            if (cbAlpha.IsChecked == true)
                            {
                                switch (random.Next(0, 2))
                                {
                                    case 0:
                                        if (cbUpperCase.IsChecked == true)
                                            pass += sAlphabet[random.Next(0, sAlphabet.Length)].ToString().ToUpper();
                                        break;
                                    case 1:
                                        if (cbLowerCase.IsChecked == true)
                                            pass += sAlphabet[random.Next(0, sAlphabet.Length)].ToString().ToLower();
                                        break;
                                    default:
                                        pass += sAlphabet[random.Next(0, sAlphabet.Length)].ToString().ToUpper();
                                        break;
                                }
                            }
                            break;

                        case 1:
                            if (cbNumbers.IsChecked == true)
                                pass += random.Next(0, 10);
                            break;

                        case 2:
                            if (cbSpecChar.IsChecked == true)
                                pass += sSpecialChars[random.Next(0, sSpecialChars.Length)].ToString();
                            break;
                    }

                }
            }
            catch(Exception ex)
            {
                Logs.Message(ex.Message);
            }



            return pass;
            
        }

        private void btGenerate_Click(object sender, RoutedEventArgs e)
        {
            int length;
            if(int.TryParse(tbLength.Text, out length) == true)
            {
                tbPreview.Clear();
                for (int i = 0; i <= length; i++)
                {
                    tbPreview.Text += GeneratePassword() + Environment.NewLine;
                    Thread.Sleep(100);
                }
            } else
            {
                // here msgbox that tbLength must be int and not char/string
            }


        }

        private void cbAlpha_Unchecked(object sender, RoutedEventArgs e)
        {
            cbNumbers.IsChecked = true;
            cbLowerCase.IsEnabled = false;
            cbUpperCase.IsEnabled = false;
        }

        private void cbNumbers_Unchecked(object sender, RoutedEventArgs e)
        {
            cbAlpha.IsChecked = true;
        }

        private void cbAlpha_Checked(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
