using System;
using System.Collections.Generic;
using System.IO;

namespace AssignmentCARS
{
    //this class represents single customer in system
    public class Customer
    {
        //unique ID used as key when storing customers in a dictionary
        public string CustomerID { get; set; }
        //customer name and password data
        public string Name { get; set; }
        public string Password { get; set; }
        //access level for customer
        public int Level { get; set; } = 1; // 1=Standard, 5=Premium, 10=VIP

        //internal list to store rental history (list used for fast appending)
        protected List<string> rentalHistory;

        //exposes read only version of list to protect internal data (encapsualtion )
        public IReadOnlyList<string> RentalHistory => rentalHistory.AsReadOnly();

        //default cosntructor used when loading customers from files
        public Customer()
        {
            CustomerID = "";
            Name = "";
            Password = "";
            Level = 1;
            rentalHistory = new List<string>();
        }

        // cosntructor used when creating new customer during signup 
        public Customer(string name, string password, int level = 1)
        {
            //GUID creates ID that is unique for each customer so there's no collisions
            CustomerID = Guid.NewGuid().ToString();
            Name = name;
            Password = password;
            Level = level;
            rentalHistory = new List<string>();
        }


        // adds rental + checks if upgrade needed
        public Customer AddRentalAndCheckUpgrade()
        {
            //fast append to list
            rentalHistory.Add("Car rented");
            //update customer level based on number of rentals
            UpdateLevelFromRentalCount();

            //automatic upgrade if 5 rentals made (now Premium customer)
            if (Level == 5 && this is not PremiumCustomer)
                return new PremiumCustomer(this);

            //automatic upgrade if 5 rentals made (now VIP customer)
            if (Level == 10 && this is not VIPCustomer)
                return new VIPCustomer(this);

            return this;
        }

        //private helper method calculates customer level from rental count
        private void UpdateLevelFromRentalCount()
        {
            int count = rentalHistory.Count;

            if (count >= 10)
                Level = 10;
            else if (count >= 5)
                Level = 5;
            else
                Level = 1;
        }

        //adds rental with car name stored
        public virtual void AddRental(string carName)
        {
            rentalHistory.Add(carName);

            // upgrade to premium at 5 rentals
            if (Level < 5 && rentalHistory.Count >= 5)
            {
                Level = 5; // Premium
            }

            // upgrade to VIP at 10 rentals
            if (Level < 10 && rentalHistory.Count >= 10)
            {
                Level = 10; //VIP
            }
        }

        //serialisation
        //this method writes customer object into a binary file
        public virtual void SaveBinary(BinaryWriter bw)
        {
            bw.Write(CustomerID);
            bw.Write(Name);
            bw.Write(Password);
            bw.Write(Level);
            //save number of rentals first
            bw.Write(rentalHistory.Count);
            //then saves each rental string
            foreach (var rental in rentalHistory)
                bw.Write(rental);
        }

        //deserialsiation
        //reads customer data back from a binary file
        //demonstratws safe data reconstruction from persistent storage
        public virtual void LoadBinary(BinaryReader br)
        {
            CustomerID = br.ReadString();
            Name = br.ReadString();
            Password = br.ReadString();
            Level = br.ReadInt32();

            //read how many rentals exist
            int count = br.ReadInt32();
            //pre-size list for better performance 
            rentalHistory = new List<string>(count);
            //read each rental back into the list
            for (int i = 0; i < count; i++)
                rentalHistory.Add(br.ReadString());
        }        
    }
}  

