using System;
using System.Threading;

namespace AssignmentCARS
{
    public static class MainMenu
    {
        public static void Show(Customer customer, Action save)
        {
            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("===== HOME MENU =====");
                Console.ResetColor();
                Console.WriteLine($"\nUser: {customer.Name}  Level: {LevelName(customer.Level)}\n");

                MenuOption(1, "Rent a Car");
                MenuOption(2, "View Rental History");
                MenuOption(3, "Show Account Info");
                MenuOption(4, "Logout");
                MenuOption(5, "Quit Program");
                Console.Write("\nChoose an option: ");

                char key = Console.ReadKey(true).KeyChar;
                string option = key.ToString();
                Console.WriteLine(key);
                //string option = Console.ReadLine()?.Trim() ?? "";
                Console.Clear();

                switch (option)
                {
                    case "1":
                        customer = RentCar.Show(customer, save);
                        save();                       // save upgraded customer
                        break;

                    case "2":
                        PrintRentalHistory(customer);
                        Pause();
                        break;

                    case "3":
                        ShowAccountInfo(customer);
                        break;

                    case "4":
                        Console.WriteLine("Logging out...");
                        Thread.Sleep(700);
                        return;

                    case "5":
                        Console.WriteLine("Thank you for using Car World!");
                        Thread.Sleep(1500);
                        Environment.Exit(0);
                        break;

                    default:
                        Console.WriteLine("Invalid option.");
                        Pause();
                        break;
                }
            }
        }

        private static void MenuOption(int number, string text)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(number);
            Console.ResetColor();
            Console.WriteLine($") {text}");
        }


        private static void PrintRentalHistory(Customer customer)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("===== RENTAL HISTORY ===== ");
            Console.WriteLine();
            Console.ResetColor();

            if (customer.RentalHistory.Count == 0)
            {
                Console.WriteLine("  None");
                return;
            }

            foreach (var rental in customer.RentalHistory)
            {
                Console.WriteLine($"  - {rental}");
            }
        }


        private static void ShowAccountInfo(Customer customer)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("===== ACCOUNT INFORMATION =====\n");
            Console.ResetColor();

            Console.WriteLine("Customer Name: {0}", customer.Name);
            Console.WriteLine("Password: {0}", customer.Password);
            Console.WriteLine("Level: {0}", LevelName(customer.Level));
            Console.WriteLine();

            PrintRentalHistory(customer);
            Pause();
        }

        private static string LevelName(int level) => level switch
        {
            10 => "VIP",
            5 => "Premium",
            _ => "Standard"
        };

        private static void Pause()
        {
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
    }
}
