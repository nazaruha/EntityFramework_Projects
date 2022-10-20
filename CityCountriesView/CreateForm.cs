using System;
using CitiesCountriesView.Data;
using CityCountriesView.Data;
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
    public partial class CreateForm : Form
    {
        public AppEFContext context { get; set; } = new AppEFContext();
        public CreateForm()
        {
            InitializeComponent();
            GetCountries();
        }
        private void GetCountries()
        {
            var list = context.Countries.ToList();
            foreach (var item in list)
            {
                cbCountries.Items.Add(item.Name);
            }
        }

        private int GetCountryId(string name)
        {
            var country = context.Countries.SingleOrDefault(c => c.Name == name);
            if (country == null)
            {
                Country newCountry = new Country() { Name = name };
                context.Countries.Add(newCountry);
                context.SaveChanges();
            }
            country = context.Countries.SingleOrDefault(c => c.Name == name);
            return country.Id;
        }

        private bool CheckCityExistence()
        {
            var city = context.Cities.SingleOrDefault(c => c.Name == txtCity.Text);
            if (city != null)
                return false;
            return true;
        }

        private bool CheckInputting()
        {
            if (String.IsNullOrWhiteSpace(txtCity.Text) || String.IsNullOrWhiteSpace(cbCountries.Text))
            {
                MessageBox.Show("Smth is empty");
                return false;
            }
            if (!CheckCityExistence())
            {
                MessageBox.Show("This city is already occupied");
                return false;
            }
            return true;
        }

        private void CreateCity()
        {
            int countryId = GetCountryId(cbCountries.Text);
            City newCity = new City() { Name = txtCity.Text, CountryId = countryId };
            context.Cities.Add(newCity);
            context.SaveChanges();
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            if (!CheckInputting())
                return;
            CreateCity();
            MessageBox.Show("City has been added");
            ClearForm();
            this.Close();
        }

        private void ClearForm()
        {
            txtCity.Clear();
            cbCountries.Text = "";
            cbCountries.SelectedItem = null;
        }
    }
}
