using CityCountriesView.Data;
using CitiesCountriesView.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CitiesCountriesView
{
    public partial class EditForm : Form
    {
        private AppEFContext context { get; set; }
        private CitiesCountriesView.DgModels.City city { get; set; }

        public EditForm(CitiesCountriesView.DgModels.City city, AppEFContext context)
        {
            InitializeComponent();
            this.context = context;
            GetCountries();
            txtCity.Text = city.Name;
            SelectCountryByName(city.Country);
            this.city = city;
        }

        private void GetCountries()
        {
            var list = context.Countries.ToList();
            foreach (var item in list)
            {
                cbCountries.Items.Add(item.Name);
            }
        }

        private void SelectCountryByName(string name)
        {
            for (int i = 0; i < cbCountries.Items.Count; i++)
            {
                if (cbCountries.Items[i].ToString() == name)
                {
                    cbCountries.SelectedIndex = i;
                }
            }
        }

        private int GetCountryId()
        {
            var country = context.Countries.SingleOrDefault(c => c.Name == cbCountries.Text);
            if (country == null)
            {
                Country newCountry = new Country() { Name = cbCountries.Text };
                context.Countries.Add(newCountry);
                context.SaveChanges();
                newCountry = context.Countries.SingleOrDefault(c => c.Name == cbCountries.Text);
                return newCountry.Id;
            }
            return country.Id;
        }

        private void CountCountryInCities()
        {
            Country country = context.Countries.SingleOrDefault(c => c.Name == city.Country);
            int count = context.Cities.Count(c => c.CountryId == country.Id);
            if (count == 0)
            {
                context.Countries.Remove(country);
                context.SaveChanges();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            int countryId = GetCountryId();
            string cityName = txtCity.Text;
            var list = context.Cities.ToList();
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Id == city.Id)
                {
                    list[i].Name = cityName;
                    list[i].CountryId = countryId;
                    context.Cities.Update(list[i]);
                    context.SaveChanges();
                    CountCountryInCities();
                    break;
                }
            }
            this.Close();
            
        }
    }
}
