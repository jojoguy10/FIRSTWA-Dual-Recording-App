﻿namespace FIRSTWA_Recorder
{
    partial class YoutubeUpload
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
            this.txtProgramTitle = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtTags = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtWideTitle = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnCopyProgram = new System.Windows.Forms.Button();
            this.btnCopyWide = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtProgramTitle
            // 
            this.txtProgramTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtProgramTitle.Location = new System.Drawing.Point(150, 48);
            this.txtProgramTitle.Name = "txtProgramTitle";
            this.txtProgramTitle.Size = new System.Drawing.Size(389, 31);
            this.txtProgramTitle.TabIndex = 0;
            this.txtProgramTitle.Click += new System.EventHandler(this.TextBox_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(15, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(230, 32);
            this.label1.TabIndex = 6;
            this.label1.Text = "PROGRAM Title:";
            // 
            // txtDescription
            // 
            this.txtDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDescription.Location = new System.Drawing.Point(150, 102);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(389, 323);
            this.txtDescription.TabIndex = 2;
            this.txtDescription.Click += new System.EventHandler(this.TextBox_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(48, 195);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(166, 32);
            this.label2.TabIndex = 8;
            this.label2.Text = "Description:";
            // 
            // txtTags
            // 
            this.txtTags.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTags.Location = new System.Drawing.Point(150, 431);
            this.txtTags.Name = "txtTags";
            this.txtTags.Size = new System.Drawing.Size(389, 31);
            this.txtTags.TabIndex = 3;
            this.txtTags.Click += new System.EventHandler(this.TextBox_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(96, 431);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(86, 32);
            this.label3.TabIndex = 9;
            this.label3.Text = "Tags:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(12, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(286, 44);
            this.label4.TabIndex = 5;
            this.label4.Text = "Youtube Fields";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(55, 76);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(157, 32);
            this.label5.TabIndex = 7;
            this.label5.Text = "WIDE Title:";
            // 
            // txtWideTitle
            // 
            this.txtWideTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtWideTitle.Location = new System.Drawing.Point(150, 75);
            this.txtWideTitle.Name = "txtWideTitle";
            this.txtWideTitle.Size = new System.Drawing.Size(389, 31);
            this.txtWideTitle.TabIndex = 1;
            this.txtWideTitle.Click += new System.EventHandler(this.TextBox_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(150, 458);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(145, 31);
            this.btnCancel.TabIndex = 10;
            this.btnCancel.Text = "Done";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnCopyProgram
            // 
            this.btnCopyProgram.Location = new System.Drawing.Point(576, 48);
            this.btnCopyProgram.Name = "btnCopyProgram";
            this.btnCopyProgram.Size = new System.Drawing.Size(75, 70);
            this.btnCopyProgram.TabIndex = 11;
            this.btnCopyProgram.Text = "Copy Program Video";
            this.btnCopyProgram.UseVisualStyleBackColor = true;
            this.btnCopyProgram.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnCopyProgram_MouseDown);
            // 
            // btnCopyWide
            // 
            this.btnCopyWide.Location = new System.Drawing.Point(576, 145);
            this.btnCopyWide.Name = "btnCopyWide";
            this.btnCopyWide.Size = new System.Drawing.Size(75, 70);
            this.btnCopyWide.TabIndex = 12;
            this.btnCopyWide.Text = "Copy Wide Video";
            this.btnCopyWide.UseVisualStyleBackColor = true;
            this.btnCopyWide.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnCopyWide_MouseDown);
            // 
            // YoutubeUpload
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(690, 501);
            this.Controls.Add(this.btnCopyWide);
            this.Controls.Add(this.btnCopyProgram);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtTags);
            this.Controls.Add(this.txtWideTitle);
            this.Controls.Add(this.txtProgramTitle);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "YoutubeUpload";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "YoutubeUpload";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.YoutubeUpload_FormClosing);
            this.Load += new System.EventHandler(this.YoutubeUpload_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnCancel;
        public System.Windows.Forms.TextBox txtProgramTitle;
        public System.Windows.Forms.TextBox txtDescription;
        public System.Windows.Forms.TextBox txtTags;
        public System.Windows.Forms.TextBox txtWideTitle;
        private System.Windows.Forms.Button btnCopyProgram;
        private System.Windows.Forms.Button btnCopyWide;
    }
}