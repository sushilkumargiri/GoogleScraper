using System;
using WpfApp.BL.Services;
using Xunit;

namespace WpfApp.Test.UnitTests
{
    public class GoogleScrappingTest
    {
        [Fact]
        public void Get_Full_URL()
        {
            string searchKey = "conveyancing software";
            string expected = "https://www.google.com.au/search?num=100&q=conveyancing software";

            var result = new GoogleScrappingService().ConstructSearchURL(searchKey);

            Assert.Equal(result, expected);
        }

    }
}
