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
    public partial class frmNoteDetail : Form
    {
        public frmNoteDetail()
        {
            InitializeComponent();
        }
        public string txt = "";
        public int id = 0;
        private void frmNoteDetail_Load(object sender, EventArgs e)
        {
            //TODO
            richTextBox1.Text = txt;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txt != richTextBox1.Text)
            {
                txt = richTextBox1.Text;
                DialogResult = DialogResult.OK;
            }
            else
                DialogResult = DialogResult.Cancel;
        }
    }
}
