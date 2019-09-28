using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace ChatOverflow.Utils
{
    public class RandomString
    {

        public delegate bool LoopUntilTrue(string randomString);

        public static string Generate(int length = 50, string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890")
        {
            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            {
                byte[] data = new byte[length];
                byte[] buffer = null;

                int maxRandom = byte.MaxValue - ((byte.MaxValue + 1) % chars.Length);

                crypto.GetBytes(data);
                char[] result = new char[length];

                for (int i = 0; i < length; i++)
                {
                    byte value = data[i];

                    while (value > maxRandom)
                    {
                        if (buffer == null)
                        {
                            buffer = new byte[1];
                        }

                        crypto.GetBytes(buffer);
                        value = buffer[0];
                    }

                    result[i] = chars[value % chars.Length];
                }

                return new string(result);
            }

        }

        public static string GenerateUntilTrue(LoopUntilTrue loop, int length = 50, string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890")
        {
            string randString = null;
            var res = false;
            do
            {
                randString = Generate(length, chars);
                res = loop(randString);
            } while (res == false);

            return randString;
        }
    }
}
