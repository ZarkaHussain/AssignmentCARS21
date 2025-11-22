using System;
using System.Collections.Generic;
using System.IO;

namespace AssignmentCARS
{
    public static class BinaryRepository
    {
        public static Dictionary<string, Customer> Load(string path)
        {
            var customers = new Dictionary<string, Customer>(StringComparer.OrdinalIgnoreCase);

            if (!File.Exists(path))
                return customers;

            FileStream file = File.Open(path, FileMode.Open);
            BinaryReader br = new BinaryReader(file);

            try
            {
                int count = br.ReadInt32(); //reads count first

                for (int i = 0; i < count; i++) //then reads each object
                {
                    string type = br.ReadString(); //read type string first to handle polymorphism (Customer,Premium,VIP)

                    Customer cust = type switch
                    {
                        "Premium" => new PremiumCustomer(),
                        "VIP" => new VIPCustomer(),
                        _ => new Customer()
                    };

                    cust.LoadBinary(br);

                    if (!string.IsNullOrWhiteSpace(cust.Name))
                        customers[cust.Name] = cust;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading customer data: {ex.Message}");
            }
            finally
            {
                br.Close();
                file.Close();
            }

            return customers;
        }

        public static void Save(Dictionary<string, Customer> customers, string path)
        {
            FileStream file = File.Open(path, FileMode.Create);
            BinaryWriter bw = new BinaryWriter(file);

            try
            {
                bw.Write(customers.Count); //writes count first

                foreach (Customer cust in customers.Values)
                {

                    string type = cust switch
                    {
                        PremiumCustomer => "Premium",
                        VIPCustomer => "VIP",
                        _ => "Customer"
                    };

                    bw.Write(type); //write type string
                    cust.SaveBinary(bw); //each customer saves
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving customer data: {ex.Message}");
            }
            finally
            {
                bw.Close(); 
                file.Close();
            }
        }
    }
}