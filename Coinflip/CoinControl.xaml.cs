using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.ComponentModel;
using Windows.UI.Core;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Coinflip
{
    public sealed partial class CoinControl : UserControl
    {
        private CoinSide _displayCoinSide;

        public CoinControl()
        {
            this.InitializeComponent();

            // Init worker with overly long name
            CoinFlippingVisualiserWorker.WorkerReportsProgress = true;
            CoinFlippingVisualiserWorker.DoWork += CoinFlippingVisualiserWorker_DoWork;
            CoinFlippingVisualiserWorker.ProgressChanged += CoinFlippingVisualiserWorker_ProgressChanged;
            CoinFlippingVisualiserWorker.RunWorkerCompleted += CoinFlippingVisualiserWorker_RunWorkerCompleted;

        }

        // Dodgy workaround to show progressbar because apparently i don't understand threading well enough
        internal BackgroundWorker CoinFlippingVisualiserWorker = new BackgroundWorker();

        public CoinSide DisplayCoinSide
        {
            get { return _displayCoinSide; }
            private set { _displayCoinSide = value; }
        }

        public void SetCoinSide(CoinSide side)
        {
            coinImage.Source = Logic.coinSides[0].ImgUri; // Hide coin while loading
            CoinFlippingVisualiserWorker.RunWorkerAsync(side);
        }

        private void CoinFlippingVisualiserWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            CoinFlippingVisualiserWorker.ReportProgress(1); // Show progressring

            // Randomized sleep because people like waiting apparently & show progress ring            
            int delay = Convert.ToInt32(Logic.GenerateNumber(200, 1500));
            Thread.Sleep(delay);
            e.Result = e.Argument;

            CoinFlippingVisualiserWorker.ReportProgress(100); // hide progressring
        }

        private void CoinFlippingVisualiserWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == 1) 
            {
                generatingProgressRing.IsActive = true;
                resultTxt.Text = "...";
            }
        }

        private void CoinFlippingVisualiserWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // Display Result
            CoinSide side = (CoinSide)e.Result;
            DisplayCoinSide = side;
            coinImage.Source = side.ImgUri;
            resultTxt.Text = side.Name;
            generatingProgressRing.IsActive = false;
        }
    }
}
