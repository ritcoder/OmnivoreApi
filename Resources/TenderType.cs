using System.Collections.Generic;
using Newtonsoft.Json;
using OmnivoreApi.Utils;

namespace OmnivoreApi.Resources
{
    /// <summary>
    /// Resource for an accepted Tender Type inside of a specific Location
    /// </summary>
    public class TenderType : IResource
    {
        public const string Url = "tender_types";

        /// <summary>
        /// The tender type’s id as stored in the POS system
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// The name of the Tender Type as stored in the POS
        /// </summary>
        public string name { get; set; }

        [JsonProperty("_links")]
        public SelfLink<TenderType> links { get; set; }

        public class Array : ResourceArray
        {
            [JsonProperty("_embedded")]
            public Store embedded { get; set; }

            public List<TenderType> items => embedded?.tenderTypes;

            public class Store
            {
                [JsonProperty("tender_types")]
                public List<TenderType> tenderTypes { get; set; }
            }
        }
    }
}