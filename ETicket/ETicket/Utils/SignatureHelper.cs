using System.Security.Cryptography;
using System.Text;

namespace ETicket.Utils
{
    public static class SignatureHelper
    {
        public static string GetSignature(string data, string password)
        {
            var inputString = string.Concat(data, password);

            var md5ResString = GetMD5Hash(inputString);
            var sha1ResString = GetSha1Hash(md5ResString);

            return sha1ResString;
        }

        private static string GetMD5Hash(string input)
        {
            using (var md5 = MD5.Create())
            {
                var inputBytes = Encoding.ASCII.GetBytes(input);
                var hashBytes = md5.ComputeHash(inputBytes);

                var builder = new StringBuilder();
                for (var i = 0; i < hashBytes.Length; i++)
                    builder.Append(hashBytes[i].ToString("X2"));

                return builder.ToString().ToLower();
            }
        }

        private static string GetSha1Hash(string input)
        {
            var bytes = Encoding.UTF8.GetBytes(input);
            var sha1 = SHA1.Create();
            var hashBytes = sha1.ComputeHash(bytes);

            return HexStringFromBytes(hashBytes);
        }

        private static string HexStringFromBytes(byte[] bytes)
        {
            var builder = new StringBuilder();
            foreach (var b in bytes)
            {
                var hex = b.ToString("x2");
                builder.Append(hex);
            }

            return builder.ToString();
        }
    }
}