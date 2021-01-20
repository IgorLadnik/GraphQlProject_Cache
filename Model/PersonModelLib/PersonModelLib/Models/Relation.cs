
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonModelLib.Models
{
    [Table("Relations")]
    public class Relation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        //[Required]
        //[StringLength(10)]
        //public string StrId { get; set; }

        public int Since { get; set; }

        [Required]
        [StringLength(15)]
        public string Kind { get; set; }

        public string Notes { get; set; }

        public int P1Id { get; set; }

        public int P2Id { get; set; }
    }
}
