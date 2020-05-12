using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wendy3Tests.Utils
{
    ///<summary>
    /// Resource operations
    ///</summary>
    public class Resource
    {
        ///<summary>
        /// Load resource file to a stirng
        ///</summary>
        ///<param name="fileName"></param>
        ///<returns></returns>
        public static string GetResourceAsString(string fileName)
        {
            var assembly = System.Reflection.Assembly.GetCallingAssembly();
            string name = assembly.GetName().Name;

            using (var reader = new StreamReader(assembly.GetManifestResourceStream(name + "." + fileName)))
            {
                string asString = reader.ReadToEnd();
                return asString;
            }
        }
    }
}
