using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Wendy.Tasks.Utils
{
    public class XML
    {
        public static T DeserializeXML<T>(string xmlDoc, string nameSpace)
        {
            Contract.Requires(xmlDoc != null);

            var doc = new XmlDocument();
            doc.LoadXml(xmlDoc);

            //var requestName = typeof(T).Name;
            //var requestNode = doc.SelectSingleNode($"//*[local-name() = '{requestName}']");
            //if (requestNode == null)
            //{
            //    throw new InvalidOperationException($"Request '{requestName}' was not found from SOAP-body");
            //}

            using (TextReader reader = new StringReader(doc.OuterXml))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T), nameSpace);
                T requestObj = (T)serializer.Deserialize(reader);
                return requestObj;
            }
        }
    }
}
