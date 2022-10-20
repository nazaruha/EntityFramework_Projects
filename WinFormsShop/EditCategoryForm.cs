using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinFormsShop.Data;
using WinFormsShop.Data.Entities;
using static System.Net.Mime.MediaTypeNames;
using Image = System.Drawing.Image;

namespace WinFormsShop
{
    public partial class EditCategoryForm : Form
    {
        private MyDataContext context { get; set; }

        public CategoryEntity category { get; set; }

        public string newTitle
        {
            get
            {
                return txtTitle.Text;
            }
        }

        public bool newIsDeleted
        {
            get
            {
                return checkIsDeleted.Checked;
            }
        }

        public string newParentId { get; private set; }

        public int newPriority
        {
            get
            {
                return (int)numPriority.Value;
            }
        }

        public DateTime newDateCreate
        {
            get
            {
                return pickerDateCreate.Value;
            }
        }

        public string newPhoto { get; set; }

        public EditCategoryForm(CategoryEntity category)
        {
            InitializeComponent();
            context = new MyDataContext();
            this.category = category;
            cbParents.DropDownStyle = ComboBoxStyle.DropDownList;
            InitializeObjects();
        }

        private void InitializeObjects()
        {
            InitializeTitle();
            InitializeParents();
            InitializePriority();
            InitializeDateCreate();
            InitializeIsDeleted();
            InitializePhoto();
        }

        private void InitializeTitle()
        {
            txtTitle.Text = category.Title;
        }

        private void InitializeParents()
        {
            if (category.ParentId == null) return;
            foreach (var item in context.Categories.ToList().Where(x => x.ParentId == null))
            {
                cbParents.Items.Add(item.Title);
            }
            var selected = context.Categories.SingleOrDefault(x => x.Id == category.ParentId);
            newParentId = selected.Id.ToString();
            cbParents.SelectedIndex = cbParents.Items.IndexOf(selected.Title);
        }

        private void InitializePriority()
        {
            numPriority.Value = category.Priority;
        }

        private void InitializeDateCreate()
        {
            string date = category.DateCreated.ToShortDateString();

            pickerDateCreate.Value = DateTime.Parse(date);
        }

        private void InitializeIsDeleted()
        {
            checkIsDeleted.Checked = category.IsDelete;
        }

        private void InitializePhoto()
        {
            String dir = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "Images");
            string path = dir + "\\160_"+category.Image;
            Stream stream = ConvertFileToByteArrayThenToMemoryStream(path);
            picPhoto.Image = Image.FromStream(stream);
        }
        private Stream ConvertFileToByteArrayThenToMemoryStream(string fileName)
        {
            byte[] byteArray = File.ReadAllBytes(fileName);
            Stream stream = new MemoryStream(byteArray);
            return stream;
        }

        private void picPhoto_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                picPhoto.Image = Image.FromFile(openFileDialog.FileName);
                newPhoto = openFileDialog.FileName;
            }
        }

        private void cbParents_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbParents.SelectedIndex == -1) return;
            newParentId = GetParentId().ToString();
        }

        private int GetParentId()
        {
            var select = context.Categories.SingleOrDefault(x => x.Title == cbParents.SelectedItem.ToString() && x.ParentId == null);
            return select.Id;
        }

    }
}
