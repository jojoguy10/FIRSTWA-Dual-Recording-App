namespace OBS_StartRecording_Network
{
    partial class RecordingSettings
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
            this.btnAccept = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtIPAddressPROGRAM = new IPAddressControlLib.IPAddressControl();
            this.label2 = new System.Windows.Forms.Label();
            this.numPort1 = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.numPort2 = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.MaskedTextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtIPAddressWIDE = new IPAddressControlLib.IPAddressControl();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.label6 = new System.Windows.Forms.Label();
            this.txtFolderLocation = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numPort1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPort2)).BeginInit();
            this.SuspendLayout();
            // 
            // btnAccept
            // 
            this.btnAccept.Location = new System.Drawing.Point(12, 172);
            this.btnAccept.Name = "btnAccept";
            this.btnAccept.Size = new System.Drawing.Size(75, 23);
            this.btnAccept.TabIndex = 3;
            this.btnAccept.Text = "Accept";
            this.btnAccept.UseVisualStyleBackColor = true;
            this.btnAccept.Click += new System.EventHandler(this.btnAccept_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(167, 172);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(103, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Program IP Address:";
            // 
            // txtIPAddressPROGRAM
            // 
            this.txtIPAddressPROGRAM.AllowInternalTab = false;
            this.txtIPAddressPROGRAM.AutoHeight = true;
            this.txtIPAddressPROGRAM.BackColor = System.Drawing.SystemColors.Window;
            this.txtIPAddressPROGRAM.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.txtIPAddressPROGRAM.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtIPAddressPROGRAM.Location = new System.Drawing.Point(121, 6);
            this.txtIPAddressPROGRAM.MinimumSize = new System.Drawing.Size(87, 20);
            this.txtIPAddressPROGRAM.Name = "txtIPAddressPROGRAM";
            this.txtIPAddressPROGRAM.ReadOnly = false;
            this.txtIPAddressPROGRAM.Size = new System.Drawing.Size(121, 20);
            this.txtIPAddressPROGRAM.TabIndex = 0;
            this.txtIPAddressPROGRAM.Text = "192.168.0.104";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 86);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Program Port:";
            // 
            // numPort1
            // 
            this.numPort1.Location = new System.Drawing.Point(121, 84);
            this.numPort1.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.numPort1.Name = "numPort1";
            this.numPort1.Size = new System.Drawing.Size(121, 20);
            this.numPort1.TabIndex = 1;
            this.numPort1.Value = new decimal(new int[] {
            4444,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 112);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Wide Port:";
            // 
            // numPort2
            // 
            this.numPort2.Location = new System.Drawing.Point(121, 110);
            this.numPort2.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.numPort2.Name = "numPort2";
            this.numPort2.Size = new System.Drawing.Size(121, 20);
            this.numPort2.TabIndex = 2;
            this.numPort2.Value = new decimal(new int[] {
            4445,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 61);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(81, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "OBS Password:";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(121, 58);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(121, 20);
            this.txtPassword.TabIndex = 5;
            this.txtPassword.Text = "password";
            this.txtPassword.Click += new System.EventHandler(this.txtPassword_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 35);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(89, 13);
            this.label5.TabIndex = 1;
            this.label5.Text = "Wide IP Address:";
            // 
            // txtIPAddressWIDE
            // 
            this.txtIPAddressWIDE.AllowInternalTab = false;
            this.txtIPAddressWIDE.AutoHeight = true;
            this.txtIPAddressWIDE.BackColor = System.Drawing.SystemColors.Window;
            this.txtIPAddressWIDE.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.txtIPAddressWIDE.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtIPAddressWIDE.Location = new System.Drawing.Point(121, 32);
            this.txtIPAddressWIDE.MinimumSize = new System.Drawing.Size(87, 20);
            this.txtIPAddressWIDE.Name = "txtIPAddressWIDE";
            this.txtIPAddressWIDE.ReadOnly = false;
            this.txtIPAddressWIDE.Size = new System.Drawing.Size(121, 20);
            this.txtIPAddressWIDE.TabIndex = 0;
            this.txtIPAddressWIDE.Text = "192.168.0.103";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 139);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(91, 13);
            this.label6.TabIndex = 6;
            this.label6.Text = "Recording Folder:";
            // 
            // txtFolderLocation
            // 
            this.txtFolderLocation.Enabled = false;
            this.txtFolderLocation.Location = new System.Drawing.Point(121, 136);
            this.txtFolderLocation.Name = "txtFolderLocation";
            this.txtFolderLocation.Size = new System.Drawing.Size(86, 20);
            this.txtFolderLocation.TabIndex = 7;
            this.txtFolderLocation.Text = "D:\\__USER\\Videos\\VM Captures";
            // 
            // button1
            // 
            this.button1.Enabled = false;
            this.button1.Location = new System.Drawing.Point(213, 134);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(29, 23);
            this.button1.TabIndex = 8;
            this.button1.Text = "...";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // RecordingSettings
            // 
            this.AcceptButton = this.btnAccept;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(248, 208);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txtFolderLocation);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.numPort2);
            this.Controls.Add(this.numPort1);
            this.Controls.Add(this.txtIPAddressWIDE);
            this.Controls.Add(this.txtIPAddressPROGRAM);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnAccept);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "RecordingSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Settings";
            ((System.ComponentModel.ISupportInitialize)(this.numPort1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPort2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnAccept;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label1;
        private IPAddressControlLib.IPAddressControl txtIPAddressPROGRAM;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numPort1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numPort2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.MaskedTextBox txtPassword;
        private System.Windows.Forms.Label label5;
        private IPAddressControlLib.IPAddressControl txtIPAddressWIDE;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtFolderLocation;
        private System.Windows.Forms.Button button1;
    }
}