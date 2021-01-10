using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JwtHelperLib;

namespace GraphQlProject.Models
{
    [Table("Users")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        //[Required]
        //[StringLength(10)]
        //public string StrId { get; set; }

        [Required]
        [StringLength(20)]
        public string UserName { get; set; }

        [Required]
        [StringLength(20)]
        public string Password { get; set; }

        [Required]
        public UserAuthType Type { get; set; }
    }
}
