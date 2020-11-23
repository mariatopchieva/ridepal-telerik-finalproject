using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace RidePal.Data.Models.PixaBayAPIModels
{
    public class PixaBayImage
    {
        [JsonPropertyName("webformatURL")]
        public string WebformatURL { get; set; }
    }
}
