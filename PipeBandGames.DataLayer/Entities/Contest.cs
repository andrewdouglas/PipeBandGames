using PipeBandGames.DataLayer.Constants;
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
            get
            {
                if (!RegistrationOpen.HasValue)
                {
                    return null;
                }

                return RegistrationOpen.Value.AddMinutes(Config.FirstEventRegistrationOpenOffset);
            }
        }

        // These are the solo events offered at the competition, not to be confused with the ScheduledSoloEvents, the actual events that will take place.
        // There may be a difference in the two since perhaps no Grade 2 drummers may register, etc.
        [ForeignKey("ContestId")]
        public List<SoloEvent> SoloEvents { get; set; } = new List<SoloEvent>();

        [ForeignKey("ContestId")]
        public List<Competitor> Competitors { get; set; } = new List<Competitor>();

        [ForeignKey("ContestId")]
        public List<ContestJudge> ContestJudges { get; set; } = new List<ContestJudge>();

        // These are the solo events that have one or more competitors in them that will be held at the contest, not 
        // to be confused with SoloEvents which is the events offered at the competition.
        [NotMapped]
        public List<SoloEvent> ScheduledSoloEvents { get; set; } = new List<SoloEvent>();
    }
}
