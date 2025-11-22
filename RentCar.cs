using System;
using System.Collections.Generic;
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
            return allCars.Where(c => c.RequiredLevel <= customerLevel)
                .OrderBy(c => c.RequiredLevel)
                .ThenBy(c => c.Price);
        }

        public static void Show(Customer customer, Action save)
        {
            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("===== RENT A CAR =====\n");
                Console.ResetColor();

                //filters cars based on levels
                var availableCars = GetAvailableCars(customer.Level).ToList();

                if (availableCars.Count == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("No cars available for your level.");
                    Console.ResetColor();
                    Pause();
                    return;
                }
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.WriteLine($"Your Level: {GetLevelName(customer.Level)}\n");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine("Available cars:\n");
                Console.ResetColor();

                for (int i = 0; i < availableCars.Count; i++)
                {
                    Console.WriteLine($"{i + 1}) {availableCars[i].GetCarInfo()}");
                }

                Console.WriteLine($"\n{availableCars.Count + 1}) Go Back");
                Console.Write("\nChoose a car to rent: ");

                string input = Console.ReadLine()?.Trim();

                if (int.TryParse(input, out int choice))
                {
                    if (choice == availableCars.Count + 1)
                    {
                        return;
                    }
                    else if (choice >= 1 && choice <= availableCars.Count)
                    {
                        Car selectedCar = availableCars[choice - 1];
                        ConfirmRental(customer, selectedCar, save);
                    }
                    else
                    {
                        Console.WriteLine("\nInvalid option. Please select a number from the list.");
                        Pause();
                    }
                }
                else
                {
                    Console.WriteLine("\nInvalid input. Please enter a number from the list to select a car."); 
                    Pause();
                }
            }
        }

        private static void ConfirmRental(Customer customer, Car car, Action save)
        {
            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("===== CONFIRM RENTAL =====\n");
                Console.ResetColor();
                Console.WriteLine($"Car: {car.Make} {car.Model}");
                Console.WriteLine($"Type: {car.Type}");
                Console.WriteLine($"Price: £{car.Price:F2}/day\n");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("Confirm rental? (y/n): ");
                Console.ResetColor();

                string confirm = Console.ReadLine()?.Trim().ToLower();

                if (confirm == "y" || confirm == "yes")
                {
                    customer.AddRental($"{car.Make} {car.Model}");
                    save(); //saves updated customer data
                    Console.WriteLine($"\nSuccessfully rented {car.Make} {car.Model}!");
                    Console.WriteLine($"Your current level: {GetLevelName(customer.Level)}");
                    Pause();
                    return;
                }
                else if (confirm == "n" || confirm == "no")
                {
                    Console.WriteLine("\nRental cancelled.");
                    Pause();
                    return;
                }
                else
                {
                    Console.WriteLine("\nInvalid input. Please enter 'y' for yes or 'n' for no.");
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