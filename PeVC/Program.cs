using PeVC.Common;
using PeVC.CompressionAlgos;
using PeVC.FileProcessing;
using PeVC.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeVC
{
    class Program
    {
        static void Main(string[] args)
        {
            ArgumentParser.Parse(args);
            Executer.StartProcessing();
        }
    }
}
