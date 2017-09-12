using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Sql;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents;
using Newtonsoft.Json;
using System.Threading;
using System.Configuration;
using System.Diagnostics;

namespace UpdateCosmos
{
    class Program
    {
        static string _iotid;
        static double _temp;
        static long _timestamp;

        static void Main(string[] args)
        {
            DocumentDBRepository<IoTData>.Initialize();
            Random rnd = new Random();
            string[] iots = new string[] { "AA", "BB", "CC" };
            int ctr = 0;
            int min = 0;
            int max = 5;
            do
            {
                if (ctr > 2) ctr = 0;
                InsertData(rnd.Next(min, max), iots [ctr++]);

                Console.Write(".");

                ConsoleKeyInfo k =  Console.ReadKey();

                if( k.Key == ConsoleKey.UpArrow) //Temp up
                {
                    max++;
                    if (max > 100)
                        max = 100;
                }

                if (k.Key == ConsoleKey.DownArrow) //Temp down
                {
                    max--;
                    if (max < 5)
                        max = 5;
                }

                if (k.Key == ConsoleKey.Escape) //break
                {
                    break;
                }

            } while (true);

            return;
        }

        private async static void InsertData(double temp, String iotID)
        {

            _iotid = iotID;
            _temp =  temp;
            _timestamp = DateTime.UtcNow.Ticks;

            IoTData data = new IoTData
            {
                Id = Guid.NewGuid().ToString(),
                iotid = _iotid,
                temp = _temp,
                timestamp = _timestamp
            };

            await DocumentDBRepository<IoTData>.CreateItemAsync(data);
        }
    }
    public class IoTData
    {
        [JsonProperty("id")]
        public string Id;

        [JsonProperty("iotid")]
        public string iotid;

        [JsonProperty("temp")]
        public double temp;

        [JsonProperty("timestamp")]
        public long timestamp;

    }
}
