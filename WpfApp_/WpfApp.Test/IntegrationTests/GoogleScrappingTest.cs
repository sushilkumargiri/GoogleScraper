using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using WpfApp.BL.Services;
using Xunit;

namespace WpfApp.Test.IntegrationTests
{
    public class GoogleScrappingTest
    {

        [Fact]
        public void Get_My_Page_Rank()
        {
            var result = new GoogleScrappingService().ScrapGoogle("www.smokeball.com.au","conveyancing software");
            Assert.NotNull(result);
        }

    }
}
