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
    public class MercureClient
    {
        private static WebClient webClient = new WebClient();
        private static JavaScriptSerializer serializer = new JavaScriptSerializer();
        private static KeyIssuer keyIssuer = new KeyIssuer();

        private AppConfig config;

        public MercureClient()
        {
            config = getConfig();
        }

        public void run()
        {
            Console.WriteLine("Клиент запущен: " + config.MercureURL);

            using (Stream stream = webClient.OpenRead(config.MercureURL))
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
                        string jsonContents = secondPart.Trim();

                        DoorKey doorKey = serializer.Deserialize<DoorKey>(jsonContents);
                        Console.WriteLine("Получен новый ключ: " + jsonContents);

                        keyIssuer.issue(doorKey);
                    }
                }
            }
        }

        private static AppConfig getConfig()
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(AppConfig));

            string buildDir = new FileInfo(Assembly.GetEntryAssembly().Location).Directory.ToString();
            string configPath = Path.Combine(new string[] {buildDir, "config.xml"});

            if (!File.Exists(configPath))
            {
                Console.WriteLine("Ошибка. Создайте файл конфигруации " + configPath);
                Console.ReadLine();
            }

            using (FileStream fs = new FileStream(configPath, FileMode.OpenOrCreate))
            {
                AppConfig config = (AppConfig)xmlSerializer.Deserialize(fs);
                return config;
            }
        }

        public void keyConfirm(DoorKey key)
        {
            try
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();

                string serializedResult = serializer.Serialize(key);
                string url = config.ServerURL + "/hune/key/" + key.Id + "/confirm";

                WebClient client = new WebClient();
                client.Headers.Add("Content-Type", "application/json");
                client.Headers.Add("X-AUTH-TOKEN", config.Token);

                client.Encoding = System.Text.Encoding.UTF8;
                string reply = client.UploadString(url, "PUT", serializedResult);

            }
            catch (Exception err)
            {
                Console.WriteLine("Ошибка при отправке запроса: " + err.Message);
            }
        }
    }
}
