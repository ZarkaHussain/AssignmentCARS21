using System;
using System.Threading;

namespace AssignmentCARS
{
    //MainMenu is responsible for showing main user interface after succesful login
    //this class focuses on User experience, robustness(input handling), Obejct oriented design as it works with customer object
    public static class MainMenu
    {
        //displays main menu and keeps running until user logs out or quits
        //save Action injected so this class doesnt depend directly on file logic
        public static void Show(Customer customer, Action save)
        {
            //infinite loop so keeps menu running until user chooses to exit
            while (true)
            {
                Console.Clear();
                //colour added to improve UI/UX and makes headings stand out better
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("===== HOME MENU =====");
                Console.ResetColor();
                //displays live user data from Customer object
                Console.WriteLine($"\nUser: {customer.Name}  Level: {LevelName(customer.Level)}\n");

                //modular method used to print menu options
                MenuOption(1, "Rent a Car");
                MenuOption(2, "View Rental History");
                MenuOption(3, "Show Account Info");
                MenuOption(4, "Logout");
                MenuOption(5, "Quit Program");

                Console.Write("\nChoose an option: ");

                //reads single key input instead of full string for faster interaction
                char key = Console.ReadKey(true).KeyChar;
                string option = key.ToString();
                Console.WriteLine(key);
                //string option = Console.ReadLine()?.Trim() ?? "";
                Console.Clear();

                //switch statement used for clean control flow, better than long if/else chains
                switch (option)
                {
                    case "1":
                        customer = RentCar.Show(customer, save); //RentCar.Show may return ugraded customer (OOP polymorphism)
                        save();                       // save updated customer data
                        break;

                    case "2":
                        PrintRentalHistory(customer); //shows rental hsitory
                        UIHelper.Pause();  //uses centralised pause method
                        break;

                    case "3":
                        ShowAccountInfo(customer); //displays full account info
                        break;

                    case "4":
                        Console.WriteLine("Logging out...");
                        Thread.Sleep(700); //small delay for better UX
                        return;

                    case "5":
                        Console.WriteLine("Thank you for using Car World!");
                        Thread.Sleep(1500);
                        Environment.Exit(0); //force program exit
                        break;

                    default: 
                        Console.WriteLine("Invalid option."); //defensive programming to handle invalid user input
                        UIHelper.Pause();
                        break;
                }
            }
        }

        //small helper method to keep menu formatting consistent
        private static void MenuOption(int number, string text)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(number);
            Console.ResetColor();
            Console.WriteLine($") {text}");
        }

        //displays rental history of user
        //demonstrates data handling and writing fast code
        private static void PrintRentalHistory(Customer customer)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("===== RENTAL HISTORY ===== ");
            Console.WriteLine();
            Console.ResetColor();

            //defensive check to avoid null or empty data errors
            if (customer.RentalHistory.Count == 0)
            {
                Console.WriteLine("  None");
                return;
            }

            //Performance test improvement
            //StringBuilder is used isntead of string concatenation in loops
            //avoids unnecessary memory allocation and improves speed
            var sb = new System.Text.StringBuilder();
            foreach (var rental in customer.RentalHistory)
                sb.AppendLine($" - {rental}");
            Console.Write(sb.ToString());            
        }

        //displays user account info
        //uses encapsulated Customer object
        private static void ShowAccountInfo(Customer customer)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("===== ACCOUNT INFORMATION =====\n");
            Console.ResetColor();

            Console.WriteLine("Customer Name: {0}", customer.Name);
            Console.WriteLine("Password: {0}", customer.Password);
            Console.WriteLine("Level: {0}", LevelName(customer.Level));
            Console.WriteLine();

            //reuses existing method to avoid duplicated logic 
            PrintRentalHistory(customer);
            UIHelper.Pause();
        }

        private static string LevelName(int level) => level switch
        {
            10 => "VIP",
            5 => "Premium",
            _ => "Standard"
        };
    }
}


