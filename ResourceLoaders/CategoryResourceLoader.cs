using System.Threading.Tasks;
using OmnivoreApi.Resources;

namespace OmnivoreApi.ResourceHandlers
{
    public class CategoryResourceLoader : ResourceLoader
    {
        public CategoryResourceLoader(Omnivore io) : base(io)
        {
        }

        public static string BuildUrl(string locationId, string categoryId = null)
            => $"{MenuResourceLoader.BuildUrl(locationId)}/{Category.Url}{(categoryId == null ? "" : $"/{categoryId}")}";
        public async Task<Category.Array> ListAsync(string locationId) => await io.GetAsyncWithException<Category.Array>(BuildUrl(locationId));
        public async Task<Category> RetrieveAsync(string locationId, string categoryId) => await io.GetAsyncWithException<Category>(BuildUrl(locationId, categoryId));
    }
}