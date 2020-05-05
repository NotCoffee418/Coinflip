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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var result = Logic.Flip();
            resultCoinControl.SetCoinSide(result);
        }

    }
}
