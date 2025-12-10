using System;

namespace AssignmentCARS
{
    //this class represents Car object in system
    public class Car
    {
        //these are properties that store car's data
        //using properties makes code cleaner and easier to maintain
        public string Make { get; set; }   //brand of car
        public string Model { get; set; }  //speciific model of car
        public string Type { get; set; }  //type of car
        public int Seats { get; set; }    //number of seats in car
        public decimal Price { get; set; }  //daily rental price
        public int RequiredLevel { get; set; }  //1=Standard, 5=Premium, 10=VIP

        //constructor method used to create fully initialised car object
        //supports good data integrity by ensuring all important values are set
        public Car(string make, string model, string type, int seats, decimal price, int requiredLevel)
        {
            Make = make;
            Model = model;
            Type = type;
            Seats = seats;
            Price = price;
            RequiredLevel = requiredLevel;
        }


        // converts numeric level into a readable name
        //improves user experience and makes UI clearer
        public string GetLevelName()
        {
            //switch statemnt clean and efficient structure
            return RequiredLevel switch
            {
                10 => "VIP",
                5 => "Premium",
                _ => "Standard"
            };
        }

        //builds formatted string to display car info in UI
        //improves code reuse and keeps formatting logic in 1 place
        public string GetCarInfo()
        {
            return $"{Make} {Model} | Type: {Type} | Seats: {Seats} | Price: £{Price:F2}/day | Level: {GetLevelName()}";
        }
    }
}



/*
 * TEST CASES FOR CAR CLASS:
 * 
 * TC1: Standard car data loaded correctly
 * Input: Car already predefined in RentCar.cs (Toyota Corolla, Sedan, 5 seats, £45.00, Level 1)
 * Expected Output: Car object contains correct values.
 * Result: PASS – Data matches expected values
 *
 * TC2: Premium car data loaded correctly
 * Input: Make="BMW", Model="3 Series", Type="Luxury Sedan", Seats=5, Price=120.00m, RequiredLevel=5
 * Expected: Car object contains correct values.
 * Result: Data matches expected values
 *
 * TC3: VIP car data loaded correctly
 * Input: Make="Ferrari", Model="F8", Type="Supercar", Seats=2, Price=900.00m, RequiredLevel=10
 * Expected: Car object contains correct values.
 * Result: Data matches expected values
 * 
 * TC4: Level Name - Standard
 * Input: RequiredLevel = 1
 * Expected Output: "Standard"
 * Result: PASS - Returns "Standard"
 *
 * TC5: Level Name - Premium
 * Input: RequiredLevel = 5
 * Expected Output: "Premium"
 * Result: PASS - Returns "Premium"
 *
 * TC6: Level Name - VIP
 * Input: RequiredLevel = 10
 * Expected Output: "VIP"
 * Result: PASS - Returns "VIP"
 *
 * TC7: Level Name - Unknown Level
 * Input: RequiredLevel = 99
 * Expected Output: "Standard" (default case)
 * Result: PASS - Returns "Standard"
 *
 * TC8: Car info formatting - Standard
 * Input: Toyota Corolla, Sedan, 5 seats, £45.00, Level 1
 * Expected:
 * "Toyota Corolla | Type: Sedan | Seats: 5 | Price: £45.00/day | Level: Standard"
 * Result: PASS – Formatted correctly
 *
 * TC9: Car Info foramtting - Premium
 * Input: BMW 3 Series, Luxury Sedan, 5 seats, £120.00, Level 5
 * Expected:
 * "BMW 3 Series | Type: Luxury Sedan | Seats: 5 | Price: £120.00/day | Level: Premium"
 * Result: PASS – Formatted correctly
 *
 * TC10: Car Info formatting - VIP
 * Input: Ferrari F8, Supercar, 2 seats, £900.00, Level 10
 * Expected:
 * "Ferrari F8 | Type: Supercar | Seats: 2 | Price: £900.00/day | Level: VIP"
 * Result: PASS – Formatted correctly
 * 
 * TC11: Price Formatting (1 decimal place)
 * Input: Price = 45.5m
 * Expected: "£45.50/day"
 * Result: PASS – Formatted correctly
 *
 * TC12: Price Formatting (whole number)
 * Input: Price = 100m
 * Expected: "£100.00/day"
 * Result: PASS – Formatted correctly
 *
 * TC13: Price Formatting (rounding)
 * Input: Price = 45.999m
 * Expected: Rounded to 2 decimal places (e.g. £46.00/day)
 * Result: PASS – Correctly rounded 
 * 
 * TC17: Update RequiredLevel Property
 * Input: RequiredLevel changed from 1 to 5
 * Expected: GetLevelName() returns "Premium"
 * Result: PASS - Returns "Premium"
 *
 *
 */
