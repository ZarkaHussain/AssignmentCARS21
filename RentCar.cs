using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AssignmentCARS
{
    public static class RentCar
    {
        private static readonly IEnumerable<Car> allCars = new List<Car>
        {
            //standard customer cars (Level1)
            new Car("Toyota", "Corolla", "Sedan", 5, 45.00m, 1),
            new Car("Honda", "Civic", "Sedan", 5, 50.00m, 1),
            new Car("Ford", "Focus", "Hatchback", 5, 42.00m, 1),

            //premium cars (Level 5)
            new Car("BMW", "3 Series", "Luxury Sedan", 5, 120.00m, 5),
            new Car("Audi", "A4", "Luxury Sedan", 5, 125.00m, 5),
            new Car("Mercedes", "C-Class", "Luxury Sedan", 5, 130.00m, 5),

            //VIP cars (Level 10)
            new Car("Porsche", "911", "Sports Car", 2, 350.00m, 10),
            new Car("Lamborghini", "Huracan", "Supercar", 2, 800.00m, 10),
            new Car("Ferrari", "F8", "Supercar", 2, 900.00m, 10),
        };

        private static IEnumerable<Car> GetAvailableCars(int customerLevel)
        {
            return allCars
                .Where(c => c.RequiredLevel <= customerLevel)
                .OrderBy(c => c.RequiredLevel)
                .ThenBy(c => c.Price);
        }

        public static Customer Show(Customer customer, Action save)
        {
            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("===== RENT A CAR =====\n");
                Console.ResetColor();

                var availableCars = GetAvailableCars(customer.Level).ToList();

                if (availableCars.Count == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("No cars available for your level.");
                    Console.ResetColor();
                    Pause();
                    return customer;
                }

                Console.WriteLine($"Your Level: {GetLevelName(customer.Level)}\n");

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Available cars:\n");
                Console.ResetColor();

                for (int i = 0; i < availableCars.Count; i++)
                {
                    Car car = availableCars[i];
                    decimal discountedPrice = Discounts.Apply(car.Price, customer.Level);
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write($"{i + 1}) ");

                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write($"{car.Make} {car.Model}");

                    Console.ForegroundColor = ConsoleColor.White;             
                    Console.Write($" | Type:{car.Type} | Seats: {car.Seats}");

                    Console.Write($" | Base Price: £{car.Price:F2 |}");

                    if (discountedPrice != car.Price)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write($"  Discounted: £{discountedPrice:F2}");
                        Console.ResetColor();
                    }

                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    Console.WriteLine($"  | Level: {GetLevelName(car.RequiredLevel)}");
                    Console.ResetColor();
                    Console.WriteLine(); //adds a blank line

                }

                Console.WriteLine($"\n{availableCars.Count + 1}) Go Back");
                Console.Write("\nChoose a car to rent: ");

                string input = Console.ReadLine()?.Trim() ?? "";

                if (int.TryParse(input, out int choice))
                {
                    if (choice == availableCars.Count + 1)
                        return customer;

                    if (choice >= 1 && choice <= availableCars.Count)
                    {
                        Car selectedCar = availableCars[choice - 1];
                        customer = ConfirmRental(customer, selectedCar, save);
                        return customer; // return upgraded customer
                    }

                    Console.WriteLine("\nInvalid option.");
                    Pause();
                }
                else
                {
                    Console.WriteLine("\nInvalid input.");
                    Pause();
                }
            }
        }

        private static Customer ConfirmRental(Customer customer, Car car, Action save)
        {
            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("===== CONFIRM RENTAL =====\n");
                Console.ResetColor();

                Console.WriteLine($"Car: {car.Make} {car.Model}");
                Console.WriteLine($"Type: {car.Type}");

                decimal finalPrice = Discounts.Apply(car.Price, customer.Level);
                Console.WriteLine($"Price: £{car.Price:F2}/day\n");
                if (finalPrice != car.Price)
                    Console.WriteLine($"Your Price:");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"£{finalPrice:F2}/day ({Discounts.GetRate(customer.Level) * 100}% off)");
                Console.ResetColor();
                Console.WriteLine();

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("Confirm rental? (y/n): ");
                Console.ResetColor();

                string confirm = Console.ReadLine()?.Trim().ToLower() ?? "";

                if (confirm == "y" || confirm == "yes")
                {
                    customer.AddRental($"{car.Make} {car.Model}");

                    //level upgrade happens inside AddRental()

                    save(); //saves single customer file

                    Console.WriteLine($"\nSuccessfully rented {car.Make} {car.Model}!");
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine($"Your current level: {GetLevelName(customer.Level)}");
                    Console.ResetColor();

                    Pause();
                    return customer; // returns upgraded customer object if upgraded
                }
                else if (confirm == "n" || confirm == "no")
                {
                    Console.WriteLine("\nRental cancelled.");
                    Pause();
                    return customer;
                }
                else
                {
                    Console.WriteLine("\nInvalid input. Enter 'y' or 'n'.");
                    Pause();
                }
            }
        }

        private static string GetLevelName(int level) => level switch
        {
            10 => "VIP",
            5 => "Premium",
            _ => "Standard"
        };

        private static void Pause()
        {
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

       

    }
}




