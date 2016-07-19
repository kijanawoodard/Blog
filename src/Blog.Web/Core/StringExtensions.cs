using System;
using System.Security.Cryptography;
using System.Text;

namespace Blog.Web.Core
{
    public static class StringExtensions
    {
        public static string ToMd5(this string input)
        {
            var asciiBytes = Encoding.ASCII.GetBytes(input);
            var hashedBytes = MD5.Create().ComputeHash(asciiBytes);
            var hashedString = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            return hashedString;
        }
    }
}