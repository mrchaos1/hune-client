using System;
using System.Diagnostics.Tracing;
using System.Net;
using System.IO;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Configuration;
using System.Collections.Specialized;
using System.Xml;
using System.Web.UI;
using System.Web.Script.Serialization;

namespace App
{
    public class Program
    {
        const string configPath = "config.xml";

        private static WebClient client = new WebClient();
        private static String url = "http://localhost:3000/.well-known/mercure?topic=test";
        private static JavaScriptSerializer serializer = new JavaScriptSerializer();

        public static void Main(string[] args)
        {
            if (!File.Exists(configPath)) {
                Console.WriteLine("Ошибка. Создайте файл конфигруации config.xml в корне программы");
                Console.ReadLine();
            }

            string readText = File.ReadAllText(configPath);

            using (Stream stream = client.OpenRead(url))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    string line = null;
                    while (null != (line = reader.ReadLine()))
                    {
                        var firstSeparator = line.IndexOf(":");

                        if (!(firstSeparator > 1)) {
                           continue;
                        }

                        var firstPart = line.Substring(0, firstSeparator);

                        if (firstPart != "data") {
                           continue;
                        }

                        string secondPart = line.Substring(firstSeparator + 1);
                        var jsonContents = secondPart.Trim();

                        DoorKey doorKey = serializer.Deserialize<DoorKey>(jsonContents);

                        Console.WriteLine(jsonContents);
                        Console.WriteLine("System code: " + doorKey.SystemCode);
                        Console.WriteLine("Hotel code: " + doorKey.HotelCode);
                        Console.WriteLine("COM number: " + doorKey.ComNumber);
                        Console.WriteLine("Arrival: " + doorKey.Arrival);
                        Console.WriteLine("Departure: " + doorKey.Departure);
                    }
                }
            }
            Console.WriteLine("Stopped!");
        }
    }

    public class DoorKey
    {
        public string SystemCode { get; set; }
        public string HotelCode  { get; set; }
        public int ComNumber { get; set; }
        public string Arrival { get; set; }
        public string Departure  { get; set; }
    }
}