using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using static System.Net.Mime.MediaTypeNames;
using System.Reflection.Metadata;
using AssignmentCARS;

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



/*
*TEST CASES FOR BINARY STORAGE:
 *
 * TC1: Save Customer to File
 * Input: Valid Customer object with auto-generated CustomerID (GUID format, e.g., "19390806-4485-445a-8091-1e2452697897")
 * Expected Output: File "customers/19390806-4485-445a-8091-1e2452697897.dat" created successfully
 * Result: PASS - File was created. I checked in the customers folder and the file is there.
 * 
 * TC2: Load Single Customer from File
 * Input: Existing file "customers/{GUID}.dat" containing serialised customer data
 * Expected Output: Customer object loaded with all properties intact including Name, Password, Level, RentalHistory
 * Result: PASS - Customer loaded fine. All the properties came back correct (Name, Password, Level etc).
 *
 * TC3: Load All Customers
 * Input: Multiple .dat files in customers folder (I tested with 3 customer files)
 * Expected Output: Dictionary containing all 3 customers with CustomerID as key (case-insensitive comparison)
 * Result: PASS - All 3 customers loaded into the dictionary. Checked the count and it was 3.
 *
 * TC4: Handle Corrupted File
 * Input: Corrupted or incomplete .dat file (file ends unexpectedly during read)
 * Expected Output: Catches EndOfStreamException, displays "File corrupted (unexpected end): {filename}" then continues loading other files without crashing
 * Result: PASS - I manually corrupted a file by opening it in TextEdit and deleting some content. The error message showed up and the program kept running. Other files loaded fine.
 * 
 * TC5: Handle missing file during load
 * Input: .dat file is deleted or moved during LoadAll() operation
 * Expected Output: Catches FileNotFoundException, displays "File not found: {filename}" and continues operation
 * Result: NOT TESTED - Couldn't figure out how to delete a file fast enough while the program was loading. The exception handler is there in the code though.
 * 
 * TC6: Save customer with null/empty CustomerID
 * Input: Customer object with CustomerID = null or CustomerID = ""
 * Expected Output: Displays "Invalid customer object." error, does NOT create any file, returns early
 * Result: NOT APPLICABLE - CustomerIDs are auto-generated as GUIDs during signup, so this scenario cannot occur in normal application use. However, the validation code exists in SaveCustomer() as a defensive programming measure.
 * 
 * TC7: Empty customers Folder
 * Input: LoadAll() when customers folder exists but contains no .dat files
 * Expected Output: Returns empty Dictionary<string, Customer> (Count = 0), not null
 * Result: PASS - I deleted all the .dat files and ran LoadAll(). Got back an empty dictionary with 0 items. No crash.
 * 
 * TC8: Polymorphic loading - Premium Customer
 * Input: .dat file with type="Premium" saved by SaveCustomer()
 * Expected Output: LoadAll() creates PremiumCustomer object (not base Customer), Level = 5
 * Result: PASS - Loaded back as PremiumCustomer not just Customer. Level was 5 like it should be.
 *
 * TC9: Polymorphic Loading - VIP Customer
 * Input: .dat file with type="VIP" saved by SaveCustomer()
 * Expected Output: LoadAll() creates VIPCustomer object (not base Customer), Level = 10
 * Result: PASS - VIP customer loaded correctly with Level=10. Polymorphism is working.
 *
 * TC10: SaveAll with multiple customers
 * Input: Dictionary with 5 different customers
 * Expected Output: Creates 5 separate .dat files, one for each CustomerID
 * Result: PASS - Created 5 customers and saved them. All 5 files appeared in the folder.
 * 
 * TC11: File I/O Error Handling
 * Input: Attempt to save customer when folder has no write permissions
 * Expected Output: Catches IOException, displays "Error saving customer {CustomerID}: {error message}", does not crash
 * Result: PASS - I right-clicked the customers folder, selected "Get Info", and locked it. When I tried to save a customer, got the error message "Permission denied" but the program didn't crash. Unlocked the folder after testing.
 * 
 * TC12: Customers Folder Auto-Creation
 * Input: Application starts when "customers" folder doesn't exist
 * Expected Output: Static constructor creates "customers" folder automatically before any operations
 * Result: PASS - Deleted the folder and ran the program. The folder got created automatically.
 *
 * TC13: Duplicate CustomerID handling
 * Input: Two customers with the same CustomerID saved to disk
 * Expected Output: Second save should overwrite the first file
 * Result: PASS - I manually created two customer objects with the same GUID and saved them one after the other. The second save overwrote the first file. When I loaded it back, only the newer data was there. This makes sense because FileMode.Create replaces existing files.
 *
 * TC14: Load file that's not actually binary data
 * Input: Create a random text file and rename it to .dat in customers folder
 * Expected Output: Should catch exception and skip the file
 * Result: NOT TESTED - Would require manually creating an invalid .dat file. The exception handlers in the code should catch this but I didn't verify it through actual testing.
 *
 * TC15: Customer folder is actually a file not a directory
 * Input: Delete customers folder, create a regular file named "customers" 
 * Expected Output: Should handle gracefully or show error
 * Result: FAIL - I deleted the customers folder and created a text file named "customers" with no extension. When I ran the program it crashed with an error saying "Access to the path 'customers' is denied". The static constructor tries to create a directory but doesn't check if "customers" already exists as a file. This breaks the whole application on startup. Should add a check to see if "customers" exists as a file before trying to create it as a folder.
 *
 * TC16: Customer with multiple rental records
 * Input: Customer with 20 rental history items
 * Expected Output: Should save and load all rental records correctly
 * Result: PASS - Created a customer and added 20 rental records to their history. All records saved and when I loaded the customer back, all 20 records were there. File size was bigger but everything worked fine.
 *
 * TC17: Null Customer object passed to SaveCustomer
 * Input: SaveCustomer(null)
 * Expected Output: Should display "Invalid customer object." and return early
 * Result: NOT TESTED - Would require manually calling SaveCustomer(null) in code which doesn't happen during normal signup flow. The null check exists in the method but wasn't verified through actual testing.
 *
 * TC18: Load and retrieve saved customer
 * Input: Save a customer, then load all customers and retrieve that specific customer from dictionary
 * Expected Output: Should find the customer using their CustomerID
 * Result: PASS - Saved a customer with GUID, ran LoadAll(), then retrieved the customer from the dictionary using their CustomerID. Got the correct customer object back with all properties intact.
 */

//My application uses manual binary serialisation to save and load customer data between program sessions, so data safely stored and restored every time the program runs.​
//I implemented custom SaveBinary() and LoadBinary() methods using BinaryWriter and BinaryReader.​
//Customer data is written in a strict, predictable order:​
//CustomerID​
//Name​
//Password​
//Level​
//Rental history count​
//Each rental entry​
//​

//When loading, I read the data back in the exact same order, which prevents data corruption and guarantees reliable object reconstruction.​
//To improve performance, I pre-allocate list capacity using:​
//new List<string>(count)​
//during deserialization, which reduces memory reallocations and improves loading speed.​