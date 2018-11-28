using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeVC.Utils
{
    public static class UUIDGenerator
    {
        public static string GenerateNewID()
        {
            var format = "{0}-{1}-{2}-{3}-{4}";
            return string.Format(
                format,
                RandomString(8),
                RandomString(4),
                RandomString(4),
                RandomString(4),
                RandomString(12));
        }

        private static Random random = new Random();
        private static string RandomString(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
