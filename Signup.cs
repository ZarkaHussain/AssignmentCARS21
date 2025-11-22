using System;
using System.Collections.Generic;

namespace AssignmentCARS
{
    public class Signup
    {
        private readonly Action _pause;
        public Signup(Action pauseFunction) => _pause = pauseFunction;

        public Customer Signup1(Dictionary<string, Customer> customers, Action saveCustomers)
        {
            string newName;
            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("=== SIGNUP ===\n");
                Console.ResetColor();
                Console.Write("Enter your full name: ");
                newName = Console.ReadLine()?.Trim() ?? "";

                if (string.IsNullOrWhiteSpace(newName))
                {
                    Console.WriteLine("Name cannot be empty.");
                    Console.ReadKey();
                    continue;
                }

                if (customers.ContainsKey(newName))
                {
                    Console.WriteLine("This name is already registered.");
                    Console.ReadKey();
                    continue;
                }

                break;
            }

            string newPassword;
            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.WriteLine($"Name: {newName}");
                Console.ResetColor();
                Console.Write("\nCreate a password (minimum 12 characters): ");
                newPassword = Console.ReadLine()?.Trim() ?? "";

                if (string.IsNullOrWhiteSpace(newPassword))
                {
                    Console.WriteLine("Please enter a password.");
                    Console.ReadKey();
                    continue;
                }

                if (newPassword.Length < 12)
                {
                    Console.WriteLine("Password must be at least 12 characters long.");
                    Console.ReadKey();
                    continue;
                }

                break;
            }

            Customer newCustomer = new Customer(newName, newPassword);
            customers[newName] = newCustomer;
            saveCustomers();

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine($"Welcome onboard, {newCustomer.Name}!");
            Console.ResetColor();
            _pause();

            return newCustomer;
        }
    }
}
