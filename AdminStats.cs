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
