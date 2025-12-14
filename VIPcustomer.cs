using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.NetworkInformation;
using System.Runtime.ConstrainedExecution;
using AssignmentCARS;

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



//This file also contains some notes(regarding the powerpoint presentation slides
//due to the fact i wasnt abkle to fit everything on the slides bcuase of the tight time constraint of 7 mins
//so im explaining decisions made. in here
//in the application. These comments are included to demonstrate understanding
//of data handling, performance, and scalability.

//Data Collections & Algorithmss
//-------------------------------
//Customers in the application are stored in a Dictionary<string, Customer>
//instead of a List. This is because a dictionary allows O(1) key-based lookups,
//which is important for login functionality and admin operations. Using a List
//would require O(n) searching through every customer, which doesnt scale well
//as the amount of data grows.

//Each customer's rental history is stored using a List<string>. Lists allow fast
//appending when a car is rented and also preserve the order of rentals, which is
//used by the application for upgrade logic.

//To maintain encapsulation, the rental history is exposed as an IReadOnlyList.
//This prevents external parts of the program from modifying internal state and
//helps ensure data integrity.

//In admin mode, LINQ is used to filter and analyse data. Methods such as Where,
//SelectMany, GroupBy, OrderByDescending, and Take are used to generate statistics
//like the most rented vehicles. This approach replaces manual loops and makes the
//code easier to read and maintain while remaining efficient.

//If this system were scaled further, in-memory dictionaries would not be ideal.
//A more suitable approach would be to use indexed database storage. Caching could
//also be introduced for frequently accessed customers to reduce disk access, and
//more efficientn memory data structures would be required for very large datasets.
//Indexing strategies could also be applied to improve LINQ query performance.

//The application currently runs with a small dataset and mostly runs on a single
//thread, so using a standard Dictionary is sufficient. In a multi threaded or
//higher load environment, a ConcurrentDictionary would be more appropriate, as it
//allows multiple threads to safely read and write data without causing race
//conditions or data corruption.

//Command line interface
//-----------------------
//These are some notes explaining decisions for the admin CLI system.

//Using a command line interface improves the admin experience. Admins can log in
//and execute tasks instantly without navigating menus, which speeds up workflow.

//The admin system is organised into separate classes for maintainability and structure:

//- AdminLogin: handles authentication and launching
//- AdminStats: handles analytics and statistics
//- AdminCustomerManager: handles customer management

//Some ideas for future improvements include:

//- Adding more detailed error messages for missing or invalid arguments
//- Avoiding hard-coded admin credentials by storing them securely and hashing them
//- Using a proper CLI parsing library for better formatting and automatic help messages
//- Supporting more advanced admin commands directly from the CLI
//- Logging failed login attempts with timestamps to improve security and auditing


//Robustness
//-------------
//These notes explain the robustness and defensive programming techniques
//used in the application.

//File operations such as loading, saving, and deleting customer data are wrapped
//in try/catch blocks. This helps handle issues like missing files, permission
//problems, or corrupted data safely. Instead of crashing, the program shows
//clear error messages and continues running.

//User input is validated using TryParse() and other defensive checks to
//prevent invalid selections and runtime exceptions, such as FormatException.

//Critical operations, like deleting files or overwriting saved data, have
//targeted exception handling to ensure failures are handled safely and clearly.

//Safe fallbacks are used throughout, including the null-coalescing operator (??)
//and default object creation. These allow system to recover gracefully if
//data missing or fails to load.

//Future improvements could include:

//- Adding file based logging to permanently record errors
//- Creating custom exception classes for specific failure cases
//- Hashing and encrypting passwords instead of storing them in plain text
//- Adding retry logic for temporary file access failures

//Overall, input validation, exception handling, and safe fallbacks are used
//to keep the system stable and prevent crashes.

//Encapsulation & constructors
//----------------------------
//These notes explain how object-oriented programming (OOP) principles
//are applied in the project.

//Encapsulation is used to protect data. For example, a customer's rental
//history is stored in a protected list and exposed only as a read-only list.
//This ensures other parts of the program cannot accidentally modify it.

//Constructors are used to ensure objects are always created in a valid state:
//- Default constructor: used for loading objects from files
//- Parameterised constructor: used for creating new users
//- Copy-style constructors: used in PremiumCustomer and VIPCustomer
//  to safely upgrade users

//Inheritance is used so specialised customer types extend the base Customer class.
//This keeps code organised, reusable, and easier to extend in the future.

