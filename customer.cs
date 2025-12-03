using System;
using System.Collections.Generic;
using System.IO;

namespace AssignmentCARS
{
    public class Customer
    {
        public string CustomerID { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public int Level { get; set; } = 1; // 1=Standard, 5=Premium, 10=VIP

        protected List<string> rentalHistory;
        public IReadOnlyList<string> RentalHistory => rentalHistory.AsReadOnly();

        public Customer()
        {
            CustomerID = "";
            Name = "";
            Password = "";
            Level = 1;
            rentalHistory = new List<string>();
        }

        public Customer(string name, string password, int level = 1)
        {
            CustomerID = Guid.NewGuid().ToString();
            Name = name;
            Password = password;
            Level = level;
            rentalHistory = new List<string>();
        }

        // adds rental + checks for upgrade
        public Customer AddRentalAndCheckUpgrade()
        {
            rentalHistory.Add("Car rented"); 
            UpdateLevelFromRentalCount();

            if (Level == 5 && this is not PremiumCustomer)
                return new PremiumCustomer(this);

            if (Level == 10 && this is not VIPCustomer)
                return new VIPCustomer(this);

            return this;
        }

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


        public virtual void SaveBinary(BinaryWriter bw)
        {
            bw.Write(CustomerID);
            bw.Write(Name);
            bw.Write(Password);
            bw.Write(Level);
            bw.Write(rentalHistory.Count);
            foreach (var rental in rentalHistory)
                bw.Write(rental);
        }

        public virtual void LoadBinary(BinaryReader br)
        {
            CustomerID = br.ReadString();
            Name = br.ReadString();
            Password = br.ReadString();
            Level = br.ReadInt32();

            int count = br.ReadInt32();
            rentalHistory = new List<string>(count);
            for (int i = 0; i < count; i++)
                rentalHistory.Add(br.ReadString());
        }
    }
}  

//rental history saved in customer class// needs own;



//using System;
//using System.Collections.Generic;

//namespace AssignmentCARS
//{
//    [Serializable]
//    public class Customer
//    {
//        public string Name { get; private set; }
//        public string Password { get; private set; }
//        public string Level { get; private set; } = "Standard";

//        private readonly List<string> _rentalHistory = new List<string>();
//        public IReadOnlyList<string> RentalHistory => _rentalHistory;

//        public Customer(string name, string password)
//        {
//            Name = name;
//            Password = password;
//        }

//        public void AddRental(string car)
//        {
//            if (!string.IsNullOrWhiteSpace(car))
//            {
//                _rentalHistory.Add(car);
//                UpdateLevel();
//            }
//        }

//        private void UpdateLevel()
//        {
//            int count = _rentalHistory.Count;

//            if (count >= 10)
//                Level = "VIP";
//            else if (count >= 5)
//                Level = "Premium";
//            else
//                Level = "Standard";
//        }
//    }
//}
