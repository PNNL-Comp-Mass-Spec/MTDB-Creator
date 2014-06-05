using System.Security.Cryptography;
using System.Text;

namespace MTDBCreator.Helpers
{
    public static class HashHelper
    {
        public static string ComputeStringHash(string s)
        {
            var sb = new StringBuilder();

            foreach (var b in Md5.ComputeHash(Encoding.UTF8.GetBytes(s)))
            {
                sb.Append(b.ToString("X2"));
            }

            return sb.ToString();
        }

        private static readonly MD5 Md5 = MD5.Create();
    }
}
