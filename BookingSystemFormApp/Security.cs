using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Security.Policy;



namespace BookingSystemFormApp
{
    /// <summary>
    /// Author: Timur Maistrenko
    /// <br></br>
    /// Security Class containing security and cryptography-related methods.
    /// </summary>
    internal class Security
    {


        private static readonly int saltLength = 16;

        /// <summary>
        /// Generates salted password hash
        /// </summary>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        /// <returns>
        /// string hashed password+salt
        /// </returns>
        public static string PasswordHash(string password, string salt)
        {
            using SHA256 sha256 = SHA256.Create();

            string saltedPassword = password+salt;
            byte[] hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(saltedPassword));

            return Encoding.UTF8.GetString(hash);
        }

        /// <summary>
        /// Generates random salt
        /// </summary>
        /// <returns>
        /// string salt
        /// </returns>
        public static string GenerateSalt()
        {
            Random rng = new();

            byte[] salt = new byte[saltLength];
            rng.NextBytes(salt);

            return Encoding.UTF8.GetString(salt);
        }

        //TODO
    }


}