/*
 * TEST CASES FOR MAIN MENU:
 * 
 * TC1: Display Menu for Standard Customer
 * Input: Customer with Name="John Den", Level=1
 * Expected Output: Displays "User: John Den  Level: Standard" and 5 menu options
 * Result: PASS - Displays correct message and 5 menu options
 * 
 * TC2: Display Menu for Premium Customer
 * Input: Customer with Name="Alicia Smyth", Level=5
 * Expected Output: Displays "User: Alicia Smyth  Level: Premium" and 5 menu options
 * Result: PASS - Displays correct message and 5 menu options
 * 
 * TC3: Display Menu for VIP Customer
 * Input: Customer with Name="Bob Jones", Level=10
 * Expected Output: Displays "User: Bob Jones  Level: VIP" and 5 menu options
 * Result: PASS - Displays correct message and 5 menu options
 * 
 * TC4: Select Option 1 - Rent a Car
 * Input: User presses '1'
 * Expected Output: Calls RentCar.Show(), may return upgraded customer object, calls save() to persist changes
 * Result: PASS - Displays cars available to rent and if level changes after rental confirmed then upgraded customer object returned and displayed on main menu
 * 
 * TC5: Select Option 2 - View Rental History with Rentals
 * Input: User presses '2', customer has 3 rentals in history
 * Expected Output: Displays "===== RENTAL HISTORY =====" and lists all 3 rentals with " - " prefix, pauses
 * Result: PASS - Correct number of rentals displayed
 * 
 * TC6: Select Option 2 - View Rental History (empty Rental History)
 * Input: User presses '2', customer has RentalHistory.Count = 0
 * Expected Output: Displays "===== RENTAL HISTORY =====" and "  None", pauses
 * Result: PASS - Displays "None"
 * 
 * TC7: Select Option 3 - Show Account Info
 * Input: User presses '3'
 * Expected Output: Displays "===== ACCOUNT INFORMATION =====", shows Name, Password, Level, and rental history, pauses
 * Result: PASS - Displays account information correctly
 * 
 * TC8: Select Option 4 - Logout
 * Input: User presses '4'
 * Expected Output: Displays "Logging out...", sleeps for 700ms, returns from Show() method (exits to welcome screen)
 * Result: PASS - Displays log out message and then returns to welcome screen
 * 
 * TC9: Select Option 5 - Quit Program
 * Input: User presses '5'
 * Expected Output: Displays "Thank you for using Car World!", sleeps for 1500ms, calls Environment.Exit(0) (terminates program)
 * Result: PASS - Correct message displayed and program terminates
 * 
 * TC10: Invalid Option - Letter
 * Input: User presses 'x' or any letter
 * Expected Output: Displays "Invalid option.", pauses, returns to main menu
 * Result: PASS - Displays "Invalid option"
 * 
 * TC11: Invalid Option - Out of Range Number
 * Input: User presses '9' or any number not 1-5
 * Expected Output: Displays "Invalid option.", pauses, returns to main menu
 * Result: PASS - Displays "Invalid option", pauses then returns to main menu
 * 
 * TC12: MenuOption Helper Method
 * Input: MenuOption(1, "Rent a Car")
 * Expected Output: Displays "1" in yellow, then ") Rent a Car" in default color
 * Result: PASS - Displayed correctly
 * 
 * TC13: PrintRentalHistory with multiple rentals
 * Input: Customer with RentalHistory = ["Toyota Corolla", "BMW 3 Series", "Ferrari F8"]
 * Expected Output: Displays header and then " - Toyota Corolla\n - BMW 3 Series\n - Ferrari F8" using StringBuilder
 * Result: PASS - Rental History with correct multiple rentals displayed
 * 
 * TC14: ShowAccountInfo - complete display
 * Input: Customer with Name="John", Password="passwrd123456789", Level=5, 2 rentals
 * Expected Output: Displays account header, "Customer Name: John", "Password: passwrd123456789", "Level: Premium", then rental history section
 * Result: PASS - All account info displayed correctly
 * 
 * TC15: Customer upgrade after rental
 * Input: Customer with 4 rentals selects Option 1, rents 5th car (upgrades to Premium)
 * Expected Output: RentCar.Show() returns upgraded customer (Level=5), save() called, menu displays "Level: Premium" on next loop
 * Result: PASS - Correctly upgrades customer after 5th rental
 * 
 * 
 * TC17: Single key input reading
 * Input: User presses key '3' (not Enter needed)
 * Expected Output: Console.ReadKey(true).KeyChar captures '3', converted to string "3", processed immediately
 * Result: Successfully processed
 * 
 * TC18: Menu loop behavior
 * Input: User selects Option 2, views history, returns to menu, then selects Option 4
 * Expected Output: Menu displays again after Option 2 completes, then logs out on Option 4
 * Result: PASS - Loop is correct
 * 
 * TC19: Large Rental History Display
 * Input: Customer with 20 rental entries in RentalHistory
 * Expected Output: All rentals displayed without crashing or freezing
 * Result: PASS – All rentals displayed correctly and program remained responsive
 * 
 * TC20: Save Action Called After Rental
 * Input: User rents car via Option 1
 * Expected Output: save() action called after RentCar.Show() returns, ensuring customer data persisted to file
 * Result: PASS - Rental saved to customer's file
 * 
 * TC21: Autotmatically log out after inactivity
 * Input: User leaves the program untouched for more than 10 minutes without pressing any keys
 * Expected Output: User is automatically logged out and returned to the main welcome log in/signup screen
 * Result: FAIL - Feature not yet added. Menu remains open, shows the system could be improved and the inactivity timeout will be better for security too.
 * 
 */