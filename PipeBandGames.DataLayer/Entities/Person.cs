using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PipeBandGames.DataLayer.Entities
{
    [Table("Person")]
    public class Person : EntityBase
    {
        public int PersonId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Title { get; set; }

        public List<PersonPipeBandAssociation> PersonPipeBandAssociations { get; set; }
    }
}
