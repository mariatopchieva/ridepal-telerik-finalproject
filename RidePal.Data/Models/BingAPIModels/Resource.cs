using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace RidePal.Data.Models.BingAPIModels
{
    public class Resource
    {

        public ICollection<BingAPIModel> resources { get; set; }

    }
}
