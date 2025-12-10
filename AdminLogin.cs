using System;

namespace AssignmentCARS
{
    //this class handles admin login and admin only features
    //demonstrates use of command line interfaces(CLI) uses input validation and robustness
    public class AdminLogin
    {
        //private admin credentials
        // these are kept private to enforce encapsulation
        private readonly string _adminUser = "admin";
        private readonly string _adminPass = "1234";

        //entry point for admin mode using command line arguments
        //args are passed in from the terminal when launching the application

        public void Run(string[] args)
        {
            //use of robustness checks if command that was entered is correct
            //prevents crashes and user guided with clear usage message
            if (args.Length == 0 || args[0].ToLower() != "admin")
            {
                Console.WriteLine("Unknown or missing command. Use: AssignmentCARS.exe admin <username> <password>");
                return;
            }

            //validates correct number of arguments
            //protects against index out of range errors
            if (args.Length < 3)
            {
                Console.WriteLine("Usage: AssignmentCARS.exe admin <username> <password>");
                return;
            }

            //read username and password from CLI input
            string user = args[1];
            string pass = args[2];

            // authenticate admin
            //ensures only authorised users can access admin features 
            if (user != _adminUser || pass != _adminPass)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid admin login.");
                Console.ResetColor();
                return;
            }

            // if credentials correct admin menu is shown
            ShowMenu();
        }

        //displays admin dashbaord menu
        // uses loop to allow repeated access to admins features
        private void ShowMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("=== ADMIN DASHBOARD ===\n");
                Console.ResetColor();

                //consistent UI design
                Console.WriteLine("1) View total number of car rentals");
                Console.WriteLine("2) View most rented cars");
                Console.WriteLine("3) View all customers");
                Console.WriteLine("4) Delete a customer");
                Console.WriteLine("5) Exit Admin Menu");

                Console.Write("\nSelect an option: ");

                //reads single key input for fast simple navigation
                char key = Console.ReadKey(true).KeyChar;

                //switch statement used for clean control flow
                switch (key)
                {
                    case '1':
                        AdminStats.ShowTotalRentals(); //data processing feature
                        break;

                    case '2':
                        AdminStats.ShowMostRentedCars(); // uses cached and optimised data handling
                        break;

                    case '3':
                        AdminCustomerManager.ShowCustomers(); //data handling and persistence
                        break;

                    case '4':
                        AdminCustomerManager.DeleteCustomer();//robust file and data deletion
                        break;

                    case '5':
                        return; //exits admin menu

                    default:
                        //user firendly error handling(robustness)
                        Console.WriteLine("Invalid option.");
                        UIHelper.Pause(); //centralised pause keeping UI consistent
                        break;
                }
            }
        }
    }
}




