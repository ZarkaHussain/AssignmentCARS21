using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;


namespace AssignmentCARS
{
    class Program
    {
        //comand line interface
        //if program launched with arguments it attempts to run in admin mode
        //separates admin features from normal user features- security/access control
        static void Main(string[] args)
        {
            // If admin login via CLI args
            if (args.Length > 0)
            {
                new AdminLogin().Run(args);
                return;
            }

            //load all customer data from (/customers/) binary files on startup
            //DATA persistence
            // allows data to survive after program is closed- persistent storage
            var customers = BinaryRepository.LoadAll();

            //THIS FOR Performance Testing
            //RentCar.RunParallelTest();


            //this save action saves updated customer data- writes back to all .dat files
            //using Action keeps saving logic reusable and clean
            Action saveAll = () => BinaryRepository.SaveAll(customers);

            //Injecting Pause() method improves flexibility and testability
            var login = new Login(UIHelper.Pause);
            var signup = new Signup(UIHelper.Pause);

            //main application loop
            //ensures app keeps running until user chooses to quit
            while (true)
            {
                Console.Clear();
                //colours and aninations added to make console UI more user friendly
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("HELLO, WELCOME TO");
                Console.ResetColor();
                Console.WriteLine();
                ShowAnimatedBanner();

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\n(1) Signup");
                Console.WriteLine("(2) Login");
                Console.WriteLine("(3) Quit");
                //Console.WriteLine("(4) Performance Test");
                Console.ResetColor();
                Console.Write("\nChoose an option: ");

                //using ReadKey improves responsiveness and prevents accidental extra input
                char key = Console.ReadKey(true).KeyChar;
                string input = key.ToString();
                Console.WriteLine(key);
                //string input = Console.ReadLine()?.Trim() ?? "";

   
                try
                {
                    //robustness- for input validation
                    //TryParse prevents crashes from non-numeric input
                    if (!int.TryParse(input, out int option))
                        throw new FormatException();

                    //defensive programming ensures only valid menu options accespted
                    if (option < 1 || option > 3)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Error: Please choose 1, 2, or 3.");
                        Console.ResetColor();
                        UIHelper.Pause();
                        continue;
                    }

                    switch (option)
                    {
                        case 1: //signup flow
                            Customer newCust = signup.Signup1(customers, saveAll); //creates new user, saves data, opens main menu
                            MainMenu.Show(newCust, saveAll);
                            break;

                        case 2: //login flow
                            Customer loggedIn = login.Login1(customers); //authenticates user and loads their data
                            MainMenu.Show(loggedIn, saveAll);
                            saveAll(); // saves updated data after session
                            break;

                        case 3: //safe exit
                            Console.WriteLine("Goodbye!");
                            Thread.Sleep(1000);
                            Environment.Exit(0);
                            break;

                        //case 4:
                        //    ShowPerformanceMenu();
                        //    break;

                    }
                }
                catch (FormatException)
                {
                    //robustness-user friendly errors
                    //prevents crash when non-numeric input entered
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error: Please enter a NUMBER, not letters.");
                    Console.ResetColor();
                    UIHelper.Pause();
                }
                catch (Exception ex)
                {
                    //catch all error handling
                    //protects program from unexpected runtime crashes
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Unexpected error: {ex.Message}");
                    Console.ResetColor();
                    UIHelper.Pause();
                }
            }
        }

        //This is for Performance testing:( multiple processors, caching, stringubuilding stufff)
        //static void ShowPerformanceMenu()
        //{
        //    var test = new PerformanceTest();

        //    while (true)
        //    {
        //        Console.Clear();
        //        Console.WriteLine("=== PERFORMANCE & PROFILING TESTS ===\n");
        //        Console.WriteLine("1) Multiple Processor Test");
        //        Console.WriteLine("2) String vs StringBuilder");
        //        Console.WriteLine("3) Caching Test");
        //        Console.WriteLine("4) Capacity Test");
        //        Console.WriteLine("5) Back\n");
        //        Console.Write("Choose: ");

        //        char key = Console.ReadKey(true).KeyChar;

        //        switch (key)
        //        {
        //            case '1': test.MultipleProcessors(); break;
        //            case '2': test.StringVsStringBuilder(); break;
        //            case '3': test.CachingTest(); break;
        //            case '4': test.CapacityTest(); break;
        //            case '5': return;

        //            default: break;
        //        }
        //    }
        //}
        ////////    UP TO HERE  ////////////


