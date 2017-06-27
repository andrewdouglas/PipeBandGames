using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PipeBandGames.DataLayer.Entities
{
    [Table("PipeBandAssociation")]
    public class PipeBandAssociation
    {
        [Key]
        public int PipeBandAssociationId { get; set; }

        public string Name { get; set; }
    }
}
