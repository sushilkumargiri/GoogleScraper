using System;
using System.Collections.Generic;
using System.Text;

namespace WpfApp.BL.Services.Interfaces
{
    public interface IGoogleScrappingService
    {
        string ScrapGoogle(string webUrl, string searchLink);
    }
}
