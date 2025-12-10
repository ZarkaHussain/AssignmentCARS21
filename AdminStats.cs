using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssignmentCARS
{
    //handles admin statistics such as rental counts
    // demonstrates data handling, caching, LINQ and performance optimisation
    public static class AdminStats
    {
        //added simple cache to store results of expensive calculations
        //improves performance by avoiding repeated loops adn disk readss
        // key is car name, value= how many times rented 
        private static Dictionary<string, int>? rentalCountCache = null;
        // clears cache when needed, for example after rentals happen
        public static void InvalidateCache()
        {
            rentalCountCache = null;
        }

        //shows total number of rentals in system
        public static void ShowTotalRentals()
        {
            //loads data from binary files
            var customers = BinaryRepository.LoadAll() ?? new Dictionary<string, Customer>();

            //LINQ algorthm used to quickly sum all rental histories
            int totalRentals = customers.Values.Sum(c => c.RentalHistory.Count);
            Console.Clear();
            Console.WriteLine("=== TOTAL RENTALS ===\n");
            Console.WriteLine($"Total number of cars rented in the system: {totalRentals}");
            UIHelper.Pause();
        }

        //shows most rented cars
        public static void ShowMostRentedCars()
        {
            Console.Clear();
            Console.WriteLine("=== MOST RENTED CARS ===\n");

            // use cache if exists instead of recalculating
            if (rentalCountCache != null)
            {
                PrintResults(rentalCountCache);
                UIHelper.Pause();
                return;
            }

            // if not, calculate and cache it 
            var customers = BinaryRepository.LoadAll() ?? new Dictionary<string, Customer>();

            //dictionary gives fast lookups by key
            rentalCountCache = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

            //nested loops build count of rentals
            foreach (var c in customers.Values)
            {
                foreach (var rental in c.RentalHistory)
                {
                    if (rentalCountCache.ContainsKey(rental))
                        rentalCountCache[rental]++;
                    else
                        rentalCountCache[rental] = 1;
                }
            }

            //prints results 
            PrintResults(rentalCountCache);
            UIHelper.Pause();
        }

        private static void PrintResults(Dictionary<string, int> rentalCounts)
        {
            if (rentalCounts.Count == 0)
            {
                Console.WriteLine("No rentals recorded.");
                return;
            }

            //StringBuilder used for fast string appending in loops
            // performance improvement using StringBuilder comapred to string concatenation
            StringBuilder sb = new StringBuilder();

            //LINQ used again to sort results by highest count
            foreach (var entry in rentalCounts.OrderByDescending(e => e.Value))
            {
                sb.Append(entry.Key)
                  .Append(": ")
                  .Append(entry.Value)
                  .Append(" times\n");
            }

            Console.Write(sb.ToString());
        }
    }
}


/*
 * TEST CASES FOR ADMIN STATS:
 * 
 * TC1: Total Rentals with no customers
 * Input: Empty customer folder (LoadAll returns empty dictionary)
 * Expected Output: Displays "Total number of cars rented in the system: 0"
 * Result: PASS – As expected 0 rentals displayed 
 * 
 * TC2: Total Rentals with single customer
 * Input: 1 customer with 5 rentals in their RentalHistory
 * Expected Output: Displays "Total number of cars rented in the system: 5"
 * Result: PASS – Total displayed correctly
 * 
 * TC3: Total Rentals with multiple customers
 * Input: 3 customers with 2, 5, and 3 rentals in their RentalHistory respectively
 * Expected Output: Displays "Total number of cars rented in the system: 10" 
 * Result: PASS – Total displayed correctly
 * 
 * TC4: Most Rented Cars with no customers
 * Input: Empty customer folder (LoadAll returns empty dictionary)
 * Expected Output: Displays "No rentals recorded."
 * Result: PASS – Displays "No rentals recorded"
 * 
 * TC5: Most Rented Cars with no rentals
 * Input: Customers exist but all have empty RentalHistory (Count = 0)
 * Expected Output: Displays "No rentals recorded"
 * Result: PASS - Displays "No rentals recorded"
 * 
 * TC6: Most Rented Cars - one car type
 * Input: "Toyota Corolla" rented 5 times across all customers
 * Expected Output: Displays "Toyota Corolla: 5 times"
 * Result: PASS - Displays "Toyota Corolla: 5 times"
 * 
 * TC7: Most Rented Cars - multiple car types (Ordered by Count)
 * Input: Toyota Corolla rented 5 times, Honda Civic rented 3 times, Ford Focus rented 1 time
 * Expected Output: Displays in descending order:
 * "Toyota Corolla: 5 times"
 * "Honda Civic: 3 times"
 * "Ford Focus: 1 times"
 * Result: PASS - Displayed in correct order
 * 
 * TC9: Cache - First run
 * Input: First time calling ShowMostRentedCars() when cache is empty
 * Expected Output: Loads data from files, calculates counts, saves to cache, shows results
 * Result: PASS - Worked as expected and results were shown
 * 
 * TC10: Cache - Second run (Using Cache)
 * Input: Call ShowMostRentedCars() again straight after TC9
 * Expected Output: Uses cached results and does not recalculate
 * Result: PASS - Results appeared instantly and no delay noticed
 * 
 * TC11: Cache Invalidation
 * Input: Call InvalidateCache() then run ShowMostRentedCars()
 * Expected Output: Cache cleared and data recalculated from files
 * Result: PASS - New results were calculated after clearing cache
 * 
 * TC12: Cache Not Invalidated After Data Change
 * Input: Add new rental data but DO NOT call InvalidateCache()
 * Expected Output: Should show updated results with new rentals
 * Result: FAIL - Output still showed old cached data (this showed why cache invalidation is needed)
 * 
 * TC13: Corrupted Customer File
 * Input: Manually corrupted a .dat file before running ShowMostRentedCars()
 * Expected Output: Program should attempt to load data and handle file issues via BinaryRepository without crashing
 * Result: FAIL - Program showed incorrect rental counts due to corrupted data this shows limitation when source data damaged
 */
