<<<<<<< HEAD
﻿using System;
=======
﻿
using System;
>>>>>>> 26ef721075f7daf65910c438cea0051f4b8a7e75
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
<<<<<<< HEAD
        public Int32 Id { get; set; }
        public string Username { get; set; }
        public int UserType { get; set; }

=======
        public Int32 Id { get; set; }      
        public string Username { get; set; }        
        public int UserType { get; set; }
     
>>>>>>> 26ef721075f7daf65910c438cea0051f4b8a7e75
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
<<<<<<< HEAD
            using (var stream = new StringReader(userContextData))
=======
            using(var stream = new StringReader(userContextData))
>>>>>>> 26ef721075f7daf65910c438cea0051f4b8a7e75
            {
                return serializer.Deserialize(stream) as UserInfo;
            }
        }
    }

<<<<<<< HEAD

=======
    
>>>>>>> 26ef721075f7daf65910c438cea0051f4b8a7e75

}
