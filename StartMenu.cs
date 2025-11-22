using System;
using System.Collections.Generic;

namespace AssignmentCARS
{
    class Menu
    {
        static string customerFile = "customer.dat";

        static void Main()
        {
            var customers = BinaryRepository.Load(customerFile);
            Action save = () => BinaryRepository.Save(customers, customerFile);

            var login = new Login(Pause);
            var signup = new Signup(Pause);

            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("===== CAR WORLD =====");
                Console.ResetColor();
                Console.WriteLine("\nHello, Welcome to Car World!\n");
                Console.WriteLine("(1) Signup");
                Console.WriteLine("(2) Login");
                Console.WriteLine("(3) Quit");
                Console.Write("\nChoose an option: ");
                string input = Console.ReadLine()?.Trim();
                Console.Clear();

                switch (input)
                {
                    case "1":
                        Customer newCustomer = signup.Signup1(customers, save);
                        MainMenu.Show(newCustomer,save);
                        break;
                    case "2":
                        Customer loggedIn = login.Login1(customers);
                        MainMenu.Show(loggedIn, save);
                        save(); // Save any changes made during the session
                        break;
                    case "3": Console.WriteLine("Goodbye!"); System.Threading.Thread.Sleep(1000); Environment.Exit(0); break;
                    default: Console.WriteLine("Invalid input."); Pause(); break;
                }
            }
        }

        static void Pause() { Console.WriteLine("\nPress any key to continue..."); Console.ReadKey(); }
    }
}



////            {

////                Console.WriteLine("a)display the guest list");
////                Console.WriteLine("b)add names");
////                Console.WriteLine("c)remove names");
////                Console.WriteLine("d) Exit");

////                string input = Console.ReadLine();

////                if (input == "a")
////                {
////                    Console.WriteLine("Guest List:");
////                    foreach (string name in nameList)
////                    {
////                        Console.WriteLine("-" + name);

////                    }
////                }

////                else if (input == "b")
////                {
////                    Console.WriteLine("Enter a new name to add to the list.");
////                    string newName = Console.ReadLine();
////                    nameList.Add(newName);
////                    Console.WriteLine($"{newName} has beeen added to the guest list");
////                }
////                else if (input == "c")
////                {
////                    Console.WriteLine("Enter a name to remove from the list");
////                    string nameToRemove = Console.ReadLine();
////                    if (nameList.Contains(nameToRemove))
////                    {
////                        nameList.Remove(nameToRemove);
////                        Console.WriteLine($"{nameToRemove} has been removed from the guest list");
////                    }
////                    else
////                    {
////                        Console.WriteLine($"{nameToRemove} is not in the guest list");
////                    }
////                }
////                else if (input == "d")
////                {
////                    Console.WriteLine("exiting program");
////                    break;
////                }
////                else
////                {
////                    Console.WriteLine("invalid input. select from a,b or c");
////                }

////            }
////        }
////    }

//using System;
//using System.Collections.Generic;
//using System.Linq;

////*example * 1 *//

//namespace Week002
//{
//    internal class Program
//    {
//        static void Main(string[] args)
//        {
//            List<int> lapTimes = new List<int>();
//            int input;

//            Console.WriteLine("Enter lap times in seconds (enter 0 to finish):");

//            // Input loop
//            while (true)
//            {
//                Console.Write("Enter lap time: ");
//                string userInput = Console.ReadLine();

//                // Try to parse input to int
//                if (int.TryParse(userInput, out input))
//                {
//                    if (input == 0)
//                    {
//                        break;
//                    }

//                    if (input > 0)
//                    {
//                        lapTimes.Add(input);
//                    }
//                    else
//                    {
//                        Console.WriteLine("Lap time must be a positive number or 0 to finish.");
//                    }
//                }
//                else
//                {
//                    Console.WriteLine("Invalid input. Please enter a number.");
//                }
//            }

