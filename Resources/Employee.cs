using System.Collections.Generic;
using Newtonsoft.Json;
using OmnivoreApi.Utils;

namespace OmnivoreApi.Resources
{
    /// <summary>
    /// Resource for a Employee inside of a specific Location.
    /// </summary>
    public class Employee : IResource
    {
        public const string Url = "employees";

        /// <summary>
        /// The employee’s id as stored in the POS system
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// The name of the employee as printed on a ticket
        /// </summary>
        [JsonProperty("check_name")]
        public string checkName { get; set; }

        /// <summary>
        /// The employee’s first name as stored in the POS
        /// </summary>
        [JsonProperty("first_name")]
        public string firstName { get; set; }

        /// <summary>
        /// The employee’s last name as stored in the POS
        /// </summary>
        [JsonProperty("last_name")]
        public string lastName { get; set; }

        /// <summary>
        /// The employee’s terminal login info.
        /// </summary>
        public string login { get; set; }

        [JsonProperty("_links")]
        public SelfLink<Employee> links { get; set; }

        public class Array : ResourceArray
        {
            [JsonProperty("_embedded")]
            public Store embedded { get; set; }

            public List<Employee> items => embedded?.employees;

            public class Store
            {
                public List<Employee> employees { get; set; }
            }
        }
    }
}