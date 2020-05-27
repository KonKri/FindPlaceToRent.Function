using System.Threading.Tasks;

namespace FindPlaceToRent.Core
{
    public interface ISearcherAndNotifier
    {
        Task NotifyAsync();
    }
}