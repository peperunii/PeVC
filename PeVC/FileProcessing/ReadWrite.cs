using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeVC.FileProcessing
{
    public static class ReadWrite
    {
        public static byte [] ReadFile(string filename)
        {
            try
            {
                return File.ReadAllBytes(filename);
            }
            catch(Exception)
            {
                return null;
            }
        }

        public static void WriteFile(byte[] data, string filename)
        {
            try
            {
                File.WriteAllBytes(filename, data);
            }
            catch (Exception)
            {
            }
        }

        public static List<string> GetSubDirFileInfo(string dir)
        {
            return
                Directory.GetFileSystemEntries(dir, "*", SearchOption.AllDirectories).ToList();
        }

        public static List<FileInfoTree> GetInfoForFileOrDir(string path, bool isFolder)
        {
            if (path.Length == 0) return new List<FileInfoTree>();

            if (isFolder)
            {
                return GetRelativePathInfoForDir(path);
            }
            else
            {
                if(!File.Exists(path))
                {
                    throw new InvalidOperationException("Trying to add a non existing file !");
                }
                else
                {
                    var name = Path.GetFileName(path);
                    var isDir = false;
                    var relative = string.Empty;

                    return new List<FileInfoTree>()
                    {
                        new FileInfoTree(Path.GetDirectoryName(path) + "\\", name, relative, isDir)
                    };
                }
            }
        }

        public static List<FileInfoTree> GetRelativePathInfoForDir(string dir)
        {
            if (dir.Length == 0) return new List<FileInfoTree>();
            if(dir[dir.Length -1] != '\\') dir += "\\";

            var allFilesAndDirs = GetSubDirFileInfo(dir);
            var fileInfo = new List<FileInfoTree>();
            var uriDir = new Uri(dir);

            foreach (var fileDir in allFilesAndDirs)
            {
                var name = Path.GetFileName(fileDir);
                var isDir = false;
                var relative = string.Empty;

                //Check if it is a dir or a file.
                if(Directory.Exists(fileDir))
                {
                    isDir = true;
                    relative = uriDir.MakeRelativeUri(new Uri(fileDir)).ToString();
                    relative = relative.Replace("%20", " ");
                    var substractedRelativeLength = relative.Length - name.Length;
                    if (substractedRelativeLength > 0)
                    {
                        relative = relative.Substring(0, substractedRelativeLength);
                    }
                    else
                    {
                        relative = string.Empty;
                    }
                    relative = relative.Replace("/", "\\");
                }
                else
                {
                    var dirname = Path.GetDirectoryName(fileDir) + "\\";
                    relative = uriDir.MakeRelativeUri(new Uri(dirname)).ToString();
                    relative = relative.Replace("%20", " ");
                    relative = relative.Replace("/", "\\");
                }

                fileInfo.Add(new FileInfoTree(dir, name, relative, isDir));
            }

            return fileInfo;
        }
    }
}
