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

namespace PasswordManager
{
    /// <summary>
    /// Interaction logic for Generator.xaml
    /// </summary>
    public partial class Generator : MetroWindow
    {
        private bool bAlphaOn = false;
        private bool bLowerOn = false;
        private bool bUpperOn = false;
        private bool bNumberOn = false;
        private bool bSpecCharsOn = false;

        private Random random;
        private string buffer = "";
        private string hash = "";
        public Generator()
        {
            InitializeComponent();
        }

        public string Generate()
        {

            buffer = "";
            random = new Random();
            string sAlpha = "ABCDEFGHIJKLMNOPRSTQUVWXYZ";
            string sNumbers = "1234567890";
            string sSpecialChars = "`~!@#$%^&*()_+-=[]{}|;':,.<>?";
            int iLetter;
            int iRandChar;

            if (cbAlpha.IsChecked == true) { bAlphaOn = true; } else { bAlphaOn = false; }
            if (cbUpperCase.IsChecked == true) { bUpperOn = true; } else { bUpperOn = false; }
            if (cbLowerCase.IsChecked == true) { bLowerOn = true; } else { bLowerOn = false; }
            if (cbNumbers.IsChecked == true) { bNumberOn = true; } else { bNumberOn = false; }
            if (cbSpecChar.IsChecked == true) { bSpecCharsOn = true; } else { bSpecCharsOn = false; }

            for(int i = 0; i <= int.Parse(tbLength.Text)-1; i++)
            {
                if(bAlphaOn == true)
                {
                    iRandChar = random.Next(0, sAlpha.Length - 1);
                    if(bLowerOn == true)
                    {

                    }
                    buffer += sAlpha[iRandChar];
                    
                }

                if(bNumberOn == true)
                {
                    iRandChar = random.Next(0, sNumbers.Length - 1);
                    buffer += sNumbers[iRandChar];
                    
                }

                if(bSpecCharsOn == true)
                {
                    iRandChar = random.Next(0, sSpecialChars.Length - 1);
                    buffer += sSpecialChars[iRandChar];
                }

                switch (buffer.Length)
                {
                    case 1:
                        i = buffer.Length - 1;
                        break;
                    case 2:
                        i = buffer.Length - 2;
                        break;

                    case 3:
                        i = buffer.Length - 3;
                        break;
                }
            }

            return buffer;
            /*
            for (int i = 0; i <= int.Parse(tbLength.Text); i++)
            {
                randChar = random.Next(0, 10);
                if (bAlphaOn == true && randChar <= 5)
                {
                    rletter = random.Next(0, alpha.Length);
                    if (bLowerOn == true)
                    {
                        if (random.Next(0, 2) == 0)
                        {
                            buffer = buffer + alpha[rletter].ToString().ToLower();
                        }
                        else {
                            if (bUpperOn == true)
                            {
                                buffer = buffer + alpha[rletter].ToString().ToUpper();
                            }
                            else {
                                buffer = buffer + alpha[rletter].ToString().ToLower();
                            }
                        }
                    }
                    else if (bUpperOn == true)
                    {
                        buffer = buffer + alpha[rletter].ToString().ToUpper();
                    }
                }



                if ((bNumberOn == true) && (randChar > 5 && randChar <= 8))
                {
                    rletter = random.Next(0, num.Length);
                    buffer = buffer + num[rletter].ToString();
                }

                if (bSpecCharsOn == true && randChar > 8)
                {
                    rletter = random.Next(0, specialChars.Length);
                    buffer = buffer + specialChars[rletter].ToString();

                }
            }


            using (MD5 md5hash = MD5.Create())
            {
                hash = GetMd5Hash(md5hash, buffer);
            }

            if (cbMd5.IsChecked == true) buffer = hash;
            return buffer;
            */

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

        private void btGenerate_Click(object sender, RoutedEventArgs e)
        {
            tbPreview.Text = Generate();
        }
    }
}
