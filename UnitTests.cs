using System;
using System.Collections.Generic;
using AssignmentCARS;

namespace AssignmentCARS
{
    public static class UnitTests
    {
        public static void Run()
        {
            Console.Clear();
            Console.WriteLine("=== UNIT TESTING ===");
            Console.WriteLine();

            TestAdminLogin();
            TestDiscounts();
            TestCustomerUpgrade();
            TestSaveAndLoad();
            TestCarFiltering();
            TestFailingDiscount();
            TestFailingAdminLogin();
            TestFailingCustomerUpgrade();
            TestFailingCarFilter();


            Console.WriteLine();
            Console.WriteLine("=== END OF TESTS ===");
            Console.WriteLine("Press any key to return...");
            Console.ReadKey();
        }

        private static void PrintResult(string name, bool result)
        {
            if (result)
                Console.WriteLine(name + ": PASS");
            else
                Console.WriteLine(name + ": FAIL");
        }

        //login test for admin
        //this test checks if admin login works properly for valid and invalid inputs.
        //expected result correct login-PASS
        //incorrect login-FAILL
        //ensures admin system doesnt allow wrong password
        private static void TestAdminLogin()
        {
            var admin = new AdminLogin();

            //correct login details
            string[] goodLogin = { "ignored", "admin", "1234" };
            //incrorect login details
            string[] badLogin = { "ignored", "wrong", "nopass" };

            //test calls
            bool shouldPass = admin.Validate(goodLogin);
            bool shouldFail = !admin.Validate(badLogin);

            PrintResult("Admin login valid details", shouldPass);
            PrintResult("Admin login invalid details", shouldFail);
        }


        //discount test
        //checks if calculations for disocunts like standard,premium,VIP customers are correct
        //expected results: standard level 1 = no discount, premium level 5= 10%dsicount, VIP level 10 =15% discount
        //but if any of calculations go wrong then test should fail
        private static void TestDiscounts()
        {
            bool standard = Discounts.Apply(100, 1) == 100; //no discount
            bool premium = Discounts.Apply(100, 5) == 90; //10% off
            bool vip = Discounts.Apply(100, 10) == 85;  //15% off

            PrintResult("Standard discount", standard);
            PrintResult("Premium discount", premium);
            PrintResult("VIP discount", vip);
        }

        //customer upgrade test
        //checks if customers are upgraded to the right level after they make certain number of rentals
        //expected: if customer makes 5 rentals then they become premium customer (level 5) then if they do 5 more rentals then they upgraded to VIP (level 10)
        //its important cos then you know upgrading system is wokring proper
        private static void TestCustomerUpgrade()
        {
            Customer c = new Customer("TestUser", "pw");

            //first 5 rentals-upgrade to Premium (level 5)
            for (int i = 0; i < 5; i++)
            {
                c.AddRental("Car");
            }

            PrintResult("Customer upgrade to Premium", c.Level == 5);

            //next 5 rentals-upgrade to VIP (level 10)
            for (int i = 0; i < 5; i++)
            {
                c.AddRental("Car");
            }

            PrintResult("Customer upgrade to VIP", c.Level == 10);
        }

        //save and load customer file test
        //this test checks if customer's data is saved to and loaded from correctly to binary file
        //customer is created then a rental is added then saved
        //expected: customer exists in loaded file and their rental history has only 1 rental
        //but if loading file or saving file is broken then this test will catch
        private static void TestSaveAndLoad()
        {
            Customer c = new Customer("SaveTest", "pass1234");
            c.AddRental("Toyota Corolla");

            //saved using this
            BinaryRepository.SaveCustomer(c);

                                                   //loads all customers
            Dictionary<string, Customer> loaded = BinaryRepository.LoadAll();

            bool savedCorrectly =
                loaded.ContainsKey(c.CustomerID) &&
                loaded[c.CustomerID].RentalHistory.Count == 1;

            PrintResult("Saving and loading rental history", savedCorrectly);
        }

