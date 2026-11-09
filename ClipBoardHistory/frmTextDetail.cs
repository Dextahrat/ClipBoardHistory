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
        public string? rtf;
        public string? imagePath;

        private void frmTextDetail_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(imagePath))
            {
                ShowImage(imagePath);
                return;
            }

            // Biçimli metin (RTF) varsa biçimiyle gösterilir; RTF bozuksa düz metne dönülür.
            if (!string.IsNullOrEmpty(rtf))
            {
                try
                {
                    richTextBox1.Rtf = rtf;
                    return;
                }
                catch (ArgumentException)
                {
                }
            }

            richTextBox1.Text = txt;
        }

        private void ShowImage(string fullPath)
        {
            if (!File.Exists(fullPath))
            {
                richTextBox1.Text = "Image file not found:" + Environment.NewLine + fullPath;
                return;
            }

            var image = ClipBoardUtil.LoadImage(fullPath);
            var pictureBox = new PictureBox
            {
                Dock = DockStyle.Fill,
                Image = image,
                // Pencereden küçük resimler gerçek boyutunda ortalanır, büyükler pencereye sığdırılır.
                SizeMode = image.Width <= ClientSize.Width && image.Height <= ClientSize.Height
                    ? PictureBoxSizeMode.CenterImage
                    : PictureBoxSizeMode.Zoom
            };

            richTextBox1.Visible = false;
            Controls.Add(pictureBox);
            Text = "ClipBoard Image (" + image.Width + " x " + image.Height + ")";

            FormClosed += (s, args) => image.Dispose();
        }
    }
}
