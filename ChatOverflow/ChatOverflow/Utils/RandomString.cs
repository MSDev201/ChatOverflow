using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace ChatOverflow.Utils
{
    public class RandomString
    {

        public static string Generate(int length = 50)
        {
            var randomString = string.Empty;
            using (var rng = new RNGCryptoServiceProvider())
            {
                var bit_count = (length * 6);
                var byte_count = ((bit_count + 7) / 8); // rounded up
                var bytes = new byte[byte_count];
                rng.GetBytes(bytes);
                randomString = Convert.ToBase64String(bytes);
            }

            if (string.IsNullOrEmpty(randomString))
                return null;

            return randomString;
        }

    }
}
