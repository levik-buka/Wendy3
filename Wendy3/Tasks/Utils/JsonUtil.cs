using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wendy.Tasks.Utils
{
    public static class JsonUtil
    {
        public static string SerializeToJson<T>(T pod)
        {
            return JsonConvert.SerializeObject(pod, Formatting.Indented, 
                new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        }

        public static T DeserializeJson<T>(string jsonDoc)
        {
            Contract.Requires(jsonDoc != null);
            
            return JsonConvert.DeserializeObject<T>(jsonDoc);

        }
    }
}
