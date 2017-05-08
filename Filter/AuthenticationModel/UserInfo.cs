using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Filters.AuthenticationModel;
using System.Xml.Serialization;
using System.IO;

namespace Filters.AuthenticationModel
{
    public class UserInfo
    {
        public Int32 Id { get; set; }      
        public string Username { get; set; }        
        public int UserType { get; set; }
        public override string ToString()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(UserInfo));
            using (var stream = new System.IO.StringWriter())
            {
                serializer.Serialize(stream, this);
                return stream.ToString();
            }
        }
        public static UserInfo FromString(string userContextData)
        {

            XmlSerializer serializer = new XmlSerializer(typeof(UserInfo));
            using(var stream = new StringReader(userContextData))            {
                return serializer.Deserialize(stream) as UserInfo;
            }
        }
    }
}
