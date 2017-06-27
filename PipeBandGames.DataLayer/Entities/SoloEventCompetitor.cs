using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PipeBandGames.DataLayer.Entities
{
    [Table("SoloEventCompetitor")]
    public class SoloEventCompetitor
    {
        [Key]
        public int SoloEventId { get; set; }

        public int CompetitorId { get; set; }

        [ForeignKey("SoloEventId")]
        public SoloEvent SoloEvent { get; set; }

        [ForeignKey("CompetitorId")]
        public Competitor Competitor { get; set; }
    }
}
