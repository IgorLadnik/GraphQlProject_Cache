using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonModelLib.Models
{
    [Table("Affiliations")]
    public class Affiliation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int Since { get; set; }

        public int OrganizationId { get; set; }
        public int RoleId { get; set; }

        public int PersonId { get; set; }
    }
}
