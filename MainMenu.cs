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
                Console.WriteLine("1) Rent a Car");
                Console.WriteLine("2) View Rental History");
                Console.WriteLine("3) Show Account Info");
                Console.WriteLine("4) Logout");
                Console.WriteLine("5) Quit Program");
                Console.Write("\nChoose an option: ");
                string option = Console.ReadLine()?.Trim() ?? "";
                Console.Clear();

                switch (option)
                {
                    case "1":
                        customer = RentCar.Show(customer, save);
                        save();                       // Save upgraded customer
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

        private static void PrintRentalHistory(Customer customer)
        {
            Console.WriteLine("Rental History:{0}",
                customer.RentalHistory.Count > 0
                ? string.Join(", ", customer.RentalHistory)
                : "None");
        }

        private static void ShowAccountInfo(Customer customer)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("===== ACCOUNT INFORMATION =====\n");
            Console.ResetColor();

            Console.WriteLine("Customer Name: {0}", customer.Name);
            Console.WriteLine("Password: {0}", customer.Password);
            Console.WriteLine("Level: {0}", LevelName(customer.Level));

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
