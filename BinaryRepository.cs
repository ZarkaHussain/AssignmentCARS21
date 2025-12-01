using System;
using System.Collections.Generic;
using System.IO;

namespace AssignmentCARS
{
    public static class BinaryRepository
    {

        private static readonly string folder = "customers";

        static BinaryRepository()
        {
            if (!Directory.Exists(folder))

                Directory.CreateDirectory(folder);
        }

        public static Dictionary<string, Customer> LoadAll()
        {
            var dict = new Dictionary<string, Customer>(StringComparer.OrdinalIgnoreCase);

            foreach (string path in Directory.GetFiles(folder, "*.dat"))
            {
                try
                {
                    using FileStream file = File.Open(path, FileMode.Open, FileAccess.Read);
                    using BinaryReader br = new BinaryReader(file);

                    string type = br.ReadString();

                    Customer cust = type switch
                    {
                        "Premium" => new PremiumCustomer(),
                        "VIP" => new VIPCustomer(),
                        _ => new Customer()
                    };

                    cust.LoadBinary(br);

                    if (!string.IsNullOrWhiteSpace(cust.CustomerID))
                        dict[cust.CustomerID] = cust;
                }
                catch (FileNotFoundException)// this exception is when a file aint found
                {
                    Console.WriteLine($"File not found: {Path.GetFileName(path)}");
                }
                catch (EndOfStreamException)
                {
                    Console.WriteLine($"File corrupted (unexpected end): {Path.GetFileName(path)}");
                }
                catch (IOException ex)
                {
                    Console.WriteLine($"I/O error reading {Path.GetFileName(path)}: {ex.Message}");
                }
                catch (Exception ex)
                {
                    //fallback for anything else unexpected happening
                    Console.WriteLine($"Error loading {Path.GetFileName(path)}: {ex.Message}");
                }
            }

            return dict;
        }


        public static void SaveCustomer(Customer cust)
        {
            //Validate early
            if (cust == null || string.IsNullOrWhiteSpace(cust.CustomerID))
            {
                Console.WriteLine("Invalid customer object.");
                return;
            }

            try
            {
                string path = Path.Combine(folder, $"{cust.CustomerID}.dat");
                using FileStream file = File.Open(path, FileMode.Create, FileAccess.Write);
                using BinaryWriter bw = new BinaryWriter(file);

                string type = cust switch
                {
                    PremiumCustomer => "Premium",
                    VIPCustomer => "VIP",
                    _ => "Customer"
                };

                bw.Write(type);
                cust.SaveBinary(bw);
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Error saving customer {cust?.CustomerID}: {ex.Message}");
            }
            
        }

     


        public static void SaveAll(Dictionary<string, Customer> customers)
        {
            foreach (var cust in customers.Values)
            {
                SaveCustomer(cust);
            }
        }

        public static Dictionary<string, Customer> Load()
        {
            return LoadAll();
        }


    }
}