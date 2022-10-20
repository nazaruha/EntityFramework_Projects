using System;
using CitiesCountriesView;
using CitiesCountriesView.Data;
using CityCountriesView.Data;
using Microsoft.EntityFrameworkCore;

namespace CityCountriesView
{
    public partial class Form1 : Form
    {
        public AppEFContext context { get; set; } = new AppEFContext();
        public Form1()
        {
            InitializeComponent();
            GetCities();
        }

        private void GetCities()
        {
            var list = context.Cities.Include(x => x.Country).ToList();
            List<CitiesCountriesView.DgModels.City> cities = new List<CitiesCountriesView.DgModels.City>();
            foreach (var item in list)
            {
                CitiesCountriesView.DgModels.City city = new CitiesCountriesView.DgModels.City() { Id = item.Id, Name = item.Name, Country = item.Country.Name };
                cities.Add(city);
            }
            dgCities.DataSource = cities;
        }

        private int GetCityId()
        {
            if (dgCities.SelectedRows.Count != 1)
                return -1;
            int rowIndex = dgCities.SelectedRows[0].Index;
            return (int)dgCities[0, rowIndex].Value;
        }

        private void CountCountryInCities(CitiesCountriesView.DgModels.City city)
        {
            Country country = context.Countries.SingleOrDefault(c => c.Name == city.Country);
            int count = context.Cities.Count(c => c.CountryId == country.Id);
            if (count == 0)
            {
                context.Countries.Remove(country);
                context.SaveChanges();
            }
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            CreateForm createForm = new CreateForm();
            createForm.ShowDialog();
            GetCities();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            int cityId = GetCityId();
            if (cityId == -1) return;
            int rowIndex = dgCities.SelectedRows[0].Index;
            CitiesCountriesView.DgModels.City city = new CitiesCountriesView.DgModels.City() { Id = cityId, Name = dgCities[1, rowIndex].Value.ToString(), Country = dgCities[2, rowIndex].Value.ToString() };
            EditForm editForm = new EditForm(city, context);
            editForm.ShowDialog();
            GetCities();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int cityId = GetCityId();
            if (cityId == -1) return;
            var city = context.Cities.SingleOrDefault(c => c.Id == cityId);
            context.Cities.Remove(city);
            context.SaveChanges();
            CitiesCountriesView.DgModels.City city1 = new CitiesCountriesView.DgModels.City() { Id = city.Id, Name = city.Name, Country = city.Country.Name };
            CountCountryInCities(city1);
            GetCities();
        }
    }
}