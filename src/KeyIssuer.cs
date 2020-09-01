using System;
using System.Diagnostics.Tracing;
using System.Text;
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
    public class KeyIssuer
    {
        [DllImport("kernel32.dll")]
        public static extern IntPtr LoadLibrary(string lpFileName);

        [DllImport("hunerf.dll", SetLastError = true)]
        public static extern int KeyCard(int Com, int CardNo, int nBlock, int Encrypt, string CardPass, string SystemCode, string HotelCode, string Pass, string Address, string SDIn,
        string STIn, string SDOut, string STOut, int LEVEL_Pass, int PassMode, int AddressMode, int AddressQty, int TimeMode, int V8, int V16, int V24, int AlwaysOpen, int OpenBolt, int TerminateOld, int ValidTimes);

        [DllImport("hunerf.dll", SetLastError = true)]
        public static extern int ReadCardSN(int Com, StringBuilder CardSN);
        private MercureClient client = new MercureClient();


        public void issue(DoorKey doorKey)
        {
            DateTime arrivalDateTime = DateTime.Parse(doorKey.Arrival);
            DateTime departureDateTime = DateTime.Parse(doorKey.Departure);

            int Com = doorKey.ComNumber;
            int CardNumber = doorKey.CardNumber;
            int nBlock = doorKey.nBlock;
            int Encrypt = doorKey.Encrypt;

            string CardPass = doorKey.CardPass;
            string RoomPass = doorKey.RoomPass;
            string SystemCode = doorKey.SystemCode;
            string HotelCode = doorKey.HotelCode;
            string Address = doorKey.Address;
            string DTPSDInVar = arrivalDateTime.ToString("yy-MM-dd");
            string DTPSTInVar = arrivalDateTime.ToString("H:mm:ss");
            string DTPSDOutVar = departureDateTime.ToString("yy-MM-dd");
            string DTPSTOutVar = departureDateTime.ToString("H:mm:ss");
            int LevelPass = doorKey.LevelPass;
            int PassMode = doorKey.PassMode;
            int AddressMode = doorKey.AddressMode;
            int AddressQty = doorKey.AddressQty;

            int TimeMode = 0;
            int V8 = 255;
            int V16 = 255;
            int V24 = 255;
            int AlwaysOpen = 0;
            int OpenBolt = 0;
            int TerminateOld = 0;

            if (doorKey.TerminateOld) {
                TerminateOld = 1;
            }

            RoomPass = GoDateTime(DateTime.Now);

            int ValidTimes = 255;

            Console.WriteLine("Com: " + Com);
            Console.WriteLine("RoomPass: " + RoomPass);
            Console.WriteLine("IN: " + DTPSDInVar + " " + DTPSTInVar);
            Console.WriteLine("OUT: " + DTPSDOutVar + " " + DTPSTOutVar);

            try
            {
                int Ret = KeyCard(
                     Com,
                     CardNumber,
                     nBlock,
                     Encrypt,
                     CardPass,
                     SystemCode,
                     HotelCode,
                     RoomPass,
                     Address,
                     DTPSDInVar,
                     DTPSTInVar,
                     DTPSDOutVar,
                     DTPSTOutVar,
                     LevelPass,
                     PassMode,
                     AddressMode,
                     AddressQty,
                     TimeMode,
                     V8,
                     V16,
                     V24,
                     AlwaysOpen,
                     OpenBolt,
                     TerminateOld,
                     ValidTimes
                 );

                if (Ret == 0)
                {
                    doorKey.IssuedAt = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                    Console.WriteLine("Write data successfully!");
                }
                else
                {
                    doorKey.ErrorMessage = "Ошибка библиотеки при записи ключа";
                }
            }
            catch (Exception err)
            {
                doorKey.ErrorMessage = "Ошибка библиотеки: " + err.Message;

            }

            if (doorKey.ErrorMessage != null) {
                Console.WriteLine(doorKey.ErrorMessage);
            }

            client.keyConfirm(doorKey);
        }

        private static string GoDateTime(DateTime DTVar)
        {
            long DW1 = 0;
            string DW2 = "";
            long Year = DTVar.Year;
            long Month = DTVar.Month;
            long Day = DTVar.Day;
            long Hour = DTVar.Hour;
            long Min = DTVar.Minute;
            long Sec = DTVar.Second;

            DW1 = (Min / 4) + (Hour << 4) + (Day << 9) + (Month << 14) + ((((Year - 8) % 1000) % 63) << 18);
            DW2 = Convert.ToString(DW1, 16);

            return DW2.ToUpper();
        }
    }
}


