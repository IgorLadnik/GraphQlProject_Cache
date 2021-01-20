using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonModelLib.Models
{
    [Table("Roles")]
    public class Role
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        //[Required]
        //[StringLength(10)]
        //public string StrId { get; set; }

        [Required]
        [StringLength(30)]
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
