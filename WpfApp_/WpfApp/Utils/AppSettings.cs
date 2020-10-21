using System;
using System.Collections.Generic;
using System.Text;

namespace WpfApp.Utils
{
    public class AppSettings
    {
        public string GoogleUrl { get; set; }

        public string ScrappingUrl { get; set; }
        public int MaxPageCount { get; set; }

        public string DefaultSearchKey { get; set; }
    }
}
