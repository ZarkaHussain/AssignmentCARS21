using System.Collections.Generic;
using System.IO;

namespace AssignmentCARS
{
    //PremiumCustomer is a type of Customer
    //demonstrates inheritance becuase extends base Customer class
    //allows system to treat Premium and standard customers differently
    internal class PremiumCustomer : Customer
    {
        //default constructor
        //automatically sets level to Premium (5)
        public PremiumCustomer() : base()
        {
            Level = 5;
        }

        // upgrade constructor
        //used wen a normal Customer is promoted to Premium
        //avoids creating completely new customer record from scratch
        //copies important data from old customer object
        public PremiumCustomer(Customer old) : base(old.Name, old.Password, 5)
        {
            //keeps same unique ID so data stays consistent when saved to files
            this.CustomerID = old.CustomerID;
            //copies rental history so no data lost during upgrade
            //uses List to keep ordered history of rentals
            this.rentalHistory = new List<string>(old.RentalHistory);
        }

        //overriden method used during deserialisation- loading from binary files
        //shows Polymorphism becuase the method behaves differently to the base class
        public override void LoadBinary(BinaryReader br)
        {
            //call base method to reuse common loading logic
            base.LoadBinary(br);
            //force lvel to Premium when loading from storage
            //ensures data integrity and prevents tampering or corruption
            Level = 5;
        }
    }
}
