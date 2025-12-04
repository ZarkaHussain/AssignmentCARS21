using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace AssignmentCARS
{
    public class PerformanceTest
    {
        public void MultipleProcessors(List<Customer> customers)
        {
            Console.Clear();
            Console.WriteLine("=== PERFORMANCE TESTS (USING MULTIPLE PROCESSORS) ===\n");

            //Always builds fresh dummy data
            customers = new List<Customer>();

            Console.WriteLine("Generating heavy dummy data...\n");

            int customerCount = 100000;
            int rentalsPerCustomer = 200;

            for (int i = 0; i < customerCount; i++)
            {
                var c = new Customer($"C{i}", "pass", 1);

                for (int r = 0; r < rentalsPerCustomer; r++)
                    c.AddRental($"Car {r}");

                customers.Add(c);
            }

            
            // 1) foreach
            Stopwatch sw = Stopwatch.StartNew();
            int total1 = 0;

            foreach (var c in customers)
                total1 += c.RentalHistory.Count;

            sw.Stop();
            Console.WriteLine($"Normal foreach: {sw.ElapsedMilliseconds} ms");

            // 2) Parallel.ForEach
            sw.Restart();
            int total2 = 0;

            Parallel.ForEach(customers, c =>
            {
                total2 += c.RentalHistory.Count;   
            });

            sw.Stop();
            Console.WriteLine($"Parallel.ForEach: {sw.ElapsedMilliseconds} ms");

            // 3)Parallel.For
            sw.Restart();
            int total3 = 0;

            Parallel.For(0, customers.Count, i =>
            {
                total3 += customers[i].RentalHistory.Count;
            });

            sw.Stop();
            Console.WriteLine($"Parallel.For: {sw.ElapsedMilliseconds} ms");

            // 4) PLINQ
            sw.Restart();
            int total4 = customers
                .AsParallel()
                .Sum(c => c.RentalHistory.Count);
            sw.Stop();

            Console.WriteLine($"AsParallel(): {sw.ElapsedMilliseconds} ms");

            Console.WriteLine("\nPress ENTER to return to the menu...");
            Console.ReadLine();
        }
    }
}




