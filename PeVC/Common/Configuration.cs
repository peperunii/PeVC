using PeVC.CompressionAlgos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeVC.Common
{
    public class Config
    {
        public CompressionType ComppressionType { get; set; }
        public RepoLocation RepositoryLocation { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string RemotePath { get; set; }
        public string DBName { get; set; }
        public string RootPath { get; set; }

        public Config(string rootDir)
        {
            ComppressionType = CompressionType.LZ4;
            RepositoryLocation = RepoLocation.LocalFS;
            Username = string.Empty;
            Password = string.Empty;
            RemotePath = string.Empty;
            DBName = string.Empty;
            RootPath = rootDir;
        }

        public void Export()
        {
            var confInfo = new List<string>();
            confInfo.Add("ComppressionType: " + this.ComppressionType);
            confInfo.Add("RepositoryLocation: " + this.RepositoryLocation);
            confInfo.Add("Username: " + this.Username);
            confInfo.Add("Password: " + this.Password);
            confInfo.Add("RemotePath: " + this.RemotePath);
            confInfo.Add("DBName: " + this.DBName);

            File.WriteAllLines(this.RootPath + "\\.conf", confInfo);
        }

        public void Import()
        {

        }
    }

    public static class Configuration
    {
        
        public static string TrackedItem;
        public static bool IsFolder;
        public static CommandType Command;
        public static string HiddenFolderNameRoot;
        public static string HiddenSubfolderNameTrackInfo;
        public static string HiddenSubfolderNameFileInfo;
        public static string ConfigfileName;
        public static string HistfileName;
        public static string LogfileName;
        public static string DestinationFolderPath;

        public static string RootRelativePath = string.Empty;
        public static bool NoTrack = false;
        public static string Name = string.Empty;
        public static string Message = string.Empty;

        public static Config Conf;

        static Configuration()
        {
            HiddenFolderNameRoot = ".pevc";
            HiddenSubfolderNameTrackInfo = "trackInfo";
            HiddenSubfolderNameFileInfo = "fileinfo";
            ConfigfileName = ".config";
            HistfileName = ".hist";
            LogfileName = ".log";
            DestinationFolderPath = ".";

            Conf = new Config(Configuration.FindRootAbsolutePath(Environment.CurrentDirectory));
        }

        public static string FindRootAbsolutePath(string currentDir)
        {
            while (true)
            {
                try
                {
                    var directories = Directory.GetDirectories(currentDir);
                    var prefixes = string.Empty;

                    foreach(var directory in directories)
                    {
                        if (Path.GetFileName(directory) == Configuration.HiddenFolderNameRoot)
                        {
                            return Path.GetFullPath(prefixes + directory);
                        }
                    }

                    currentDir = Directory.GetParent(currentDir).FullName;
                    prefixes += "..\\";
                }
                catch(Exception)
                {
                    throw new Exception("Unable to find pevc root dir. You should init first.");
                }
            }
        }

        internal static void AddToHist(string userRequest)
        {
            var histFilename = Conf.RootPath + "\\" + Configuration.HistfileName;

            var formattedRequest = string.Format("[{0}] {1}\n", DateTime.Now, userRequest);
            File.AppendAllText(histFilename, formattedRequest);
        }
    } 
}
