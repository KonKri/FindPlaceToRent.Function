using System;
using FindPlaceToRent.Core;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace FindPlaceToRent.Function
{
    public class FindPlaceToRentFunction
    {
        private readonly ISearcherAndNotifier _notifer;

        public FindPlaceToRentFunction(ISearcherAndNotifier notifer)
        {
            _notifer = notifer;
        }

        [FunctionName("FindPlaceToRentFunction")]
        public async System.Threading.Tasks.Task RunAsync([TimerTrigger("0 */5 * * * *")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"FindPlaceToRentFunction executed at: {DateTime.Now}");
            
            // search and notify.
            await _notifer.NotifyAsync();
        }
    }
}
