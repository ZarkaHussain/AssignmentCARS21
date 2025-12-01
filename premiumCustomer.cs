using System.Collections.Generic;
using System.IO;

namespace AssignmentCARS
{
    internal class PremiumCustomer : Customer
    {
        public PremiumCustomer() : base()
        {
            Level = 5;
        }

        // upgrade constructor
        public PremiumCustomer(Customer old) : base(old.Name, old.Password, 5)
        {
            this.CustomerID = old.CustomerID;
            this.rentalHistory = new List<string>(old.RentalHistory);
        }

        public override void LoadBinary(BinaryReader br)
        {
            base.LoadBinary(br);
            Level = 5;
        }
    }
}
