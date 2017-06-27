using System;

namespace PipeBandGames.DataLayer.Entities
{
    public abstract class EntityBase
    {
        public DateTime Created { get; set; }

        public int CreatedBy { get; set; }

        public DateTime? Updated { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTime? Deleted { get; set; }

        public int? DeletedBy { get; set; }

        public bool IsDeleted { get; set; }
    }
}
