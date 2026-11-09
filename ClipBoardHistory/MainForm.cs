using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;


namespace ClipBoardHistory
{
    public partial class MainForm : Form
    {
        ClipBoardUtil _clipBoardUtil = new ClipBoardUtil();
        List<ClipBoardData> _clipBoardDatas = new List<ClipBoardData>();
        string? _lastText;
        string? _lastImageFile;
        IntPtr _listenerHandle = IntPtr.Zero;

        public MainForm()
        {
            InitializeComponent();
            dataGridView1.CellStateChanged += dataGridView1_CellStateChanged;
        }

        // İlk sütun (gün rengi şeridi) seçilemez: seçilirse hemen seçim kaldırılır.
        // Böylece tablo odak aldığında WinForms'un ilk hücreyi otomatik seçmesi de görünmez olur
        // ve şerit her zaman günün renginde görünür.
        private void dataGridView1_CellStateChanged(object? sender, DataGridViewCellStateChangedEventArgs e)
        {
            if (e.StateChanged == DataGridViewElementStates.Selected && e.Cell.ColumnIndex == 0 && e.Cell.Selected)
                e.Cell.Selected = false;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // Tablo ilk kez doldurulmadan ÖNCE kapatılmalı. Aksi halde (örn. cmbDateFilter.SelectedIndex
            // atanınca tetiklenen ilk doldurmada) sınıftaki ek özellikler için otomatik sütunlar eklenir
            // ve tasarımdaki sütunların sırası kayar.
            dataGridView1.AutoGenerateColumns = false;

            SQLiteDbContext.Initialize();

            // Açılış anında panoda olan metin kaydedilmez, sadece "son metin" olarak hatırlanır.
            var initialText = _clipBoardUtil.GetClipboardData();
            _lastText = initialText == AppConstants.InstanceKey ? " " : initialText;

            disableStripMenuItem.Checked = false;
            notifyIcon1.Icon = Properties.Resources.Untitled;
            cmbDateFilter.SelectedIndex = 1;
            this.ShowInTaskbar = false;
            _clipBoardDatas = _clipBoardUtil.GetClipBoardDatas(cmbDateFilter.SelectedIndex);
            RefreshGrid();

            this.Hide();

        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        // Pencere tanıtıcısı (handle) yeniden oluşturulabildiği için (örn. ShowInTaskbar değişince)
        // pano dinleyici kaydı her handle oluşturulduğunda yapılır, yok edildiğinde kaldırılır.
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            var error = _clipBoardUtil.RegisterClipboardListener(this.Handle);
            if (error != 0)
                ExceptionHandler.HandleException(new Win32Exception(error, "AddClipboardFormatListener failed"));
            else
                _listenerHandle = this.Handle;
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            if (_listenerHandle != IntPtr.Zero)
            {
                _clipBoardUtil.UnregisterClipboardListener(_listenerHandle);
                _listenerHandle = IntPtr.Zero;
            }
            base.OnHandleDestroyed(e);
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == (int)ClipBoardUtil.Msgs.WM_CLIPBOARDUPDATE)
            {
                OnClipboardUpdate();
                m.Result = IntPtr.Zero;
                return;
            }

            base.WndProc(ref m);
        }

        private void OnClipboardUpdate()
        {
            string txt = _clipBoardUtil.GetClipboardData();

            if (txt == AppConstants.InstanceKey)
            {//This code --> AppConstants.InstanceKey send by Program.cs when user try to start second instance
                //we handle this specific string to activete our mainForm
                notifyIcon1_DoubleClick(this, EventArgs.Empty);
                var last = string.IsNullOrEmpty(_lastText) ? " " : _lastText;
                Clipboard.SetText(last);
                return;
            }

            if (chkDisableCapture.Checked)
                return;

            // Uygulamanın kendi penceresi aktifken (örn. "Copy" menüsüyle panoya geri koyarken) kayıt yapılmaz.
            if (Form.ActiveForm != null)
                return;

            // 1) Metin varsa: düz metin + varsa RTF/HTML biçimi kaydedilir.
            //    (Excel gibi programlar metinle birlikte resim de koyar; bu durumda metin kaydedilir.)
            if (!string.IsNullOrEmpty((txt ?? "").Trim()))
            {
                if (txt == _lastText)
                    return;

                SaveRecord(new ClipBoardData()
                {
                    CBText = txt,
                    Rtf = _clipBoardUtil.GetRichText(TextDataFormat.Rtf),
                    Html = _clipBoardUtil.GetRichText(TextDataFormat.Html),
                    CreateDate = NowWithoutMilliseconds()
                });
                _lastText = txt;
                _lastImageFile = null;
                return;
            }

            // 2) Metin yoksa ve resim varsa: resim PNG olarak kaydedilir.
            // 3) Dosya kopyalamaları (Gezgin'de dosya kopyalama) ne metin ne resim içerdiği için kaydedilmez.
            var imageFile = _clipBoardUtil.SaveClipboardImage();
            if (imageFile == null || imageFile == _lastImageFile)
                return;

            SaveRecord(new ClipBoardData() { ImagePath = imageFile, CreateDate = NowWithoutMilliseconds() });
            _lastImageFile = imageFile;
            _lastText = null;
        }

        private void SaveRecord(ClipBoardData data)
        {
            using var dbcontext = new SQLiteDbContext();
            dbcontext.ClipBoardDatas!.Add(data);
            dbcontext.SaveChanges();
            _clipBoardDatas.Add(data);
            RefreshGrid();
        }

        private static DateTime NowWithoutMilliseconds()
        {
            var now = DateTime.Now;
            return now.AddTicks(-(now.Ticks % TimeSpan.TicksPerSecond)); // veritabanı saniye hassasiyetinde saklar
        }

