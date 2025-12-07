using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;


namespace AssignmentCARS
{
    class Program
    {
        //comand line interface
        //if program launched with arguments it attempts to run in admin mode
        //separates admin features from normal user features- security/access control
        static void Main(string[] args)
        {
            // If admin login via CLI args
            if (args.Length > 0)
            {
                new AdminLogin().Run(args);
                return;
            }

            //load all customer data from (/customers/) binary files on startup
            //DATA persistence
            // allows data to survive after program is closed- persistent storage
            var customers = BinaryRepository.LoadAll();

            //THIS FOR Performance Testing
            //RentCar.RunParallelTest();


            //this save action saves updated customer data- writes back to all .dat files
            //using Action keeps saving logic reusable and clean
            Action saveAll = () => BinaryRepository.SaveAll(customers);

            //Injecting Pause() method improves flexibility and testability
            var login = new Login(UIHelper.Pause);
            var signup = new Signup(UIHelper.Pause);

            //main application loop
            //ensures app keeps running until user chooses to quit
            while (true)
            {
                Console.Clear();
                //colours and aninations added to make console UI more user friendly
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

                //using ReadKey improves responsiveness and prevents accidental extra input
                char key = Console.ReadKey(true).KeyChar;
                string input = key.ToString();
                Console.WriteLine(key);
                //string input = Console.ReadLine()?.Trim() ?? "";

   
                try
                {
                    //robustness- for input validation
                    //TryParse prevents crashes from non-numeric input
                    if (!int.TryParse(input, out int option))
                        throw new FormatException();

                    //defensive programming ensures only valid menu options accespted
                    if (option < 1 || option > 3)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Error: Please choose 1, 2, or 3.");
                        Console.ResetColor();
                        UIHelper.Pause();
                        continue;
                    }

                    switch (option)
                    {
                        case 1: //signup flow
                            Customer newCust = signup.Signup1(customers, saveAll); //creates new user, saves data, opens main menu
                            MainMenu.Show(newCust, saveAll);
                            break;

                        case 2: //login flow
                            Customer loggedIn = login.Login1(customers); //authenticates user and loads their data
                            MainMenu.Show(loggedIn, saveAll);
                            saveAll(); // saves updated data after session
                            break;

                        case 3: //safe exit
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
                    //robustness-user friendly errors
                    //prevents crash when non-numeric input entered
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error: Please enter a NUMBER, not letters.");
                    Console.ResetColor();
                    UIHelper.Pause();
                }
                catch (Exception ex)
                {
                    //catch all error handling
                    //protects program from unexpected runtime crashes
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Unexpected error: {ex.Message}");
                    Console.ResetColor();
                    UIHelper.Pause();
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


        //simple animation used to improve user experience
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

            //slow printing for animation effect
            foreach (var line in banner)
            {
                Console.WriteLine(line);
                Thread.Sleep(80);
            }

            Console.ResetColor();
        }

    }
}
