using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonModelLib.Models
{
    [Table("Persons")]
    public class Person
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        [StringLength(20)]
        public string GivenName { get; set; }

        [Required]
        [StringLength(20)]
        public string Surname { get; set; }

        public int Born { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }
    }
}
