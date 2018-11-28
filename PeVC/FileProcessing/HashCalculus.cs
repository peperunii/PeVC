using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PeVC.FileProcessing
{
    public static class HashCalculus
    {
        public static byte [] GetHash(byte [] dataInput)
        {
            using (var hashAlgorithm = SHA256.Create())
            {
                // Convert the input string to a byte array and compute the hash.
                return hashAlgorithm.ComputeHash(dataInput);
            }
        }
    }
}
