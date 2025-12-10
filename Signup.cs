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


/*
 * TEST CASES FOR SIGNUP:
 * 
 * TC1: Valid signup with minimum assword length
 * Input: name="Alicia Smyth", password="pass234" (exactly 12 characters)
 * Expected Output: Customer object created with auto-generated GUID CustomerID, added to customers dictionary, saved to "customers/{GUID}.dat" file, displays "Welcome onboard, Alicia Smyth!" in blue
 * Result: PASS - Customer created successfully checked customers folder there was a new .dat file with a GUID name, welcome message correctly shown
 * 
 * TC2: Valid signup with long password
 * Input: name="Bob Jackson", password="verylongggggsecurepassword12345"
 * Expected Output: Customer created successfully, saved to file, displays welcome message
 * Result: PASS - Customer created successfully and welcome mesage shown
 * 
 * TC3: Empty name input
 * Input: name="" (empty string)
 * Expected Output: Displays "Name cannot be empty.", waits for key press, loops back to name prompt
 * Result: PASS - Displays "Name cannot be empty." Pressed a key then it asked for my name again.
 * 
 * TC5: Duplicate name - exact match
 * Input: name="John Doe" when "John Doe" already exists in customers dictionary
 * Expected Output: Displays "This name is already registered.", waits for key press, loops back to name prompt
 * Result: PASS - Displays "This name is already registered.", pressed a key and it asked for name again
 * 
 * TC6: Duplicate name - case insensitive
 * Input: name="john jackson" when "John Jackson" exists (different casing)
 * Expected Output: StringComparison.OrdinalIgnoreCase detects duplicate, displays "This name is already registered.", prompts again
 * Result: PASS - Displays "This name is already registered.", pressed a key and it asked for name again
 * 
 * TC7: Duplicate name - mixed case variations
 * Input: name="JOHN Den" or "JoHn DEn" when "John Den" exists
 * Expected Output: Detects as duplicate due to case-insensitive comparison, rejects signup
 * Result: PASS - Displays "This name is already registered.", pressed a key and it asked for name again
 * 
 * TC8: Unique name after duplicate attempt
 * Input: First attempt "John Doe" (duplicate), second attempt "Jane Smith" (unique)
 * Expected Output: First attempt rejected, second attempt proceeds to password input
 * Result: PASS - Allows me to go to the password input
 * 
 * TC9: Empty password input
 * Input: password="" (empty string)
 * Expected Output: Displays "Please enter a password.", waits for key press, loops back to password prompt
 * Result: PASS - Displayed "Please enter a password", pressed a key then asked for password again
 *
 * TC10: Password too short - 11 Characters
 * Input: password="shortpass11" (11 characters, one below minimum)
 * Expected Output: Displays "Password must be at least 12 characters long.", waits for key press, prompts again
 * Result: PASS - Dispalyed "Password must be at least 12 characters long" presed a key then got asked for password input again
 * 
 * 
 * TC11: Password exactly 12 characters (Boundary)
 * Input: password="exactly12chr" (exactly 12 characters)
 * Expected Output: Accepts password (Length >= 12), proceeds with signup successfully
 * Result: PASS - Password accepted and shown welcome message and taken to main menu
 * 
 * 
 * TC12: CustomerID Auto-Generation
 * Input: Valid name and password provided
 * Expected Output: Customer constructor generates unique GUID for CustomerID (e.g., "19390806-4485-445a-8091-1e2452697897")
 * Result: PASS - Signed up successfully then checked the customers folder and it made a new .dat file with a unique ID.
 * 
 * TC13: Binary File Save
 * Input: New customer successfully created
 * Expected Output: BinaryRepository.SaveCustomer() creates file "customers/{CustomerID}.dat" with serialized customer data
 * Result: PASS - File created in customers folder with GUID as the filename.
 * 
 * TC14: Success Message Display
 * Input: Signup completed successfully for customer named "Alice Smith"
 * Expected Output: Screen cleared, displays "Welcome onboard, Alice Smith!" in blue color, calls _pause()
 * Result:
 * 
 * TC15: Name with Leading/Trailing Whitespace
 * Input: name="  Alicia Smyth  " (spaces before and after)
 * Expected Output: Trim() removes whitespace, "Alicia Smyth" used as actual name
 * Result: PASS - Screen cleared and "Welcome onboard, Alicia Smyth!" in blue.Then paused waiting for me to press a key
 * 
 * TC22: Password visible while signing up
 * Input: Type password during signup
 * Expected Output: Password needs to be masked (shown as * )
 * Result: FAIL - When I typed a password, every character appeared on screen in plain text
 * IMPROVEMENT: This needs to be hashed becuase anyone looking at the screen would see it. implement a method that hides characters as you type for better security
 * 
 * TC30: No Complex password rules
 * Input: password = "1876456789012"
 * Expected Output: Should require letters, numbers and symbols
 * Result: FAIL – System only checked for length and not complexity
 * IMPROVEMENT: this would improve security and make password much harder to work out for anyone trying to invade someone's account
 */