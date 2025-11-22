using System;
using System.Collections.Generic;

namespace AssignmentCARS
{
    public class Login
    {
        private readonly Action _pause;
        public Login(Action pauseFunction) => _pause = pauseFunction;

        public Customer Login1(Dictionary<string, Customer> customers)
        {
            while (true) //keep looping until login succeeds
            {
                //get username
                string loginName;
                while (true)
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine("=== LOGIN ===\n");
                    Console.ResetColor();
                    Console.Write("Enter your full name: ");
                    loginName = Console.ReadLine()?.Trim() ?? "";

                    if (!string.IsNullOrWhiteSpace(loginName))
                        break;

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Name cannot be empty.");
                    Console.ReadKey();
                    Console.ResetColor();
                }

                //get password
                string loginPassword;
                while (true)
                {
                    Console.Clear();
                    Console.WriteLine($"Name: {loginName}\n");
                    Console.Write("Enter your password: ");
                    loginPassword = Console.ReadLine()?.Trim() ?? "";

                    if (!string.IsNullOrWhiteSpace(loginPassword))
                        break;

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Password cannot be empty.");
                    Console.ReadKey();
                    Console.ResetColor();
                }

                Console.Clear();

                //check login detials
                if (customers.TryGetValue(loginName, out Customer c) && c.Password == loginPassword)
                {
                    Console.WriteLine($"Welcome back, {c.Name}!");
                    Console.WriteLine($"Your current level: {LevelName(c.Level)}");
                    _pause();
                    return c; //login successful then return customer
                }

                //login details incorrect loops again
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Username or password incorrect. Please try again.");
                Console.ResetColor();
                _pause();
            }
        }

        private string LevelName(int level) => level switch
        {
            10 => "VIP",
            5 => "Premium",
            _ => "Standard"
        };
    }
}



//if
//(!found)
//    Console.Clear();
//Console.ForegroundColor = ConsoleColor.Red;
//Console WriteLine("No customer found or wrong password. ");
//Console.ResetColor();
//else
//    Console.Clear();
//Console.ForegroundColor = ConsoleColor.Red;
//Console.WriteLine('Invalid input. Please type either 1 or 2. Thanks.");
//Console.