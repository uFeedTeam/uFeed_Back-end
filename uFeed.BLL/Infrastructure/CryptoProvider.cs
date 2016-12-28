using System;
using System.Security.Cryptography;
using System.Text;

namespace uFeed.BLL.Infrastructure
{
    public static class CryptoProvider
    {
        public static string GetMd5Hash(string plaintext)
        {
            var md5Provider = new MD5CryptoServiceProvider();
            var hasedvalue = md5Provider.ComputeHash(Encoding.Default.GetBytes(plaintext));
            var str = new StringBuilder();
            for (int counter = 0; counter < hasedvalue.Length; counter++)
            {
                str.Append(hasedvalue[counter].ToString("x2"));
            }

            return str.ToString();
        }

        public static bool VerifyMd5Hash(string plainText, string prevhashedvalue)
        {
            var hashedvalue2 = GetMd5Hash(plainText);

            var strcomparer = StringComparer.OrdinalIgnoreCase;

            if (strcomparer.Compare(hashedvalue2, prevhashedvalue).Equals(0))
            {
                return true;
            }

            return false;
        }
    }
}
