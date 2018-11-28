using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace PeVC.Common
{
    public static class Init
    {
        public static void Start(bool isLocal = true)
        {
            // 1. Create hidden folder
            // 2.Create a subfolder for filesinfo and trackinfo
            // 3. add configuration file

            Init.IsInitiallized();

            DirectoryInfo di = Directory.CreateDirectory(Environment.CurrentDirectory + "\\" + Configuration.HiddenFolderNameRoot);
            di.Attributes = FileAttributes.Directory | FileAttributes.Hidden;

            Directory.CreateDirectory(Environment.CurrentDirectory + "\\" + Configuration.HiddenFolderNameRoot + "\\" + Configuration.HiddenSubfolderNameFileInfo);
            Directory.CreateDirectory(Environment.CurrentDirectory + "\\" + Configuration.HiddenFolderNameRoot + "\\" + Configuration.HiddenSubfolderNameTrackInfo);

            Configuration.Conf.Export();
        }

        //All PeVC functionality relies on initialized pevc folder - with a valid configuration.
        //Call this function in the beggining of each extension function.
        public static void IsInitiallized()
        {
            if (Directory.Exists(Configuration.HiddenFolderNameRoot))
            {
                Console.WriteLine("PeVC already initialilzed.\nTo remove tracking, type: \"pevc remove\"\nTo change db type: \"pevc set location\"");
            }
        }
    }
}
