namespace OBS_StartRecording_Network
{
    partial class Settings
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
            this.txtIPAddress = new IPAddressControlLib.IPAddressControl();
            this.label2 = new System.Windows.Forms.Label();
            this.numPort1 = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.numPort2 = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.MaskedTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.numPort1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPort2)).BeginInit();
            this.SuspendLayout();
            // 
            // btnAccept
            // 
            this.btnAccept.Location = new System.Drawing.Point(9, 110);
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
            this.btnCancel.Location = new System.Drawing.Point(116, 110);
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
            this.label1.Size = new System.Drawing.Size(86, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "OBS IP Address:";
            // 
            // txtIPAddress
            // 
            this.txtIPAddress.AllowInternalTab = false;
            this.txtIPAddress.AutoHeight = true;
            this.txtIPAddress.BackColor = System.Drawing.SystemColors.Window;
            this.txtIPAddress.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.txtIPAddress.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtIPAddress.Location = new System.Drawing.Point(104, 6);
            this.txtIPAddress.MinimumSize = new System.Drawing.Size(87, 20);
            this.txtIPAddress.Name = "txtIPAddress";
            this.txtIPAddress.ReadOnly = false;
            this.txtIPAddress.Size = new System.Drawing.Size(87, 20);
            this.txtIPAddress.TabIndex = 0;
            this.txtIPAddress.Text = "192.168.100.10";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "OBS Port #1:";
            // 
            // numPort1
            // 
            this.numPort1.Location = new System.Drawing.Point(104, 58);
            this.numPort1.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.numPort1.Name = "numPort1";
            this.numPort1.Size = new System.Drawing.Size(87, 20);
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
            this.label3.Location = new System.Drawing.Point(12, 86);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "OBS Port #2:";
            // 
            // numPort2
            // 
            this.numPort2.Location = new System.Drawing.Point(104, 84);
            this.numPort2.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.numPort2.Name = "numPort2";
            this.numPort2.Size = new System.Drawing.Size(87, 20);
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
            this.label4.Location = new System.Drawing.Point(12, 35);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(81, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "OBS Password:";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(104, 32);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(87, 20);
            this.txtPassword.TabIndex = 5;
            this.txtPassword.Text = "password";
            this.txtPassword.Click += new System.EventHandler(this.txtPassword_Click);
            // 
            // Settings
            // 
            this.AcceptButton = this.btnAccept;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(200, 142);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.numPort2);
            this.Controls.Add(this.numPort1);
            this.Controls.Add(this.txtIPAddress);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnAccept);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Settings";
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
        private IPAddressControlLib.IPAddressControl txtIPAddress;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numPort1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numPort2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.MaskedTextBox txtPassword;
    }
}