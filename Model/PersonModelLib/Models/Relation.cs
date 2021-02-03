
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonModelLib.Models
{
    [Table("Relations")]
    public class Relation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(15)]
        public string Kind { get; set; }

        public int Since { get; set; }

        public string Notes { get; set; }

        public int P1Id { get; set; }

        public int P2Id { get; set; }
    }
}
