using System.Collections.Generic;
using System.IO;

namespace AssignmentCARS
{
    //represents VIP customer which is highest customer level
    internal class VIPCustomer : Customer
    {
        //default constructor
        //calls base constructor and sets level to VIP (10)
        public VIPCustomer() : base()
        {
            Level = 10;
        }

        //takes an existing customer and upgrades them to VIP
        public VIPCustomer(Customer old) : base(old.Name, old.Password, 10)
        {
            //preserve the original customer ID
            this.CustomerID = old.CustomerID;
            //copy existing rental history
            this.rentalHistory = new List<string>(old.RentalHistory);
        }

        //overrides binary loading to ensure level is always set to VIP
        public override void LoadBinary(BinaryReader br)
        {
            //load common customer data from base class
            base.LoadBinary(br);
            //force level to VIP after loading
            Level = 10;
        }
    }
}
