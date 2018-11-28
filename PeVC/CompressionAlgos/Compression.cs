using LZ4;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeVC.CompressionAlgos
{
    public static class Compression
    {
        public static byte[] Compress(byte[] data, CompressionType algoType)
        {
            switch(algoType)
            {
                case CompressionType.DotNet:
                    return CompressionDotNet(data);

                case CompressionType.Zip:
                    return Compression7Zip(data);

                case CompressionType.LZ4:
                    return CompressionLZ4(data);
            }

            return null;
        }

        public static byte[] Decompress(byte[] data, CompressionType algoType)
        {
            switch (algoType)
            {
                case CompressionType.DotNet:
                    return DecompressionDotNet(data);
                    
                case CompressionType.Zip:
                    return Decompression7Zip(data);

                case CompressionType.LZ4:
                    return DecompressionLZ4(data);
            }

            return null;
        }

        //METHODS:
        //DotNet
        private static byte[] CompressionDotNet(byte[] data)
        {
            MemoryStream output = new MemoryStream();
            using (DeflateStream dstream = new DeflateStream(output, CompressionLevel.Optimal))
            {
                dstream.Write(data, 0, data.Length);
            }
            return output.ToArray();
        }

        private static byte[] DecompressionDotNet(byte[] data)
        {
            MemoryStream input = new MemoryStream(data);
            MemoryStream output = new MemoryStream();
            using (DeflateStream dstream = new DeflateStream(input, CompressionMode.Decompress))
            {
                dstream.CopyTo(output);
            }
            return output.ToArray();
        }

        //7Zip
        private static byte[] Compression7Zip(byte[] data)
        {
            return SevenZip.Compression.LZMA.SevenZipHelper.Compress(data);
        }

        private static byte[] Decompression7Zip(byte[] data)
        {
            MemoryStream input = new MemoryStream(data);
            MemoryStream output = new MemoryStream();
            using (DeflateStream dstream = new DeflateStream(input, CompressionMode.Decompress))
            {
                dstream.CopyTo(output);
            }
            return output.ToArray();
        }

        //LZ4
        private static byte[] CompressionLZ4(byte[] data)
        {
            return LZ4Codec.Wrap(data);
        }

        private static byte[] DecompressionLZ4(byte[] data)
        {
            return LZ4Codec.Unwrap(data);
        }
    }
}
