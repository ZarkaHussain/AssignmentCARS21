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



/*
 * TEST CASES FOR LOGIN:
 * 
 * TC1: Valid login - Standard customer
 * Input: name="John Den", password="password1234" (matching existing customer with Level=1)
 * Expected Output: Displays "Welcome back, John Den!" in blue, displays "Your current level: Standard" in yellow, returns customer object
 * Result: PASS - Successfully logs in and displays welcome message and current level message
 * 
 * TC2: Valid login - Premium customer
 * Input: name="Alicia Smyth", password="pass123" (matching customer with Level=5)
 * Expected Output: Displays "Welcome back, Alicia Smyth!", displays "Your current level: Premium", returns customer object
 * Result: PASS - Successfully logs in and displays welcome message and current level message
 * 
 * TC3: Valid Login - VIP Customer
 * Input: name="Bob Jackson", password="password123" (matching customer with Level=10)
 * Expected Output: Displays "Welcome back, Bob Jackson!", displays "Your current level: VIP", returns customer object
 * Result: PASS - Successfully logs in and displays welcome message and current level message
 * 
 * TC4: Invalid username (User doesn't exist)
 * Input: name="None", password="anypass"
 * Expected Output: No match found in dictionary, displays "Username or password incorrect. Please try again." in red, loops back to name input
 * Result: PASS - Displays "Username or password incorrect. Please try again." in red, loops back to name input
 * 
 * TC5: Invalid Password (Correct Username, Wrong Password)
 * Input: name="John Den" (exists), password="wrongpass"
 * Expected Output: Name matches but password not the stored password so displays "Username or password incorrect. Please try again.", loops back to name input
 * Result: PASS - Displays "Username or password incorrect. Please try again.", loops back to name input
 * 
 * TC6: Username and password Incorrect
 * Input: name="Wrong", password="wrongpass"
 * Expected Output: No match found so displays "Username or password incorrect. Please try again.", prompts retry
 * Result: PASS - Displays "Username or password incorrect. Please try again.", prompts retry
 * 
 * TC7: Name input empty
 * Input: name="" (empty string)
 * Expected Output: Displays "Name cannot be empty." in red, waits for key press, loops back to name prompt
 * Result: PASS - Displays "Name cannot be empty." in red, waits for key press, loops back to name prompt
 * 
 * TC8: Whitespace only name
 * Input: name="   " (spaces only)
 * Expected Output: Trim() converts to empty string, triggers IsNullOrWhiteSpace check, displays "Name cannot be empty.", prompts again
 * Result: PASS - Displays "Name cannot be empty.", prompts again
 * 
 * TC9: Empty password input
 * Input: Valid name entered, then password="" (empty string)
 * Expected Output: Displays "Please enter a password." in red, waits for key press, loops back to password prompt
 * Result: FAIL - Displays "Please enter a password" but colour is still white, loops back to password prompt
 * 
 * TC10: Whitespace only password
 * Input: Valid name entered, then password="    " (spaces only)
 * Expected Output: Trim() converts to empty string, displays "Please enter a password", prompts again
 * Result: PASS - Displays "Please enter a password", prompts again
 * 
 * TC11: Case insensitive name match - Uppercase
 * Input: name="JOHN DEN" when stored as "John Den"
 * Expected Output: StringComparison.OrdinalIgnoreCase matches successfully, logs in if password correct
 * Result: PASS - Successfully logs in
 * 
 * TC12: Case insensitive name match - lowercase
 * Input: name="john den" when stored as "John Den"
 * Expected Output: Case-insensitive comparison succeeds, logs in if password correct
 * Result: PASS - Successfully logs in
 * 
 * TC13: Case insensitive name match - mixed case
 * Input: name="jOhN deN" when stored as "John Den"
 * Expected Output: Case-insensitive comparison succeeds, logs in if password correct
 * Result: PASS - Successfully logs in
 * 
 * TC14: Password is case sensitive
 * Input: name="John Den" (correct), password="PASSWORD1234" when stored as "password1234"
 * Expected Output: Password comparison is exact match (no OrdinalIgnoreCase), displays "Username or password incorrect", fails login
 * Result: PASS - Login unsuccessful
 * 
 * TC15: Name with leading/trailing whitespace
 * Input: name="  John Den  " (spaces before and after)
 * Expected Output: Trim() removes whitespace, "John Den" used for matching, succeeds if password correct
 * Result: PASS - Successfully logs in
 * 
 * TC16: Password with leading/trailing whitespace
 * Input: name="John Doe", password="  password1234  " when stored as "password1234"
 * Expected Output: Trim() removes whitespace, exact match comparison succeeds, logs in successfully
 * Result: PASS - Successfully logs in
 * 
 * TC17: Retry after failed login
 * Input: First attempt with wrong password, second attempt with correct password
 * Expected Output: First attempt fails with error message, loops back, second attempt succeeds and returns customer
 * Result: PASS - Successfully logs in
 * 
 * TC18: Multiple failed login attempts
 * Input: Three consecutive failed login attempts
 * Expected Output: Each attempt displays error message, allows 2 retries (no lockout), 3rd retry results in account locked
 * Result: FAIL - Allows logs in to account
 * NOTE: THIS COULD BE ENHANCED FOR SECURITY IMPROVEMENTS IN THE FUTURE SO ACCOUNT IS LOCKED AFTER MULTIPLE FAILED LOGIN ATTEMPTS (3 LOGIN ATTEMPTS ALLOWED)
 * 
 * TC19: Empty customer Dictionary
 * Input: Login attempt when customers dictionary is empty (no users exist)
 * Expected Output: Loop completes without match, displays "Username or password incorrect", prompts retry
 * Result:
 * 
 * 
 */