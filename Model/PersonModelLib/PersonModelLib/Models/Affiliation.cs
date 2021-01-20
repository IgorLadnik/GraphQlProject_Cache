using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonModelLib.Models
{
    [Table("Affiliations")]
    public class Affiliation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        //[Required]
        //[StringLength(10)]
        //public string StrId { get; set; }

        public int Since { get; set; }

        public int OrganizationId { get; set; }
        public int RoleId { get; set; }

        public int PersonId { get; set; }
    }
}
