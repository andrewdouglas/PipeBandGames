using PipeBandGames.DataLayer.Constants;
using PipeBandGames.DataLayer.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PipeBandGames.DataLayer.Entities
{
    [Table("SoloEvent")]
    public class SoloEvent
    {
        [Key]
        public int SoloEventId { get; set; }

        public int ContestId { get; set; }

        [ForeignKey("ContestId")]
        public Contest Contest { get; set; }

        public DateTime? Start { get; set; }

        [ForeignKey("SoloEventId")]
        public List<SoloEventCompetitor> SoloEventCompetitors { get; set; } = new List<SoloEventCompetitor>();

        public Judge Judges { get; set; }

        public Grade Grade { get; set; }

        public Instrument Instrument { get; set; }

        public Idiom Idiom { get; set; }

        public int DurationMinutes
        {
            get
            {
                return this.SoloEventCompetitors.Count * this.MinutesPerCompetitor;
            }
        }

        // If the event has >= 20 competitors, it should be split into 2
        public string SplitLetter { get; set; }

        // EUSPBA recommends planning 15 minutes per piobaireachd performance and 5 minutes for all light music performances
        // TODO: Move this property elsewhere
        [NotMapped]
        public int MinutesPerCompetitor
        {
            get
            {
                return this.Idiom == Idiom.Piobaireachd ? Config.PiobaireachdEventDuration : Config.LightMusicEventDuration;
            }
        }

        public override string ToString()
        {
            string standard = $"{this.Grade} {this.Idiom} {this.Instrument}";
            if (!string.IsNullOrEmpty(this.SplitLetter))
            {
                standard += $" Group {this.SplitLetter}";
            }

            if (this.Start.HasValue)
            {
                standard += $" Start: {this.Start.Value.ToString("t")}";
            }

            return standard;
        }
    }
}
