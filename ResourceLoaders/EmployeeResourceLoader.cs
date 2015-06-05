using System.Threading.Tasks;
using OmnivoreApi.Resources;

namespace OmnivoreApi.ResourceLoaders
{
    public class EmployeeResourceLoader : ResourceLoader
    {
        public EmployeeResourceLoader(Omnivore io) : base(io)
        {
        }

        public static string BuildUrl(string locationId, string employeeId = null)
            => $"{LocationResourceLoader.BuildUrl(locationId)}/{Employee.Url}{(employeeId == null ? "" : $"/{employeeId}")}";
        public async Task<Employee.Array> ListAsync(string locationId) => await io.GetAsyncWithException<Employee.Array>(BuildUrl(locationId));
        public async Task<Employee> RetrieveAsync(string locationId, string employeeId) => await io.GetAsyncWithException<Employee>(BuildUrl(locationId,employeeId));
    }
}