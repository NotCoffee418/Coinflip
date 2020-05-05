using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Coinflip
{
    public class CoinSide
    {
        public CoinSide(int id, string name, string imgPath)
        {
            Id = id;
            Name = name;
            ImgPath = imgPath;
        }

        private BitmapImage imgBmp = null;

        public int Id { get; private set; }
        public string Name { get; private set; }
        public string ImgPath { get; private set; }
        public BitmapImage ImgUri { 
            get
            {
                // Store BitmapImage from path
                if (imgBmp == null)
                {
                    // No path, blank image
                    if (ImgPath.Length == 0)
                        imgBmp = new BitmapImage();
                    // Create image from path
                    else imgBmp = new BitmapImage(new Uri($"ms-appx:///{ImgPath}"));
                }

                // Return stored image
                return imgBmp;
            }
        }

    }
}