        //filter car by level
        //checks that only the cars the customer has the right level for are shown
        //expcted:standard customer only sees cars that are level 1, premium see level 1 and 5 cars and VIP see all of cars
        //this important because then shows the filtering logic is correct
        private static void TestCarFiltering()
        {
            List<Car> standard = RentCar.GetCarsByLevel(1);
            List<Car> premium = RentCar.GetCarsByLevel(5);
            List<Car> vip = RentCar.GetCarsByLevel(10);

            bool checkStandard = standard.TrueForAll(x => x.RequiredLevel == 1);
            bool checkPremium = premium.Exists(x => x.RequiredLevel == 5);
            bool checkVIP = vip.Exists(x => x.RequiredLevel == 10);

            PrintResult("Standard car filtering", checkStandard);
            PrintResult("Premium car filtering", checkPremium);
            PrintResult("VIP car filtering", checkVIP);
        }

        //failing discount test
        private static void TestFailingDiscount()
        {
            //expect wrong discount
            bool failTest = Discounts.Apply(100, 5) == 50; //premium should give 10% off (90), not 50

            PrintResult("Deliberate failing discount test", failTest);
        }

        //failing admin login test
        //this tests check invalid credntails are rejectd
        private static void TestFailingAdminLogin()
        {
            var admin = new AdminLogin();

            //expect admin login to succeed with wrong password (should fail)
            string[] wrongLogin = { "ignored", "admin", "wrongpass" };
            bool failTest = admin.Validate(wrongLogin); //this will return false- test expects true

            PrintResult("Deliberate failing admin login test", failTest);
        }

        //failing customer upgrade test
        //simulates ealry upgrades to ensure system only upgrades at correct threshold
        private static void TestFailingCustomerUpgrade()
        {
            Customer c = new Customer("FailUser", "pw");

            //expect customer to be VIP after 3 rentals (but really you need 10)
            for (int i = 0; i < 3; i++)
            {
                c.AddRental("Car");
            }

            bool failTest = c.Level == 10; // will be false- test expects true
            PrintResult("Deliberate failing customer upgrade test", failTest);
        }

        //failing car filter test
        //this test checks customers cant see cars above the level theyre at
        private static void TestFailingCarFilter()
        {
            //expect standard customer to see VIP cars (should not happen)
            List<Car> standard = RentCar.GetCarsByLevel(1);
            bool failTest = standard.Exists(x => x.RequiredLevel == 10); //false- test expects true
            PrintResult("Deliberate failing car filter test", failTest);
        }

    }
}

//I did these tests to confirm that the admin login works correctly, customer upgrades correctly, disocunts added correctly, data saves and loads corect and cars are only shown if the right customer level has access to it

//1)admin login test- showed only correct credentials admin/1234 was accepted
//if they was incrorect then theyd be rejected
//this makes access to admin only featutres more secure

//2)disocunt test-verified that the different memebrships of customers had correct dsicounts applied

//3)customer level upgrade test-simultated mutliple rentals, and customers upgraded correctly.
//for example from Standard to Premium after making 5 rentals, from Premium to VIP after making 10 rentls

//4)saving/loading customer data test- binary repository tested making sure customer data and rental hsitory saves to file and reloads correctly
//this test verified that the saved customer and their rental hsitory were there
//important cos then customer data not lost and is securely saved

//5)filter car by level test- tests confirmed customers only see cars that their membership gives them access to
//so like Premium and VIP cars are only showed to the correctly upgraded customers and not to standard ones

//some unit tests i made to fail just to show the testing framework actually detects errors
//it shows: test robustness- so if feature broken or not behaving right, test fails
//veriifies logic- so by failing them it confirms test check expected behaviour proeprly
//and they catch edge cases so by simulating incrorrect inputs ensures system doesnt accept invalid behaviours

//To conclude, these unit tests verified that the system's core logic/functinaluity behaves like we expcted.
//this helps to ensure key features carry on working proprly even if we further develop them or the other aspects of applciation.


//THIS WAS THE OUTPUT OF MY TESTS WEN I RAN THEM IN Program.cs


//== UNIT TESTING ===

//Admin login valid details: PASS
//Admin login invalid details: PASS
//Standard discount: PASS
//Premium discount: PASS
//VIP discount: PASS
//Customer upgrade to Premium: PASS
//Customer upgrade to VIP: PASS
//Saving and loading rental history: PASS
//Standard car filtering: PASS
//Premium car filtering: PASS
//VIP car filtering: PASS
//Deliberate failing discount test: FAIL
//Deliberate failing admin login test: FAIL
//Deliberate failing customer upgrade test: FAIL
//Deliberate failing car filter test: FAIL

//=== END OF TESTS ===
//Press any key to return