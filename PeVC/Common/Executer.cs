using PeVC.CompressionAlgos;
using PeVC.FileProcessing;
using PeVC.Tracks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeVC.Common
{
    public static class Executer
    {
        public static void StartProcessing()
        {
            Console.WriteLine(Configuration.Command);
            switch(Configuration.Command)
            {
                case CommandType.Init:
                    Init.Start();
                    break;

                case CommandType.Track:
                    Track.AddTrack(Configuration.TrackedItem, Configuration.Name);
                    break;

                case CommandType.Status:
                    //Get all trakced files and their hashes and compare to a current list of files and dirs
                    var currentStatus = ReadWrite.GetRelativePathInfoForDir(Configuration.Conf.RootPath);

                    break;

                case CommandType.History:
                    var histFilename = Configuration.Conf.RootPath + "\\" + Configuration.HistfileName;
                    var histLines = File.ReadAllLines(histFilename);
                    
                    foreach(var line in histLines)
                    {
                        Console.WriteLine(line);
                    }

                    break;

                case CommandType.Checkout:
                    //Two things may happen with this command:
                    // - checkout a file or folder to an earlier stage
                    // - Copy track info to a new location

                    //Option 1
                    if(Directory.Exists(Configuration.TrackedItem) ||
                        File.Exists(Configuration.TrackedItem))
                    {

                    }
                    //Option 2
                    else
                    {
                        var trackId = Configuration.Name;
                        using (var dbContext = new DBEntities())
                        {
                            var track = (from t in dbContext.tracks
                                             where t.trackname == trackId || t.uuid == trackId
                                             select t).FirstOrDefault();
                            if(track != null)
                            {
                                var fileInfosUUIDs = from t in dbContext.trackinfoes
                                                     where t.uuid == track.uuidtrackinfo
                                                     select t.uuidfile;
                                var filesInfo = from t in dbContext.fileinfoes
                                                where fileInfosUUIDs.Contains(t.uuid)
                                                select t;

                                var mainDir = Configuration.DestinationFolderPath;
                                foreach(var fileInfo in filesInfo)
                                {
                                    var relativePath = fileInfo.relativepath;
                                    if (relativePath != string.Empty)
                                    {
                                        var relationsSplits = relativePath.Split('\\');

                                        var dirToCreate = mainDir + "\\";
                                        for (int i = 0; i < relationsSplits.Count(); i++)
                                        {
                                            dirToCreate += (relationsSplits[i] + "\\");
                                            Directory.CreateDirectory(dirToCreate);
                                        }
                                    }

                                    var compressionType = (CompressionType)Enum.Parse(typeof(CompressionType), fileInfo.compressiontype);
                                    var decompressedData = Compression.Decompress(fileInfo.data, compressionType);

                                    File.WriteAllBytes(mainDir + fileInfo.relativepath + "\\" + fileInfo.filename, decompressedData);
                                }

                                if(!Configuration.NoTrack)
                                {
                                    Track.AddTrack(mainDir, track.trackname);
                                }
                            }
                            else
                            {
                                Console.WriteLine("No Folder/File/Track info found");
                            }
                        }
                    }

                    break;
            }
        }
    }
}
