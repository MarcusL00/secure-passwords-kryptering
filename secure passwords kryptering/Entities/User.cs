using System;

namespace secure_passwords_kryptering.Entities
{
    internal class User
    {
        public string Username { get; set; }
        public string HashedPassword { get; set; }

        public User(string username, string hashedPassword)
        {
            Username = username;
            HashedPassword = hashedPassword;
        }
    }
}