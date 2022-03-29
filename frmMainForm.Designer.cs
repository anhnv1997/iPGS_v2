
namespace Kztek.iZCU
{
    partial class frmMainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMainForm));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.tsmi_System = new System.Windows.Forms.ToolStripMenuItem();
            this.tsbSetting = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.deviceTreeView1 = new Kztek.iZCU.DeviceTreeView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.dataGridViewEvent = new System.Windows.Forms.DataGridView();
            this.IndexColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DateTimeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CameraNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SlotChangeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ImageColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label6 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblEventDateTime = new System.Windows.Forms.Label();
            this.lblPlateNumber3 = new System.Windows.Forms.Label();
            this.lblPlateNumber2 = new System.Windows.Forms.Label();
            this.lblPlateNumber1 = new System.Windows.Forms.Label();
            this.lblEmptySlot = new System.Windows.Forms.Label();
            this.lblCameraName = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblEventType = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.timerStatus = new System.Windows.Forms.Timer(this.components);
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewEvent)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmi_System});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1184, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // tsmi_System
            // 
            this.tsmi_System.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbSetting});
            this.tsmi_System.Name = "tsmi_System";
            this.tsmi_System.Size = new System.Drawing.Size(69, 20);
            this.tsmi_System.Text = "Hệ thống";
            // 
            // tsbSetting
            // 
            this.tsbSetting.Image = ((System.Drawing.Image)(resources.GetObject("tsbSetting.Image")));
            this.tsbSetting.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.tsbSetting.Name = "tsbSetting";
            this.tsbSetting.Size = new System.Drawing.Size(170, 22);
            this.tsbSetting.Text = "Tham số hệ thống";
            this.tsbSetting.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.tsbSetting.Click += new System.EventHandler(this.TsmSetting_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 24);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.deviceTreeView1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.dataGridViewEvent);
            this.splitContainer1.Panel2.Controls.Add(this.label6);
            this.splitContainer1.Panel2.Controls.Add(this.panel1);
            this.splitContainer1.Size = new System.Drawing.Size(1184, 701);
            this.splitContainer1.SplitterDistance = 263;
            this.splitContainer1.TabIndex = 1;
            // 
            // deviceTreeView1
            // 
            this.deviceTreeView1.DeviceImage = 1;
            this.deviceTreeView1.DeviceImageError = 2;
            this.deviceTreeView1.DeviceRootImage = 0;
            this.deviceTreeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.deviceTreeView1.ImageIndex = 0;
            this.deviceTreeView1.ImageList = this.imageList1;
            this.deviceTreeView1.Location = new System.Drawing.Point(0, 0);
            this.deviceTreeView1.Name = "deviceTreeView1";
            this.deviceTreeView1.SelectedImageIndex = 0;
            this.deviceTreeView1.Size = new System.Drawing.Size(263, 701);
            this.deviceTreeView1.TabIndex = 0;
            this.deviceTreeView1.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.deviceTreeView1_NodeMouseDoubleClick);
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "home909.ico");
            this.imageList1.Images.SetKeyName(1, "Video_Camera.ico");
            this.imageList1.Images.SetKeyName(2, "error.ico");
            this.imageList1.Images.SetKeyName(3, "autos-icon 24.png");
            this.imageList1.Images.SetKeyName(4, "car-add-icon 24.png");
            // 
            // dataGridViewEvent
            // 
            this.dataGridViewEvent.AllowUserToAddRows = false;
            this.dataGridViewEvent.AllowUserToDeleteRows = false;
            this.dataGridViewEvent.AllowUserToResizeRows = false;
            this.dataGridViewEvent.BackgroundColor = System.Drawing.Color.White;
            this.dataGridViewEvent.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridViewEvent.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.IndexColumn,
            this.DateTimeColumn,
            this.CameraNameColumn,
            this.SlotChangeColumn,
            this.ImageColumn});
            this.dataGridViewEvent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewEvent.Location = new System.Drawing.Point(0, 332);
            this.dataGridViewEvent.Name = "dataGridViewEvent";
            this.dataGridViewEvent.RowHeadersVisible = false;
            this.dataGridViewEvent.RowTemplate.Height = 25;
            this.dataGridViewEvent.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewEvent.Size = new System.Drawing.Size(917, 369);
            this.dataGridViewEvent.TabIndex = 1;
            // 
            // IndexColumn
            // 
            this.IndexColumn.HeaderText = "STT";
            this.IndexColumn.Name = "IndexColumn";
            this.IndexColumn.Width = 50;
            // 
            // DateTimeColumn
            // 
            this.DateTimeColumn.HeaderText = "Thời gian";
            this.DateTimeColumn.Name = "DateTimeColumn";
            this.DateTimeColumn.Width = 150;
            // 
            // CameraNameColumn
            // 
            this.CameraNameColumn.HeaderText = "Camera";
            this.CameraNameColumn.Name = "CameraNameColumn";
            this.CameraNameColumn.Width = 120;
            // 
            // SlotChangeColumn
            // 
            this.SlotChangeColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.SlotChangeColumn.HeaderText = "Sự kiện";
            this.SlotChangeColumn.Name = "SlotChangeColumn";
            // 
            // ImageColumn
            // 
            this.ImageColumn.HeaderText = "Image";
            this.ImageColumn.Name = "ImageColumn";
            this.ImageColumn.Visible = false;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.DarkGreen;
            this.label6.Dock = System.Windows.Forms.DockStyle.Top;
            this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(0, 307);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(917, 25);
            this.label6.TabIndex = 2;
            this.label6.Text = "Sự kiện";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblEventDateTime);
            this.panel1.Controls.Add(this.lblPlateNumber3);
            this.panel1.Controls.Add(this.lblPlateNumber2);
            this.panel1.Controls.Add(this.lblPlateNumber1);
            this.panel1.Controls.Add(this.lblEmptySlot);
            this.panel1.Controls.Add(this.lblCameraName);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.lblEventType);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(917, 307);
            this.panel1.TabIndex = 0;
            // 
            // lblEventDateTime
            // 
            this.lblEventDateTime.AutoSize = true;
            this.lblEventDateTime.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblEventDateTime.Location = new System.Drawing.Point(382, 52);
            this.lblEventDateTime.Name = "lblEventDateTime";
            this.lblEventDateTime.Size = new System.Drawing.Size(157, 21);
            this.lblEventDateTime.TabIndex = 12;
            this.lblEventDateTime.Text = "2021-22-04 09:00:00";
            // 
            // lblPlateNumber3
            // 
            this.lblPlateNumber3.AutoSize = true;
            this.lblPlateNumber3.Location = new System.Drawing.Point(484, 272);
            this.lblPlateNumber3.Name = "lblPlateNumber3";
            this.lblPlateNumber3.Size = new System.Drawing.Size(16, 15);
            this.lblPlateNumber3.TabIndex = 11;
            this.lblPlateNumber3.Text = "...";
            // 
            // lblPlateNumber2
            // 
            this.lblPlateNumber2.AutoSize = true;
            this.lblPlateNumber2.Location = new System.Drawing.Point(484, 228);
            this.lblPlateNumber2.Name = "lblPlateNumber2";
            this.lblPlateNumber2.Size = new System.Drawing.Size(16, 15);
            this.lblPlateNumber2.TabIndex = 10;
            this.lblPlateNumber2.Text = "...";
            // 
            // lblPlateNumber1
            // 
            this.lblPlateNumber1.AutoSize = true;
            this.lblPlateNumber1.Location = new System.Drawing.Point(484, 184);
            this.lblPlateNumber1.Name = "lblPlateNumber1";
            this.lblPlateNumber1.Size = new System.Drawing.Size(16, 15);
            this.lblPlateNumber1.TabIndex = 9;
            this.lblPlateNumber1.Text = "...";
            // 
            // lblEmptySlot
            // 
            this.lblEmptySlot.AutoSize = true;
            this.lblEmptySlot.Location = new System.Drawing.Point(484, 140);
            this.lblEmptySlot.Name = "lblEmptySlot";
            this.lblEmptySlot.Size = new System.Drawing.Size(24, 15);
            this.lblEmptySlot.TabIndex = 8;
            this.lblEmptySlot.Text = "3/3";
            // 
            // lblCameraName
            // 
            this.lblCameraName.AutoSize = true;
            this.lblCameraName.Location = new System.Drawing.Point(484, 96);
            this.lblCameraName.Name = "lblCameraName";
            this.lblCameraName.Size = new System.Drawing.Size(57, 15);
            this.lblCameraName.TabIndex = 7;
            this.lblCameraName.Text = "Camera 1";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(382, 272);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(57, 15);
            this.label5.TabIndex = 6;
            this.label5.Text = "Biển số 3:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(382, 228);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 15);
            this.label4.TabIndex = 5;
            this.label4.Text = "Biển số 2:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(382, 184);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 15);
            this.label1.TabIndex = 4;
            this.label1.Text = "Biển số 1:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(382, 96);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(51, 15);
            this.label3.TabIndex = 3;
            this.label3.Text = "Camera:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(382, 140);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "Số chỗ trống:";
            // 
            // lblEventType
            // 
            this.lblEventType.AutoSize = true;
            this.lblEventType.BackColor = System.Drawing.Color.Crimson;
            this.lblEventType.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblEventType.ForeColor = System.Drawing.Color.White;
            this.lblEventType.Location = new System.Drawing.Point(385, 6);
            this.lblEventType.Name = "lblEventType";
            this.lblEventType.Size = new System.Drawing.Size(114, 25);
            this.lblEventType.TabIndex = 1;
            this.lblEventType.Text = "THÔNG TIN";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(357, 307);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 725);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1184, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // timerStatus
            // 
            this.timerStatus.Interval = 1000;
            this.timerStatus.Tick += new System.EventHandler(this.timerStatus_Tick);
            // 
            // frmMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1184, 747);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.statusStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "frmMainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Kztek.iZCU";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMainForm_FormClosing);
            this.Load += new System.EventHandler(this.frmMainForm_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewEvent)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem tsmi_System;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lblEventType;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblPlateNumber3;
        private System.Windows.Forms.Label lblPlateNumber2;
        private System.Windows.Forms.Label lblPlateNumber1;
        private System.Windows.Forms.Label lblEmptySlot;
        private System.Windows.Forms.Label lblCameraName;
        private System.Windows.Forms.DataGridView dataGridViewEvent;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblEventDateTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn IndexColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn DateTimeColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn CameraNameColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn SlotChangeColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ImageColumn;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Timer timerStatus;
        private System.Windows.Forms.ToolStripMenuItem tsmSetting;
        private DeviceTreeView deviceTreeView1;
        private System.Windows.Forms.ToolStripMenuItem tsbSetting;
    }
}

