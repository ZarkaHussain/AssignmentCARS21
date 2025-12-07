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
