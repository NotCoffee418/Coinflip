using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Windows.Storage;
using Windows.UI.Xaml;

namespace Coinflip
{
    public static class History
    {
        static History()
        {
            LoadHistory();

            // Save history on exit
            Application.Current.Suspending += Current_Suspending;
        }


        public struct Entry
        {
            public Entry(int coinSideId, DateTime flipTime)
            {
                CoinSideId = coinSideId;
                FlipTime = flipTime;
            }

            public int CoinSideId;
            public DateTime FlipTime;
        }

        public static ObservableCollection<HistoryItem> HistoryList = new ObservableCollection<HistoryItem>();
        public static List<Entry> EntryData = new List<Entry>();

        public static async void AddHistoryItem(CoinSide side)
        {
            // Add to memory & UI
            DateTime now = DateTime.Now;
            HistoryList.Insert(0, new HistoryItem(side, now.ToString(CultureInfo.InvariantCulture)));
            EntryData.Insert(0, new Entry(side.Id, now));
        }

        

        public static async Task LoadHistory()
        {
            string json = "";
            try
            {
                // Load json
                StorageFile flipHistoryFile = await ApplicationData.Current.LocalFolder.GetFileAsync("flipHistory.json");
                json = await FileIO.ReadTextAsync(flipHistoryFile);
            }
            catch (FileNotFoundException e)
            {
                return; // No history yet
            }

            // json to memory
            EntryData = JsonConvert.DeserializeObject<List<Entry>>(json);
            if (EntryData == null)
            {
                EntryData = new List<Entry>();
                return;
            }

            // Display recent history
            int i = 0;
            foreach (var e in EntryData)
            {
                // lazy limiter
                if (i < 25) i++;
                else break;

                // Add to history
                var hi = new HistoryItem(Logic.coinSides[e.CoinSideId], e.FlipTime.ToString(CultureInfo.InvariantCulture));
                HistoryList.Add(hi); // Needs to be add since the list stores date descending
            }
        }

        /// <summary>
        /// Likes to cause issues with threading, hence the try catch stop
        /// </summary>
        /// <param name="force"></param>
        static int updateAttempts = 0;
        public static async void UpdateHistoryFile(bool force = false)
        {
            try
            {
                // Update history file
                string json = JsonConvert.SerializeObject(EntryData);
                StorageFile flipHistoryFile = await ApplicationData.Current.LocalFolder.CreateFileAsync(
                    "flipHistory.json", CreationCollisionOption.ReplaceExisting);
                await FileIO.WriteTextAsync(flipHistoryFile, json);
                updateAttempts = 0;
            }
            catch (Exception ex)
            {
                if (force && updateAttempts < 3)
                {
                    Thread.Sleep(100);
                    updateAttempts++;
                    UpdateHistoryFile();
                }

            }            
        }

        private static void Current_Suspending(object sender, Windows.ApplicationModel.SuspendingEventArgs e)
        {
            UpdateHistoryFile(force:true);
        }
    }
}
