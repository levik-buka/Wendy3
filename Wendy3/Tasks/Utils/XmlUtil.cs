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
    /// <summary>
    /// XML utilities
    /// </summary>
    public static class XmlUtil
    {
        /// <summary>
        /// Deserialize XML document
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xmlDoc"></param>
        /// <param name="nameSpace"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Deserialize XML from text reader
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xmlReader"></param>
        /// <param name="nameSpace"></param>
        /// <returns></returns>
        public static T DeserializeXML<T>(TextReader xmlReader, string nameSpace)
        {
            Contract.Requires(xmlReader != null);

            using (var reader = XmlReader.Create(xmlReader, new XmlReaderSettings()))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T), nameSpace);
                T requestObj = (T)serializer.Deserialize(reader);
                return requestObj;
            }
        }
    }
}
