using System.Collections.Generic;
using Newtonsoft.Json;
using OmnivoreApi.Utils;

namespace OmnivoreApi.Resources
{
    /// <summary>
    /// Resource for an Order Type inside of a specific Location
    /// </summary>
    public class OrderType : IResource
    {
        public const string Url = "order_types";

        /// <summary>
        /// The order type’s id as stored in the POS system
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// Whether or not tickets can currently be created with this Order Type
        /// </summary>
        public bool available { get; set; }

        /// <summary>
        /// The name of the order type as stored in the POS
        /// </summary>
        public string name { get; set; }

        [JsonProperty("_links")]
        public SelfLink<OrderType> links { get; set; }

        public class Array : ResourceArray
        {
            [JsonProperty("_embedded")]
            public Store embedded { get; set; }

            public List<OrderType> items => embedded?.orderTypes;

            public class Store
            {
                [JsonProperty("order_types")]
                public List<OrderType> orderTypes { get; set; }
            }
        }
    }
}