using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RidePal.Models
{
    public class FilterCriteria
    {
        public string Name { get; set; }

        public ICollection<string> GenresNames { get; set; }

        public ICollection<int> DurationLimits { get; set; }

    }
}
