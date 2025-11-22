using System.IO;

namespace AssignmentCARS
{
    internal class PremiumCustomer : Customer
    {
        public PremiumCustomer() : base("", "", 5) { }
        public PremiumCustomer(string name, string password) : base(name, password, 5) { }

        internal string GetPerk() => "Access to premium cars";

        public override void SaveBinary(BinaryWriter bw) => base.SaveBinary(bw);
        public override void LoadBinary(BinaryReader br) => base.LoadBinary(br);
    }
}
