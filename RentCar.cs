using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AssignmentCARS
{
    //static class for handling car rental menu
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

        //filter cars based on customer's level
        //only cars that user qualifies for are returned
        //ordered by required level first then by price
        private static IEnumerable<Car> GetAvailableCars(int customerLevel)
        {
            return allCars
                .Where(c => c.RequiredLevel <= customerLevel)
                .OrderBy(c => c.RequiredLevel)
                .ThenBy(c => c.Price);
        }

        //
        //this is a public helper method for the unit testing
        public static List<Car> GetCarsByLevel(int level)
        {
            List<Car> result = new List<Car>();

            foreach (var car in allCars)
            {
                if (car.RequiredLevel <= level)
                    result.Add(car);
            }

            return result;
        }
        //

        //shows Rent Car menu and handles selection logic
        //returns possibly updated customer object after rental
        public static Customer Show(Customer customer, Action save)
        {
            while (true)
            {
                Console.Clear(); //clear console and show header
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("===== RENT A CAR =====\n");
                Console.ResetColor();

                //get list of available cars for this user's level
                var availableCars = GetAvailableCars(customer.Level).ToList();

                //if no cars available, return to previuos menu
                if (availableCars.Count == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("No cars available for your level.");
                    Console.ResetColor();
                    UIHelper.Pause();
                    return customer;
                }

                //display users current membership level
                Console.WriteLine($"Your Level: {GetLevelName(customer.Level)}\n");

                //display heading for car list
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Available cars:\n");
                Console.ResetColor();

                //loop through and display each available car
                for (int i = 0; i < availableCars.Count; i++)
                {
                    Car car = availableCars[i];
                    //apply any level based discount
                    decimal discountedPrice = Discounts.Apply(car.Price, customer.Level);
                    Console.ForegroundColor = ConsoleColor.White;
                    //display index number
                    Console.Write($"{i + 1}) ");

                    //display car name
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write($"{car.Make} {car.Model}");

                    //display car details
                    Console.ForegroundColor = ConsoleColor.White;             
                    Console.Write($" | Type:{car.Type} | Seats: {car.Seats}");

                    //display base price
                    Console.Write($" | Base Price: £{car.Price:F2 |}");

                    //display discounted price if available
                    if (discountedPrice != car.Price)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write($"  Discounted: £{discountedPrice:F2}");
                        Console.ResetColor();
                    }

                    //display level requirement for car
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    Console.WriteLine($"  | Level: {GetLevelName(car.RequiredLevel)}");
                    Console.ResetColor();
                    Console.WriteLine(); //adds a blank line for spacing

                }

                //option to return to previous menu
                Console.WriteLine($"\n{availableCars.Count + 1}) Go Back");
                Console.Write("\nChoose a car to rent: ");

                //read user input
                string input = Console.ReadLine()?.Trim() ?? "";

                //validate numeric input
                if (int.TryParse(input, out int choice))
                {
                    //go back option
                    if (choice == availableCars.Count + 1)
                        return customer;

                    //valid car selection
                    if (choice >= 1 && choice <= availableCars.Count)
                    {
                        Car selectedCar = availableCars[choice - 1];
                        //handle confirmation and rental logic
                        customer = ConfirmRental(customer, selectedCar, save);
                        return customer; // return upgraded customer if level changed
                    }

                    //invalid number
                    Console.WriteLine("\nInvalid option.");
                    UIHelper.Pause();
                }
                else
                {
                    //non-numeric input
                    Console.WriteLine("\nInvalid input.");
                    UIHelper.Pause();
                }
            }
        }

        //handles confirmation screen before finalising a rental
        private static Customer ConfirmRental(Customer customer, Car car, Action save)
        {
            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("===== CONFIRM RENTAL =====\n");
                Console.ResetColor();

                //show selected car details
                Console.WriteLine($"Car: {car.Make} {car.Model}");
                Console.WriteLine($"Type: {car.Type}");

                //calculate final price including discounts
                decimal finalPrice = Discounts.Apply(car.Price, customer.Level);
                Console.WriteLine($"Price: £{car.Price:F2}/day\n");
                //show discounted price if different
                if (finalPrice != car.Price)
                    Console.WriteLine($"Your Price:");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"£{finalPrice:F2}/day ({Discounts.GetRate(customer.Level) * 100}% off)");
                Console.ResetColor();
                Console.WriteLine();

                //ask user to confirm
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("Confirm rental? (y/n): ");
                Console.ResetColor();

                string confirm = Console.ReadLine()?.Trim().ToLower() ?? "";
                //user confirmed rental
                if (confirm == "y" || confirm == "yes")
                {
                    //add rental to customer history
                    customer.AddRental($"{car.Make} {car.Model}");

                    //level upgrade happens inside AddRental()

                    save(); //saves single customer file

                    Console.WriteLine($"\nSuccessfully rented {car.Make} {car.Model}!");
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine($"Your current level: {GetLevelName(customer.Level)}");
                    Console.ResetColor();

                    UIHelper.Pause();
                    return customer; // returns upgraded customer object if upgraded
                }
                //user cancelled rental
                else if (confirm == "n" || confirm == "no")
                {
                    Console.WriteLine("\nRental cancelled.");
                    UIHelper.Pause();
                    return customer;
                }
                else
                {
                    //invalid confirmation input
                    Console.WriteLine("\nInvalid input. Enter 'y' or 'n'.");
                    UIHelper.Pause();
                }
            }
        }

        //converts numeric level into name
        private static string GetLevelName(int level) => level switch
        {
            10 => "VIP",
            5 => "Premium",
            _ => "Standard"
        };

    }
}



