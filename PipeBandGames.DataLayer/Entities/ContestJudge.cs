using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace PipeBandGames.DataLayer.Entities
{
    [Table("ContestJudge")]
    public class ContestJudge
    {
        [Key]
        public int ContestJudgeId { get; set; }

        public int ContestId { get; set; }

        [ForeignKey("ContestId")]
        public Contest Contest { get; set; }

        public int JudgeId { get; set; }

        [ForeignKey("JudgeId")]
        public Judge Judge { get; set; }

        // Solo events assigned to the judge.
        // This is a read-only convenience property.
        [NotMapped]
        public ReadOnlyCollection<SoloEvent> SoloEvents
        {
            get
            {
                return this.Contest.SoloEvents.Where(x => x.Judge != null && x.Judge.JudgeId == this.Judge.JudgeId).ToList().AsReadOnly();
            }
        }
    }
}
