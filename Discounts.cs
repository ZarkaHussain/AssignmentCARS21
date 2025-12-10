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


/*
 * TEST CASES FOR DISCOUNTS CLASS:
 * 
 * Test Case 1: Standard Customer Discount (Level 1)
 * Input: price=£100.00, level=1
 * Expected Output: £100.00 (0% discount applied)
 * Result: Displays price as £100.00
 * 
 * Test Case 2: Premium Customer Discount (Level 5)
 * Input: price=£100.00, level=5
 * Expected Output: £90.00 (10% discount applied)
 * Result: Displays price as £90.00
 * 
 * Test Case 3: VIP Customer Discount (Level 10)
 * Input: price=£100.00, level=10
 * Expected Output: £85.00 (15% discount applied)
 * Result: Displays price as £85.00
 * 
 * 
 * Test Case 4: Real VIP Car Price (Ferrari F8)
 * Input: price=£900.00, level=10
 * Expected Output: £765.00 (15% discount = £900 - £135)
 * Result: Displays price as £765.00
 */