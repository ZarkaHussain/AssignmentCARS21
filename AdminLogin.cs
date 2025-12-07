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




