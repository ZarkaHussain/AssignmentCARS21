using System;
using System.Collections.Generic;
using System.IO;

namespace AssignmentCARS
{
    //this class is responsible for data persistence
    //saves and loads customer data to from files so not data is lost wen app closes
    public static class BinaryRepository
    {
        //folder where customer files are stored
        //keeping data in a dedicated folder improves organisation and securty.
        private static readonly string folder = "customers";

        //static constructor runs once wen class first used
        //makes sure storage folder always exists
        static BinaryRepository()
        {
            //defensive programming this prevents crashes if folder misisng
            if (!Directory.Exists(folder))

                Directory.CreateDirectory(folder);
        }

        //loads every customer object from disk into memory
        //uses dictionary for kast key based loookup
        public static Dictionary<string, Customer> LoadAll()
        {
            var dict = new Dictionary<string, Customer>(StringComparer.OrdinalIgnoreCase);

            //loop through all customer files
            foreach (string path in Directory.GetFiles(folder, "*.dat"))
            {
                try
                {
                    //FileStream opens file at byte level
                    using FileStream file = File.Open(path, FileMode.Open, FileAccess.Read);

                    //BinaryReader used for deserialisation
                    //convertt raw bytes back into real C# objects
                    using BinaryReader br = new BinaryReader(file);

                    //read saved customer "type"
                    string type = br.ReadString();


                    //Polymorphism-creates correct object based on saved type
                    //inheritance and polymorphism
                    Customer cust = type switch
                    {
                        "Premium" => new PremiumCustomer(),
                        "VIP" => new VIPCustomer(),
                        _ => new Customer()
                    };

                    //deserialisation step
                    //reads binary data back into object fields
                    cust.LoadBinary(br);

                    //validate before adding
                    if (!string.IsNullOrWhiteSpace(cust.CustomerID))
                        dict[cust.CustomerID] = cust;
                }

                //very specific exception handling improves robustness of my program
                catch (FileNotFoundException)// this exception is when a file aint found
                {
                    Console.WriteLine($"File not found: {Path.GetFileName(path)}");
                }
                catch (EndOfStreamException)
                {
                    //happens when file incomplete or corrupted
                    Console.WriteLine($"File corrupted (unexpected end): {Path.GetFileName(path)}");
                }
                catch (IOException ex)
                {
                    //handles disk or permission issues
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

        //saves single customer to disk
        //serialisation here converts object into bytes for storage
        public static void SaveCustomer(Customer cust)
        {
            //Validate early for improving robustness
            if (cust == null || string.IsNullOrWhiteSpace(cust.CustomerID))
            {
                Console.WriteLine("Invalid customer object.");
                return;
            }

            try
            {
                // creates files path for this customer
                string path = Path.Combine(folder, $"{cust.CustomerID}.dat");
                //FileStream opens file for writing
                using FileStream file = File.Open(path, FileMode.Create, FileAccess.Write);
                //BianryWriter used for serialsiation
                //converts object data into stream of bytes
                using BinaryWriter bw = new BinaryWriter(file);

                //save type for polymorphic loading later
                string type = cust switch
                {
                    PremiumCustomer => "Premium",
                    VIPCustomer => "VIP",
                    _ => "Customer"
                };

                //write metadata
                bw.Write(type);

                //serialisation
                //writes object fields as binary
                cust.SaveBinary(bw);
            }
            catch (IOException ex)
            {
                //error handling rpevents crahses
                Console.WriteLine($"Error saving customer {cust?.CustomerID}: {ex.Message}");
            }
            
        }

     

        //saves all customers to disk
        //used after bulk updates like delete operations
        public static void SaveAll(Dictionary<string, Customer> customers)
        {
            foreach (var cust in customers.Values)
            {
                SaveCustomer(cust);
            }
        }

        //simple alias method for reaadability
        public static Dictionary<string, Customer> Load()
        {
            return LoadAll();
        }


    }
}