//Future improvements if system scaled up could include:
//- Using interfaces to reduce tight coupling between classes
//- Adding dependency injection to make testing easier
//- Using factory patterns to control object creation
//- Using DTOs (Data Transfer Objects) to separate data transfer from business logic
//- Making the base class abstract to enforce consistency

//DTOs are simple classes used only to move data between parts of the system
//without adding extra logic.

//Inhertance & polymorphism
//--------------------------
//These notes explain how inheritance and polymorphism are applied in the system.

//Inheritance is used so that specialised customer classes, such as PremiumCustomer
//and VIPCustomer, extend the base Customer class. This keeps the code organised,
//reusable, and easy to maintain.

//Polymorphism is used by overriding methods in child classes. For example,
//LoadBinary() is overridden in VIPCustomer so that child class can adjust
//behaviour while still reusing base logic with base.LoadBinary(). ensuring
//specialised behaviour applied while keeping code reusable.

//Constructor chaining is used when upgrading customers. existing Customer
//object is passed into the child class constructor to preserve the same ID and
//rental history instead of creating a new customer from scratch.

//Because of this design, the system can treat all customer types as Customer
//while still executing the correct specialised behaviour at runtime.

//Future improvements for scalability and maintainability could include:
//- Making the base Customer class abstract to enforce consistent behaviour
//- Using interfaces for shared behaviour across classes
//- Implementing design patterns like Strategy for upgrades and discounts
//- Adding dependency injection to reduce tight coupling like i did in signup/login

//These changes would make the system more scalable and enterprise-ready.


//Serialisation & Binary Files
//----------------------------
//These notes explain why binary files were chosen for data storage.

//Binary files were selected instead of JSON, XML, or plain text because they:
//- Are faster to read and write, which is important for large data sets
//- Are more compact than text formats
//- Are harder to tamper with
//- Are well suited to low-level file I/O, aligning with the Systems Programming focus

//Binary storage also meets the Security and Data Persistence criteria in the marking brief.

//BinaryReader and BinaryWriter are used to efficiently store customer data
//across sessions, keeping the data portable and simple to manage.

//Optional improvements if the system scaled further:
//- Encrypt binary files to protect sensitive data
//- Hash passwords instead of storing plain text
//- Add checksums to detect file corruption
//- Replace flat files with a structured database like SQLite or SQL Server

//Writing fast code
//----------------
//These notes explain performance optimisations and fast code practices.

//When loading customer data from a binary file, the number of rentals is stored
//and read first. This allows the list to be sized precisely avoiding repeated
//resizing and memory reallocation during item addition. This improves performance
//and reduces memory usage especially for large rental histories.

//Preset capacity was not used elsewhere because most collections grow dynamically
//at runtime (e.g., new customers or added rentals). pre-allocating capacity would
//require guessing sizes, potentially wasting memory and complicating code
//without guaranteed benefit. The optimisation in LoadBinary() remains valid even
//as the system grows because file loading is deterministic with a known data size.

//Benefits:
//- Faster deserialization
//- Less memory reallocation
//- Scales well as rental histories grow

//In AdminStats, AsParallel() is used because calculating rental statistics is
//CPU-bound and processes large in-memory datasets. Conditional parallelism
//ensures:
//- Small datasets avoid unnecessary overhead.
//- Automatic performance improvements for large datasets
//- Safe multi-threading using ConcurrentDictionary

//Testing with heavy dummy data (1,000,000 customers × 100 rentals) showed
//that AsParallel() performs best with very large datasets.

//Overall, preset list capacity where the size is known and conditional parallel
//LINQ in AdminStats are targeted optimisations that improve performance and
//future scalability without harming current execution.

//Writing fast code Part2
//------------------
//These notes explain caching and string optimisation in the system.

//Originally, selecting Most Rented Cars caused:
//- Customers being reloaded from disk each time
//- Rental counts recalculated repeatedly

//To fix this, a cache (rentalCountCache) was added along with an
//InvalidateCache() method, which refreshes data only when rentals change.
//This reduces disk I/O and looping, making lookups O(1) and improving performance.

//StringBuilder is used in PrintResults() because concatenating strings with '+='
//inside loops creates a new string object each time due to string immutability.
//This approach leads to repeated memory allocation and cleanup, making the process less
//efficient for large outputs.

//StringBuilder modifies a single mutable buffer in memory this allows text
//to be appended efficiently. even though current output is small this ensures
//better performance and lower memory usage if the system scales.

//Benefits:
//- Faster string construction in loops
//- Reduced memory allocations
//- Cleaner, maintainable code
//- Scales well as output size increases





