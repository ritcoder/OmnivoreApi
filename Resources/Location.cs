using System.Collections.Generic;
using Newtonsoft.Json;
using OmnivoreApi.Utils;

namespace OmnivoreApi.Resources
{
    /// <summary>
    /// A Location resource contains data about a restaurant location.
    /// Locations can be created, modified, replaced, and deleted.
    /// </summary>
    public class Location : IResource
    {
        public const string Url = "locations";

        /// <summary>
        /// The location’s id within our database
        /// </summary>
        public string id { get; set; }

        public Address address { get; set; }

        /// <summary>
        /// The name of the Location
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 10 digits, no punctuation
        /// </summary>
        public string phone { get; set; }

        public string website { get; set; }

        [JsonProperty("_links")]
        public Links links { get; set; }

        public class Address
        {
            public string city { get; set; }

            /// <summary>
            /// 2 character code (e.g. “CA”)
            /// </summary>
            public string state { get; set; }

            public string street1 { get; set; }
            public string street2 { get; set; }
            public string zip { get; set; }
        }

        public class Links : SelfLink<Location>
        {
            public ArrayReference<Employee> employees { get; set; }
            public ArrayReference<Menu> menus { get; set; }

            [JsonProperty("order_types")]
            public ArrayReference<OrderType> orderTypes { get; set; }

            [JsonProperty("revenue_centers")]
            public ArrayReference<RevenueCenter> revenueCenters { get; set; }

            public ArrayReference<Table> tables { get; set; }

            [JsonProperty("tender_types")]
            public ArrayReference<TenderType> tenderTypes { get; set; }

            public ArrayReference<Ticket> tickets { get; set; }
        }

        public class Array : ResourceArray
        {
            [JsonProperty("_embedded")]
            public Store embedded { get; set; }

            public List<Location> items => embedded?.locations;

            public class Store
            {
                public List<Location> locations { get; set; }
            }
        }
    }
}