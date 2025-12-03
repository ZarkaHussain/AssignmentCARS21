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
            while (true)
            {
                // name
                string loginName;
                while (true)
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine("=== LOGIN ===\n");
                    Console.ResetColor();

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("Enter your full name and hit enter: ");
                    Console.ResetColor();
                    loginName = Console.ReadLine()?.Trim() ?? "";

                    if (!string.IsNullOrWhiteSpace(loginName))
                        break; 

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Name cannot be empty.");
                    Console.ReadKey();
                }

                // password
                string loginPassword;
                while (true)
                {
                    Console.Clear();
                    Console.WriteLine($"Name: {loginName}\n");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("Enter your password and hit enter: ");
                    Console.ResetColor();
                    loginPassword = Console.ReadLine()?.Trim() ?? "";

                    if (!string.IsNullOrWhiteSpace(loginPassword))
                        break;

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Password cannot be empty.");
                    Console.ReadKey();
                }

                Console.Clear();

                // find customer by name
                foreach (var cust in customers.Values)
                {
                    if (cust.Name.Equals(loginName, StringComparison.OrdinalIgnoreCase)
                        && cust.Password == loginPassword)
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine($"Welcome back, {cust.Name}!");
                        Console.ResetColor();
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"\nYour current level: {LevelName(cust.Level)}");
                        Console.ResetColor();
                        _pause();
                        return cust;
                    }
                }

                // failed login
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