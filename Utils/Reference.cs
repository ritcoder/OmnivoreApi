using System.Threading.Tasks;

namespace OmnivoreApi.Utils
{
    public class Reference<T>
    {
        public string href { get; set; }
        public string profile { get; set; }

        public async Task<T> GetAsync(Omnivore io)
        {
            return await io.GetAsyncWithException<T>(href, false);
        }
    }
}