        private void RefreshGrid()
        {
            List<ClipBoardData> tempDataList = new List<ClipBoardData>();

            tempDataList = _clipBoardDatas.Where(x => (x.CBText ?? "").ToLower().Contains(txtSearch.Text.ToLower()) || (x.Note ?? "").ToLower().Contains(txtSearch.Text.ToLower()) || string.IsNullOrEmpty(txtSearch.Text)).ToList();

            if (chkNoteAdded.Checked)
            {
                tempDataList = tempDataList.Where(x => !string.IsNullOrEmpty(x.Note)).ToList();
            }

            //order
            tempDataList = tempDataList.OrderByDescending(x => x.CreateDate).ToList();


            dataGridView1.DataSource = null;
            dataGridView1.Refresh();

            dataGridView1.DataSource = tempDataList;
            dataGridView1.Refresh();

        }

        private void deleteToolStripMenuItem_Click(object? sender, EventArgs e)
        {
            if (sender is not ToolStripMenuItem mItem || mItem.Tag is not DataGridView.HitTestInfo info)
                return;

            var id = Convert.ToInt32(dataGridView1.Rows[info.RowIndex].Cells[0].Value);

            using var dbcontext = new SQLiteDbContext();
            var clipBoardData = dbcontext.ClipBoardDatas.FirstOrDefault(x => x.Id == id);
            var imageFile = clipBoardData?.ImagePath;
            if (clipBoardData != null)
            {
                dbcontext.ClipBoardDatas.Remove(clipBoardData);
                dbcontext.SaveChanges();
            }
            _clipBoardDatas.RemoveAll(x => x.Id == id);

            // Aynı resmi kullanan başka kayıt kalmadıysa resim dosyası da silinir.
            if (!string.IsNullOrEmpty(imageFile) && !dbcontext.ClipBoardDatas.Any(x => x.ImagePath == imageFile))
            {
                _clipBoardUtil.DeleteImageFile(imageFile);
                if (_lastImageFile == imageFile)
                    _lastImageFile = null;
            }

            RefreshGrid();


        }


        // Kaydı (resim veya biçimli metin dahil) panoya geri koyar.
        private void copyToolStripMenuItem_Click(object? sender, EventArgs e)
        {
            if (sender is not ToolStripMenuItem mItem || mItem.Tag is not DataGridView.HitTestInfo info)
                return;

            var id = Convert.ToInt32(dataGridView1.Rows[info.RowIndex].Cells[0].Value);
            var record = _clipBoardUtil.GetClipBoardData(id);
            if (record != null)
                _clipBoardUtil.SetClipboard(record);
        }

        private void dataGridView1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {

                var info = dataGridView1.HitTest(e.X, e.Y);
                var showPoint = new Point(e.X + this.Left + dataGridView1.Left, e.Y + this.Top + dataGridView1.Top);
                if (info.RowIndex < 0) return;
                ContextMenuStrip m = new ContextMenuStrip();
                var copyItem = new ToolStripMenuItem("Copy", null, copyToolStripMenuItem_Click);
                copyItem.Tag = info;
                m.Items.Add(copyItem);
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
                // Listede Rtf/Html yüklü değil; tam kayıt veritabanından çekilir.
                if (dataGridView1.Rows[e.RowIndex].DataBoundItem is not ClipBoardData row)
                    return;
                var record = _clipBoardUtil.GetClipBoardData(row.Id) ?? row;

                using var frm = new frmTextDetail();
                frm.txt = record.CBText ?? "";
                frm.rtf = record.Rtf;
                frm.imagePath = record.ImageFullPath;
                frm.ShowDialog();
            }
            else if (e.ColumnIndex == 3)
            {//Note formunu aç.
                var frm = new frmNoteDetail();
                frm.txt = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value?.ToString() ?? "";
                frm.id = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value);
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    using var dbcontext = new SQLiteDbContext();
                    var clipBoardData = dbcontext.ClipBoardDatas.FirstOrDefault(x => x.Id == frm.id);
                    if (clipBoardData != null)
                    {
                        clipBoardData.Note = frm.txt;
                        dbcontext.SaveChanges();
                    }

                    var listItem = _clipBoardDatas.FirstOrDefault(x => x.Id == frm.id);
                    if (listItem != null)
                        listItem.Note = frm.txt;

                    RefreshGrid();
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

            RefreshGrid();
        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            if (e.RowIndex < 0) return;
            // Renk sütun numarasıyla değil, satırın bağlı olduğu kayıttan okunur; sütun sırası değişse de etkilenmez.
            if (dataGridView1.Rows[e.RowIndex].DataBoundItem is ClipBoardData item)
                dataGridView1.Rows[e.RowIndex].Cells[0].Style.BackColor = item.DayColor;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            _clipBoardDatas = _clipBoardUtil.GetClipBoardDatas(cmbDateFilter.SelectedIndex);
            RefreshGrid();

        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSearch_Click(this, new EventArgs());
            }
        }

        private void chkNoteAdded_CheckedChanged(object sender, EventArgs e)
        {
            RefreshGrid();
        }

        private void chkDisableCapture_CheckedChanged(object sender, EventArgs e)
        {
            disableStripMenuItem.Checked = chkDisableCapture.Checked;
            notifyIcon1.Icon = chkDisableCapture.Checked ? Properties.Resources.UntitledDisable : Properties.Resources.Untitled;
        }

        private void disableStripMenuItem_Click(object sender, EventArgs e)
        {
            chkDisableCapture.Checked = !chkDisableCapture.Checked;
        }
    }
}
