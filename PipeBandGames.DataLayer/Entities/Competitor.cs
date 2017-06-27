using PipeBandGames.DataLayer.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PipeBandGames.DataLayer.Entities
{
    [Table("Competitor")]
    public class Competitor
    {
        [Key]
        public int CompetitorId { get; set; }

        [ForeignKey("ContestId")]
        public Contest Contest { get; set; }

        public PipeGrade PipeGrade { get; set; }

        public SnareGrade SnareGrade { get; set; }

        public TenorGrade TenorGrade { get; set; }

        public BassGrade BassGrade { get; set; }

        public DrumMajorGrade DrumMajorGrade { get; set; }

        [ForeignKey("CompetitorId")]
        public List<SoloEventCompetitor> RegisteredSoloEvents { get; set; } = new List<SoloEventCompetitor>();

        public DateTime Registered { get; set; }
    }
}
