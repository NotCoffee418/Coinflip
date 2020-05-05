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
        public CoinSide(int id, string name, string imgPath, string iconPath = "")
        {
            Id = id;
            Name = name;
            ImgPath = imgPath;
            IconPath = iconPath;
        }

        private BitmapImage imgBmp = null;
        private BitmapImage iconBmp = null;

        public int Id { get; private set; }
        public string Name { get; private set; }
        public string ImgPath { get; private set; }
        public string IconPath { get; private set; }
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

        public BitmapImage IconUri
        {
            get
            {
                // Store BitmapImage from path
                if (iconBmp == null)
                {
                    // No path, blank image
                    if (ImgPath.Length == 0)
                        iconBmp = new BitmapImage();
                    // Create image from path
                    else iconBmp = new BitmapImage(new Uri($"ms-appx:///{ImgPath}"));
                }

                // Return stored image
                return iconBmp;
            }
        }

    }
}
