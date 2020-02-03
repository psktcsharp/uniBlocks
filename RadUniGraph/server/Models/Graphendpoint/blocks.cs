using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace UniBlocksGraph.Models.Graphendpoint
{
    public partial class Blocks
    {
        [JsonPropertyName("blockName")]
        public string BlockName
        {
            get;
            set;
        }
    }
}
