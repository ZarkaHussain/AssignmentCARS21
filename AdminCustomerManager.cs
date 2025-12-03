using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AssignmentCARS
{
    public static class AdminCustomerManager
    {
        public static void ShowCustomers()
        {
            Dictionary<string, Customer> customers = new();

            //small try/catch: only around loading customers
            try
            {
                customers = BinaryRepository.LoadAll();

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading customers: {ex.Message}");
                Pause();
                return;
            }

            if (customers.Count == 0)
            {
                Console.WriteLine("No customers found.");
                Pause();
                return;
            }

            List<Customer> list = customers.Values
                .OrderBy(c => c.Name)
                .ToList();

            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== CUSTOMER LIST ===\n");

                for (int i = 0; i < list.Count; i++)
                    Console.WriteLine($"{i + 1}) {list[i].Name}");

                int backOption = list.Count + 1;
                Console.WriteLine($"\n{backOption}) Back");

                Console.Write("\nSelect a customer to view more details: ");

                char key = Console.ReadKey(true).KeyChar;
                if (!int.TryParse(key.ToString(), out int choice)

                    || choice < 1 || choice > backOption)
                {
                    Console.WriteLine("Invalid selection.");
                    Pause();
                    continue;
                }

                if (choice == backOption)
                    return;

                ShowCustomerDetails(list[choice - 1]);
            }
        }

        private static void ShowCustomerDetails(Customer c)
        {
            Console.Clear();
            Console.WriteLine("=== CUSTOMER DETAILS ===\n");

            Console.WriteLine($"Name: {c.Name}");
            Console.WriteLine($"Password: {c.Password}");
            Console.WriteLine($"Customer ID: {c.CustomerID}");
            Console.WriteLine($"Level: {c.Level}");

            Console.WriteLine("\nRental History:");

            if (c.RentalHistory.Count == 0)
                Console.WriteLine("  No rentals recorded.");
            else
                foreach (var item in c.RentalHistory)
                    Console.WriteLine($"  - {item}");

            Pause();
        }

        public static void DeleteCustomer()
        {
            Dictionary<string, Customer> customers = new();

            // small try/catch for load
            try
            {
                customers = BinaryRepository.LoadAll();
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading customers: {ex.Message}");
                Pause();
                return;
            }

            if (customers.Count == 0)
            {
                Console.WriteLine("No customers to delete.");
                Pause();
                return;
            }

            List<Customer> list = customers.Values
                .OrderBy(c => c.Name)
                .ToList();

            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== DELETE CUSTOMER ===\n");

                for (int i = 0; i < list.Count; i++)
                    Console.WriteLine($"{i + 1}) {list[i].Name}");

                int backOption = list.Count + 1;
                Console.WriteLine($"\n{backOption}) Back");

                Console.Write("\nSelect a customer to delete: ");

                char key = Console.ReadKey(true).KeyChar;
                if (!int.TryParse(key.ToString(), out int choice)
                || choice < 1 || choice > backOption)
                {
                    Console.WriteLine("Invalid selection.");
                    Pause();
                    continue;
                }

                if (choice == backOption)
                    return;

                Customer target = list[choice - 1]; 

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\nAre you sure you want to delete '{target.Name}'?");
                Console.Write("Type YES to confirm: ");
                Console.ResetColor();

                string confirm = (Console.ReadLine()??"").Trim().ToUpper();

                if (confirm != "YES")
                {
                    Console.WriteLine("Cancelled.");
                    Pause();
                    return;
                }

                string filePath = $"customers/{target.CustomerID}.dat";

                // small try/catch around actual file deletion
                try
                {
                    if (File.Exists(filePath))
                        File.Delete(filePath);
                }
                catch (UnauthorizedAccessException uae)
                {
                    Console.WriteLine($"Permission denied deleting file: {uae.Message}");
                    Pause();
                    return;
                }
                catch (IOException ioEx)
                {
                    Console.WriteLine($"I/O error deleting file: {ioEx.Message}");
                    Pause();
                    return;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Unexpected error deleting file: {ex.Message}");
                    Pause();
                    return;
                }

                // remove from dictionary
                customers.Remove(target.CustomerID);

                // small try/catch around SaveAll
                try
                {
                    BinaryRepository.SaveAll(customers);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error saving updated customer list: {ex.Message}");
                    Pause();
                    return;
                }

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\nCustomer '{target.Name}' deleted successfully!");
                Console.ResetColor();
                Pause();
                return;
            }
        }

        private static void Pause()
        {
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
    }
}
