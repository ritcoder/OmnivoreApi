using System.Threading.Tasks;
using OmnivoreApi.Resources;

namespace OmnivoreApi.ResourceLoaders
{
    public class MenuItemResourceLoader : ResourceLoader
    {
        public MenuItemResourceLoader(Omnivore io) : base(io)
        {
        }

        public static string BuildUrl(string locationId, string menuItemId = null)
            => $"{MenuResourceLoader.BuildUrl(locationId)}/{MenuItem.Url}{(menuItemId == null ? "" : $"/{menuItemId}")}";
        public async Task<MenuItem.Array> ListAsync(string locationId) => await io.GetAsyncWithException<MenuItem.Array>(BuildUrl(locationId));
        public async Task<MenuItem> RetrieveAsync(string locationId, string menuItemId) => await io.GetAsyncWithException<MenuItem>(BuildUrl(locationId, menuItemId));
    }
}