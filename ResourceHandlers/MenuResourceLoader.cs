using System.Threading.Tasks;
using OmnivoreApi.Resources;

namespace OmnivoreApi.ResourceHandlers
{
    public class MenuResourceLoader : ResourceLoader
    {
        public MenuResourceLoader(Omnivore io) : base(io)
        {
        }

        public static string BuildUrl(string locationId) => $"{LocationResourceLoader.BuildUrl(locationId)}/{Menu.Url}";
        public async Task<Menu> GetAsync(string locationId) => await io.GetAsyncWithException<Menu>(BuildUrl(locationId));
    }
}