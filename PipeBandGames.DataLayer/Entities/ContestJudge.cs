using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
    }
}
