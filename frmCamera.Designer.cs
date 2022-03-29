
namespace Kztek.iZCU
{
    partial class frmCamera
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCamera));
            this.label1 = new System.Windows.Forms.Label();
            this.tbID = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbCameraName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbCameraIP = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbCameraPort = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tbCameraUsername = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tbCameraPassword = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.ButtonCancel = new System.Windows.Forms.Button();
            this.cbCameraType = new System.Windows.Forms.ComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(39, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(18, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "ID";
            // 
            // tbID
            // 
            this.tbID.Enabled = false;
            this.tbID.Location = new System.Drawing.Point(132, 18);
            this.tbID.Name = "tbID";
            this.tbID.Size = new System.Drawing.Size(140, 23);
            this.tbID.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(39, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "Loại";
            // 
            // tbCameraName
            // 
            this.tbCameraName.Location = new System.Drawing.Point(132, 76);
            this.tbCameraName.Name = "tbCameraName";
            this.tbCameraName.Size = new System.Drawing.Size(140, 23);
            this.tbCameraName.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(39, 79);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(25, 15);
            this.label3.TabIndex = 4;
            this.label3.Text = "Tên";
            // 
            // tbCameraIP
            // 
            this.tbCameraIP.Location = new System.Drawing.Point(132, 105);
            this.tbCameraIP.Name = "tbCameraIP";
            this.tbCameraIP.Size = new System.Drawing.Size(140, 23);
            this.tbCameraIP.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(39, 108);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(17, 15);
            this.label4.TabIndex = 6;
            this.label4.Text = "IP";
            // 
            // tbCameraPort
            // 
            this.tbCameraPort.Location = new System.Drawing.Point(132, 134);
            this.tbCameraPort.Name = "tbCameraPort";
            this.tbCameraPort.Size = new System.Drawing.Size(140, 23);
            this.tbCameraPort.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(39, 137);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(36, 15);
            this.label5.TabIndex = 8;
            this.label5.Text = "Cổng";
            // 
            // tbCameraUsername
            // 
            this.tbCameraUsername.Location = new System.Drawing.Point(132, 163);
            this.tbCameraUsername.Name = "tbCameraUsername";
            this.tbCameraUsername.Size = new System.Drawing.Size(140, 23);
            this.tbCameraUsername.TabIndex = 11;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(39, 166);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(85, 15);
            this.label6.TabIndex = 10;
            this.label6.Text = "Tên đăng nhập";
            // 
            // tbCameraPassword
            // 
            this.tbCameraPassword.Location = new System.Drawing.Point(132, 192);
            this.tbCameraPassword.Name = "tbCameraPassword";
            this.tbCameraPassword.Size = new System.Drawing.Size(140, 23);
            this.tbCameraPassword.TabIndex = 13;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(39, 195);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(57, 15);
            this.label7.TabIndex = 12;
            this.label7.Text = "Mật khẩu";
            // 
            // btnSave
            // 
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSave.Location = new System.Drawing.Point(161, 256);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 29);
            this.btnSave.TabIndex = 14;
            this.btnSave.Text = "Lưu lại";
            this.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // ButtonCancel
            // 
            this.ButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ButtonCancel.Image = ((System.Drawing.Image)(resources.GetObject("ButtonCancel.Image")));
            this.ButtonCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ButtonCancel.Location = new System.Drawing.Point(248, 256);
            this.ButtonCancel.Name = "ButtonCancel";
            this.ButtonCancel.Size = new System.Drawing.Size(75, 29);
            this.ButtonCancel.TabIndex = 15;
            this.ButtonCancel.Text = "Đóng";
            this.ButtonCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ButtonCancel.UseVisualStyleBackColor = true;
            this.ButtonCancel.Click += new System.EventHandler(this.ButtonCancel_Click);
            // 
            // cbCameraType
            // 
            this.cbCameraType.FormattingEnabled = true;
            this.cbCameraType.Items.AddRange(new object[] {
            "HIKVision",
            "Dahua"});
            this.cbCameraType.Location = new System.Drawing.Point(132, 47);
            this.cbCameraType.Name = "cbCameraType";
            this.cbCameraType.Size = new System.Drawing.Size(140, 23);
            this.cbCameraType.TabIndex = 16;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.cbCameraType);
            this.panel1.Controls.Add(this.tbID);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.tbCameraPassword);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.tbCameraName);
            this.panel1.Controls.Add(this.tbCameraUsername);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.tbCameraIP);
            this.panel1.Controls.Add(this.tbCameraPort);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(311, 238);
            this.panel1.TabIndex = 17;
            // 
            // frmCamera
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(344, 290);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.ButtonCancel);
            this.Controls.Add(this.btnSave);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmCamera";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Camera";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbCameraName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbCameraIP;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbCameraPort;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbCameraUsername;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tbCameraPassword;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button ButtonCancel;
        private System.Windows.Forms.ComboBox cbCameraType;
        protected System.Windows.Forms.TextBox tbID;
        protected System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Panel panel1;
    }
}