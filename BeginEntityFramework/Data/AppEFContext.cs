using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeginEntityFramework.Data
{
    public class AppEFContext : DbContext // унаслідуєм цей клас для того, щоб працювати з БД
    {
        public AppEFContext()
        {
            /*this.Database.EnsureCreated();*/ // перевіряє чи існує ЬД. Якщо ні, то створює її
            this.Database.Migrate(); // якщо міграцій немає, то воно їх саме накатає
            
        }
        public DbSet<Car> Cars { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<City> Cities { get; set; }
        protected override void OnConfiguring( DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.;Integrated Security=True;Initial Catalog=pd123db;");
        }
    }
}
