using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssignmentCARS
{
    public static class AdminStats
    {
        //added simple cache
        // key is car name, value= how many times rented 
        private static Dictionary<string, int>? rentalCountCache = null;
        // clears cache when needed, for example after rentals happen
        public static void InvalidateCache()
        {
            rentalCountCache = null;
        }

        public static void ShowTotalRentals()
        {
            var customers = BinaryRepository.LoadAll() ?? new Dictionary<string, Customer>();
            int totalRentals = customers.Values.Sum(c => c.RentalHistory.Count);
            Console.Clear();
            Console.WriteLine("=== TOTAL RENTALS ===\n");
            Console.WriteLine($"Total number of cars rented in the system: {totalRentals}");
            Pause();
        }

        public static void ShowMostRentedCars()
        {
            Console.Clear();
            Console.WriteLine("=== MOST RENTED CARS ===\n");

            // use cache if exists
            if (rentalCountCache != null)
            {
                PrintResults(rentalCountCache);
                Pause();
                return;
            }

            // if not calculate and cache it 
            var customers = BinaryRepository.LoadAll() ?? new Dictionary<string, Customer>();
            rentalCountCache = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

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

            PrintResults(rentalCountCache);
            Pause();
        }

        private static void PrintResults(Dictionary<string, int> rentalCounts)
        {
            if (rentalCounts.Count == 0)
            {
                Console.WriteLine("No rentals recorded.");
                return;
            }

            // performance improvement using StringBuilder
            StringBuilder sb = new StringBuilder();

            foreach (var entry in rentalCounts.OrderByDescending(e => e.Value))
            {
                sb.Append(entry.Key)
                  .Append(": ")
                  .Append(entry.Value)
                  .Append(" times\n");
            }

            Console.Write(sb.ToString());
        }

        private static void Pause()
        {
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
    }
}
