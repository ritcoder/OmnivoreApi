using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace OmnivoreApi.Tests
{
    public class ResourceTester
    {
        private Omnivore io { get; }

        public ResourceTester(Omnivore io)
        {
            this.io = io;
        }

        public async Task<Dictionary<string,object>>  RunAll()
        {
            var result = new Dictionary<string, object>();
            //get locations
            Log("List Locations");
            var listLocationsResult = await io.locations.ListAsync();
            result["Get Locations"] = listLocationsResult.items != null;
            //retrieve locations
            Log("Retrieve Location");
            var firstLocation = listLocationsResult.embedded.locations[0];
            var retrieveLocationResult = await io.locations.RetrieveAsync(firstLocation.id);
            result["Retrieve Location"] = firstLocation.id == retrieveLocationResult.id;
            //tables
            Log("Get table");

            return result;
        }

        private void Log(object value) => WriteLine("{0}", value);
        private void Log(string description, object value)
        {
            WriteLine("{0} >> {1}", description, value);
        }
    }
}
