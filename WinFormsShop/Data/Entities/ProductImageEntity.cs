using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsShop.Data.Entities
{
    //[Table("tblProductImages")] // ДАТА АНОТАЦІЯ
    public class ProductImageEntity
    {
        [Key]
        public int Id { get; set; }
        [Required, StringLength(200)]
        public string Name { get; set; }
        public int Priority { get; set; } // в якому пріорітеті будуть йти фотографії
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public virtual ProductEntity Product { get; set; }
    }
}
