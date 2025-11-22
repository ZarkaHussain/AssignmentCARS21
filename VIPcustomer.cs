using System.IO;

namespace AssignmentCARS
{
    internal class VIPCustomer : Customer
    {
        public VIPCustomer() : base("", "", 10) { }
        public VIPCustomer(string name, string password) : base(name, password, 10) { }

        internal string GetPerk() => "Access to all cars and special discounts.";

        public override void SaveBinary(BinaryWriter bw) => base.SaveBinary(bw);
        public override void LoadBinary(BinaryReader br) => base.LoadBinary(br);
    }
}
