using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace RidePal.Data.Models.Abstract
{
    public abstract class Entity
    {
        [DisplayName("Created on")]
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        [DisplayName("Modified on")]
        public DateTime? ModifiedOn { get; set; }
        [DisplayName("Deleted on")]
        public DateTime? DeletedOn { get; set; }

        [DisplayName("Is Deleted")]
        public bool IsDeleted { get; set; }
    }
}
