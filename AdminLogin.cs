using System;
using System.Collections.Generic;

namespace AssignmentCARS
{
    public class AdminLogin
    {
        //private admin credentials
        private readonly string _adminUser = "admin";
        private readonly string _adminPass = "1234";

        public void Run(string[] args)
        {
            string command = args[0].ToLower();

            if (command != "admin")
            {
                Console.WriteLine("Unknown command: " + command);
                return;
            }

            if (args.Length < 3)
            {
                Console.WriteLine("Usage: AssignmentCARS.exe admin <username> <password>");
                return;
            }

            string user = args[1];
            string pass = args[2];

            //command-line authentication
            if (user != _adminUser || pass != _adminPass)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid admin login.");
                Console.ResetColor();
                return;
            }

            ShowDashboard();
        }

        private void ShowDashboard()
        {
            var customers = BinaryRepository.Load("customer.dat")
                ?? new Dictionary<string, Customer>();

            int totalRentals = 0;
            Dictionary<string, int> rentalCounts = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

            // Loop through ALL customers
            foreach (var customer in customers.Values)
            {
                foreach (var rental in customer.RentalHistory)
                {
                    totalRentals++;

                    if (rentalCounts.ContainsKey(rental))
                        rentalCounts[rental]++;
                    else
                        rentalCounts[rental] = 1;
                }
            }

            // Find most rented car
            var mostRented = rentalCounts.Count > 0
                ? rentalCounts.MaxBy(kvp => kvp.Value)
                : new KeyValuePair<string, int>("None", 0);

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("=== ADMIN DASHBOARD ===\n");
            Console.ResetColor();

           
            Console.WriteLine($"Total number of cars rented in the system: {totalRentals}");
            Console.WriteLine($"Most rented car: {mostRented.Key} ({mostRented.Value} times)");

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

    }
}


