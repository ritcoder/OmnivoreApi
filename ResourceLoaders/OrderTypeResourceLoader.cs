using System.Threading.Tasks;
using OmnivoreApi.Resources;

namespace OmnivoreApi.ResourceLoaders
{
    public class OrderTypeResourceLoader : ResourceLoader
    {
        public OrderTypeResourceLoader(Omnivore io) : base(io)
        {
        }

        public static string BuildUrl(string locationId, string orderTypeId = null)
            => $"{LocationResourceLoader.BuildUrl(locationId)}/{OrderType.Url}{(orderTypeId == null ? "" : $"/{orderTypeId}")}";
        public async Task<OrderType.Array> ListAsync(string locationId) => await io.GetAsyncWithException<OrderType.Array>(BuildUrl(locationId));
        public async Task<OrderType> RetrieveAsync(string locationId, string orderTypeId) => await io.GetAsyncWithException<OrderType>(BuildUrl(locationId, orderTypeId));
    }
}