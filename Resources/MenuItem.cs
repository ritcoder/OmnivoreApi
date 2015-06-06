using System.Collections.Generic;
using Newtonsoft.Json;
using OmnivoreApi.Utils;

namespace OmnivoreApi.Resources
{
    /// <summary>
    /// Items that exist on a Location’s Menu
    /// </summary>
    public class MenuItem : IResource
    {
        public const string Url = "items";

        /// <summary>
        /// The menu item’s id as stored in the POS. Sometimes a compound value derived from other data
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// The name of the Menu Item as stored in the POS
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// The price, in cents
        /// </summary>
        public int price { get; set; }

        [JsonProperty("price_levels")]
        public List<PriceLevel> priceLevels { get; set; }

        /// <summary>
        /// Whether or not the item is currently available for order
        /// </summary>
        public bool inStock { get; set; }

        /// <summary>
        /// The number of Modifier Groups associated with the Menu Item
        /// </summary>
        public int modifierGroupsCount { get; set; }

        [JsonProperty("_links")]
        public Links links { get; set; }

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

        public class Links : SelfLink<MenuItem>
        {
            [JsonProperty("modifier_groups")]
            public ArrayReference<ModifierGroup> modifierGroups { get; set; }
        }

        public class Array : ResourceArray
        {
            [JsonProperty("_embedded")]
            public Store embedded { get; set; }

            public List<MenuItem> items => embedded?.menuItems;

            public class Store
            {
                [JsonProperty("menu_items")]
                public List<MenuItem> menuItems { get; set; }
            }
        }
    }
}