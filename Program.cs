using System;
using System.Collections.Generic;
using System.Threading;

namespace AssignmentCARS
{
    class Program
    {
        static void Main(string[] args)
        {
            // If admin login via CLI args
            if (args.Length > 0)
            {
                new AdminLogin().Run(args);
                return;
            }

            // Load all customer files from /customers/
            var customers = BinaryRepository.LoadAll();

            // Save action to write all .dat files
            Action saveAll = () => BinaryRepository.SaveAll(customers);

            var login = new Login(Pause);
            var signup = new Signup(Pause);

            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("HELLO, WELCOME TO");
                Console.ResetColor();
                Console.WriteLine();
                ShowAnimatedBanner();

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\n(1) Signup");
                Console.WriteLine("(2) Login");
                Console.WriteLine("(3) Quit");
                Console.ResetColor();
                Console.Write("\nChoose an option: ");
                string input = Console.ReadLine()?.Trim() ?? "";

   
                try
                {
                    if (!int.TryParse(input, out int option))
                        throw new FormatException();

                    if (option < 1 || option > 3)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Error: Please choose 1, 2, or 3.");
                        Console.ResetColor();
                        Pause();
                        continue;
                    }

                    switch (option)
                    {
                        case 1:
                            Customer newCust = signup.Signup1(customers, saveAll);
                            MainMenu.Show(newCust, saveAll);
                            break;

                        case 2:
                            Customer loggedIn = login.Login1(customers);
                            MainMenu.Show(loggedIn, saveAll);
                            saveAll();
                            break;

                        case 3:
                            Console.WriteLine("Goodbye!");
                            Thread.Sleep(1000);
                            Environment.Exit(0);
                            break;
                    }
                }
                catch (FormatException)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error: Please enter a NUMBER, not letters.");
                    Console.ResetColor();
                    Pause();
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Unexpected error: {ex.Message}");
                    Console.ResetColor();
                    Pause();
                }
            }
        }

        static void Pause()
        {
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        static void ShowAnimatedBanner()
        {
            string[] banner = new[]
            {
@" ██████╗  █████╗ ██████╗        ██╗    ██╗ ██████╗ ██████╗ ██╗     ██████╗ ",
@"██╔════╝ ██╔══██╗██╔══██╗       ██║    ██║██╔═══██╗██╔══██╗██║     ██   ██╗",
@"██║      ███████║██████╔╝       ██║ █╗ ██║██║   ██║██████╔╝██║     ██   ██║ ",
@"██║      ██╔══██║██╔══██╗       ██║███╗██║██║   ██║██╔══██╗██║     ██   ██║ ",
@"╚██████╗ ██║  ██║██║  ██║       ╚███╔███╔╝╚██████╔╝██║  ██║███████╗███████║",
@" ╚═════╝ ╚═╝  ╚═╝╚═╝  ╚═╝        ╚══╝╚══╝  ╚═════╝ ╚═╝  ╚═╝╚══════╝╚═════╝"
            };

            Console.ForegroundColor = ConsoleColor.Blue;

            foreach (var line in banner)
            {
                Console.WriteLine(line);
                Thread.Sleep(80);
            }

            Console.ResetColor();
        }
    }
}


//using System;
//using System.Collections.Generic;

//namespace AssignmentCARS
//{
//    class Program
//    {
//        static string customerFile = "customer.dat";

//        static void Main(string[] args)
//        {
//            //command line argument for admin login
//            if (args.Length > 0)
//            {
//                new AdminLogin().Run(args);
//                return; //skips the normal menu
//            }

//            var customers = BinaryRepository.Load(customerFile)
//            ?? new Dictionary<string, Customer>(StringComparer.OrdinalIgnoreCase);

//            customers = new Dictionary<string, Customer>(customers, StringComparer.OrdinalIgnoreCase);

//            Action save = () => BinaryRepository.Save(customers, customerFile);

//            var login = new Login(Pause);
//            var signup = new Signup(Pause);

//            while (true)
//            {
//                Console.Clear();
//                Console.ForegroundColor = ConsoleColor.Yellow;
//                Console.WriteLine("HELLO, WELCOME TO");
//                Console.ResetColor();
//                Console.WriteLine();
//                ShowAnimatedBanner();
//                Console.ForegroundColor = ConsoleColor.Yellow;
//                Console.WriteLine("\n(1) Signup");
//                Console.WriteLine("(2) Login");
//                Console.WriteLine("(3) Quit");
//                Console.ResetColor();
//                Console.Write("\nChoose an option: ");
//                string input = Console.ReadLine()?.Trim();
//                Console.Clear();

//                switch (input)
//                {
//                    case "1":
//                        Customer newCustomer = signup.Signup1(customers, save);
//                        MainMenu.Show(newCustomer, save);
//                        break;
//                    case "2":
//                        Customer loggedIn = login.Login1(customers);
//                        MainMenu.Show(loggedIn, save);
//                        save(); // Save any changes made during the session
//                        break;
//                    case "3": Console.WriteLine("Goodbye!"); System.Threading.Thread.Sleep(1000); Environment.Exit(0); break;
//                    default: Console.WriteLine("Invalid input."); Pause(); break;
//                }
//            }
//        }

//        static void Pause()
//        {
//            Console.WriteLine("\nPress any key to continue..."); Console.ReadKey();
//        }


//        static void ShowAnimatedBanner()
//        {
//            string[] banner = new[]
//            {
//@" ██████╗  █████╗ ██████╗        ██╗    ██╗ ██████╗ ██████╗ ██╗     ██████╗ ",
//@"██╔════╝ ██╔══██╗██╔══██╗       ██║    ██║██╔═══██╗██╔══██╗██║     ██   ██╗",
//@"██║      ███████║██████╔╝       ██║ █╗ ██║██║   ██║██████╔╝██║     ██   ██║ ",
//@"██║      ██╔══██║██╔══██╗       ██║███╗██║██║   ██║██╔══██╗██║     ██   ██║ ",
//@"╚██████╗ ██║  ██║██║  ██║       ╚███╔███╔╝╚██████╔╝██║  ██║███████╗███████║",
//@" ╚═════╝ ╚═╝  ╚═╝╚═╝  ╚═╝        ╚══╝╚══╝  ╚═════╝ ╚═╝  ╚═╝╚══════╝╚═════╝"
//    };

//            Console.ForegroundColor = ConsoleColor.Blue;

//            foreach (var line in banner)
//            {
//                Console.WriteLine(line);
//                Thread.Sleep(80);
//            }

//            Console.ResetColor();
//        }




//    }
//}

