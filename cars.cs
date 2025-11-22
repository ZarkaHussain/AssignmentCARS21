using System;

namespace AssignmentCARS
{
    public class Car
    {
        public string Make { get; set; }
        public string Model { get; set; }
        public string Type { get; set; }
        public int Seats { get; set; }
        public decimal Price { get; set; }
        public int RequiredLevel { get; set; }  //1=Standard, 5=Premium, 10=VIP

        public Car(string make, string model, string type, int seats, decimal price, int requiredLevel)
        {
            Make = make;
            Model = model;
            Type = type;
            Seats = seats;
            Price = price;
            RequiredLevel = requiredLevel;
        }

        public string GetLevelName()
        {
            return RequiredLevel switch
            {
                10 => "VIP",
                5 => "Premium",
                _ => "Standard"
            };
        }

        public string GetCarInfo()
        {
            return $"{Make} {Model} | Type: {Type} | Seats: {Seats} | Price: £{Price:F2}/day | Level: {GetLevelName()}";
        }
    }
}

