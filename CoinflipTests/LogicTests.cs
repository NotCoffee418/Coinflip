
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Coinflip;
using System.Linq;
using System.Collections.Generic;

namespace CoinflipTests
{
    [TestClass]
    public class LogicTests
    {
        [TestMethod]
        public void GenerateNumber_OneToHundered_WithinRange()
        {
            uint result = 0;
            for (int i = 0; i < 1000; i++)
            {
                result = Logic.GenerateNumber(1, 100);
                Assert.IsTrue(result >= 1 && result <= 100);
            }
        }

        [TestMethod]
        public void GenerateNumber_OneToTen_CanFindMinAndMax()
        {
            bool minFound = false;
            bool maxFound = false;
            uint number = 0;
            for (int i = 0; i < 10000; i++)
            {
                number = Logic.GenerateNumber(1, 10);
                if (number == 1)
                    minFound = true;
                else if (number == 10)
                    maxFound = true;

                // Break if found
                if (minFound && maxFound)
                    break;
            }

            Assert.IsTrue(minFound);
            Assert.IsTrue(maxFound);
        }

        [TestMethod]
        public void GenerateNumber_ZeroToOne_NoExceptionOnZero()
        {
            uint number = 0;
            do
            {
                number = Logic.GenerateNumber(0, 1);
            } while (number != 0);
        }



        [TestMethod]
        public void GenerateNumber_ZeroToNinetynine_VerifyRandomness()
        {
            uint number = 0;
            // Generate dictionary and add each number
            var matchesPerNumber = new Dictionary<uint, int>();
            for (uint i = 0; i < 1000; i++)
                matchesPerNumber.Add(i, 0);
            

            // Generate numbers & count the amount of times each number is found
            for (int i = 0; i < 10000000; i++)
            {
                number = Logic.GenerateNumber(0, 999);
                matchesPerNumber[number]++;
            }

            // Verify that each number has acceptable number of matches
            var failedNumbers = matchesPerNumber
                .Where(i => i.Value < 9500);
            Assert.AreEqual(0, failedNumbers.Count());
        }
    }
}
