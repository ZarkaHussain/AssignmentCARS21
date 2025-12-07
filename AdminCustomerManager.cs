using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AssignmentCARS
{
    // this class is responsible for all admin side customer management features
    // Key focuses: data handling, robustness (error handling)
    // includes performance improvements like StringBuilder
    public static class AdminCustomerManager
    {
        // displays customer list for admin
        public static void ShowCustomers()
        {
            //this use of a (collection) dictionary gives a much faster lookup(compared to List) by CustomerID (0(1) access)

            Dictionary<string, Customer> customers = new();

            //small try/catch: only around loading customers
            // for improvement of robustness as safely handles file loading errors
            try
            {
                //load all customer data from binary storage
                customers = BinaryRepository.LoadAll();

            }
            catch (Exception ex)
            {
                // user friendly error message instead of crashing
                Console.WriteLine($"Error loading customers: {ex.Message}");
                UIHelper.Pause();
                return;
            }

            //defensive programming: safely handles empty dataset
            if (customers.Count == 0)
            {
                Console.WriteLine("No customers found.");
                UIHelper.Pause();
                return;
            }

            //LINQ used to sort customers by name in alphabetical order
            // makes UI easier to use, demonstrates algorithmic thinking
            List<Customer> list = customers.Values
                .OrderBy(c => c.Name)
                .ToList();

            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== CUSTOMER LIST ===\n");

                //StringBuilder added for performance(instead of string concatenation)
                //better performance when building large text output
                StringBuilder sb = new StringBuilder();

                //build in memeory menu efficiently
                for (int i = 0; i < list.Count; i++)
                    sb.AppendLine($"{i + 1}) {list[i].Name}");

                Console.Write(sb.ToString());

                int backOption = list.Count + 1;
                Console.WriteLine($"\n{backOption}) Back");

                Console.Write("\nInput a number to view that customer's details and press enter: ");

                //trims input to avoid whitespace bugs
                string input = Console.ReadLine()?.Trim() ?? "";

                //input validation for robustness, agian for prevention of crashes
                if (!int.TryParse(input, out int choice)
                    || choice < 1 || choice > backOption)
                {
                    Console.WriteLine("Invalid selection.");
                    UIHelper.Pause(); 
                    continue;
                }

                //allows user to return to previous menu safely
                if (choice == backOption)
                    return;

                //display selected customer's data
                ShowCustomerDetails(list[choice - 1]);
            }
        }

        // shows full details for a single customer
        private static void ShowCustomerDetails(Customer c)
        {
            Console.Clear();
            Console.WriteLine("=== CUSTOMER DETAILS ===\n");

            //stored data is structured so clearly displayed for admin
            Console.WriteLine($"Name: {c.Name}");
            Console.WriteLine($"Password: {c.Password}");
            Console.WriteLine($"Customer ID: {c.CustomerID}");
            Console.WriteLine($"Level: {c.Level}");

            Console.WriteLine("\nRental History:");

            //graceful handling if emtpy rental history
            if (c.RentalHistory.Count == 0)
                Console.WriteLine("  No rentals recorded.");
            else
                //loop through rental history list (ordered collection)
                foreach (var item in c.RentalHistory)
                    Console.WriteLine($"  - {item}");

            UIHelper.Pause();
        }

        //allows admin to safely delete customer
        public static void DeleteCustomer()
        {
            Dictionary<string, Customer> customers = new();

            // small try/catch for load
            //safe loading for customer data
            try
            {
                customers = BinaryRepository.LoadAll();

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading customers: {ex.Message}");
                UIHelper.Pause();
                return;
            }

            //defensive check avoids invalid operations
            if (customers.Count == 0)
            {
                Console.WriteLine("No customers to delete.");
                UIHelper.Pause();
                return;
            }

            //LINQ ordering for better user experience
            List<Customer> list = customers.Values
                .OrderBy(c => c.Name)
                .ToList();

            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== DELETE CUSTOMER ===\n");

                //StringBuilder added for performance when listing customers
                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < list.Count; i++)
                    sb.AppendLine($"{i + 1}) {list[i].Name}");

                Console.Write(sb.ToString());

                int backOption = list.Count + 1;
                Console.WriteLine($"\n{backOption}) Back");

                Console.Write("\nSelect a customer to delete: ");

                string input = Console.ReadLine()?.Trim() ?? "";

                //robust input validation
                if (!int.TryParse(input, out int choice)
                    || choice < 1 || choice > backOption)
                {
                    Console.WriteLine("Invalid selection.");
                    UIHelper.Pause();
                    continue;
                }

                //safe exit option
                if (choice == backOption)
                    return;

                Customer target = list[choice - 1];

                //warning using colours improves UX (highlights warnings clearly)
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\nAre you sure you want to delete '{target.Name}'?");
                Console.Write("Type YES to confirm: ");
                Console.ResetColor();


                //double confirmation to avoid deletion by accident 
                string confirm = (Console.ReadLine() ?? "").Trim().ToUpper();

                if (confirm != "YES")
                {
                    Console.WriteLine("Cancelled.");
                    UIHelper.Pause();
                    return;
                }

                //file path maps directly to binary stored customer
                string filePath = $"customers/{target.CustomerID}.dat";

                //small try/catch around actual file deletion, improves robustness as it has specific exception handling
                try
                {
                    if (File.Exists(filePath))
                        File.Delete(filePath);
                }
                catch (UnauthorizedAccessException uae)
                {
                    Console.WriteLine($"Permission denied deleting file: {uae.Message}");
                    UIHelper.Pause();
                    return;
                }
                catch (IOException ioEx)
                {
                    Console.WriteLine($"I/O error deleting file: {ioEx.Message}");
                    UIHelper.Pause();
                    return;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Unexpected error deleting file: {ex.Message}");
                    UIHelper.Pause();
                    return;
                }

                // remove deleted customer from in-memory collection
                customers.Remove(target.CustomerID);

                // small try/catch around SaveAll
                //saves updated data back to persistent storage
                try
                {
                    BinaryRepository.SaveAll(customers);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error saving updated customer list: {ex.Message}");
                    UIHelper.Pause();
                    return;
                }

                //success messge improves user feedback
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\nCustomer '{target.Name}' deleted successfully!");
                Console.ResetColor();
                UIHelper.Pause();
                return;
            }
        }

        
    }
}
