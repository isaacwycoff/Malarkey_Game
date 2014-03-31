using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace Malarkey
{
    class SettingsManager
    {
        private static SettingsManager manager = new SettingsManager();

        public static SettingsManager GetInstance()
        {
            return manager;
        }

        public SettingsManager()
        {
             StringBuilder sb = new StringBuilder();
             StringWriter sw = new StringWriter(sb);
 
             using (JsonWriter writer = new JsonTextWriter(sw))
             {
                writer.Formatting = Formatting.Indented;
 
                writer.WriteStartObject();
                writer.WritePropertyName("CPU");
                writer.WriteValue("Intel");
                writer.WritePropertyName("PSU");
                writer.WriteValue("500W");
                writer.WritePropertyName("Drives");
                writer.WriteStartArray();
                writer.WriteValue("DVD read/writer");
                writer.WriteComment("(broken)");
                writer.WriteValue("500 gigabyte hard drive");
                writer.WriteValue("200 gigabype hard drive");
                writer.WriteEnd();
                writer.WriteEndObject();
            }

            /*
            using (StreamReader reader = File.OpenText(@"c:\settings.json"))
            {
                JObject o = (JObject)
            //    JObject o = (JObject)JToken.ReadFrom(new JsonTextReader(reader));
                // do stuff
            }
            */

             string json = @"{
               'CPU': 'Intel',
               'PSU': '500W',
               'Drives': [
                 'DVD read/writer'
                 /*(broken)*/,
                 '500 gigabyte hard drive',
            '200 gigabype hard drive'   ]
            }";

            JsonTextReader reader = new JsonTextReader(new StringReader(json));
            while (reader.Read())
            {
                if (reader.Value != null)
                    Console.WriteLine("Token: {0}, Value: {1}", reader.TokenType, reader.Value);
                else
                    Console.WriteLine("Token: {0}", reader.TokenType);
            }
            
//            Console.WriteLine(sb.ToString());

            String output = sb.ToString();
        }
    }
}