        //simple animation used to improve user experience
        static void ShowAnimatedBanner()
        {
            string[] banner = new[]
            {
@" ██████╗  █████╗ ██████╗        ██╗    ██╗ ██████╗ ██████╗ ██╗     ██████╗ ",
@"██╔════╝ ██╔══██╗██╔══██╗       ██║    ██║██╔═══██╗██╔══██╗██║     ██   ██╗",
@"██║      ███████║██████╔╝       ██║ █╗ ██║██║   ██║██████╔╝██║     ██   ██║ ",
@"██║      ██╔══██║██╔══██╗       ██║███╗██║██║   ██║██╔══██╗██║     ██   ██║ ",
@"╚██████╗ ██║  ██║██║  ██║       ╚███╔███╔╝╚██████╔╝██║  ██║███████╗███████║",
@" ╚═════╝ ╚═╝  ╚═╝╚═╝  ╚═╝        ╚══╝╚══╝  ╚═════╝ ╚═╝  ╚═╝╚══════╝╚═════╝"
            };

            Console.ForegroundColor = ConsoleColor.Blue;

            //slow printing for animation effect
            foreach (var line in banner)
            {
                Console.WriteLine(line);
                Thread.Sleep(80);
            }

            Console.ResetColor();
        }

    }
}



/*
 * TEST CASES FOR PROGRAM (MAIN ENTRY POINT):
 * 
 * TC1: Launch with Admin Arguments
 * Input: Program launched with args = ["admin", "admin", "1234"]
 * Expected Output: Detects args.Length > 0, creates AdminLogin instance, calls Run(args), bypasses normal user flow
 * Result: PASS - Admin menu displayed
 * 
 * TC2: Launch with No Arguments (Normal User Mode)
 * Input: Program launched with args = [] (empty)
 * Expected Output: Skips admin login, loads customers from BinaryRepository.LoadAll(), displays main welcome menu
 * Result: PASS - Main welcome menu displayed
 * 
 * TC3: Customer Data Loading on Startup
 * Input: Program starts, customers folder contains 5 .dat files
 * Expected Output: BinaryRepository.LoadAll() loads all 5 customers into dictionary before showing menu
 * Result: PASS - All customer file successfully loaded at start
 * 
 * TC4: Select Option 1 - Signup Flow
 * Input: User presses '1' on welcome menu
 * Expected Output: Calls signup.Signup1(), creates new customer, saves to file, opens MainMenu.Show() with new customer
 * Result: PASS - Successful signup took user to main menu.
 * 
 * TC5: Select Option 2 - Login Flow
 * Input: User presses '2' on welcome menu
 * Expected Output: Calls login.Login1(), authenticates user, opens MainMenu.Show() with logged-in customer, calls saveAll() after session ends
 * Result: PASS - Login successful and main menu displayed
 * 
 * TC6: Select Option 3 - Quit Program
 * Input: User presses '3' on welcome menu
 * Expected Output: Displays "Goodbye!", sleeps for 1000ms, calls Environment.Exit(0) to terminate program
 * Result: PASS - Goodbye message displayed and program closed correctly  
 * 
 * TC7: Invalid Option - Out of Range (High)
 * Input: User presses '9' or any number > 3
 * Expected Output: Displays "Error: Please choose 1, 2, or 3." in red, pauses, returns to welcome menu
 * Result: PASS - Coorect error message shown, user forced to choose between 1,2 or 3
 * 
 * TC8: Invalid Option - Out of Range (Low)
 * Input: User presses '0' or any number < 1
 * Expected Output: Displays "Error: Please choose 1, 2, or 3." in red, pauses, returns to welcome menu
 * Result: PASS - Correct error message shown, user forced to choose between 1,2 or 3
 * 
 * TC9: Non-Numeric Input - Letter
 * Input: User presses 'x' or any letter
 * Expected Output: int.TryParse fails, throws FormatException, catches it, displays "Error: Please enter a NUMBER, not letters." in red
 * Result: PASS - Coorect error message shown, user forced to choose between 1,2 or 3
 * 
 * TC10: Non-Numeric Input - Special Character
 * Input: User presses '@' or '$'
 * Expected Output: TryParse fails, catches FormatException, displays number error message, pauses, returns to menu
 * Result: PASS - Exception caught and correct error message shown, menu displayed
 * 
 * TC11: Main Loop Continues After Invalid Input
 * Input: User enters invalid input twice, then enters '1'
 * Expected Output: First two attempts show error and loop back, third attempt (valid) proceeds to signup
 * Result: PASS - Correctly recovers Menu after invalid inputs then proceeded to signup after correct '1' input
 * 
 * TC12: SaveAll Action Creation
 * Input: Program starts
 * Expected Output: Creates Action saveAll = () => BinaryRepository.SaveAll(customers), can be passed to Login/Signup/MainMenu
 * Result: PASS - SaveAll action created successfully and reused in methods
 * 
 * TC13: Dependency Injection - Pause Function
 * Input: Program creates Login and Signup objects
 * Expected Output: Both initialized with UIHelper.Pause as constructor parameter (Pause(UIHelper.Pause))
 * Result: PASS - UIHelper.Pause successfully passed into Login and Signup classes
 * 
 * TC14: Exception Handling - Unexpected Error
 * Input: Unexpected runtime exception occurs (e.g., OutOfMemoryException)
 * Expected Output: Catches generic Exception, displays "Unexpected error: {ex.Message}" in red, pauses, continues running
 * Result: FAIL - Could not safely simulate OutOfMemoryException in console app
 * 
 * TC15: Console Cleared Before Menu Display
 * Input: User completes any action and returns to welcome menu
 * Expected Output: Console.Clear() called at start of loop, screen cleared before showing menu again
 * Result: PASS - Screen clears before displaying menu
 * 
 * TC16: ShowAnimatedBanner Display
 * Input: Program displays welcome menu
 * Expected Output: Shows "CAR WORLD" banner in blue color, animated line-by-line with 80ms delay between lines
 * Result: PASS - Banner shown correctly
 * 
 * TC17: Single Key Input Reading
 * Input: User presses '2' key
 * Expected Output: Console.ReadKey(true).KeyChar captures '2', converts to string "2", processes immediately without Enter key
 * Result: PASS - Proceeds successfully without enter key
 * 
 * TC18: Session Flow - Signup to MainMenu
 * Input: User chooses Signup, creates account successfully
 * Expected Output: signup.Signup1() returns new Customer object, MainMenu.Show() called with that customer, user enters main application
 * Result: PASS - User taken to MainMneu successfully after signing up succssfully
 * 
 * TC19: Session Flow - Login to MainMenu to Logout
 * Input: User chooses Login, logs in, uses app, then logs out from MainMenu
 * Expected Output: login.Login1() returns customer, MainMenu.Show() runs, when user logs out MainMenu returns, saveAll() called, returns to welcome menu
 * Result: PASS - Loggd in, used menu, logged out and returned to welcome screen
 * 
 * TC20: Color Formatting - Yellow Text
 * Input: Program displays welcome menu
 * Expected Output: "HELLO, WELCOME TO" and menu options (1, 2, 3) displayed in yellow (ConsoleColor.Yellow)
 * Result: PASS - Logged in, used menu, logged out and returned to welcome screen
 * 
 * TC21: Color Formatting - Blue Banner
 * Input: ShowAnimatedBanner() called
 * Expected Output: Banner displayed in blue (ConsoleColor.Blue), color reset after banner
 * Result:  PASS - Banner displayed in blue correctly then colour resetted for text displayed after
 * 
 * TC22: Color Formatting - Red Error Messages
 * Input: User enters invalid input
 * Expected Output: Error messages displayed in red (ConsoleColor.Red), color reset after message
 * Result: PASS - Error messages displayed in red correctly
 * 
 * TC23: Banner Animation Timing
 * Input: ShowAnimatedBanner() called with 6 lines in banner array
 * Expected Output: Each line printed with Thread.Sleep(80) between lines
 * Result: PASS – Display delay observed visually
 * 
 * TC24: Program Loop Never Exits (Except on Quit)
 * Input: User runs program and navigates menus multiple times
 * Expected Output: Program keeps running and returns to main menu until user selects option 3 (Quit)
 * Result: PASS – Program stayed open and returned to menu until option 3 selected
 * 
 * TC25: Press Enter key on main menu (empty input)
 * Input: Press Enter without typing anything
 * Expected Output: Should handle gracefully with error message
 * Result: FAIL - When I just pressed Enter without typing a number I just got "Error: Please enter a NUMBER, not letters."
 * IMPROVEMENT: Need to look at this and match error messages more accordingly and be prepared and have more detailed error messages ready for as many scenarios as i can think of.
 */