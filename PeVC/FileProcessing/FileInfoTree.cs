using LZ4;
using PeVC.CompressionAlgos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeVC.FileProcessing
{
    public class FileInfoTree
    {
        public string Filename { get; set; }
        public string RelativePath { get; set; }
        public bool IsDirectory { get; set; }
        public byte [] Hash { get; set; }
        public int SizeInBytes { get; set; }
        public bool IsTracked { get; set; }
        private string RelativeToDir { get; set; }
        public DateTime LastModificationDate { get; set; }
        public CompressionType CompressionType { get; set; }

        public FileInfoTree(
            string relativeToDir,
            string filename, 
            string relativePath, 
            bool isDirectory)
        {
            this.RelativeToDir = relativeToDir;
            this.Filename = filename;
            this.RelativePath = relativePath;
            this.IsDirectory = isDirectory;
            this.IsTracked = true;

            if (!this.IsDirectory)
            {
                try
                {
                    this.LastModificationDate = File.GetLastWriteTimeUtc(this.GetFullFilename());
                    var fileBytes = File.ReadAllBytes(this.GetFullFilename());
                    this.Hash = HashCalculus.GetHash(fileBytes);
                    this.SizeInBytes = fileBytes.Length;
                }
                catch(Exception)
                {
                    //Remove this file from index
                    this.IsTracked = false;
                }
            }
            else
            {
                this.LastModificationDate = Directory.GetLastWriteTimeUtc(this.GetFullFilename());
            }
        }

        public string GetFullFilename(string path)
        {
            return
                path + "\\" + this.RelativePath + this.Filename;
        }

        public string GetFullFilename()
        {
            return
                this.RelativeToDir + this.RelativePath + this.Filename;
        }

        public byte[] GetCompressedData(CompressionType compressionType = CompressionType.DotNet)
        {
            this.CompressionType = compressionType;

            if (this.IsDirectory) return new byte[0];
            var fileBytes = File.ReadAllBytes(this.GetFullFilename());
            var compressedBytes = Compression.Compress(fileBytes, compressionType);

            return compressedBytes;
        }
    }
}
