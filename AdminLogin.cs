using System;

namespace AssignmentCARS
{
    public class AdminLogin
    {
        //private admin credentials
        private readonly string _adminUser = "admin";
        private readonly string _adminPass = "1234";

        public void Run(string[] args)
        {
            if (args.Length == 0 || args[0].ToLower() != "admin")
            {
                Console.WriteLine("Unknown or missing command. Use: AssignmentCARS.exe admin <username> <password>");
                return;
            }

            if (args.Length < 3)
            {
                Console.WriteLine("Usage: AssignmentCARS.exe admin <username> <password>");
                return;
            }

            string user = args[1];
            string pass = args[2];

            // authenticate admin
            if (user != _adminUser || pass != _adminPass)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid admin login.");
                Console.ResetColor();
                return;
            }

            ShowMenu();
        }

        private void ShowMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("=== ADMIN DASHBOARD ===\n");
                Console.ResetColor();

                Console.WriteLine("1) View total number of car rentals");
                Console.WriteLine("2) View most rented cars");
                Console.WriteLine("3) View all customers");
                Console.WriteLine("4) Delete a customer");
                Console.WriteLine("5) Exit Admin Menu");

                Console.Write("\nSelect an option: ");

                char key = Console.ReadKey(true).KeyChar;

                switch (key)
                {
                    case '1':
                        AdminStats.ShowTotalRentals();
                        break;

                    case '2':
                        AdminStats.ShowMostRentedCars();
                        break;

                    case '3':
                        AdminCustomerManager.ShowCustomers();
                        break;

                    case '4':
                        AdminCustomerManager.DeleteCustomer();
                        break;

                    case '5':
                        return;

                    default:
                        Console.WriteLine("Invalid option.");
                        Pause();
                        break;
                }
            }
        }

        private void Pause()
        {
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
    }
}




//using System;
//using System.Collections.Generic;
//using System.Linq;

//namespace AssignmentCARS
//{
//    public class AdminLogin
//    {
//        //private admin credentials
//        private readonly string _adminUser = "admin";
//        private readonly string _adminPass = "1234";

//        public void Run(string[] args)
//        {
//            if (args.Length == 0 || args[0].ToLower() != "admin")
//            {
//                Console.WriteLine("Unknown or missing command. Use: AssignmentCARS.exe admin <username> <password>");
//                return;
//            }

//            if (args.Length < 3)
//            {
//                Console.WriteLine("Usage: AssignmentCARS.exe admin <username> <password>");
//                return;
//            }

//            string user = args[1];
//            string pass = args[2];

//            // Authenticate admin
//            if (user != _adminUser || pass != _adminPass)
//            {
//                Console.ForegroundColor = ConsoleColor.Red;
//                Console.WriteLine("Invalid admin login.");
//                Console.ResetColor();
//                return;
//            }

//            ShowDashboard();
//        }

//        private void ShowDashboard()
//        {
//            // 🔥 Load all customers from /customers/*.dat
//            var customers = BinaryRepository.LoadAll()
//                ?? new Dictionary<string, Customer>();

//            int totalRentals = 0;
//            Dictionary<string, int> rentalCounts = new(StringComparer.OrdinalIgnoreCase);

//            // Count rentals across all customers
//            foreach (var customer in customers.Values)
//            {
//                foreach (var rental in customer.RentalHistory)
//                {
//                    totalRentals++;

//                    if (rentalCounts.ContainsKey(rental))
//                        rentalCounts[rental]++;
//                    else
//                        rentalCounts[rental] = 1;
//                }
//            }

//            // Determine most rented car
//            var mostRented = rentalCounts.Count > 0
//                ? rentalCounts.MaxBy(kvp => kvp.Value)
//                : new KeyValuePair<string, int>("None", 0);

//            Console.Clear();
//            Console.ForegroundColor = ConsoleColor.Cyan;
//            Console.WriteLine("=== ADMIN DASHBOARD ===\n");
//            Console.ResetColor();

//            Console.WriteLine($"Total number of cars rented in the system: {totalRentals}");
//            Console.WriteLine($"Most rented car: {mostRented.Key} ({mostRented.Value} times)");

//            Console.WriteLine("\nPress any key to exit...");
//            Console.ReadKey();
//        }
//    }
//}
