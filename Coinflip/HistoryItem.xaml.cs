using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Coinflip
{
    public sealed partial class HistoryItem : UserControl
    {
        public HistoryItem(CoinSide side, string timeString)
        {
            this.InitializeComponent();
            coinImg.Source = side.IconUri;
            resultTxt.Text = side.Name;
            timeTxt.Text = timeString;
        }

        private void HistoryItem_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            flyout.ShowAt(this, e.GetPosition(this));
        }

        private string GetShareMessage(bool includeUrl = true)
        {
            string appUrl = includeUrl ? "%0Ahttps://www.microsoft.com/store/apps/9N2F0SZ4LVM8" : string.Empty;
            return $"I flipped a coin at {timeTxt.Text} and got {resultTxt.Text}.{appUrl}";
        }


        private void menuCopy_Click(object sender, RoutedEventArgs e)
        {
            DataPackage dataPackage = new DataPackage();
            dataPackage.SetText(GetShareMessage(includeUrl:false));
            Clipboard.SetContent(dataPackage);
        }

        private async void menuShareTweet_Click(object sender, RoutedEventArgs e)
        {
            string url = "https://twitter.com/intent/tweet?text=" + GetShareMessage();
            await Windows.System.Launcher.LaunchUriAsync(new Uri(url));
        }

        private async void menuShareFacebook_Click(object sender, RoutedEventArgs e)
        {
            string url = "https://www.facebook.com/sharer/sharer.php?u=https://www.microsoft.com/store/apps/9N2F0SZ4LVM8&quote=" + GetShareMessage(includeUrl:false);
            await Windows.System.Launcher.LaunchUriAsync(new Uri(url));
        }
    }
}
