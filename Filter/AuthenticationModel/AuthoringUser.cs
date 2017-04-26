using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;

namespace Filters.AuthenticationModel
{
    [Serializable]
    public class AuthoringUser:IIdentity
    {
        public Int32 Id { get; set; }
        public string Name { get; private set; }
        public string Username { get; set; }
        public int UserType { get; set; }
        public string AuthenticationType { get { return "BusTicketing"; } }
        public bool IsAuthenticated { get { return true; } }
        public AuthoringUser() { }
        public AuthoringUser(string name, Int32 userId, string Username, int UserType)
        {
            this.Name = name;
            this.Id = userId;
            this.Username = Username;
            this.UserType = UserType;
        }

        public AuthoringUser(string name, UserInfo userInfo)
            : this(name, userInfo.Id, userInfo.Username, userInfo.UserType)
        {
            this.Id = userInfo.Id;
        }
        public AuthoringUser(FormsAuthenticationTicket ticket)
            :this(ticket.Name,UserInfo.FromString(ticket.UserData))
        {
            if (ticket == null) throw new ArgumentNullException("ticket");
        }
    }
}
