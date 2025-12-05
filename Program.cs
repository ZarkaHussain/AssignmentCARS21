using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;


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

            //load all customer files from /customers/
            var customers = BinaryRepository.LoadAll();

            //THIS FOR Multiple Processor Testing
            //RentCar.RunParallelTest();


            //thi save action writes to all .dat files
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
                //Console.WriteLine("(4) Performance Test");
                Console.ResetColor();
                Console.Write("\nChoose an option: ");
                char key = Console.ReadKey(true).KeyChar;
                string input = key.ToString();
                Console.WriteLine(key);
                //string input = Console.ReadLine()?.Trim() ?? "";

   
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

                        //case 4:
                        //    ShowPerformanceMenu();
                        //    break;

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

        //This is for Performance testing:( multiple processors, caching, stringubuilding stufff)
        //static void ShowPerformanceMenu()
        //{
        //    var test = new PerformanceTest();

        //    while (true)
        //    {
        //        Console.Clear();
        //        Console.WriteLine("=== PERFORMANCE & PROFILING TESTS ===\n");
        //        Console.WriteLine("1) Multiple Processor Test");
        //        Console.WriteLine("2) String vs StringBuilder");
        //        Console.WriteLine("3) Caching Test");
        //        Console.WriteLine("4) Capacity Test");
        //        Console.WriteLine("5) Back\n");
        //        Console.Write("Choose: ");

        //        char key = Console.ReadKey(true).KeyChar;

        //        switch (key)
        //        {
        //            case '1': test.MultipleProcessors(); break;
        //            case '2': test.StringVsStringBuilder(); break;
        //            case '3': test.CachingTest(); break;
        //            case '4': test.CapacityTest(); break;
        //            case '5': return;

        //            default: break;
        //        }
        //    }
        //}
        ////////    UP TO HERE  ////////////


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
