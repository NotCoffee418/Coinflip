using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Storage;
using Windows.UI.Xaml.Controls;

namespace Coinflip
{
    class SoundPlayer
    {
        static SoundPlayer()
        {
            player.Source = MediaSource.CreateFromUri(new Uri("ms-appx:///Assets/Sounds/coindrop.mp3"));
            player.Volume = 50;
        }

        private static MediaPlayer player = new MediaPlayer();


        public static void PlayCoinFlipSound()
        {
            if (!IsMuted)
                player.Play();
        }

        public static bool IsMuted { get; set; }
    }
}
