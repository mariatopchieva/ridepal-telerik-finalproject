using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace RidePal.Data.Models.PixaBayAPIModels
{
    public class PixaBayImageCollection
    {
        [JsonPropertyName("data")]
        public IList<PixaBayImage> PixaBayImages { get; set; }
    }
}
