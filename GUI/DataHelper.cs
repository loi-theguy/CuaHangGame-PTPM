using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Globalization;

namespace GUI
{
    public class DataHelper
    {
        
        public BitmapImage GetBitmapImage(string imageName)
        {
            Uri uri = new Uri("Images/" + imageName, UriKind.Relative);
            return new BitmapImage(uri);
        }

        public string ComputeHash(string input)
        {
            using (SHA256 hash = SHA256.Create())
            {
                byte[] rawOutput = hash.ComputeHash(Encoding.UTF8.GetBytes(input));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < rawOutput.Length; i++)
                    builder.Append(rawOutput[i].ToString("x2"));
                return builder.ToString();
            }
        }
        public bool IsPhoneNumberValid(string phoneNumber)
        {
            long t;
            if (!long.TryParse(phoneNumber, out t))
                return false;
            if (phoneNumber.Length != 10)
                return false;
            return true;
        }
        public bool IsEmailValid(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Normalize the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    string domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException e)
            {
                return false;
            }
            catch (ArgumentException e)
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }


    }
}
