using System.Security.Cryptography;
using System.Text;

namespace MTDBCreator.Helpers
{
    public static class HashHelper
    {
        public static string ComputeStringHash(string s)
        {
            var sb = new StringBuilder();

            foreach (var b in md5.ComputeHash(Encoding.UTF8.GetBytes(s)))
            {
                sb.Append(b.ToString("X2"));
            }

            return sb.ToString();
        }

        private static MD5 md5 = MD5.Create();
    }
}
