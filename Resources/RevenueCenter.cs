using System.Collections.Generic;
using Newtonsoft.Json;
using OmnivoreApi.Utils;

namespace OmnivoreApi.Resources
{
    /// <summary>
    /// A Revenue Center is a specified sub-section of a Location. For example, a Location might have both a bar and a dining area.
    /// Revenue Centers are divided differently at every Location
    /// </summary>
    public class RevenueCenter : IResource
    {
        public const string Url = "revenue_centers";

        /// <summary>
        /// The revenue center’s id as stored in the POS system
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// Whether or not this is the default Revenue Center for new Tickets
        /// </summary>
        public bool @default { get; set; }

        /// <summary>
        /// The name of the Revenue Center as stored in the POS
        /// </summary>
        public string name { get; set; }

        [JsonProperty("_links")]
        public SelfLink<RevenueCenter> links { get; set; }

        [JsonProperty("_embedded")]
        public Embedded embedded { get; set; }

        /// <summary>
        /// Only contains Tickets with a remaining balance
        /// </summary>
        public List<Ticket> openTickets => embedded?.openTickets;

        public List<Table> tables => embedded?.tables;

        public class Embedded
        {
            /// <summary>
            /// Only contains Tickets with a remaining balance
            /// </summary>
            [JsonProperty("open_tickets")]
            public List<Ticket> openTickets { get; set; }

            public List<Table> tables { get; set; }
        }

        public class Array : ResourceArray
        {
            [JsonProperty("_embedded")]
            public Store embedded { get; set; }

            public List<RevenueCenter> items => embedded?.revenueCenters;

            public class Store
            {
                [JsonProperty("revenue_centers")]
                public List<RevenueCenter> revenueCenters { get; set; }
            }
        }
    }
}