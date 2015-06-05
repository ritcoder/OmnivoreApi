using System.Collections.Generic;
using Newtonsoft.Json;
using OmnivoreApi.Utils;

namespace OmnivoreApi.Resources
{
    /// <summary>
    /// Resource for a Table inside of a specific Location and Revenue Center
    /// </summary>
    public class Table : IResource
    {
        public const string Url = "tables";

        /// <summary>
        /// The table’s id as stored in the POS
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// Whether or not the table is ready for immediate use
        /// </summary>
        public bool available { get; set; }

        /// <summary>
        /// Alphanumeric identifier from the POS. Not guarenteed to be the same as the id
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// Integer identifier from the POS. Not guarenteed to be the same as the id
        /// </summary>
        public int number { get; set; }

        /// <summary>
        /// Number of seats at the table
        /// </summary>
        public int seats { get; set; }

        [JsonProperty("_embedded")]
        public Embedded embedded { get; set; }

        [JsonProperty("_links")]
        public SelfLink<Table> links { get; set; }

        /// <summary>
        /// Only contains Tickets with a remaining balance
        /// </summary>
        public List<Ticket> openedTickets => embedded?.openTickets;

        public RevenueCenter revenueCenter => embedded?.revenueCenter;

        public class Embedded
        {
            [JsonProperty("open_tickets")]
            public List<Ticket> openTickets { get; set; }

            [JsonProperty("revenue_center")]
            public RevenueCenter revenueCenter { get; set; }
        }

        public class Array : ResourceArray
        {
            [JsonProperty("_embedded")]
            public Store embedded { get; set; }

            public List<Table> items => embedded?.tables;

            public class Store
            {
                public List<Table> tables { get; set; }
            }
        }
    }
}