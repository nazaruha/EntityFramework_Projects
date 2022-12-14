using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsShop.Data.Entities
{
    [Table("tblCategories")]
    public class CategoryEntity // Entity - бо воно буде в БД
    {
        [Key]
        public int Id { get; set; }
        public bool IsDelete { get; set; }
        public DateTime DateCreated { get; set; }
        [Required, StringLength(255)]
        public string Title { get; set; }
        [StringLength(200)]
        public string Image { get; set; }
        public int Priority { get; set; }
        [ForeignKey("Parent")]
        public int ? ParentId { get; set; } // ? означає, що поле не обов'язкове
        public virtual CategoryEntity Parent { get; set; }  
        public virtual IList<ProductEntity> Products { get; set; }
    }
}
