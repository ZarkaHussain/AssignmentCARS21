
using System;
namespace AssignmentCARS
{
    public static class UIHelper
    {
        //centralised pause method keeps UI consistent
        //(using this one single pause method and reused it everywhere instead of writing Console.WriteLine("Press any key...");Console.ReadKey();)
        //decided to add this so i didnt have to add private functions in different files

        //improves maintainability becuase only 1 place to update pause behaviour
        //consistent user experience because same message and behaviour everyhwere
        //cleaner architecture

        //also links to robustness and well architectured classes becuase its made codebase easier to manage and less prone to errors.
        public static void Pause()
        {
            //displays consistent message to user
            System.Console.WriteLine("\nPress any key to continue...");
            //waits for key press and prevents buffered input from breaking menus
            System.Console.ReadKey(true);
        }
    }
}


