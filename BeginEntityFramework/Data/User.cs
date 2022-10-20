using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeginEntityFramework.Data
{
    [Table("tblUsers")]
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required, StringLength(maximumLength: 200)]
        public string Email { get; set; }
        [Required, StringLength(maximumLength: 100)]
        public string FirstName { get; set; }
        [Required, StringLength(maximumLength: 100)]
        public string LastName { get; set; }

        public override string ToString()
        {
            return $"{Id}\t{Email}\t{FirstName} {LastName}";
        }
    }
}
