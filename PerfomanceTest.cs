using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AssignmentCARS
{
    public class PerformanceTest
    {
        public void MultipleProcessors()
        {
            Console.Clear();
            Console.WriteLine("=== PERFORMANCE TESTS (USING MULTIPLE PROCESSORS) ===\n");

            //Fresh dummy data
            List<Customer> customers = new List<Customer>();

            Console.WriteLine("Generating heavy dummy data...\n");

            int customerCount = 100000;          // increased
            int rentalsPerCustomer = 500;        // increased

            for (int i = 0; i < customerCount; i++)
            {
                var c = new Customer($"C{i}", "pass", 1);

                for (int r = 0; r < rentalsPerCustomer; r++)
                    c.AddRental($"Car {r}");

                customers.Add(c);
            }

            // HEAVY WORK METHOD — added only for profiling
            int HeavyWork()
            {
                int x = 0;
                for (int i = 0; i < 20000; i++)      // increased workload
                    x += (i * 16) % 8;
                return x;
            }

            // 1) foreach
            Stopwatch sw = Stopwatch.StartNew();

            int total1 = 0;
            foreach (var c in customers)
            {
                total1 += HeavyWork();
            }

            sw.Stop();
            Console.WriteLine($"Normal foreach (HEAVY DATA): {sw.ElapsedMilliseconds} ms");

            // 2) Parallel.ForEach
            sw.Restart();

            int total2 = 0;
            Parallel.ForEach(customers, c =>
            {
                total2 += HeavyWork();     
            });

            sw.Stop();
            Console.WriteLine($"Parallel.ForEach (HEAVY DATA): {sw.ElapsedMilliseconds} ms");

            // 3) Parallel.For
            sw.Restart();

            int total3 = 0;
            Parallel.For(0, customers.Count, i =>
            {
                total3 += HeavyWork();      
            });

            sw.Stop();
            Console.WriteLine($"Parallel.For (HEAVY DATA): {sw.ElapsedMilliseconds} ms");

            // 4) PLINQ
            sw.Restart();

            int total4 = customers
                .AsParallel()
                .Sum(c => HeavyWork());

            sw.Stop();
            Console.WriteLine($"AsParallel() (HEAVY DATA): {sw.ElapsedMilliseconds} ms");

            // NEW SMALL DATA TESTS
            Console.WriteLine("\n=== SMALL DATA TESTS (100 customers, 5 rentals) ===\n");

            List<Customer> smallCustomers = new List<Customer>();
            int smallCount = 100;
            int smallRentals = 5;

            for (int i = 0; i < smallCount; i++)
            {
                var c = new Customer($"Small{i}", "pass", 1);
                for (int r = 0; r < smallRentals; r++)
                    c.AddRental($"Car {r}");
                smallCustomers.Add(c);
            }

            // SMALL foreach
            sw.Restart();
            int s1 = 0;
            foreach (var c in smallCustomers)
                s1 += HeavyWork();
            sw.Stop();
            Console.WriteLine($"Small foreach: {sw.ElapsedMilliseconds} ms");

            // SMALL Parallel.ForEach
            sw.Restart();
            int s2 = 0;
            Parallel.ForEach(smallCustomers, c => s2 += HeavyWork());
            sw.Stop();
            Console.WriteLine($"Small Parallel.ForEach: {sw.ElapsedMilliseconds} ms");

            // SMALL Parallel.For
            sw.Restart();
            int s3 = 0;
            Parallel.For(0, smallCustomers.Count, i => s3 += HeavyWork());
            sw.Stop();
            Console.WriteLine($"Small Parallel.For: {sw.ElapsedMilliseconds} ms");

            // SMALL AsParallel()
            sw.Restart();
            int s4 = smallCustomers.AsParallel().Sum(c => HeavyWork());
            sw.Stop();
            Console.WriteLine($"Small AsParallel(): {sw.ElapsedMilliseconds} ms");

            Console.WriteLine("\nPress ENTER to return to the menu...");
            Console.ReadLine();
        }

        // STRING vs STRINGBUILDER
        public void StringVsStringBuilder()
        {
            Console.Clear();
            Console.WriteLine("=== STRING PERFORMANCE TEST ===\n");

            int loops = 50000;

            Stopwatch sw = Stopwatch.StartNew();
            string slow = "";
            for (int i = 0; i < loops; i++)
                slow += "x";
            sw.Stop();
            Console.WriteLine($"String: {sw.ElapsedMilliseconds} ms");

            sw.Restart();
            StringBuilder sb = new();
            for (int i = 0; i < loops; i++)
                sb.Append("x");
            sw.Stop();
            Console.WriteLine($"StringBuilder: {sw.ElapsedMilliseconds} ms");

            // NEW SMALL STRING TEST
            Console.WriteLine("\n--- SMALL STRING TEST (100 loops) ---");

            sw.Restart();
            string smallSlow = "";
            for (int i = 0; i < 100; i++)
                smallSlow += "x";
            sw.Stop();
            Console.WriteLine($"Small String: {sw.ElapsedMilliseconds} ms");

            sw.Restart();
            StringBuilder sbSmall = new();
            for (int i = 0; i < 100; i++)
                sbSmall.Append("x");
            sw.Stop();
            Console.WriteLine($"Small StringBuilder: {sw.ElapsedMilliseconds} ms");

            Console.WriteLine("\nPress ENTER to return...");
            Console.ReadLine();
        }


        //Capacity
        public void CapacityTest()
        {
            Console.Clear();
            Console.WriteLine("=== LIST CAPACITY PERFORMANCE TEST ===\n");

            int items = 2_000_000; // heavy enough to show difference

            Stopwatch sw = Stopwatch.StartNew();

            // 1) No capacity
            List<int> normalList = new List<int>();
            for (int i = 0; i < items; i++)
                normalList.Add(i);
            sw.Stop();
            Console.WriteLine($"Without capacity (HEAVY): {sw.ElapsedMilliseconds} ms");

            // 2) With capacity
            sw.Restart();
            List<int> capacityList = new List<int>(items);
            for (int i = 0; i < items; i++)
                capacityList.Add(i);
            sw.Stop();
            Console.WriteLine($"With pre-set capacity (HEAVY): {sw.ElapsedMilliseconds} ms");

            // NEW SMALL CAPACITY TEST
            Console.WriteLine("\n--- SMALL CAPACITY TEST (1000 items) ---");

            int smallItems = 1000;

            sw.Restart();
            List<int> smallNormal = new List<int>();
            for (int i = 0; i < smallItems; i++)
                smallNormal.Add(i);
            sw.Stop();
            Console.WriteLine($"Without capacity (SMALL): {sw.ElapsedMilliseconds} ms");

            sw.Restart();
            List<int> smallCap = new List<int>(smallItems);
            for (int i = 0; i < smallItems; i++)
                smallCap.Add(i);
            sw.Stop();
            Console.WriteLine($"With capacity (SMALL): {sw.ElapsedMilliseconds} ms");

            Console.WriteLine("\nPress ENTER to return...");
            Console.ReadLine();
        }

        // CACHING
        Dictionary<int, int> factorialCache = new();

        private int ExpensiveFactorial(int n)
        {
            Thread.Sleep(2); // fake heavy work
            return n == 0 ? 1 : n * ExpensiveFactorial(n - 1);
        }

        private int CachedFactorial(int n)
        {
            if (factorialCache.TryGetValue(n, out int value))
                return value;

            int result = ExpensiveFactorial(n);
            factorialCache[n] = result;
            return result;
        }

        public void CachingTest()
        {
            Console.Clear();
            Console.WriteLine("=== CACHING PERFORMANCE TEST ===\n");

            int loops = 10;

            Stopwatch sw = Stopwatch.StartNew();
            for (int i = 0; i < loops; i++)
                ExpensiveFactorial(10);
            sw.Stop();
            Console.WriteLine($"Without cache: {sw.ElapsedMilliseconds} ms");

            sw.Restart();
            for (int i = 0; i < loops; i++)
                CachedFactorial(10);
            sw.Stop();
            Console.WriteLine($"With cache: {sw.ElapsedMilliseconds} ms");

            Console.WriteLine("\nPress ENTER to return...");
            Console.ReadLine();
        }
    }
}




// This file is basically the last topic contents and tests perofrmance and profiles  