//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Diagnostics;
//using System.Threading.Tasks;


//namespace AssignmentCARS
//{
//    public static class RentCar
//    {
//        private static readonly IEnumerable<Car> allCars = new List<Car>
//        {
//            //standard customer cars (Level1)
//            new Car("Toyota", "Corolla", "Sedan", 5, 45.00m, 1),
//            new Car("Honda", "Civic", "Sedan", 5, 50.00m, 1),
//            new Car("Ford", "Focus", "Hatchback", 5, 42.00m, 1),

//            //premium cars (Level 5)
//            new Car("BMW", "3 Series", "Luxury Sedan", 5, 120.00m, 5),
//            new Car("Audi", "A4", "Luxury Sedan", 5, 125.00m, 5),
//            new Car("Mercedes", "C-Class", "Luxury Sedan", 5, 130.00m, 5),

//            //VIP cars (Level 10)
//            new Car("Porsche", "911", "Sports Car", 2, 350.00m, 10),
//            new Car("Lamborghini", "Huracan", "Supercar", 2, 800.00m, 10),
//            new Car("Ferrari", "F8", "Supercar", 2, 900.00m, 10),
//        };

//        private static IEnumerable<Car> GetAvailableCars(int customerLevel)
//        {
//            return allCars
//                .Where(c => c.RequiredLevel <= customerLevel)
//                .OrderBy(c => c.RequiredLevel)
//                .ThenBy(c => c.Price);
//        }

//        public static Customer Show(Customer customer, Action save)
//        {
//            while (true)
//            {
//                Console.Clear();
//                Console.ForegroundColor = ConsoleColor.Magenta;
//                Console.WriteLine("===== RENT A CAR =====\n");
//                Console.ResetColor();

//                var availableCars = GetAvailableCars(customer.Level).ToList();

//                if (availableCars.Count == 0)
//                {
//                    Console.ForegroundColor = ConsoleColor.Red;
//                    Console.WriteLine("No cars available for your level.");
//                    Console.ResetColor();
//                    Pause();
//                    return customer;
//                }

//                Console.ForegroundColor = ConsoleColor.DarkBlue;
//                Console.WriteLine($"Your Level: {GetLevelName(customer.Level)}\n");
//                Console.ResetColor();

//                Console.ForegroundColor = ConsoleColor.DarkCyan;
//                Console.WriteLine("Available cars:\n");
//                Console.ResetColor();

//                for (int i = 0; i < availableCars.Count; i++)
//                {
//                    Car car = availableCars[i];
//                    decimal discountedPrice = Discounts.Apply(car.Price, customer.Level);
//                    Console.Write($"{i + 1}) {car.Make} {car.Model} | Type:{car.Type} | Seats: {car.Seats}");
//                    Console.Write($" | Base Price: £{car.Price:F2}");

//                    if (discountedPrice != car.Price)
//                        Console.Write($" | Discounted: £{discountedPrice:F2}");

//                    Console.WriteLine($" | Level: {GetLevelName(car.RequiredLevel)}");
//                }

//                Console.WriteLine($"\n{availableCars.Count + 1}) Go Back");
//                Console.Write("\nChoose a car to rent: ");

//                string input = Console.ReadLine()?.Trim() ?? "";

//                if (int.TryParse(input, out int choice))
//                {
//                    if (choice == availableCars.Count + 1)
//                        return customer;

