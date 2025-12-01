using System;
using System.Collections.Generic;
using System.Linq;

namespace AssignmentCARS
{
    public static class AdminStats
    {
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
            var customers = BinaryRepository.LoadAll() ?? new Dictionary<string, Customer>();

            Dictionary<string, int> rentalCounts = new(StringComparer.OrdinalIgnoreCase);

            foreach (var c in customers.Values)
            {
                foreach (var rental in c.RentalHistory)
                {
                    if (rentalCounts.ContainsKey(rental))
                        rentalCounts[rental]++;
                    else
                        rentalCounts[rental] = 1;
                }
            }

            Console.Clear();
            Console.WriteLine("=== MOST RENTED CARS ===\n");

            if (rentalCounts.Count == 0)
            {
                Console.WriteLine("No rentals recorded.");
                Pause();
                return;
            }

            foreach (var entry in rentalCounts.OrderByDescending(e => e.Value))
                Console.WriteLine($"{entry.Key}: {entry.Value} times");

            Pause();
        }

        private static void Pause()
        {
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
    }
}
