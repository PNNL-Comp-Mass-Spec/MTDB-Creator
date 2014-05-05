using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace MTDBCreator.Helpers
{
    public static class HashHelper
    {
        public static string ComputeStringHash(string s)
        {
            StringBuilder sb = new StringBuilder();

            foreach (byte b in md5.ComputeHash(Encoding.UTF8.GetBytes(s)))
            {
                sb.Append(b.ToString("X2"));
            }

            return sb.ToString();
        }

        private static MD5 md5 = MD5.Create();
    }
}