//                    if (choice >= 1 && choice <= availableCars.Count)
//                    {
//                        Car selectedCar = availableCars[choice - 1];
//                        customer = ConfirmRental(customer, selectedCar, save);
//                        return customer; // IMPORTANT: return upgraded customer
//                    }

//                    Console.WriteLine("\nInvalid option.");
//                    Pause();
//                }
//                else
//                {
//                    Console.WriteLine("\nInvalid input.");
//                    Pause();
//                }
//            }
//        }

//        private static Customer ConfirmRental(Customer customer, Car car, Action save)
//        {
//            while (true)
//            {
//                Console.Clear();
//                Console.ForegroundColor = ConsoleColor.Magenta;
//                Console.WriteLine("===== CONFIRM RENTAL =====\n");
//                Console.ResetColor();

//                Console.WriteLine($"Car: {car.Make} {car.Model}");
//                Console.WriteLine($"Type: {car.Type}");

//                decimal finalPrice = Discounts.Apply(car.Price, customer.Level);
//                Console.WriteLine($"Price: £{car.Price:F2}/day\n");
//                if (finalPrice != car.Price)
//                    Console.WriteLine($"Your Price: £{finalPrice:F2}/day ({Discounts.GetRate(customer.Level) * 100}% off)");
//                Console.WriteLine();

//                Console.ForegroundColor = ConsoleColor.Yellow;
//                Console.Write("Confirm rental? (y/n): ");
//                Console.ResetColor();

//                string confirm = Console.ReadLine()?.Trim().ToLower() ?? "";

//                if (confirm == "y" || confirm == "yes")
//                {
//                    customer.AddRental($"{car.Make} {car.Model}");

//                    // Level upgrade happens inside AddRental()

//                    save(); // saves single customer file

//                    Console.WriteLine($"\nSuccessfully rented {car.Make} {car.Model}!");
//                    Console.WriteLine($"Your current level: {GetLevelName(customer.Level)}");

//                    Pause();
//                    return customer; // return possibly upgraded customer object
//                }
//                else if (confirm == "n" || confirm == "no")
//                {
//                    Console.WriteLine("\nRental cancelled.");
//                    Pause();
//                    return customer;
//                }
//                else
//                {
//                    Console.WriteLine("\nInvalid input. Enter 'y' or 'n'.");
//                    Pause();
//                }
//            }
//        }

//        private static string GetLevelName(int level) => level switch
//        {
//            10 => "VIP",
//            5 => "Premium",
//            _ => "Standard"
//        };

//        private static void Pause()
//        {
//            Console.WriteLine("\nPress any key to continue...");
//            Console.ReadKey();
//        }

//        // === MULTIPLE PROCESSORS TESTING ===
//        //called this from Program.cs
//        public static void RunParallelTest()
//        {
//            Console.Clear();
//            Console.WriteLine("=== MULTIPLE PROCESSORS TEST: Discount Calculations ===\n");

//            // Create two separate lists
//            List<Car> cars1 = allCars.ToList();
//            List<Car> cars2 = allCars.ToList();
//            List<Car> cars3 = allCars.ToList();

//            int level = 10; // VIP = biggest discount → more work

//            //NORMAL FOREACH 
//            var timer = Stopwatch.StartNew();
//            foreach (Car car in cars1)
//            {
//                Discounts.Apply(car.Price, level);
//            }
//            timer.Stop();
//            Console.WriteLine("Normal foreach time: {0} ms", timer.ElapsedMilliseconds);

//            //PARALLEL FOREACH
//            timer = Stopwatch.StartNew();
//            Parallel.ForEach(cars2, car =>
//            {
//                Discounts.Apply(car.Price, level);
//            });
//            timer.Stop();
//            Console.WriteLine("Parallel.ForEach time: {0} ms", timer.ElapsedMilliseconds);

//            //PLINQ AsParallel 
//            timer = Stopwatch.StartNew();
//            cars3
//                .AsParallel()
//                .ForAll(car =>
//                {
//                    Discounts.Apply(car.Price, level);
//                });
//            timer.Stop();
//            Console.WriteLine("AsParallel() time: {0} ms", timer.ElapsedMilliseconds);

//            Console.WriteLine("\n(As expected: normal foreach is fastest because work is tiny.)");

//            Pause();
//        }



//    }
//}