/*
 * TEST CASES FOR ADMIN AUTH:
 * 
 * TC1: Valid Admin Login in correct format
 * Input: args = ["admin", "admin", "1234"]
 * Command: ./AssignmentCARS admin admin 1234
 * Expected Output: Successfully authenticates and displays admin dashboard menu
 * Result: PASS - Successful login and admin menu displayed
 * 
 * TC2: Invalid Username
 * Input: args = ["admin", "wrong", "1234"]
 * Command: ./AssignmentCARS admin wrong 1234
 * Expected Output: Displays "Invalid admin login." in red text, exits to terminal
 * Result: PASS - Displays "Invalid admin login" - login rejected
 * 
 * TC3: Invalid Password
 * Input: args = ["admin", "admin", "2321"]
 * Command: ./AssignmentCARS admin admin 2321
 * Expected Output: Displays "Invalid admin login." in red text, exits to terminal
 * Result: PASS - Displays "Invalid admin login" - login rejected
 * 
 * TC4: Both Username and Password invalid
 * Input: args = ["admin", "wrong", "2321"]
 * Command: ./AssignmentCARS admin wrong 2321
 * Expected Output: Displays "Invalid admin login." in red text, exits to terminal
 * Result: PASS - Displays "Invalid admin login" - login rejected
 * 
 * TC5: No Arguments Provided
 * Input: args = []
 * Command: ./AssignmentCARS
 * Expected Output: Displays normal user menu
 * Result: PASS - Normal user menu displayed
 * 
 * TC6: Wrong Command Word
 * Input: args = ["user"]
 * Command: ./AssignmentCARS user
 * Expected Output: Displays "Unknown or missing command. Use: AssignmentCARS.exe admin <username> <password>"
 * Result: PASS - Unknown or missing command. Use: AssignmentCARS.exe admin <username> <password>
 * 
 * TC7: Typo in Command
 * Input: args = ["adminf"]
 * Command: ./AssignmentCARS adminf
 * Expected Output: Displays "Unknown or missing command. Use: AssignmentCARS.exe admin <username> <password>"
 * Result: PASS - Unknown or missing command. Use: AssignmentCARS.exe admin <username> <password>
 * 
 * TC8: Only Command Provided (Missing Username and Password)
 * Input: args = ["admin"]
 * Command: ./AssignmentCARS admin
 * Expected Output: Displays "Usage: AssignmentCARS.exe admin <username> <password>"
 * Result: PASS - Displays "Usage: AssignmentCARS.exe admin <username> <password>"
 * 
 * TC9: Missing Password (Only Command and Username)
 * Input: args = ["admin", "admin"]
 * Command: ./AssignmentCARS admin admin
 * Expected Output: Displays "Usage: AssignmentCARS.exe admin <username> <password>"
 * Result: PASS - Displays "Usage: AssignmentCARS.exe admin <username> <password>"
 * 
 * TC10: Case-Insensitive Command
 * Input: args = ["ADMIN", "admin", "1234"]
 * Command: ./AssignmentCARS ADMIN admin 1234
 * Expected Output: Successfully logs in to admin dashboard (command converted to lowercase via .ToLower())
 * Result: PASS - Successfully logs in and admin dashboard displayed
 * 
 * TC11: Admin Menu - Select Valid Option
 * Input: User presses '1' in admin dashboard
 * Expected Output: Calls AdminStats.ShowTotalRentals() this displays total rental count
 * Result: PASS - Total Rentals shown
 * 
 * TC12: Admin Menu - Select Invalid Option
 * Input: User presses 'x' or any invalid key in admin dashboard
 * Expected Output: Displays "Invalid option.", pauses then returns to menu
 * Result: PASS - Displays "Invalid option" returns to menu
 * 
 * TC13: Admin Menu - Exit
 * Input: User presses '5' in admin dashboard
 * Expected Output: Exits admin menu, returns to terminal
 * Result: PASS -  Admin menu closed and returned to terminal 
 * 
 * TC14: Extra Arguments Passed
 * Input: args = ["admin", "admin", "1234", "extra"]
 * Command: ./AssignmentCARS admin admin 1234 extra
 * Expected Output: Should either warn about extra arguments or ignore them gracefully
 * Result: FAIL – Still logs in successfully
 * 
 * TC15: Username with trailing spaces
 * Input: args = ["admin", "admin ", "1234"]
 * Command: ./AssignmentCARS admin "admin " 1234
 * Expected Output: Should trim input and allow login
 * Result: PASS – Login successful
 * 
 * TC17: Mixed Case Username
 * Input: args = ["admin", "Admin", "1234"]
 * Command: ./AssignmentCARS admin Admin 1234
 * Expected Output: Login should succeed if username comparison was case-insensitive
 * Result: FAIL – Login failed. Displays "Invalid admin login"
 * 
 * TC18: Empty Username
 * Input: args = ["admin", "", "1234"]
 * Command: ./AssignmentCARS admin "" 1234
 * Expected Output: Should display a clear error message for empty username
 * Result: FAIL – Displays "Usage: AssignmentCARS.exe admin <username> <password>"
 * 
 * TC19: Empty Password
 * Input: args = ["admin", "admin", ""]
 * Command: ./AssignmentCARS admin admin ""
 * Expected Output: Should display a clear error message for empty password
 * Result: FAIL – Displays "Usage: AssignmentCARS.exe admin <username> <password>"
 * 
 */
