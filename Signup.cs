using System;
using System.Collections.Generic;

namespace AssignmentCARS
{
    public class Signup
    {
        //here I decided not to call UIHelper.Pause() directly. I just injected the pause behaviour using this private readonly Action_pause and passed through constructor
        //this was to avoid tightly coupling the Signup class to the console UI
        //this is an example of an OOP principle called Dependency Injection
        //instead of the class deciding how to pause, it just calls _pause() and calling code decides wat pause it actually is
        //it makes the class more flexible and easier to test and reuse in other situations, later for an improvement i could swap this pause logic for something else(fake pause for unit testing) wihtout changing the class itself.
        private readonly Action _pause;

        //constructor injects pause behavious into this class
        public Signup(Action pauseFunction) => _pause = pauseFunction;

        //main signup method: handles user input, validation and saving data
        public Customer Signup1(Dictionary<string, Customer> customers, Action saveAll)
        {
            string newName;
            //get names that arent duplicated
            //loops until valid
            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("=== SIGNUP ===\n");
                Console.ResetColor();
                Console.Write("Enter your full name and hit enter: ");
                newName = Console.ReadLine()?.Trim() ?? "";

                //example of robustness as input validation prevents empty or data thats not valid
                if (string.IsNullOrWhiteSpace(newName))
                {
                    Console.WriteLine("Name cannot be empty.");
                    Console.ReadKey();
                    continue;
                }

                //check duplicate names using iteration over dictionary values, using algorithm
                //demonstrates wokring wiht collections and matching logic
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

                // only exits loop when a valid/unique name provided
                if (!duplicate)
                    break;   //this exits the loop if the name is valid  
            }

            string newPassword;
            //get valid password
            //loops until then
            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.WriteLine($"Name: {newName}");
                Console.ResetColor();
                Console.Write("\nCreate a password (minimum 12 characters) and hit enter : ");
                newPassword = Console.ReadLine()?.Trim() ?? "";

                //robustness improved as it prevnts empty passwords
                if (string.IsNullOrWhiteSpace(newPassword))
                {
                    Console.WriteLine("Please enter a password.");
                    Console.ReadKey();
                    continue;
                }

                // data validation: enforces password length
                if (newPassword.Length < 12)
                {
                    Console.WriteLine("Password must be at least 12 characters long.");
                    Console.ReadKey();
                    continue;
                }

                break;
            }

            //OOP: creating new customer object encapsulates data and behaviour
            // create new customer (auto creates CustomerID)
            Customer newCustomer = new Customer(newName, newPassword);

            //add using CustomerID as key
            //uses dictionary collection for fast key-based access (O(1))
            //supports efficient data handling
            customers[newCustomer.CustomerID] = newCustomer;

            // Save customer file
            //data persistance- saves customer to binary file storage
            BinaryRepository.SaveCustomer(newCustomer);

            // SaveAll updates master list if needed
            saveAll();

            //user experience- clear success feedback with colour
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"Welcome onboard, {newCustomer.Name}!");
            Console.ResetColor();

            //calls injected pause behaviour (decoupled from UI implementation)
            _pause();

            return newCustomer;
        }
    }
}
