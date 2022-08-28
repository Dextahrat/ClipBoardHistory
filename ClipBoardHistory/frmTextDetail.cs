using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClipBoardHistory
{
    public partial class frmTextDetail : Form
    {
        public frmTextDetail()
        {
            InitializeComponent();
        }
        public string txt = "";
        private void frmTextDetail_Load(object sender, EventArgs e)
        {
            //TODO
            richTextBox1.Text = txt;
        }
    }
}
