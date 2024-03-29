﻿namespace ClipBoardHistory
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CreateDateWithDay = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Brief = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Note = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CBText = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CreateDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DayColor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.showToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.disableStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cmbDateFilter = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.chkDisableCapture = new System.Windows.Forms.CheckBox();
            this.chkNoteAdded = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToOrderColumns = true;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dataGridView1.BackgroundColor = System.Drawing.Color.White;
            this.dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Id,
            this.CreateDateWithDay,
            this.Brief,
            this.Note,
            this.CBText,
            this.CreateDate,
            this.DayColor});
            this.dataGridView1.Location = new System.Drawing.Point(12, 36);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowHeadersWidth = 51;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.RowsDefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView1.RowTemplate.Height = 25;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dataGridView1.Size = new System.Drawing.Size(776, 410);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView1_CellMouseDoubleClick);
            this.dataGridView1.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dataGridView1_RowPostPaint);
            this.dataGridView1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dataGridView1_MouseDown);
            // 
            // Id
            // 
            this.Id.DataPropertyName = "Id";
            this.Id.HeaderText = "";
            this.Id.MinimumWidth = 6;
            this.Id.Name = "Id";
            this.Id.ReadOnly = true;
            this.Id.Width = 6;
            // 
            // CreateDateWithDay
            // 
            this.CreateDateWithDay.DataPropertyName = "CreateDateWithDay";
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft YaHei", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.CreateDateWithDay.DefaultCellStyle = dataGridViewCellStyle1;
            this.CreateDateWithDay.HeaderText = "Date";
            this.CreateDateWithDay.MinimumWidth = 6;
            this.CreateDateWithDay.Name = "CreateDateWithDay";
            this.CreateDateWithDay.ReadOnly = true;
            this.CreateDateWithDay.Width = 115;
            // 
            // Brief
            // 
            this.Brief.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Brief.DataPropertyName = "Brief";
            this.Brief.HeaderText = "Text";
            this.Brief.MinimumWidth = 6;
            this.Brief.Name = "Brief";
            this.Brief.ReadOnly = true;
            // 
            // Note
            // 
            this.Note.DataPropertyName = "Note";
            this.Note.HeaderText = "Note";
            this.Note.MinimumWidth = 6;
            this.Note.Name = "Note";
            this.Note.ReadOnly = true;
            this.Note.Width = 125;
            // 
            // CBText
            // 
            this.CBText.DataPropertyName = "CBText";
            this.CBText.HeaderText = "FullText";
            this.CBText.MinimumWidth = 6;
            this.CBText.Name = "CBText";
            this.CBText.ReadOnly = true;
            this.CBText.Visible = false;
            this.CBText.Width = 500;
            // 
            // CreateDate
            // 
            this.CreateDate.DataPropertyName = "CreateDate";
            this.CreateDate.HeaderText = "TarihNormal";
            this.CreateDate.MinimumWidth = 6;
            this.CreateDate.Name = "CreateDate";
            this.CreateDate.ReadOnly = true;
            this.CreateDate.Visible = false;
            this.CreateDate.Width = 115;
            // 
            // DayColor
            // 
            this.DayColor.DataPropertyName = "DayColor";
            this.DayColor.HeaderText = "DayColor";
            this.DayColor.MinimumWidth = 6;
            this.DayColor.Name = "DayColor";
            this.DayColor.ReadOnly = true;
            this.DayColor.Visible = false;
            this.DayColor.Width = 125;
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "Clipboard History";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.DoubleClick += new System.EventHandler(this.notifyIcon1_DoubleClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showToolStripMenuItem,
            this.exitToolStripMenuItem,
            this.disableStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(181, 92);
            // 
            // showToolStripMenuItem
            // 
            this.showToolStripMenuItem.Name = "showToolStripMenuItem";
            this.showToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.showToolStripMenuItem.Text = "Show";
            this.showToolStripMenuItem.Click += new System.EventHandler(this.showToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // disableStripMenuItem
            // 
            this.disableStripMenuItem.CheckOnClick = true;
            this.disableStripMenuItem.Name = "disableStripMenuItem";
            this.disableStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.disableStripMenuItem.Text = "Disable";
            this.disableStripMenuItem.Click += new System.EventHandler(this.disableStripMenuItem_Click);
            // 
            // cmbDateFilter
            // 
            this.cmbDateFilter.FormattingEnabled = true;
            this.cmbDateFilter.Items.AddRange(new object[] {
            "Last 1 day",
            "Last 1 Week",
            "Last 1 Month",
            "Last 1 Year",
            "ALL"});
            this.cmbDateFilter.Location = new System.Drawing.Point(83, 7);
            this.cmbDateFilter.Name = "cmbDateFilter";
            this.cmbDateFilter.Size = new System.Drawing.Size(137, 23);
            this.cmbDateFilter.TabIndex = 1;
            this.cmbDateFilter.SelectedIndexChanged += new System.EventHandler(this.cmbDateFilter_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "Show from";
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(264, 7);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(202, 23);
            this.txtSearch.TabIndex = 3;
            this.txtSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSearch_KeyDown);
            // 
            // btnSearch
            // 
            this.btnSearch.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSearch.BackgroundImage")));
            this.btnSearch.Location = new System.Drawing.Point(466, 7);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(24, 24);
            this.btnSearch.TabIndex = 4;
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // chkDisableCapture
            // 
            this.chkDisableCapture.AutoSize = true;
            this.chkDisableCapture.Location = new System.Drawing.Point(668, 9);
            this.chkDisableCapture.Name = "chkDisableCapture";
            this.chkDisableCapture.Size = new System.Drawing.Size(120, 19);
            this.chkDisableCapture.TabIndex = 5;
            this.chkDisableCapture.Text = "Disable Capturing";
            this.chkDisableCapture.UseVisualStyleBackColor = true;
            this.chkDisableCapture.CheckedChanged += new System.EventHandler(this.chkDisableCapture_CheckedChanged);
            // 
            // chkNoteAdded
            // 
            this.chkNoteAdded.AutoSize = true;
            this.chkNoteAdded.Location = new System.Drawing.Point(503, 10);
            this.chkNoteAdded.Name = "chkNoteAdded";
            this.chkNoteAdded.Size = new System.Drawing.Size(121, 19);
            this.chkNoteAdded.TabIndex = 6;
            this.chkNoteAdded.Text = "Records with note";
            this.chkNoteAdded.UseVisualStyleBackColor = true;
            this.chkNoteAdded.CheckedChanged += new System.EventHandler(this.chkNoteAdded_CheckedChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.chkNoteAdded);
            this.Controls.Add(this.chkDisableCapture);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbDateFilter);
            this.Controls.Add(this.dataGridView1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "ClipBoard History";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DataGridView dataGridView1;
        private NotifyIcon notifyIcon1;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem showToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ComboBox cmbDateFilter;
        private Label label1;
        private TextBox txtSearch;
        private Button btnSearch;
        private DataGridViewTextBoxColumn Id;
        private DataGridViewTextBoxColumn CreateDateWithDay;
        private DataGridViewTextBoxColumn Brief;
        private DataGridViewTextBoxColumn Note;
        private DataGridViewTextBoxColumn CBText;
        private DataGridViewTextBoxColumn CreateDate;
        private DataGridViewTextBoxColumn DayColor;
        private CheckBox chkNoteAdded;
        private ToolStripMenuItem disableStripMenuItem;
        private CheckBox chkDisableCapture;
    }
}