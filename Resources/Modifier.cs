using System.Collections.Generic;
using Newtonsoft.Json;
using OmnivoreApi.Utils;

namespace OmnivoreApi.Resources
{
    /// <summary>
    /// Possible Modifiers to be used on a Menu Item when creating a Ticket Item
    /// </summary>
    public class Modifier : IResource
    {
        public const string Url = "modifiers";
        public string id { get; set; }
        public string name { get; set; }

        [JsonProperty("price_per_unit")]
        public int pricePerUnit { get; set; }

        [JsonProperty("price_levels")]
        public List<PriceLevel> priceLevels { get; set; }

        [JsonProperty("_links")]
        public SelfLink<Modifier> links { get; set; }

        public class PriceLevel
        {
            /// <summary>
            /// Price Level Identifier
            /// </summary>
            public string id { get; set; }

            /// <summary>
            /// The price of the menu item at this price level, in cents
            /// </summary>
            public int price { get; set; }
        }

        public class Array : ResourceArray
        {
            [JsonProperty("_embedded")]
            public Store embedded { get; set; }

            public List<Modifier> items => embedded?.modifiers;

            public class Store
            {
                public List<Modifier> modifiers { get; set; }
            }
        }
    }
}