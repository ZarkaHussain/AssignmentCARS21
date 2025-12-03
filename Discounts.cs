using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AssignmentCARS
{
    public static class Discounts
    {
        //returns discount rate based on level of customer
        public static decimal GetRate(int level) =>
            level switch
            {
                10 => 0.15m, // VIP = 15%
                5 => 0.10m,  // Premium = 10%
                _ => 0.00m   // Standard = 0%
            };

        // applies discount based on level of customer
        public static decimal Apply(decimal price, int level)
        {
            decimal discounted = price * (1 - GetRate(level));
            return Math.Round(discounted, 2);
        }
    }
}
