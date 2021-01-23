using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaWebApp.Models
{
    public class Admin : User
    {
        /* This class is just to have a difference between admins and users */
        public Admin(User user)
        {
            UserID = user.UserID;
            Email = user.Email;
            FirstName = user.FirstName;
            LastName = user.LastName;
            PasswordHash = user.PasswordHash;
            PasswordSalt = user.PasswordSalt;
        }
        public Admin()
        {

        }
    }
}
