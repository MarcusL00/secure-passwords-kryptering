using secure_passwords_kryptering.Entities;
using secure_passwords_kryptering.Utils;
using System;
using System.IO;

namespace secure_passwords_kryptering.Controllers
{
    public class UserController
    {
        private readonly string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data", "users.txt");
        private const int MaxFailedAttempts = 5;
        private const int LockoutDurationMinutes = 15;

        public UserController()
        {
            // Ensure the directory exists
            string directory = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

        public void CreateUser(string username, string password)
        {
            string hashedPassword = EncryptionService.HashPassword(password);
            User user = new User(username, hashedPassword);

            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine($"{user.Username},{user.HashedPassword},0,{DateTime.MinValue}");
            }

            Console.WriteLine("User created successfully!");
        }

        public bool VerifyUser(string username, string password)
        {
            if (!File.Exists(filePath))
            {
                return false;
            }

            string[] lines = File.ReadAllLines(filePath);
            for (int i = 0; i < lines.Length; i++)
            {
                string[] parts = lines[i].Split(',');
                if (parts.Length == 4 && parts[0] == username)
                {
                    string storedHashedPassword = parts[1];
                    int failedAttempts = int.Parse(parts[2]);
                    DateTime lockoutEndTime = DateTime.Parse(parts[3]);

                    if (failedAttempts >= MaxFailedAttempts && DateTime.Now < lockoutEndTime)
                    {
                        Console.WriteLine("Account is locked. Try again later.");
                        return false;
                    }

                    if (EncryptionService.VerifyPassword(password, storedHashedPassword))
                    {
                        // Reset failed attempts on successful login
                        lines[i] = $"{username},{storedHashedPassword},0,{DateTime.MinValue}";
                        File.WriteAllLines(filePath, lines);
                        return true;
                    }
                    else
                    {
                        failedAttempts++;
                        if (failedAttempts >= MaxFailedAttempts)
                        {
                            lockoutEndTime = DateTime.Now.AddMinutes(LockoutDurationMinutes);
                        }
                        lines[i] = $"{username},{storedHashedPassword},{failedAttempts},{lockoutEndTime}";
                        File.WriteAllLines(filePath, lines);
                        Console.WriteLine("Invalid password. Try again.");
                        return false;
                    }
                }
            }

            return false;
        }
    }
}