using System.Diagnostics;
using System.Windows.Forms;


namespace ClipBoardHistory
{
    public partial class MainForm : Form
    {
        ClipBoardUtil _clipBoardUtil = new ClipBoardUtil();
        bool _firstRun = true;
        List<ClipBoardData> _clipBoardDatas = new List<ClipBoardData>();

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            cmbDateFilter.SelectedIndex = 1;
            this.ShowInTaskbar = false;
            _clipBoardUtil.RegisterClipboardViewer(this.Handle);
            dataGridView1.AutoGenerateColumns = false;
            _clipBoardDatas = _clipBoardUtil.GetClipBoardDatas(cmbDateFilter.SelectedIndex);


            dataGridView1.DataSource = _clipBoardDatas;
            this.Hide();

        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            _clipBoardUtil.UnregisterClipboardViewer(this.Handle);
            Application.Exit();
        }

        

        protected override void WndProc(ref Message m)
        {
            switch ((ClipBoardUtil.Msgs)m.Msg)
            {
                case ClipBoardUtil.Msgs.WM_DRAWCLIPBOARD:


                    string txt = _clipBoardUtil.GetClipboardData();
                    if (_firstRun)
                    {
                        if (txt == AppConstants.InstanceKey)
                            txt = "";
                        _firstRun =false;
                        return;
                    }

                    if (txt == AppConstants.InstanceKey)
                    {//This code --> AppConstants.InstanceKey send by Program.cs when user try to start second instance
                        //we handle this specific string to activete our mainForm
                        var lastText = _clipBoardUtil.GetLastClipboardText();
                        notifyIcon1_DoubleClick(null, null);

                        Clipboard.SetText(lastText??"");
                        return;
                    }
                    if (!string.IsNullOrEmpty(txt))
                    {
                        if(txt == _clipBoardUtil.GetLastClipboardText())
                        {
                            return;
                        }

                        if (Form.ActiveForm == null)
                        {
                            using var dbcontext = new SQLiteDbContext();
                            dbcontext.Database.EnsureCreated();
                            ClipBoardData data = new ClipBoardData() { CBText = txt, CreateDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") };
                            dbcontext.ClipBoardDatas.Add(data);
                            dbcontext.SaveChanges();
                            _clipBoardDatas.Add(data);
                            _clipBoardDatas = _clipBoardDatas.OrderByDescending(x => x.CreateDate).ToList();
                            dataGridView1.DataSource= _clipBoardDatas;
                            

                        }
                    }

                    ClipBoardUtil.SendMessage(ClipBoardUtil._ClipboardViewerNext, m.Msg, m.WParam, m.LParam);
                    break;

                case ClipBoardUtil.Msgs.WM_CHANGECBCHAIN:
                    Debug.WriteLine("WM_CHANGECBCHAIN: lParam: " + m.LParam, "WndProc");


                    if (m.WParam == ClipBoardUtil._ClipboardViewerNext)
                    {
                        ClipBoardUtil._ClipboardViewerNext = m.LParam;
                    }
                    else
                    {
                        ClipBoardUtil.SendMessage(ClipBoardUtil._ClipboardViewerNext, m.Msg, m.WParam, m.LParam);
                    }
                    break;

                default:
                    base.WndProc(ref m);
                    break;

            }

        }



        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var mItem = (ToolStripMenuItem)sender;
            var info = (DataGridView.HitTestInfo)(mItem.Tag);
            
            var id = Convert.ToInt32(dataGridView1.Rows[info.RowIndex].Cells[0].Value);

            using var dbcontext = new SQLiteDbContext();
            dbcontext.Database.EnsureCreated();
            var clipBoardData = dbcontext.ClipBoardDatas.FirstOrDefault(x => x.Id == id);
            dbcontext.ClipBoardDatas.Remove(clipBoardData);
            dbcontext.SaveChanges();
            _clipBoardDatas.Remove(_clipBoardDatas.FirstOrDefault(x=>x.Id == id));

            dataGridView1.DataSource = null;
            dataGridView1.Refresh();

            dataGridView1.DataSource = _clipBoardDatas;
            dataGridView1.Refresh();

        }


        private void dataGridView1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {

                var info = dataGridView1.HitTest(e.X, e.Y);
                var showPoint = new Point(e.X + this.Left + dataGridView1.Left, e.Y+this.Top+dataGridView1.Top);
                if (info.RowIndex < 0) return;
                ContextMenuStrip m = new ContextMenuStrip();
                var mItem = new ToolStripMenuItem("Delete", null, deleteToolStripMenuItem_Click);
                mItem.Tag = info;
                m.Items.Add(mItem);
                m.Show(showPoint);

                dataGridView1.ClearSelection();
                dataGridView1.Rows[info.RowIndex].Cells[info.ColumnIndex].Selected = true;

            }
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (e.ColumnIndex == 2)
            {//Text formunu aç.
                var frm = new frmTextDetail();
                frm.txt = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
                frm.ShowDialog();
            }
            else if (e.ColumnIndex == 3)
            {//Note formunu aç.
                var frm = new frmNoteDetail();
                frm.txt = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value == null ? "" : dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                frm.id = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
                if(frm.ShowDialog() == DialogResult.OK)
                {
                    using var dbcontext = new SQLiteDbContext();
                    dbcontext.Database.EnsureCreated();
                    var clipBoardData = dbcontext.ClipBoardDatas.FirstOrDefault(x => x.Id == frm.id);
                    clipBoardData.Note = frm.txt;
                    dbcontext.SaveChanges();
                    _clipBoardDatas.FirstOrDefault(x => x.Id == frm.id).Note=frm.txt;

                    dataGridView1.DataSource = null;
                    dataGridView1.Refresh();

                    dataGridView1.DataSource = _clipBoardDatas;
                    dataGridView1.Refresh();
                }
                
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
            Application.Exit();
        }

        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        {
            notifyIcon1_DoubleClick(sender, e);
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            this.TopMost = true;
            this.Show();
            this.Focus();
            this.TopMost = false;
        }

        private void cmbDateFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            _clipBoardDatas = _clipBoardUtil.GetClipBoardDatas(cmbDateFilter.SelectedIndex);

            dataGridView1.DataSource = null;
            dataGridView1.Refresh();

            dataGridView1.DataSource = _clipBoardDatas;
            dataGridView1.Refresh();
        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            if (e.RowIndex < 0) return;
            dataGridView1.Rows[e.RowIndex].Cells[0].Style.BackColor = (Color)(dataGridView1.Rows[e.RowIndex].Cells[6].Value);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            _clipBoardDatas = _clipBoardUtil.GetClipBoardDatas(cmbDateFilter.SelectedIndex);

            dataGridView1.DataSource = null;
            dataGridView1.Refresh();
            var a = _clipBoardDatas.Where(x => (x.CBText ?? "").ToLower().Contains(txtSearch.Text.ToLower()) || (x.Note ?? "").ToLower().Contains(txtSearch.Text.ToLower()) || string.IsNullOrEmpty(txtSearch.Text));
            dataGridView1.DataSource = a.ToList();

            dataGridView1.Refresh();
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSearch_Click(this, new EventArgs());
            }
        }
    }
}