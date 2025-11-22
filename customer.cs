using System;
using System.Collections.Generic;
using System.IO;

namespace AssignmentCARS
{
    public class Customer
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public int Level { get; set; } = 1; // 1=Standard, 5=Premium, 10=VIP

        protected List<string> rentalHistory;
        public IReadOnlyList<string> RentalHistory => rentalHistory.AsReadOnly();

        public Customer()  // parameterless constructor for loading
        {
            Name = "";
            Password = "";
            Level = 1;
            rentalHistory = new List<string>();
        }

        public Customer(string name, string password, int level = 1)
        {
            Name = name;
            Password = password;
            Level = level;
            rentalHistory = new List<string>();
        }

        public void AddRental(string carName)
        {
            rentalHistory.Add(carName);
            UpdateLevelFromRentalCount();
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

        public virtual void SaveBinary(BinaryWriter bw)
        {
            bw.Write(Name);
            bw.Write(Password);
            bw.Write(Level);
            bw.Write(rentalHistory.Count);
            foreach (var rental in rentalHistory)
                bw.Write(rental);
        }

        public virtual void LoadBinary(BinaryReader br)
        {
            Name = br.ReadString();
            Password = br.ReadString();
            Level = br.ReadInt32();
            int count = br.ReadInt32();
            rentalHistory = new List<string>(count);
            for (int i = 0; i < count; i++)
                rentalHistory.Add(br.ReadString());
        }

        public override string ToString()
        {
            string levelName = Level switch
            {
                10 => "VIP",
                5 => "Premium",
                _ => "Standard"
            };
            return $"Customer Name: {Name}\n" +
                   $"Password: {Password}\n" +
                   $"Level: {levelName}\n" +
                   $"Rental History: {string.Join(", ", rentalHistory)}";
        }
    }
}



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
