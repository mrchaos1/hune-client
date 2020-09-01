using System;
using System.Diagnostics.Tracing;
using System.Net;
using System.IO;
using System.ComponentModel;
using System.Configuration;
using System.Collections.Specialized;
using System.Xml;
using System.Web.UI;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using System.Runtime.Serialization;

using System.Runtime.InteropServices;
using System.Reflection;
using System.Reflection.Emit;
using System.Diagnostics;

namespace App
{
    public class DoorKey
    {
        public int Id { get; set; }
        public string SystemCode { get; set; }
        public string HotelCode  { get; set; }
        public int ComNumber { get; set; }
        public string Arrival { get; set; }
        public string Departure { get; set; }
        public int CardNumber { get; set; }
        public string Address { get; set; }
        public bool TerminateOld { get; set; }

        public string ErrorMessage { get; set; }
        public string IssuedAt { get; set; }

        public int nBlock { get; set; }
        public int Encrypt { get; set; }
        public string CardPass { get; set; }
        public string RoomPass { get; set; }
        public int LevelPass { get; set; }
        public int PassMode { get; set; }
        public int AddressMode { get; set; }
        public int AddressQty { get; set; }
        public int TimeMode { get; set; }

        public DoorKey()
        {
            this.nBlock = 4;
            this.Encrypt = 1;
            this.CardPass = "82A094FFFFFF";
            this.RoomPass = "111222";
            this.LevelPass = 3;
            this.PassMode = 1;
            this.AddressMode = 0;
            this.AddressQty = 1;
            this.TimeMode = 0;
        }
    }
}
