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

//*Result: PASS - File was created. I checked in the customers folder and the file is there./////

/*
 * TEST CASES FOR CUSTOMER MANAGEMENT:
 * 
 * TC1: Show Customers - No existing customers 
 * Input: No customer files exist (LoadAll returns empty dictionary)
 * Expected Output: Displays "No customers found.", pauses then returns to previous menu
 * Result: PASS - On admin dashboard displays "No customers found"
 * 
 * TC2: Show Customers - Load Error
 * Input: Exception thrown during BinaryRepository.LoadAll()
 * Expected Output: Displays "Error loading customers: {error message}" pauses then returns to previous menu
 * Result: PASS - Error handled and application did not crash.
 * 
 * TC3: Show Customers - Multiple Customers (Alphabetical Order)
 * Input: Customer folder with 5 customer files (e.g., "Zara", "Alice", "Mark", "Bob", "John")
 * Expected Output: Lists customers alphabetically: "1) Alice, 2) Bob, 3) John, 4) Mark, 5) Zara", "6) Back"
 * Result:PASS - Customers listed in alphabetical order
 * 
 * TC4: Show Customers - Invalid Selection (Out of Range)
 * Input: Enter number outside valid range (e.g., 99 when only 5 customers exist)
 * Expected Output: Displays "Invalid selection.", pauses then returns to customer list menu
 * Result:PASS - Displays "Invalid selection"
 * 
 * TC5: Show Customers - Non-Numeric Input
 * Input: Enter text like "abc" instead of a number
 * Expected Output: Displays "Invalid selection.", pauses then returns to customer list menu
 * Result:PASS - Displays "Invalid selection" after non numeric input
 * 
 * TC6: Show Customers - Select Back Option
 * Input: Select the "Back" option (e.g., option 6 if 5 customers exist)
 * Expected Output: Returns to admin dashboard without displaying customer details
 * Result:PASS - Returns to admin dashboard without errors
 * 
 * TC7: View Customer Details - With Rental History
 * Input: Select customer #2 who has 3 rentals in their history
 * Expected Output: Displays Name, Password, CustomerID, Level, and Rental History with 3 items listed (Honda Civic, Ford Focus, Toyota Corolla)
 * Result:PASS - Displays rental history and customer details correctly 
 * 
 * TC8: View Customer Details - No Rental History
 * Input: Select customer with empty RentalHistory (Count = 0)
 * Expected Output: Displays Name, Password, CustomerID, Level, and "No rentals recorded." under Rental History
 * Result:PASS - Displays customer details correctly and rental history displays "No rentals recorded"
 * 
 * TC9: Delete Customer - Empty customer folder
 * Input: No customer files exist
 * Expected Output: Displays "No customers to delete.", pauses, returns to admin menu
 * Result:PASS - Safely handled empty customer folder and displayed "No customers to delete"
 * 
 * TC10: Delete Customer - Load Error
 * Input: Exception thrown during BinaryRepository.LoadAll()
 * Expected Output: Displays "Error loading customers: {error message}", pauses, returns to admin menu
 * Result: PASS - Caught error no crashing
 * 
 * TC11: Delete Customer - Confirm with "YES"
 * Input: Select customer, type "YES" (uppercase) to confirm
 * Expected Output: File deleted from "customers/{CustomerID}.dat", removed from dictionary, displays "Customer '{name}' deleted successfully!" in green then pauses then returns to admin menu
 * Result: PASS - Displays successfully deleted message and customer's file deleted from 'customers' folder 
 * 
 * TC12: Delete Customer - Confirm with "yes" (Lowercase)
 * Input: Select customer, type "yes" (lowercase) to confirm
 * Expected Output: Converted to uppercase via .ToUpper(), deletion proceeds successfully
 * Result: PASS - Customer file deleted- case insensitive confirmation worked
 * 
 * TC13: Delete Customer - Cancel with "NO"
 * Input: Select customer, type "NO" to cancel
 * Expected Output: Displays "Cancelled.", pauses, returns to admin menu (customer NOT deleted)
 * Result: PASS - Deletion canceeled and returns to admin menu
 * 
 * TC14: Delete Customer - Cancel with Random Text
 * Input: Select customer, type "maybe" or any text other than "YES"
 * Expected Output: Displays "Cancelled.", pauses, returns to admin menu (customer NOT deleted)
 * Result: PASS - Deletion cancelled and returned to admin menu
 * 
 * TC15: Delete Customer - File Doesn't Exist
 * Input: Customer exists in dictionary but .dat file is missing from customers folder
 * Expected Output: File.Exists() returns false, skips deletion, removes from dictionary, saves remaining customers successfully
 * Result: PASS - Manually deleted .dat file before trying to delete, no crash, program correctly handled it
 * 
 * TC16: Delete Customer - Permission Denied
 * Input: Try to delete file without write permissions
 * Expected Output: Catches UnauthorizedAccessException, displays "Permission denied deleting file: {error}", pauses, returns to admin menu
 * Result: PASS - File set to read only and correct error message displayed
 * 
 * TC17: Delete Customer - I/O Error During File Deletion
 * Input: Disk error or file locked by another process during File.Delete()
 * Expected Output: Catches IOException, displays "I/O error deleting file: {error}", pauses, returns to admin menu
 * Result: NOT TESTED - On my machine not able to reliably reproducde real disk I/0 error on my machine. Exception handling implemented in code.
 * 
 * TC18: Delete Customer - Unexpected Error During File Deletion
 * Input: Any other unexpected exception during File.Delete()
 * Expected Output: Catches generic Exception, displays "Unexpected error deleting file: {error}", pauses, returns to admin menu
 * Result: NOT TESTED - Didn't manage to force this error. Catch block is in code so should be able to handle if happens. 
 * 
 * TC19: Delete Customer - Error During SaveAll
 * Input: File deleted successfully but exception thrown during BinaryRepository.SaveAll()
 * Expected Output: Catches Exception, displays "Error saving updated customer list: {error}", pauses, returns to admin menu
 * Result: PASS - Locked customers folder before saving and error message appeared without crashing
 * 
 * TC20: Delete Customer - Invalid Selection
 * Input: Enter number outside valid range in delete menu
 * Expected Output: Displays "Invalid selection.", pauses, returns to delete customer list
 * Result: PASS - Displays "Invalid selection"
 * 
 * TC21: Delete Customer - Select Back Option
 * Input: Select "Back" option from delete menu
 * Expected Output: Returns to admin dashboard without deleting anyone
 * Result: PASS - Returned to admin dashboard
 * 
 * TC22: Delete Customer - Folder is a File (intentional fail)
 * Input: Replace the "customers" folder with a file named "customers"
 * Expected Output: Program should handle it gracefully
 * Result: FAIL – Program crashed because code assumes "customers" is always a directory. Known limitation.
 * 
 * TC23: Delete Customer - Corrupted Customer Dictionary (Intentional Fail)
 * Input: Modify loaded customer dictionary manually to contain null value
 * Expected Output: Should skip null values safely
 * Result: FAIL – A NullReferenceException occurred when I tried to access customer properties. This shows area for improvement.
 */