/*
 * TEST CASES FOR CAR RENTAL:
 * 
 * TC1: Standard customer car access (Level 1)
 * Input: Customer with Level=1
 * Expected Output: Shows only 3 cars with RequiredLevel=1 (Ford Focus £42, Toyota Corolla £45, Honda Civic £50) ordered by RequiredLevel then Price
 * Result: PASS - Only the 3 cars displayed
 * 
 * TC2: Premium customer car access (Level 5)
 * Input: Customer with Level=5
 * Expected Output: Shows 6 cars - Level 1 cars (Ford, Toyota, Honda) + Level 5 cars (BMW £120, Audi £125, Mercedes £130), ordered by level then price
 * Result: PASS - 6 cars displayed (standard cars and premium cars)
 * 
 * TC3: VIP customer car access (Level 10)
 * Input: Customer with Level=10
 * Expected Output: Shows all 9 cars including Level 10 (Porsche £350, Lamborghini £800, Ferrari £900), ordered by level then price
 * Result: PASS - 9 cars displayed (standard cars, premium cars and VIP cars)
 * 
 * 
 * TC4: No discount for Standard customer
 * Input: Standard customer (Level 1) views Toyota Corolla (£45 base price)
 * Expected Output: Shows only "Base Price: £45.00" with no discounted price displayed (0% discount)
 * Result: PASS - No discounted price displayed for standard customer
 * 
 * TC5: Discount display for Premium customer
 * Input: Premium customer (Level 5) views BMW 3 Series (£120 base price)
 * Expected Output: Shows "Base Price: £120.00" and "Discounted: £108.00" in green (10% off: £120 × 0.90 = £108)
 * Result: PASS - Discount of 10% displayed for premium customer
 * 
 * TC6: Discount display for VIP customer
 * Input: VIP customer (Level 10) views Ferrari F8 (£900 base price)
 * Expected Output: Shows "Base Price: £900.00" and "Discounted: £765.00" in green (15% off: £900 × 0.85 = £765)
 * Result: PASS - Discount of 15% displayed for premium customer
 * 
 * TC7: Select go back option
 * Input: Customer selects the "Go Back" option (e.g., option 4 if 3 cars available)
 * Expected Output: Returns to previous menu without renting, customer object unchanged
 * Result: PASS - Successfully returns to previous menu with no change to customer object 
 * 
 * TC8: Invalid car selection (Out of Range)
 * Input: Enter number outside valid range (e.g., 99 when only 3 cars available)
 * Expected Output: Displays "Invalid option.", pauses, returns to car list
 * Result: PASS - Displays message "Invalid option" car list shown
 * 
 * TC9: Non numeric input in car selection
 * Input: Enter text like "abc" instead of a number
 * Expected Output: Displays "Invalid input.", pauses, returns to car list
 * Result: PASS - Displays "Invalid input", car list shown
 * 
 * TC10: Confirm Rental - Type "y"
 * Input: Select car, type "y" (lowercase)
 * Expected Output: Car name added to RentalHistory, customer saved via save(), displays "Successfully rented {car}!" and current level, returns customer object
 * Result: PASS - Success message shwon and car displayed in my rental history when I checked it
 * 
 * TC11: Confirm Rental - Type "yes"
 * Input: Select car, type "yes" (full word)
 * Expected Output: Same as Test Case 10 (both "y" and "yes" accepted)
 * Result: PASS - Success message shwon and car displayed in my rental history when I checked it
 * 
 * TC12: Confirm Rental - Type "Y" (Uppercase)
 * Input: Select car, type "Y" (uppercase)
 * Expected Output: Converted to lowercase via .ToLower(), rental proceeds successfully
 * Result: PASS - Success message shwon and car displayed in my rental history when I checked it
 * 
 * TC13: Cancel Rental - Type "n"
 * Input: Select car, type "n"
 * Expected Output: Displays "Rental cancelled.", pauses, returns to main menu, no rental added, customer unchanged
 * Result: PASS - displays "Rental cancelled" checked rental history and car wasnt added
 * 
 * TC14: Cancel Rental - Type "no"
 * Input: Select car, type "no" (full word)
 * Expected Output: Same as Test Case 13 (both "n" and "no" accepted)
 * Result: PASS - Displays "Rental cancelled" checked rental history and car wasnt added
 * 
 * TC15: Invalid Confirmation Input
 * Input: Type random text like "maybe" or "ok" instead of y/n
 * Expected Output: Displays "Invalid input. Enter 'y' or 'n'.", pauses, loops back to confirmation prompt
 * Result: PASS - Displays "Rental cancelled" checked rental history and car wasnt added
 * 
 * TC16: Auto-upgrade Standard to Premium (5 rentals)
 * Input: Customer with 4 existing rentals rents 5th car
 * Expected Output: AddRental() upgrades Level from 1 to 5, displays "Your current level: Premium" after rental
 * Result: PASS - After renting 5th car, got success message saying "Your current level: Premium". When I went back to the car list, I could now see the BMW, Audi, and Mercedes cars that weren't there before.
 * 
 * TC17: Auto-upgrade Premium to VIP (10 rentals)
 * Input: Customer with 9 existing rentals rents 10th car
 * Expected Output: AddRental() upgrades Level from 5 to 10, displays "Your current level: VIP" after rental
 * Result: PASS - After renting 10th car message said "Your current level: VIP". When I went back to the car list again, all 9 cars including the supercars were now there, available for me to hire.
 * 
 * TC18: No Upgrade (Insufficient Rentals)
 * Input: Customer with 2 rentals rents 3rd car
 * Expected Output: Level remains at 1 (Standard), displays "Your current level: Standard"
 * Result: PASS - After renting 3rd car level remained at 1 still only could rent between the 3 Standard cars
 * 
 * TC19: Car Ordering by RequiredLevel then Price
 * Input: VIP customer views all cars
 * Expected Output: Cars displayed in order: Ford Focus (1,£42), Toyota Corolla (1,£45), Honda Civic (1,£50), BMW (5,£120), Audi (5,£125), Mercedes (5,£130), Porsche (10,£350), Lamborghini (10,£800), Ferrari (10,£900)
 * Result: PASS - All cars appeared in correct order. Standard cars first, then premium cars, then VIP cars, all sorted by price.
 * 
 * 
 * 
 */