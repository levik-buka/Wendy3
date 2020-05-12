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
    public static class XmlUtil
    {
        public static T DeserializeXML<T>(string xmlDoc, string nameSpace)
        {
            Contract.Requires(xmlDoc != null);

            using (var reader = XmlReader.Create(new StringReader(xmlDoc), new XmlReaderSettings()))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T), nameSpace);
                T requestObj = (T)serializer.Deserialize(reader);
                return requestObj;
            }
        }
    }
}
