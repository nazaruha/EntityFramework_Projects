using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinFormsShop.Data.Entities;

namespace WinFormsShop.Data
{
    public class MyDataContext : DbContext
    {
        public MyDataContext()
        {
            this.Database.Migrate(); // якщо міграцій немає автоматично запускаємо (БД створиться автоматично)
        }
        public DbSet<CategoryEntity> Categories { get; set; }
        public DbSet<ProductEntity> Products { get; set; }
        public DbSet<ProductImageEntity> ProductImages { get; set; }
        public DbSet<SentSMS> SMS { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.;Integrated Security=True;Initial Catalog=pd123dbShop"); // строка підключення до БД 
        }

        //Fluent API
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<ProductImageEntity>(x => 
                x.ToTable("tblProductImages")
            );
        }
    }
}
