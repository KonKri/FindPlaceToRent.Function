using System;
using System.Threading.Tasks;

namespace FindPlaceToRent.Function.Services.Crawlers
{
    public class AdsCrawler : IAdsCrawler
    {
        public Task GetAdDetails(string adUrl)
        {
            throw new NotImplementedException();
        }

        public Task GetAdsSummarized()
        {
            throw new NotImplementedException();
        }
    }
}