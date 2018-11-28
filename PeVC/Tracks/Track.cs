using PeVC.FileProcessing;
using PeVC.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeVC.Tracks
{
    public static class Track
    {
        public static void AddTrack(string path, string trackName = "")
        {
            Console.WriteLine("Adding '" + path + "' ...");
            var isFolder = Directory.Exists(path);

            var pathInfos = ReadWrite.GetInfoForFileOrDir(path, isFolder);

            if(pathInfos.Count > 0)
            {
                using (var dbContext = new DBEntities())
                {
                    var newTrack = new track();
                    newTrack.uuid = UUIDGenerator.GenerateNewID();
                    newTrack.location = isFolder ? path : Path.GetDirectoryName(path);
                    newTrack.trackname = trackName == string.Empty ? newTrack.uuid : trackName;

                    newTrack.uuidtrackinfo = UUIDGenerator.GenerateNewID();
                    dbContext.tracks.Add(newTrack);
                    
                    foreach (var pathInfo in pathInfos)
                    {
                        var fileUUID = UUIDGenerator.GenerateNewID();
                        var fileInfo = new fileinfo();
                        var trackinfo = new trackinfo();

                        trackinfo.uuid = newTrack.uuidtrackinfo;
                        trackinfo.uuidfile = fileUUID;

                        fileInfo.uuid = fileUUID;
                        fileInfo.timestamp = (DateTime.Now - pathInfo.LastModificationDate).TotalDays;
                        fileInfo.size = pathInfo.SizeInBytes;
                        fileInfo.relativepath = pathInfo.RelativePath;

                        fileInfo.hash = pathInfo.Hash;
                        fileInfo.filename = pathInfo.Filename;
                        fileInfo.data = pathInfo.GetCompressedData(CompressionAlgos.CompressionType.LZ4);
                        fileInfo.compressedsize = fileInfo.data.Length;
                        fileInfo.compressiontype = pathInfo.CompressionType.ToString();

                        dbContext.trackinfoes.Add(trackinfo);
                        dbContext.fileinfoes.Add(fileInfo);
                    }

                    dbContext.SaveChanges();
                }

                Console.WriteLine("'" + path + "' is now tracked");
            }
            else
            {
                Console.WriteLine("Unable to track '" + path + "'");
            }
        }
    }
}
