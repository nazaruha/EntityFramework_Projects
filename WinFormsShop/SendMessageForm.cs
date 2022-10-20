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
    public partial class SendMessageForm : Form
    {
        public string mWhom
        {
            get
            {
                return txtWhom.Text;
            }
        }
        public string mTopic
        {
            get
            {
                return txtTopic.Text;
            }
        }
        public string mMessage
        {
            get
            {
                return txtMessage.Text;
            }
        }
        public SendMessageForm()
        {
            InitializeComponent();
        }
    }
}
