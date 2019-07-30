using System;
using System.Security.Cryptography;

namespace Bardez.Projects.ReusableCode
{
    public static class ArrayExtensions
    {
        /// <summary>Returns a string representing the SHA256 hash of the byte array.</summary>
        /// <param name="input"></param>
        /// <remarks>
        /// Based on http://stackoverflow.com/a/14709940/103058 and http://stackoverflow.com/a/1183291/103058
        /// </remarks>
        public static string GetSha256Hash(this Byte[] input)
        {
            SHA256Managed crypt = new SHA256Managed();
            String hash = String.Empty;
            Byte[] crypto = crypt.ComputeHash(input);
            foreach (Byte theByte in crypto)
            {
                hash += theByte.ToString("x2");
            }

            return hash;
        }

        /// <summary>Similar to <see cref="String.IsNullOrEmpty" />, but on <see cref="Array" /> types</summary>
        /// <typeparam name="TType">The type of the <see cref="Array" /></typeparam>
        /// <param name="array"><see cref="Array" /> of type <typeparamref name="TType" /> to interrogate</param>
        /// <returns>True on null or no items</returns>
        public static Boolean IsNullOrEmpty<TType>(this TType[] array)
        {
            return array == null || array.Length == 0;
        }

        /// <summary>Similar to <see cref="String.IsNullOrEmpty" />, but on <see cref="Array" /> types</summary>
        /// <typeparam name="TType">The type of the <see cref="Array" /></typeparam>
        /// <param name="array"><see cref="Array" /> of type <typeparamref name="TType" /> to interrogate</param>
        /// <returns>True on null or no items</returns>
        public static Boolean IsNullOrBlank<TType>(this TType[] array)
        {
            Boolean wasFoundEmpty = array.IsNullOrEmpty();
            if (!wasFoundEmpty)
            {
                Boolean allNull = true;
                foreach (TType element in array)
                {
                    if (element != null)
                    {
                        allNull = false;
                        break;
                    }
                }

                wasFoundEmpty = allNull;
            }

            return wasFoundEmpty;
        }
    }
}