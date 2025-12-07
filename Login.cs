using System;
using System.Collections.Generic;

namespace AssignmentCARS
{
    //this class handles customer login
    //focuses: robustness(validation), OOP principles, clean user experience
    public class Login
    {
        //pause behaviour injected so this class isnt tightly coupled to console
        //makes class eaiser to test and reuse
        //reference:   
        private readonly Action _pause;

        //constructor dependency injection for better testability/flexibilty
        public Login(Action pauseFunction) => _pause = pauseFunction;

        //main login method auhtenticates agaisnt customer data
        public Customer Login1(Dictionary<string, Customer> customers)
        {
            //keeps looping untilvalid login (robust control flow)
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

                    // Trim() removes unwanted spaces to prevent inout bugs
                    loginName = Console.ReadLine()?.Trim() ?? "";

                    //validation to prevent empty input (robustness)
                    if (!string.IsNullOrWhiteSpace(loginName))
                        break; 

                    //clear feedback for user
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

                    //validation ensures data integrity and prevents crashes
                    if (!string.IsNullOrWhiteSpace(loginPassword))
                        break;

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Password cannot be empty.");
                    Console.ReadKey();
                }

                Console.Clear();

                // find customer by name
                //iterates through dictionary values
                //case insensitive name matching improves usability
                foreach (var cust in customers.Values)
                {
                    if (cust.Name.Equals(loginName, StringComparison.OrdinalIgnoreCase)
                        && cust.Password == loginPassword)
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        //success feedback
                        Console.WriteLine($"Welcome back, {cust.Name}!");
                        Console.ResetColor();
                        //shows level using helper method
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"\nYour current level: {LevelName(cust.Level)}");
                        Console.ResetColor();
                        _pause(); //uses injected pause instead of hard coding console logic
                        return cust; //returns authenticated user object
                    }
                }

                // failed login handling
                //does not crash program allows retry (robustness)
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Username or password incorrect. Please try again.");
                Console.ResetColor();
                _pause();
            }
        }

        //converts numeric level into readable text
        //demonstrated abstraction and clean code practices
        private string LevelName(int level) => level switch
        {
            10 => "VIP",
            5 => "Premium",
            _ => "Standard"
        };
    }
}

