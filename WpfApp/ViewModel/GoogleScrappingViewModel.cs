using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Input;
using WpfApp.BL.Services;
using WpfApp.BL.Services.Interfaces;
using WpfApp.Model;
using WpfApp.Utils;

namespace WpfApp.ViewModel
{
    public class GoogleScrappingViewModel : INotifyPropertyChanged
    {
        public ICommand GoogleScrappingCommand { get;set;}
        public GoogleScrappingViewModel()
        {
            GoogleScrappingCommand = new Command(Execute, CanExecute);
            Ranks = new GoogleScrappingService().ScrapGoogle(SearchURL,SearchKey);
        }
        private bool CanExecute(object parameter) 
        {
            return true;
        }
        private void Execute(object parameter)
        {
            Ranks = new GoogleScrappingService().ScrapGoogle(SearchURL,SearchKey);
        }

        private ObservableCollection<GoogleScrapping> searchResults;

        public ObservableCollection<GoogleScrapping> SearchResults
        {
            get { return searchResults; }
            set { searchResults = value; NotifyPropertyChanged("SearchResults"); }
        }


        /// <summary>
        /// impleent INotifyPropertyChanged interface
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string prop)
        {
            PropertyChangedEventHandler evntHandler = PropertyChanged;
            if (evntHandler != null)
            {
                evntHandler(this, new PropertyChangedEventArgs(prop));
            }
        }

        #region properties

        private string searchKey = "conveyancing software";
        /// <summary>
        /// Search text for screen scraping
        /// </summary>
        public string SearchKey
        {
            get { return searchKey; }
            set { searchKey = value; NotifyPropertyChanged(SearchKey); }
        }

        private string searchURL = "https://www.smokeball.com.au/";
        /// <summary>
        /// Search URL(smokeball.com.au in this case).
        /// </summary>
        public string SearchURL
        {
            get { return searchURL; }
            set { searchURL = value; }
        }

        private string ranks;
        /// <summary>
        /// Rank of the search text on google
        /// </summary>
        public string Ranks
        {
            get { return ranks; }
            private set { ranks = value; NotifyPropertyChanged(Ranks); }
        }
        #endregion
    }
}
