using Microsoft.EntityFrameworkCore;
using CitiesCountriesView.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityCountriesView.Data
{
    public class AppEFContext : DbContext // унаслідуєм цей клас для того, щоб працювати з БД
    {
        public AppEFContext()
        {
            this.Database.Migrate();
        }

        public DbSet<Country> Countries { get; set; }
        public DbSet<City> Cities { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.;Integrated Security=True;Initial Catalog=CitiesCountries2;");
        }
    }
}
