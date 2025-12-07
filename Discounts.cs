using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AssignmentCARS
{
    //this class handles discount logic for application
    //follows single responsibilty principle by doing 1 job only which is calculating discounts
    //by keeping this logic here improves maintainability and makes codebase cleaner
    public static class Discounts
    {
        //returns discount rate based on level of customer
        //switch expression makes code conciser and more efficient
        //avoids duplicated if/else logic across application
        //improves perofrmance slightly by keeping logic centralised
        public static decimal GetRate(int level) =>
            level switch
            {
                10 => 0.15m, // VIP = 15% discount
                5 => 0.10m,  // Premium = 10% discount
                _ => 0.00m   // Standard = 0% discount
            };

        // applies discount based on level of customer
        //separates calculation logic from UI logic
        //easier to test and reuse in different parts of the program
        public static decimal Apply(decimal price, int level)
        {
            //calculate discounted price 
            decimal discounted = price * (1 - GetRate(level));

            //rounds to 2 decimal places
            return Math.Round(discounted, 2);
        }
    }
}
