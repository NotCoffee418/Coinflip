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
        public struct CoinSide
        {
            public CoinSide(int id, string name, string imgPath)
            {
                Id = id;
                Name = name;
                ImgPath = imgPath;
            }

            public readonly int Id;
            public readonly string Name;
            public readonly string ImgPath;
        }

        public static readonly List<CoinSide> coinSides = new List<CoinSide>()
        {
            new CoinSide(0, "Heads", @"Assets\Images\usa-1939-5c-heads.png"),
            new CoinSide(1, "Tails", @"Assets\Images\usa-1939-5c-heads.png"),
            //new CoinSide(2, "Side", @"Assets\Images\usa-1939-5c-side.png"),
        };


        private static RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();

        // todo: this is broken, first and last number both have 50% hits fewer than they should for some reason
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
            return Convert.ToUInt32(min + (rngOutput * multiplier));
        }


        public static CoinSide Flip()
        {
            var x = GenerateNumber(0, 1);
            return coinSides[0];
        }
    }
}
