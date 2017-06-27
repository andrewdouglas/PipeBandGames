using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PipeBandGames.DataLayer.Enums;

namespace PipeBandGames.DataLayer.Entities
{
    [Table("Judge")]
    public class Judge
    {
        [Key]
        public int JudgeId { get; set; }

        public int PersonId { get; set; }

        [ForeignKey("PersonId")]
        public Person Person { get; set; }

        public List<Idiom> Idioms { get; set; }
    }
}
