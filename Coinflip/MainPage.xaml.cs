using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using VungleSDK;
using Windows.UI.Core;
using Windows.ApplicationModel.Core;
using Windows.Storage;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Coinflip
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            // Load flip history
            historyList.ItemsSource = History.HistoryList;

            // Watch backgroundworker to reenable flip btn
            resultCoinControl.CoinFlippingVisualiserWorker.RunWorkerCompleted += 
                CoinFlippingVisualiserWorker_RunWorkerCompleted;

            // Toggle mute btn accoording to settings
            if (ApplicationData.Current.LocalSettings.Values["MuteFlipSound"] == null)
                muteToggleBtn.IsChecked = false;
            else
                muteToggleBtn.IsChecked = (bool)ApplicationData.Current.LocalSettings.Values["MuteFlipSound"] == true;
        }

        VungleAd sdkInstance = null;

        public void InitVungleAd()
        {
            // Prepare SDK instance
            VungleSDKConfig sdkConfig = new VungleSDKConfig();
            sdkConfig.DisableBannerRefresh = false; // Default: false
            sdkInstance = AdFactory.GetInstance("5e9a5bb33f4c15000110043f", sdkConfig);

            // Prepare control
            vungleBottomBanner.AppID = "5e9a5bb33f4c15000110043f";
            vungleBottomBanner.Placement = "BOTTOM-8749976";

            // Event to load after init
            sdkInstance.OnInitCompleted += SdkInstance_OnInitCompleted;
        }

        private async void SdkInstance_OnInitCompleted(object sender, ConfigEventArgs e)
        {
            await CoreApplication.MainView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                  new DispatchedHandler(() =>
                    vungleBottomBanner.LoadAndPlayBannerAd("BOTTOM-8749976", VungleBannerSizes.BannerLeaderboard_728x90)
                  ));
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            InitVungleAd();
        }

        private void flipBtn_Click(object sender, RoutedEventArgs e)
        {
            // Disable flip btn
            flipBtn.IsEnabled = false;

            // Play Sound
            SoundPlayer.PlayCoinFlipSound();

            // Run flip "animation"
            var result = Logic.Flip();
            resultCoinControl.SetCoinSide(result);
        }
        private void CoinFlippingVisualiserWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            // Reenable flip button when done flipping
            flipBtn.IsEnabled = true;
        }


        private void historyPaneToggleBtn_Checked(object sender, RoutedEventArgs e)
        {
            pageSplitView.IsPaneOpen = true;
        }

        private void historyPaneToggleBtn_Unchecked(object sender, RoutedEventArgs e)
        {
            pageSplitView.IsPaneOpen = false;
        }


        // Open/close pane automatically/by default depending on app window size
        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            bool paneOpen = e.NewSize.Width > 970;
            historyPaneToggleBtn.IsChecked = paneOpen;
        }

        // Mute controls
        private void muteToggleBtn_Checked(object sender, RoutedEventArgs e)
        {
            ApplicationData.Current.LocalSettings.Values["MuteFlipSound"] = true;
            SoundPlayer.IsMuted = true;
        }
        private void muteToggleBtn_Unchecked(object sender, RoutedEventArgs e)
        {
            ApplicationData.Current.LocalSettings.Values["MuteFlipSound"] = false;
            SoundPlayer.IsMuted = false;
        }
    }
}
