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

