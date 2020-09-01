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
    public class Program
    {
        private static MercureClient client = new MercureClient();

        public static void Main(string[] args)
        {
            runRecursion();
            Console.WriteLine("Завершено.");
        }

        private static void runRecursion()
        {
            client.run();
            runRecursion();
        }
    }
}