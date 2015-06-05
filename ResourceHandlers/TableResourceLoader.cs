using System.Threading.Tasks;
using OmnivoreApi.Resources;

namespace OmnivoreApi.ResourceHandlers
{
    public class TableResourceLoader : ResourceLoader
    {
        public TableResourceLoader(Omnivore io) : base(io)
        {
        }

        public static string BuildUrl(string locationId, string tableId = null)
            => $"{LocationResourceLoader.BuildUrl(locationId)}/{Table.Url}{(tableId == null ? "" : $"/{tableId}")}";
        public async Task<Table.Array> ListAsync(string locationId) => await io.GetAsyncWithException<Table.Array>(BuildUrl(locationId));
        public async Task<Table> RetrieveAsync(string locationId, string tableId) => await io.GetAsyncWithException<Table>(BuildUrl(locationId,tableId));
    }
}