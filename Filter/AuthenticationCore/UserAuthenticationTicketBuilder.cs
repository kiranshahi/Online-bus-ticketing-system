using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;
using Filters.AuthenticationModel;
using Entities.Models;

namespace Filters.AuthenticationCore
{
    public class UserAuthenticationTicketBuilder
    {
        public static UserInfo CreateUserContextFromUser(User user)
        {
            var userContext = new UserInfo
            {
                Id = user.User_Id,
                Username = user.Username,
                UserType = user.UserRole
            };
            return userContext;
        }

        public static FormsAuthenticationTicket CreateAuthenticationTicket(User user)
        {
            UserInfo userInfo = CreateUserContextFromUser(user);
            var ticket = new FormsAuthenticationTicket(
                1,
               // user.Emailid,
               user.Username,
                DateTime.Now,
                DateTime.Now.Add(FormsAuthentication.Timeout),
                false,
                userInfo.ToString());
            return ticket;
        }

    }
}
