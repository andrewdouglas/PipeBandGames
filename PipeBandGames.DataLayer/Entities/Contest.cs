using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace PipeBandGames.DataLayer.Entities
{
    [Table("Contest")]
    public class Contest
    {
        [Key]
        public int ContestId { get; set; }

        public DateTime? DoorsOpen { get; set; }

        public DateTime? RegistrationOpen { get; set; }

        public DateTime? LastSoloEventComplete
        {
            get
            {
                DateTime? firstSoloStart = this.FirstSoloEventStart;
                if (!firstSoloStart.HasValue)
                {
                    return null;
                }

                return firstSoloStart.Value.AddMinutes(this.SoloEvents.Sum(x => x.DurationMinutes));
            }
        }

        public DateTime? FirstSoloEventStart
        {
            get { return this.SoloEvents.OrderBy(x => x.Start).FirstOrDefault(x => x.Start != null)?.Start; }
        }

        [ForeignKey("ContestId")]
        public List<SoloEvent> SoloEvents { get; set; } = new List<SoloEvent>();

        [ForeignKey("ContestId")]
        public List<Competitor> Competitors { get; set; } = new List<Competitor>();

        [ForeignKey("ContestId")]
        public List<ContestJudge> ContestJudges { get; set; } = new List<ContestJudge>();
    }
}
