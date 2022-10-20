using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsShop.Data.Entities
{
    [Table("tblSentSMS")]
    public class SentSMS
    {
        [Key]
        public int Id { get; set; }
        [Required, StringLength(20)]
        public string Phone { get; set; }
        [Required, StringLength(200)]
        public string Message { get; set; }
    }
}
