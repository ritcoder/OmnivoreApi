using System.Collections.Generic;
using Newtonsoft.Json;
using OmnivoreApi.Utils;

namespace OmnivoreApi.Resources
{
    /// <summary>
    /// A Category on a Menu that contains Menu Items
    /// </summary>
    public class Category : IResource
    {
        public const string Url = "categories";

        /// <summary>
        /// The menu category’s id as stored in the POS. Sometimes a compound value derived from other data
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// The name of the Category as stored in the POS
        /// </summary>
        public string name { get; set; }

        [JsonProperty("_links")]
        public Links links { get; set; }

        [JsonProperty("_embedded")]
        public Embedded embedded { get; set; }

        public List<MenuItem> items => embedded?.items;

        public class Links : SelfLink<Category>
        {
            public ArrayReference<MenuItem> items { get; set; }
        }

        public class Embedded
        {
            public List<MenuItem> items { get; set; }
        }

        public class Array : ResourceArray
        {
            [JsonProperty("_embedded")]
            public Store embedded { get; set; }

            public List<Category> items => embedded?.categories;

            public class Store
            {
                public List<Category> categories { get; set; }
            }
        }
    }
}