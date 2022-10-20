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
    public partial class SendSMSForm : Form
    {
        public string mPhone
        {
            get
            {
                return txtPhone.Text;
            }
        }
        public string mSMS
        {
            get
            {
                return txtSMS.Text;
            }
        }
        public SendSMSForm()
        {
            InitializeComponent();
        }
    }
}
