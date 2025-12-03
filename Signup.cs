using System;
using System.Collections.Generic;

namespace AssignmentCARS
{
    public class Signup
    {
        private readonly Action _pause;
        public Signup(Action pauseFunction) => _pause = pauseFunction;

        public Customer Signup1(Dictionary<string, Customer> customers, Action saveAll)
        {
            string newName;
            //get names that arent duplicated
            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("=== SIGNUP ===\n");
                Console.ResetColor();
                Console.Write("Enter your full name and hit enter: ");
                newName = Console.ReadLine()?.Trim() ?? "";

                if (string.IsNullOrWhiteSpace(newName))
                {
                    Console.WriteLine("Name cannot be empty.");
                    Console.ReadKey();
                    continue;
                }

                //check duplicate names
                bool duplicate = false;
                foreach (var c in customers.Values)
                {
                    if (c.Name.Equals(newName, StringComparison.OrdinalIgnoreCase))
                    {
                        Console.WriteLine("This name is already registered.");
                        Console.ReadKey();
                        duplicate = true;
                        break;
                    }
                }

                if (!duplicate)
                    break;   //this exits the loop if the name is valid  
            }

            string newPassword;
            //get valid password
            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.WriteLine($"Name: {newName}");
                Console.ResetColor();
                Console.Write("\nCreate a password (minimum 12 characters) and hit enter : ");
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

            // create new customer (auto creates CustomerID)
            Customer newCustomer = new Customer(newName, newPassword);

            //add using CustomerID as key
            customers[newCustomer.CustomerID] = newCustomer;

            // Save customer file
            BinaryRepository.SaveCustomer(newCustomer);

            // SaveAll updates master list if needed
            saveAll();

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"Welcome onboard, {newCustomer.Name}!");
            Console.ResetColor();
            _pause();

            return newCustomer;
        }
    }
}
