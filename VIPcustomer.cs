using System.Collections.Generic;
using System.IO;

namespace AssignmentCARS
{
    internal class VIPCustomer : Customer
    {
        public VIPCustomer() : base()
        {
            Level = 10;
        }

        public VIPCustomer(Customer old) : base(old.Name, old.Password, 10)
        {
            this.CustomerID = old.CustomerID;
            this.rentalHistory = new List<string>(old.RentalHistory);
        }

        public override void LoadBinary(BinaryReader br)
        {
            base.LoadBinary(br);
            Level = 10;
        }
    }
}
