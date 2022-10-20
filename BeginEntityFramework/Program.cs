using BeginEntityFramework.Data;
using System;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace BeginEntityFramework
{
    class Program
    {
        
        public static void Main(string[] args)
        {

            //2 rows above for writing in ukrainian
            Console.InputEncoding = Encoding.UTF8;
            Console.OutputEncoding = Encoding.UTF8;
            //Console.WriteLine("Привіт");


            AppEFContext context = new AppEFContext();
            //AddCar("Малюк", "Жигулі", "2107", context);
            //EditCar(4, context);
            //RemoveCar(4, context);
            GetCars(context);

            //AddCountry("Dublin", context);
            GetCountries(context);
            //AddCity("Dublin", 1, context);
            GetCities(context);

        }

        private static void AddCar(string name, string mark, string model, AppEFContext cntx)
        {
            Car newCar = new Car() { Name = name, Mark = mark, Model = model };

            cntx.Cars.Add(newCar);
            cntx.SaveChanges();
            Console.WriteLine($"Car Id = {newCar.Id}");
        }

        private static void AddCountry(string name, AppEFContext con)
        {
            Country newCountry = new Country() { Name = name };
            con.Countries.Add(newCountry);
            con.SaveChanges();
        }
            
        private static void AddCity(string name, int countryId, AppEFContext con)
        {
            City newCity = new City() { Name = name, CountryId = countryId };
            con.Cities.Add(newCity);
            con.SaveChanges();
        }

        private static void GetCars(AppEFContext cntx)
        {
            var list = cntx.Cars.ToList();
            foreach (var item in list)
            {
                Console.WriteLine(item);
            }
        }

        private static void GetCountries(AppEFContext con)
        {
            var list = con.Countries.ToList();
            foreach (var item in list)
            {
                Console.WriteLine(item);
            }
        }

        private static void GetCities(AppEFContext con)
        {
            var list = con.Cities.Include(x => x.Country).ToList();
            foreach (var item in list)
            {
                Console.WriteLine(item);
            }
        }

        private static void EditCar(int id, AppEFContext cntx)
        {
            var car = cntx.Cars.SingleOrDefault(x => x.Id == id);
            if (car != null)
            {
                Console.Write("Input name: ");
                string name = Console.ReadLine(); // укр мову чогось сприймати не буде
                car.Name = name;
                cntx.SaveChanges();
            }
        }

        private static void RemoveCar(int id, AppEFContext cntx)
        {
            var car = cntx.Cars.SingleOrDefault(x =>x.Id == id);
            if (car != null)
            {
                cntx.Remove(car);
                cntx.SaveChanges();
            }
        }
    }
}