//            // Check if any lap times were entered
//            if (lapTimes.Count > 0)
//            {
//                // Calculate average
//                double average = Average(lapTimes);

//                // Sort lap times (fastest to slowest)
//                lapTimes.Sort();

//                // Output results
//                Console.WriteLine($"\nNumber of laps: {lapTimes.Count}");
//                Console.WriteLine($"Average lap time: {average:F2} seconds");

//                Console.WriteLine("Lap times from fastest to slowest:");
//                foreach (int time in lapTimes)
//                {
//                    Console.WriteLine($"{time} seconds");
//                }
//            }
//            else
//            {
//                Console.WriteLine("No lap times were entered.");
//            }
//        }

//        // Average function
//        static double Average(List<int> times)
//        {
//            return times.Average();
//        }
//    }
//}

////*example*2*//

////namespace Example2
////{
////    internal class Program
////    {
////        static void Main(string[] args)
////        {
////            HashSet<int> idSet = new HashSet<int>();

////            while (true)
////            {
////                string input = Console.ReadLine();
////                if (input == "q")
////                    break;

////                    int num = Convert.ToInt32(input);
////                if (idSet.Contains(num))
////                {
////                    Console.WriteLine("The id already exists");
////                }
////                else
////                {
////                    idSet.Add(num);
////                }
////            }
////            foreach (int id in idSet)
////            {
////                Console.WriteLine(id);
////            }
////        }
////    }
////}

////*example*3*//
////namespace Example3
////{
////    internal class Program
////    {
////        static void Main(string[]args)
////        {
////            Dictionary<string, int> dictionary = new Dictionary<string, int>();
////            dictionary.Add("Junghun Yoo", 16357460);
////            dictionary.Add("Laibah Khan", 123456);
////            dictionary.Add("Freya Johnson", 23456789);

////            while (true)
////            {
////                string name = Console.ReadLine();
////                if (name == "q")
////                    break;
////                if(dictionary.ContainsKey(name))
////                {
////                    Console.WriteLine(dictionary[name]);
////                }
////                else
////                {
////                    Console.WriteLine(("The patient does not exist"));

////                }
////            }

////        }
////    }
////}


////*example*4*//

////namespace Example4
////{
////    internal class Program
////    {
////        static void Main(string[] args)
////        {
////            List<string> nameList = new List<string>();
////            nameList.Add("laibah");
////            nameList.Add("freya");
////            nameList.Add("john");
////            nameList.Add("adam");

////            while (true)
////            {

////                Console.WriteLine("a)display the guest list");
////                Console.WriteLine("b)add names");
////                Console.WriteLine("c)remove names");
////                Console.WriteLine("d) Exit");

////                string input = Console.ReadLine();

////                if (input == "a")
////                {
////                    Console.WriteLine("Guest List:");
////                    foreach (string name in nameList)
////                    {
////                        Console.WriteLine("-" + name);

////                    }
////                }

////                else if (input == "b")
////                {
////                    Console.WriteLine("Enter a new name to add to the list.");
////                    string newName = Console.ReadLine();
////                    nameList.Add(newName);
////                    Console.WriteLine($"{newName} has beeen added to the guest list");
////                }
////                else if (input == "c")
////                {
////                    Console.WriteLine("Enter a name to remove from the list");
////                    string nameToRemove = Console.ReadLine();
////                    if (nameList.Contains(nameToRemove))
////                    {
////                        nameList.Remove(nameToRemove);
////                        Console.WriteLine($"{nameToRemove} has been removed from the guest list");
////                    }
////                    else
////                    {
////                        Console.WriteLine($"{nameToRemove} is not in the guest list");
////                    }
////                }
////                else if (input == "d")
////                {
////                    Console.WriteLine("exiting program");
////                    break;
////                }
////                else
////                {
////                    Console.WriteLine("invalid input. select from a,b or c");
////                }

////            }
////        }
////    }
////}