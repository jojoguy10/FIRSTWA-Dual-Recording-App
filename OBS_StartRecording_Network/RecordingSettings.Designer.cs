namespace FIRSTWA_Recorder
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
            this.label5 = new System.Windows.Forms.Label();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.txtIPAddressPROGRAM = new System.Windows.Forms.TextBox();
            this.txtIPAddressWIDE = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtIPAddressPC = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.numYear = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.numYear)).BeginInit();
            this.SuspendLayout();
            // 
            // btnAccept
            // 
            this.btnAccept.Location = new System.Drawing.Point(8, 122);
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
            this.btnCancel.Location = new System.Drawing.Point(165, 122);
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
            this.label1.Location = new System.Drawing.Point(12, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(103, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Program IP Address:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 63);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(89, 13);
            this.label5.TabIndex = 1;
            this.label5.Text = "Wide IP Address:";
            // 
            // txtIPAddressPROGRAM
            // 
            this.txtIPAddressPROGRAM.Location = new System.Drawing.Point(121, 36);
            this.txtIPAddressPROGRAM.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtIPAddressPROGRAM.Name = "txtIPAddressPROGRAM";
            this.txtIPAddressPROGRAM.Size = new System.Drawing.Size(121, 20);
            this.txtIPAddressPROGRAM.TabIndex = 9;
            this.txtIPAddressPROGRAM.Text = "192.168.100.35";
            // 
            // txtIPAddressWIDE
            // 
            this.txtIPAddressWIDE.Location = new System.Drawing.Point(121, 62);
            this.txtIPAddressWIDE.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtIPAddressWIDE.Name = "txtIPAddressWIDE";
            this.txtIPAddressWIDE.Size = new System.Drawing.Size(121, 20);
            this.txtIPAddressWIDE.TabIndex = 9;
            this.txtIPAddressWIDE.Text = "192.168.100.34";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 90);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "PC IP Address:";
            // 
            // txtIPAddressPC
            // 
            this.txtIPAddressPC.Location = new System.Drawing.Point(121, 88);
            this.txtIPAddressPC.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtIPAddressPC.Name = "txtIPAddressPC";
            this.txtIPAddressPC.Size = new System.Drawing.Size(121, 20);
            this.txtIPAddressPC.TabIndex = 9;
            this.txtIPAddressPC.Text = "192.168.100.70";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 11);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Year: ";
            // 
            // numYear
            // 
            this.numYear.Location = new System.Drawing.Point(121, 9);
            this.numYear.Maximum = new decimal(new int[] {
            3000,
            0,
            0,
            0});
            this.numYear.Minimum = new decimal(new int[] {
            2019,
            0,
            0,
            0});
            this.numYear.Name = "numYear";
            this.numYear.Size = new System.Drawing.Size(120, 20);
            this.numYear.TabIndex = 11;
            this.numYear.Value = new decimal(new int[] {
            2020,
            0,
            0,
            0});
            // 
            // RecordingSettings
            // 
            this.AcceptButton = this.btnAccept;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(248, 155);
            this.Controls.Add(this.numYear);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtIPAddressPC);
            this.Controls.Add(this.txtIPAddressWIDE);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtIPAddressPROGRAM);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnAccept);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "RecordingSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Settings";
            ((System.ComponentModel.ISupportInitialize)(this.numYear)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnAccept;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.TextBox txtIPAddressPROGRAM;
        private System.Windows.Forms.TextBox txtIPAddressWIDE;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtIPAddressPC;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numYear;
    }
}