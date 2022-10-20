using WinFormsShop.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsShop
{
    public partial class AddCategoryForm : Form
    {
        public bool isPhoto { get; set; } = false;
        public string mTitle
        {
            get
            {
                return txtName.Text;
            }
        }
        public int mPrioriy
        {
            get
            {
                return int.Parse(numPriority.Value.ToString());
            }
        }
        public string mImage { get; set; }
        public AddCategoryForm()
        {
            InitializeComponent();
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
