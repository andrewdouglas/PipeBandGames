using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PipeBandGames.DataLayer.Entities
{
    [Table("PersonPipeBandAssociation")]
    public class PersonPipeBandAssociation
    {
        [Key]
        public int PersonPipeBandAssociationId { get; set; }

        public int PersonId { get; set; }

        public int PipeBandAssociationId { get; set; }

        [ForeignKey("PersonId")]
        public Person Person { get; set; }

        [ForeignKey("PipeBandAssociationId")]
        public PipeBandAssociation PipeBandAssociation { get; set; }
    }
}
