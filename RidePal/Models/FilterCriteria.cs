using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RidePal.Models
{
    public class FilterCriteria
    {
        public string Name { get; set; }

        public List<string> GenresNames { get; set; } = new List<string>();

        public List<int> DurationLimits { get; set; } = new List<int>();

    }
}
