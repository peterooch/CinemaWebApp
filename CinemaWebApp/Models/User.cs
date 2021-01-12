using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;

namespace CinemaWebApp.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public bool VerifyPassword(string password)
        {
            using SHA512Managed HashObject = new SHA512Managed();
            /* Get bytes for combined string */
            byte[] BytesToHash = Encoding.UTF8.GetBytes(password + PasswordSalt);
            /* Get hash result for combined string*/
            string HashString = Encoding.UTF8.GetString(HashObject.ComputeHash(BytesToHash));

            /* Now check if it matches the stored hashed password */ 
            return HashString == PasswordHash;
        }
        public void StorePassword(string password)
        {
            using SHA512Managed HashObject = new SHA512Managed();
            using RNGCryptoServiceProvider rngObject = new RNGCryptoServiceProvider();
            byte[] rngBytes = new byte[64];

            /* Generate new salt */
            rngObject.GetBytes(rngBytes);
            PasswordSalt = Encoding.UTF8.GetString(rngBytes);

            /* Hash password with salt and store result */
            byte[] BytesToHash = Encoding.UTF8.GetBytes(password + PasswordSalt);
            PasswordHash = Encoding.UTF8.GetString(HashObject.ComputeHash(BytesToHash));
        }
    }
}
