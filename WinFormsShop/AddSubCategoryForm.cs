using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinFormsShop.Data.Entities;
using WinFormsShop.Data;

namespace WinFormsShop
{
    public partial class AddSubCategoryForm : Form
    {
        public MyDataContext context { get; set; }
        public bool isPhoto { get; set; } = false;
        public string mCategoryId
        {
            get
            {
                var findCategory = context.Categories.ToList().Find(x => x.Title == cbCategories.Text);
                if (findCategory == null)
                    return null;
                return findCategory.Id.ToString();
            }
        }
        public string mSubCategory
        {
            get
            {
                return txtSubCatTitle.Text;
            }
        }
        public int mPriority
        {
            get
            {
                return int.Parse(numPriority.Value.ToString());
            }
        }
        public string mImage { get; set; }

        public AddSubCategoryForm()
        {
            InitializeComponent();
            cbCategories.DropDownStyle = ComboBoxStyle.DropDownList;
            context = new MyDataContext();
            GetCategories();

        }

        private void GetCategories()
        {
            foreach (var item in context.Categories.ToList().Where(x => x.ParentId == null))
            {
                cbCategories.Items.Add(item.Title);
            }
        }

        private void picPhoto_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                //picPhoto.SizeMode = PictureBoxSizeMode.StretchImage;
                picPhoto.Image = Image.FromFile(openFileDialog.FileName);
                mImage = openFileDialog.FileName;
                isPhoto = true;
            }
        }
    }
}
