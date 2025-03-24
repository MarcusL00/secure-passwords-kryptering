using System;
using secure_passwords_kryptering.Controllers;

namespace secure_passwords_kryptering
{
    internal class Program
    {
        static void Main(string[] args)
        {
            UserController userController = new UserController();

            while (true)
            {
                Console.WriteLine("Select an option:");
                Console.WriteLine("1. Create User");
                Console.WriteLine("2. Login");
                Console.WriteLine("3. Exit");
                string option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        CreateUser(userController);
                        break;
                    case "2":
                        LoginUser(userController);
                        break;
                    case "3":
                        return;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }

        static void CreateUser(UserController userController)
        {
            Console.WriteLine("Enter username:");
            string username = Console.ReadLine();

            Console.WriteLine("Enter password:");
            string password = Console.ReadLine();

            userController.CreateUser(username, password);
        }

        static void LoginUser(UserController userController)
        {
            Console.WriteLine("Enter username:");
            string username = Console.ReadLine();

            Console.WriteLine("Enter password:");
            string password = Console.ReadLine();

            bool isVerified = userController.VerifyUser(username, password);
            Console.WriteLine(isVerified ? "Login successful!" : "Login failed. Please try again.");
        }
    }
}