using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace RidePal.Data.Models.BingAPIModels
{
    public class ResourceSet
    {
        public ICollection<Resource> resourceSets { get; set; }
        
    }
}
