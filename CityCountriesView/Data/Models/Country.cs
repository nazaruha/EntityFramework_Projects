using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitiesCountriesView.Data
{
    [Table("tblCountries")]
    public class Country
    {
        [Key]
        public int Id { get; set; }
        [Required, StringLength(maximumLength: 200)]
        public string Name { get; set; }
        [StringLength(maximumLength: 250)] // ALLOWS NULL
        public string FlagImage { get; set; }
        public virtual IList<City> Cities { get; set; }

    }
}
