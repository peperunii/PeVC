//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PeVC
{
    using System;
    using System.Collections.Generic;
    
    public partial class fileinfo
    {
        public int id { get; set; }
        public string uuid { get; set; }
        public string filename { get; set; }
        public string relativepath { get; set; }
        public Nullable<double> timestamp { get; set; }
        public byte[] hash { get; set; }
        public Nullable<long> size { get; set; }
        public byte[] data { get; set; }
        public string compressiontype { get; set; }
        public Nullable<long> compressedsize { get; set; }
    }
}