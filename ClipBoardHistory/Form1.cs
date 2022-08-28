using System.Diagnostics;
using System.Windows.Forms;


namespace ClipBoardHistory
{
    public partial class Form1 : Form
    {
        ClipBoardUtil _clipBoardUtil = new ClipBoardUtil();
        bool _firstRun = true;
        List<ClipBoardData> _clipBoardDatas = new List<ClipBoardData>();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _clipBoardUtil.RegisterClipboardViewer(this.Handle);
            dataGridView1.AutoGenerateColumns = false;
            using var dbcontext = new SQLiteDbContext();
            dbcontext.Database.EnsureCreated();
            _clipBoardDatas = dbcontext.ClipBoardDatas.ToList().OrderByDescending(x => x.CreateDate).ToList();


            dataGridView1.DataSource = _clipBoardDatas;


        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            _clipBoardUtil.UnregisterClipboardViewer(this.Handle);
            Application.Exit();
        }

        

        protected override void WndProc(ref Message m)
        {
            switch ((ClipBoardUtil.Msgs)m.Msg)
            {
                //
                // The WM_DRAWCLIPBOARD message is sent to the first window 
                // in the clipboard viewer chain when the content of the 
                // clipboard changes. This enables a clipboard viewer 
                // window to display the new content of the clipboard. 
                //
                case ClipBoardUtil.Msgs.WM_DRAWCLIPBOARD:

                    Debug.WriteLine("WindowProc DRAWCLIPBOARD: " + m.Msg, "WndProc");

                    string txt = _clipBoardUtil.GetClipboardData();
                    if (!string.IsNullOrEmpty(txt))
                    {

                        if (!_firstRun)
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

                        _firstRun = false;
                    }
                    //
                    // Each window that receives the WM_DRAWCLIPBOARD message 
                    // must call the SendMessage function to pass the message 
                    // on to the next window in the clipboard viewer chain.
                    //
                    ClipBoardUtil.SendMessage(ClipBoardUtil._ClipboardViewerNext, m.Msg, m.WParam, m.LParam);
                    break;


                //
                // The WM_CHANGECBCHAIN message is sent to the first window 
                // in the clipboard viewer chain when a window is being 
                // removed from the chain. 
                //
                case ClipBoardUtil.Msgs.WM_CHANGECBCHAIN:
                    Debug.WriteLine("WM_CHANGECBCHAIN: lParam: " + m.LParam, "WndProc");

                    // When a clipboard viewer window receives the WM_CHANGECBCHAIN message, 
                    // it should call the SendMessage function to pass the message to the 
                    // next window in the chain, unless the next window is the window 
                    // being removed. In this case, the clipboard viewer should save 
                    // the handle specified by the lParam parameter as the next window in the chain. 

                    //
                    // wParam is the Handle to the window being removed from 
                    // the clipboard viewer chain 
                    // lParam is the Handle to the next window in the chain 
                    // following the window being removed. 
                    if (m.WParam == ClipBoardUtil._ClipboardViewerNext)
                    {
                        //
                        // If wParam is the next clipboard viewer then it
                        // is being removed so update pointer to the next
                        // window in the clipboard chain
                        //
                        ClipBoardUtil._ClipboardViewerNext = m.LParam;
                    }
                    else
                    {
                        ClipBoardUtil.SendMessage(ClipBoardUtil._ClipboardViewerNext, m.Msg, m.WParam, m.LParam);
                    }
                    break;

                default:
                    //
                    // Let the form process the messages that we are
                    // not interested in
                    //
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
                frm.txt = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                frm.ShowDialog();
            }
            else if (e.ColumnIndex == 3)
            {//Note formunu aç.

            }
        }
    }
}