namespace FIRSTWA_Recorder
{
    partial class AudioSettings
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
            this.rdoProgramStereo = new System.Windows.Forms.RadioButton();
            this.rdoProgLeft = new System.Windows.Forms.RadioButton();
            this.rdoProgRight = new System.Windows.Forms.RadioButton();
            this.rdoWideRight = new System.Windows.Forms.RadioButton();
            this.rdoWideLeft = new System.Windows.Forms.RadioButton();
            this.rdoWideStereo = new System.Windows.Forms.RadioButton();
            this.btnAccept = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.wideGroup = new System.Windows.Forms.GroupBox();
            this.programGroup = new System.Windows.Forms.GroupBox();
            this.wideGroup.SuspendLayout();
            this.programGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // rdoProgramStereo
            // 
            this.rdoProgramStereo.AutoSize = true;
            this.rdoProgramStereo.Location = new System.Drawing.Point(6, 19);
            this.rdoProgramStereo.Name = "rdoProgramStereo";
            this.rdoProgramStereo.Size = new System.Drawing.Size(56, 17);
            this.rdoProgramStereo.TabIndex = 2;
            this.rdoProgramStereo.TabStop = true;
            this.rdoProgramStereo.Text = "Stereo";
            this.rdoProgramStereo.UseVisualStyleBackColor = true;
            // 
            // rdoProgLeft
            // 
            this.rdoProgLeft.AutoSize = true;
            this.rdoProgLeft.Location = new System.Drawing.Point(77, 19);
            this.rdoProgLeft.Name = "rdoProgLeft";
            this.rdoProgLeft.Size = new System.Drawing.Size(73, 17);
            this.rdoProgLeft.TabIndex = 3;
            this.rdoProgLeft.TabStop = true;
            this.rdoProgLeft.Text = "Left Mono";
            this.rdoProgLeft.UseVisualStyleBackColor = true;
            // 
            // rdoProgRight
            // 
            this.rdoProgRight.AutoSize = true;
            this.rdoProgRight.Location = new System.Drawing.Point(156, 19);
            this.rdoProgRight.Name = "rdoProgRight";
            this.rdoProgRight.Size = new System.Drawing.Size(80, 17);
            this.rdoProgRight.TabIndex = 4;
            this.rdoProgRight.TabStop = true;
            this.rdoProgRight.Text = "Right Mono";
            this.rdoProgRight.UseVisualStyleBackColor = true;
            // 
            // rdoWideRight
            // 
            this.rdoWideRight.AutoSize = true;
            this.rdoWideRight.Location = new System.Drawing.Point(156, 19);
            this.rdoWideRight.Name = "rdoWideRight";
            this.rdoWideRight.Size = new System.Drawing.Size(80, 17);
            this.rdoWideRight.TabIndex = 7;
            this.rdoWideRight.TabStop = true;
            this.rdoWideRight.Text = "Right Mono";
            this.rdoWideRight.UseVisualStyleBackColor = true;
            // 
            // rdoWideLeft
            // 
            this.rdoWideLeft.AutoSize = true;
            this.rdoWideLeft.Location = new System.Drawing.Point(77, 19);
            this.rdoWideLeft.Name = "rdoWideLeft";
            this.rdoWideLeft.Size = new System.Drawing.Size(73, 17);
            this.rdoWideLeft.TabIndex = 6;
            this.rdoWideLeft.TabStop = true;
            this.rdoWideLeft.Text = "Left Mono";
            this.rdoWideLeft.UseVisualStyleBackColor = true;
            // 
            // rdoWideStereo
            // 
            this.rdoWideStereo.AutoSize = true;
            this.rdoWideStereo.Location = new System.Drawing.Point(6, 19);
            this.rdoWideStereo.Name = "rdoWideStereo";
            this.rdoWideStereo.Size = new System.Drawing.Size(56, 17);
            this.rdoWideStereo.TabIndex = 5;
            this.rdoWideStereo.TabStop = true;
            this.rdoWideStereo.Text = "Stereo";
            this.rdoWideStereo.UseVisualStyleBackColor = true;
            // 
            // btnAccept
            // 
            this.btnAccept.Location = new System.Drawing.Point(12, 120);
            this.btnAccept.Name = "btnAccept";
            this.btnAccept.Size = new System.Drawing.Size(75, 23);
            this.btnAccept.TabIndex = 8;
            this.btnAccept.Text = "Accept";
            this.btnAccept.UseVisualStyleBackColor = true;
            this.btnAccept.Click += new System.EventHandler(this.btnAccept_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(182, 120);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // wideGroup
            // 
            this.wideGroup.Controls.Add(this.rdoWideStereo);
            this.wideGroup.Controls.Add(this.rdoWideLeft);
            this.wideGroup.Controls.Add(this.rdoWideRight);
            this.wideGroup.Location = new System.Drawing.Point(12, 66);
            this.wideGroup.Name = "wideGroup";
            this.wideGroup.Size = new System.Drawing.Size(245, 48);
            this.wideGroup.TabIndex = 10;
            this.wideGroup.TabStop = false;
            this.wideGroup.Text = "Wide Audio";
            // 
            // programGroup
            // 
            this.programGroup.Controls.Add(this.rdoProgramStereo);
            this.programGroup.Controls.Add(this.rdoProgLeft);
            this.programGroup.Controls.Add(this.rdoProgRight);
            this.programGroup.Location = new System.Drawing.Point(12, 12);
            this.programGroup.Name = "programGroup";
            this.programGroup.Size = new System.Drawing.Size(245, 48);
            this.programGroup.TabIndex = 11;
            this.programGroup.TabStop = false;
            this.programGroup.Text = "Program Audio";
            // 
            // AudioSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(270, 153);
            this.Controls.Add(this.programGroup);
            this.Controls.Add(this.wideGroup);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnAccept);
            this.Name = "AudioSettings";
            this.Text = "AudioSettings";
            this.wideGroup.ResumeLayout(false);
            this.wideGroup.PerformLayout();
            this.programGroup.ResumeLayout(false);
            this.programGroup.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.RadioButton rdoProgramStereo;
        private System.Windows.Forms.RadioButton rdoProgLeft;
        private System.Windows.Forms.RadioButton rdoProgRight;
        private System.Windows.Forms.RadioButton rdoWideRight;
        private System.Windows.Forms.RadioButton rdoWideLeft;
        private System.Windows.Forms.RadioButton rdoWideStereo;
        private System.Windows.Forms.Button btnAccept;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox wideGroup;
        private System.Windows.Forms.GroupBox programGroup;
    }
}