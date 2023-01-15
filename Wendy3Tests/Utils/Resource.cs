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
        ///<returns>empty string if no resource found</returns>
        public static string GetResourceAsString(string fileName)
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetCallingAssembly();
            
            string name = assembly.GetName().Name ?? string.Empty;
            Stream? resourceStream = assembly.GetManifestResourceStream(name + "." + fileName);

            if (resourceStream != null)
            {
                using (var reader = new StreamReader(resourceStream))
                {
                    string asString = reader.ReadToEnd();
                    return asString;
                }
            }

            return string.Empty;
        }
    }
}
