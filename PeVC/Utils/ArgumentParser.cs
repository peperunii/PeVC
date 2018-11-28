using PeVC.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeVC.Utils
{
    public static class ArgumentParser
    {
        internal static void Parse(string[] args)
        {
            try
            {
                var userRequest = string.Join(" ", args);
                Configuration.AddToHist(userRequest);

                for (int i = 0; i < args.Length; i++)
                {
                    switch (args[i].ToLower())
                    {
                        /*COMMANDS*/
                        case "init":
                            Configuration.Command = CommandType.Init;
                            break;

                        case "add":
                        case "track":
                            Configuration.TrackedItem = GetPathFromArgs(args, i);
                            Configuration.IsFolder = Directory.Exists(Configuration.TrackedItem);
                            Configuration.Command = CommandType.Track;
                            break;

                        case "log":
                            Configuration.TrackedItem = GetPathFromArgs(args, i);
                            Configuration.IsFolder = Directory.Exists(Configuration.TrackedItem);
                            Configuration.Command = CommandType.Log;
                            break;

                        case "status":
                        case "info":
                            Configuration.Command = CommandType.Status;
                            break;

                        case "-cm":
                        case "commit":
                        case "update":
                            break;

                        case "-cho":
                        case "checkout":
                            Configuration.TrackedItem = GetPathFromArgs(args, i);
                            Configuration.Name = args[i + 1];
                            Configuration.IsFolder = Directory.Exists(Configuration.TrackedItem);
                            Configuration.Command = CommandType.Checkout;
                            break;

                        case "hist":
                            Configuration.Command = CommandType.History;
                            break;


                        /*PARAMS*/
                        case "-d":
                        case "into":
                        case "dest":
                            Configuration.DestinationFolderPath = GetPathFromArgs(args, i);
                            break;

                        case "-nt":
                        case "notrack":
                            Configuration.NoTrack = true;
                            break;

                        case "-n":
                        case "name":
                            Configuration.Name = args[i + 1];
                            break;

                        case "-m":
                        case "message":
                            break;

                        case "set": //used to set all parameters from the config file.
                            var parameter = args[i + 1];
                            var value = args[i + 2];

                            if(parameter == "location")
                            {

                            }
                            if(parameter == "username")
                            {

                            }
                            if (parameter == "password")
                            {

                            }
                            if (parameter == "compression")
                            {

                            }
                            if (parameter == "remote")
                            {

                            }
                            if (parameter == "dbname")
                            {

                            }
                            break;
                            
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Incorrect argument list.");
            }
        }

        internal static string GetPathFromArgs(string[] args, int currentIndex)
        {
            var value = args[currentIndex + 1];

            if (value.Contains('\"'))
            {
                for (int ii = currentIndex + 2; ii < args.Count(); ii++)
                {
                    value += " ";
                    value += args[ii];
                    if (args[ii].Contains("\""))
                    {
                        break;
                    }
                }
            }

            return Path.GetFullPath(value);
        }
    }
}
