using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace WpfApp.Model
{
    public class GoogleScrapping:INotifyPropertyChanged
    {
        private string searchKey = "conveyancing software";
        /// <summary>
        /// Search text for screen scraping
        /// </summary>
        public string SearchKey
        {
            get { return searchKey; }
            set { searchKey = value; OnPropertyChanged(SearchKey); }
        }
        private int maxSearchResult = 100;
        /// <summary>
        /// Search text for screen scraping
        /// </summary>
        public int MaxSearchResult
        {
            get { return maxSearchResult; }
            set { maxSearchResult = value; OnPropertyChanged("MaxSearchResult"); }
        }
        private string searchWebsite = "https://www.google.com.au/";
        /// <summary>
        /// Search URL(google.com in this case) with private set. Can be set locally.
        /// </summary>
        public string SearchWebsite
        {
            get { return searchWebsite; }
            private set { searchWebsite = value; }
        }
        private string searchURL;
        /// <summary>
        /// Search URL(google.com in this case) with private set. Can be set locally.
        /// </summary>
        public string SearchURL
        {
            get { return SearchWebsite + "?num=" + maxSearchResult + "&q=" + searchKey; }
            private set { searchURL = value; }
        }
        private int rank;
        /// <summary>
        /// Rank of the search text on google
        /// </summary>
        public int Rank
        {
            get { return rank; }
            private set { rank = value; }
        }

        /// <summary>
        /// impleent INotifyPropertyChanged interface
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string prop)
        {
            PropertyChangedEventHandler evntHandler = PropertyChanged;
            if (evntHandler != null)
            {
                evntHandler(this, new PropertyChangedEventArgs(prop));
            }

        }
    }
}
