using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace Coinflip
{
    public class Logic
    {

        public static readonly List<CoinSide> coinSides = new List<CoinSide>()
        {
            new CoinSide(0, "...", string.Empty),
            new CoinSide(1, "Heads", @"Assets\Images\usa-1939-5c-heads.png"),
            new CoinSide(2, "Tails", @"Assets\Images\usa-1939-5c-tails.png"),
            //new CoinSide(3, "Side", @"Assets\Images\usa-1939-5c-side.png"),
        };


        private static RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();

        public static uint GenerateNumber(int min, int max)
        {
            if (max <= min)
                throw new ArgumentOutOfRangeException();

            // Generate random number
            byte[] resultBytes = new byte[4];
            rng.GetBytes(resultBytes);
            uint rngOutput = BitConverter.ToUInt32(resultBytes, 0);

            // Make it fit our min/max requirements
            int diff = max - min + 1;
            double multiplier = (1d / (double)UInt32.MaxValue) * diff;
            return Convert.ToUInt32(Math.Floor(min + (rngOutput * multiplier)));
        }


        public static CoinSide Flip()
        {
            int randomNumber = Convert.ToInt32(GenerateNumber(1, 2));
            return coinSides[randomNumber];
        }
    }
}
