using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wendy.Tasks.Utils
{
    /// <summary>
    /// JSON utilities
    /// </summary>
    public static class JsonUtil
    {
        /// <summary>
        /// Serialize object to JSON
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pod"></param>
        /// <returns></returns>
        public static string SerializeToJson<T>(T pod)
        {
            return JsonConvert.SerializeObject(pod, Formatting.Indented, 
                new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        }

        /// <summary>
        /// Deserialize JSON string to object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonDoc"></param>
        /// <returns></returns>
        public static T DeserializeJson<T>(string jsonDoc)
        {
            Contract.Requires(jsonDoc != null);
            
            return JsonConvert.DeserializeObject<T>(jsonDoc);

        }

        /// <summary>
        /// Deserialize JSON from text reader
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonReader"></param>
        /// <returns></returns>
        public static T DeserializeJson<T>(System.IO.TextReader jsonReader)
        {
            Contract.Requires(jsonReader != null);

            // deserialize JSON directly from a file
            using (JsonReader file = new JsonTextReader(jsonReader))
            {
                JsonSerializer serializer = new JsonSerializer();
                return serializer.Deserialize<T>(file);
            }
        }
    }
}
