using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeginEntityFramework.Data
{
    [Table("tblCars")]
    public class Car
    {
        [Key] // Meybe it means PRIMARY KEY
        public int Id { get; set; }
        [Required, StringLength(200)/*довжина поля*/] // означає що обов'язкове поле NOT NULL
        public string Name { get; set; }
        [Required, StringLength(100)]
        public string Model { get; set; }
        [Required, StringLength(100)]
        public string Mark { get; set; }

        public override string ToString()
        {
            return $"{Id}\t{Name}\t{Mark}\t{Model}";
        }
    }
}
