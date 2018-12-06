namespace OBS_StartRecording_Network
{
    partial class frmMain
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
            this.components = new System.ComponentModel.Container();
            this.btnStartRecording = new System.Windows.Forms.Button();
            this.btnStopRecording = new System.Windows.Forms.Button();
            this.radioBtnQual = new System.Windows.Forms.RadioButton();
            this.radioBtnQuarter = new System.Windows.Forms.RadioButton();
            this.radioBtnSemi = new System.Windows.Forms.RadioButton();
            this.radioBtnFinal = new System.Windows.Forms.RadioButton();
            this.groupMatchTypes = new System.Windows.Forms.GroupBox();
            this.radioBtnPractice = new System.Windows.Forms.RadioButton();
            this.numMatchNumber = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.recordingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uploadsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.streamingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label3 = new System.Windows.Forms.Label();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.chkReplay = new System.Windows.Forms.CheckBox();
            this.numReplayNumber = new System.Windows.Forms.NumericUpDown();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.groupEvent = new System.Windows.Forms.GroupBox();
            this.comboEventName = new System.Windows.Forms.ComboBox();
            this.chkRecordWide = new System.Windows.Forms.CheckBox();
            this.chkProgramRecord = new System.Windows.Forms.CheckBox();
            this.groupMatch = new System.Windows.Forms.GroupBox();
            this.timerElapsed = new System.Windows.Forms.Timer(this.components);
            this.lblElapsedTime = new System.Windows.Forms.Label();
            this.btnConnectProgram = new System.Windows.Forms.Button();
            this.btnConnectWide = new System.Windows.Forms.Button();
            this.groupStatus = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.ledRecordWIDE = new System.Windows.Forms.Panel();
            this.ledRecordPROGRAM = new System.Windows.Forms.Panel();
            this.btnOpenRecordings = new System.Windows.Forms.Button();
            this.btnUpload = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.groupOBS = new System.Windows.Forms.GroupBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.lblProgramPath = new System.Windows.Forms.LinkLabel();
            this.lblWidePath = new System.Windows.Forms.LinkLabel();
            this.groupMatchTypes.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMatchNumber)).BeginInit();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numReplayNumber)).BeginInit();
            this.groupEvent.SuspendLayout();
            this.groupMatch.SuspendLayout();
            this.groupStatus.SuspendLayout();
            this.groupOBS.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnStartRecording
            // 
            this.btnStartRecording.Location = new System.Drawing.Point(12, 285);
            this.btnStartRecording.Name = "btnStartRecording";
            this.btnStartRecording.Size = new System.Drawing.Size(182, 52);
            this.btnStartRecording.TabIndex = 0;
            this.btnStartRecording.Text = "Start Recording";
            this.btnStartRecording.UseVisualStyleBackColor = true;
            this.btnStartRecording.Click += new System.EventHandler(this.btnStartRecording_Click);
            // 
            // btnStopRecording
            // 
            this.btnStopRecording.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnStopRecording.Enabled = false;
            this.btnStopRecording.Location = new System.Drawing.Point(221, 285);
            this.btnStopRecording.Name = "btnStopRecording";
            this.btnStopRecording.Size = new System.Drawing.Size(182, 52);
            this.btnStopRecording.TabIndex = 0;
            this.btnStopRecording.Text = "Stop Recording";
            this.btnStopRecording.UseVisualStyleBackColor = true;
            this.btnStopRecording.Click += new System.EventHandler(this.btnStopRecording_Click);
            // 
            // radioBtnQual
            // 
            this.radioBtnQual.AutoSize = true;
            this.radioBtnQual.Location = new System.Drawing.Point(76, 19);
            this.radioBtnQual.Name = "radioBtnQual";
            this.radioBtnQual.Size = new System.Drawing.Size(83, 17);
            this.radioBtnQual.TabIndex = 2;
            this.radioBtnQual.Text = "Qualification";
            this.radioBtnQual.UseVisualStyleBackColor = true;
            this.radioBtnQual.CheckedChanged += new System.EventHandler(this.radioBtnMatchType_CheckedChanged);
            // 
            // radioBtnQuarter
            // 
            this.radioBtnQuarter.AutoSize = true;
            this.radioBtnQuarter.Location = new System.Drawing.Point(165, 19);
            this.radioBtnQuarter.Name = "radioBtnQuarter";
            this.radioBtnQuarter.Size = new System.Drawing.Size(79, 17);
            this.radioBtnQuarter.TabIndex = 2;
            this.radioBtnQuarter.Text = "Quarterfinal";
            this.radioBtnQuarter.UseVisualStyleBackColor = true;
            this.radioBtnQuarter.CheckedChanged += new System.EventHandler(this.radioBtnMatchType_CheckedChanged);
            // 
            // radioBtnSemi
            // 
            this.radioBtnSemi.AutoSize = true;
            this.radioBtnSemi.Location = new System.Drawing.Point(250, 19);
            this.radioBtnSemi.Name = "radioBtnSemi";
            this.radioBtnSemi.Size = new System.Drawing.Size(67, 17);
            this.radioBtnSemi.TabIndex = 2;
            this.radioBtnSemi.Text = "Semifinal";
            this.radioBtnSemi.UseVisualStyleBackColor = true;
            this.radioBtnSemi.CheckedChanged += new System.EventHandler(this.radioBtnMatchType_CheckedChanged);
            // 
            // radioBtnFinal
            // 
            this.radioBtnFinal.AutoSize = true;
            this.radioBtnFinal.Location = new System.Drawing.Point(323, 19);
            this.radioBtnFinal.Name = "radioBtnFinal";
            this.radioBtnFinal.Size = new System.Drawing.Size(47, 17);
            this.radioBtnFinal.TabIndex = 2;
            this.radioBtnFinal.Text = "Final";
            this.radioBtnFinal.UseVisualStyleBackColor = true;
            this.radioBtnFinal.CheckedChanged += new System.EventHandler(this.radioBtnMatchType_CheckedChanged);
            // 
            // groupMatchTypes
            // 
            this.groupMatchTypes.Controls.Add(this.radioBtnPractice);
            this.groupMatchTypes.Controls.Add(this.radioBtnQual);
            this.groupMatchTypes.Controls.Add(this.radioBtnFinal);
            this.groupMatchTypes.Controls.Add(this.radioBtnSemi);
            this.groupMatchTypes.Controls.Add(this.radioBtnQuarter);
            this.groupMatchTypes.Location = new System.Drawing.Point(6, 19);
            this.groupMatchTypes.Name = "groupMatchTypes";
            this.groupMatchTypes.Size = new System.Drawing.Size(378, 45);
            this.groupMatchTypes.TabIndex = 3;
            this.groupMatchTypes.TabStop = false;
            this.groupMatchTypes.Text = "Match Type";
            // 
            // radioBtnPractice
            // 
            this.radioBtnPractice.AutoSize = true;
            this.radioBtnPractice.Checked = true;
            this.radioBtnPractice.Location = new System.Drawing.Point(6, 19);
            this.radioBtnPractice.Name = "radioBtnPractice";
            this.radioBtnPractice.Size = new System.Drawing.Size(64, 17);
            this.radioBtnPractice.TabIndex = 2;
            this.radioBtnPractice.TabStop = true;
            this.radioBtnPractice.Text = "Practice";
            this.radioBtnPractice.UseVisualStyleBackColor = true;
            this.radioBtnPractice.CheckedChanged += new System.EventHandler(this.radioBtnMatchType_CheckedChanged);
            // 
            // numMatchNumber
            // 
            this.numMatchNumber.Location = new System.Drawing.Point(92, 65);
            this.numMatchNumber.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.numMatchNumber.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numMatchNumber.Name = "numMatchNumber";
            this.numMatchNumber.Size = new System.Drawing.Size(52, 20);
            this.numMatchNumber.TabIndex = 6;
            this.numMatchNumber.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Match Number:";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.settingsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(579, 24);
            this.menuStrip1.TabIndex = 8;
            this.menuStrip1.Text = "Menu Strip";
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.recordingToolStripMenuItem,
            this.uploadsToolStripMenuItem,
            this.streamingToolStripMenuItem});
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.settingsToolStripMenuItem.Text = "Settings";
            // 
            // recordingToolStripMenuItem
            // 
            this.recordingToolStripMenuItem.Name = "recordingToolStripMenuItem";
            this.recordingToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.recordingToolStripMenuItem.Text = "Recording";
            this.recordingToolStripMenuItem.Click += new System.EventHandler(this.recordingToolStripMenuItem_Click);
            // 
            // uploadsToolStripMenuItem
            // 
            this.uploadsToolStripMenuItem.Name = "uploadsToolStripMenuItem";
            this.uploadsToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.uploadsToolStripMenuItem.Text = "Uploading";
            this.uploadsToolStripMenuItem.Click += new System.EventHandler(this.uploadsToolStripMenuItem_Click);
            // 
            // streamingToolStripMenuItem
            // 
            this.streamingToolStripMenuItem.Name = "streamingToolStripMenuItem";
            this.streamingToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.streamingToolStripMenuItem.Text = "Streaming";
            this.streamingToolStripMenuItem.Click += new System.EventHandler(this.streamingToolStripMenuItem_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 28);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Event Name:";
            // 
            // chkReplay
            // 
            this.chkReplay.AutoSize = true;
            this.chkReplay.Location = new System.Drawing.Point(256, 66);
            this.chkReplay.Name = "chkReplay";
            this.chkReplay.Size = new System.Drawing.Size(59, 17);
            this.chkReplay.TabIndex = 11;
            this.chkReplay.Text = "Replay";
            this.chkReplay.UseVisualStyleBackColor = true;
            this.chkReplay.CheckedChanged += new System.EventHandler(this.chkReplay_CheckedChanged);
            // 
            // numReplayNumber
            // 
            this.numReplayNumber.Location = new System.Drawing.Point(321, 65);
            this.numReplayNumber.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numReplayNumber.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numReplayNumber.Name = "numReplayNumber";
            this.numReplayNumber.Size = new System.Drawing.Size(52, 20);
            this.numReplayNumber.TabIndex = 6;
            this.numReplayNumber.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numReplayNumber.ValueChanged += new System.EventHandler(this.numReplayNumber_ValueChanged);
            // 
            // folderBrowserDialog1
            // 
            this.folderBrowserDialog1.Description = "Storage location for recordings";
            // 
            // groupEvent
            // 
            this.groupEvent.Controls.Add(this.comboEventName);
            this.groupEvent.Controls.Add(this.chkRecordWide);
            this.groupEvent.Controls.Add(this.chkProgramRecord);
            this.groupEvent.Controls.Add(this.label3);
            this.groupEvent.Location = new System.Drawing.Point(12, 100);
            this.groupEvent.Name = "groupEvent";
            this.groupEvent.Size = new System.Drawing.Size(391, 77);
            this.groupEvent.TabIndex = 12;
            this.groupEvent.TabStop = false;
            this.groupEvent.Text = "Event Specific";
            // 
            // comboEventName
            // 
            this.comboEventName.FormattingEnabled = true;
            this.comboEventName.Location = new System.Drawing.Point(85, 25);
            this.comboEventName.Name = "comboEventName";
            this.comboEventName.Size = new System.Drawing.Size(247, 21);
            this.comboEventName.TabIndex = 12;
            this.comboEventName.SelectedIndexChanged += new System.EventHandler(this.comboEventName_SelectedIndexChanged);
            // 
            // chkRecordWide
            // 
            this.chkRecordWide.AutoSize = true;
            this.chkRecordWide.Checked = true;
            this.chkRecordWide.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkRecordWide.Location = new System.Drawing.Point(118, 54);
            this.chkRecordWide.Name = "chkRecordWide";
            this.chkRecordWide.Size = new System.Drawing.Size(89, 17);
            this.chkRecordWide.TabIndex = 11;
            this.chkRecordWide.Text = "Record Wide";
            this.chkRecordWide.UseVisualStyleBackColor = true;
            // 
            // chkProgramRecord
            // 
            this.chkProgramRecord.AutoSize = true;
            this.chkProgramRecord.Checked = true;
            this.chkProgramRecord.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkProgramRecord.Location = new System.Drawing.Point(9, 54);
            this.chkProgramRecord.Name = "chkProgramRecord";
            this.chkProgramRecord.Size = new System.Drawing.Size(103, 17);
            this.chkProgramRecord.TabIndex = 11;
            this.chkProgramRecord.Text = "Record Program";
            this.chkProgramRecord.UseVisualStyleBackColor = true;
            // 
            // groupMatch
            // 
            this.groupMatch.Controls.Add(this.groupMatchTypes);
            this.groupMatch.Controls.Add(this.numMatchNumber);
            this.groupMatch.Controls.Add(this.chkReplay);
            this.groupMatch.Controls.Add(this.numReplayNumber);
            this.groupMatch.Controls.Add(this.label2);
            this.groupMatch.Location = new System.Drawing.Point(12, 183);
            this.groupMatch.Name = "groupMatch";
            this.groupMatch.Size = new System.Drawing.Size(391, 96);
            this.groupMatch.TabIndex = 13;
            this.groupMatch.TabStop = false;
            this.groupMatch.Text = "Match Specific";
            // 
            // timerElapsed
            // 
            this.timerElapsed.Interval = 10;
            this.timerElapsed.Tick += new System.EventHandler(this.timerElapsed_Tick);
            // 
            // lblElapsedTime
            // 
            this.lblElapsedTime.AutoSize = true;
            this.lblElapsedTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblElapsedTime.Location = new System.Drawing.Point(140, 342);
            this.lblElapsedTime.Name = "lblElapsedTime";
            this.lblElapsedTime.Size = new System.Drawing.Size(135, 29);
            this.lblElapsedTime.TabIndex = 14;
            this.lblElapsedTime.Text = "00:00:00.00";
            // 
            // btnConnectProgram
            // 
            this.btnConnectProgram.Location = new System.Drawing.Point(6, 19);
            this.btnConnectProgram.Name = "btnConnectProgram";
            this.btnConnectProgram.Size = new System.Drawing.Size(176, 26);
            this.btnConnectProgram.TabIndex = 15;
            this.btnConnectProgram.Text = "Connect to Program OBS";
            this.btnConnectProgram.UseVisualStyleBackColor = true;
            this.btnConnectProgram.Click += new System.EventHandler(this.btnConnectProgram_Click);
            // 
            // btnConnectWide
            // 
            this.btnConnectWide.Location = new System.Drawing.Point(209, 19);
            this.btnConnectWide.Name = "btnConnectWide";
            this.btnConnectWide.Size = new System.Drawing.Size(182, 26);
            this.btnConnectWide.TabIndex = 16;
            this.btnConnectWide.Text = "Connect to Wide OBS";
            this.btnConnectWide.UseVisualStyleBackColor = true;
            this.btnConnectWide.Click += new System.EventHandler(this.btnConnectWide_Click);
            // 
            // groupStatus
            // 
            this.groupStatus.Controls.Add(this.label5);
            this.groupStatus.Controls.Add(this.label4);
            this.groupStatus.Controls.Add(this.ledRecordWIDE);
            this.groupStatus.Controls.Add(this.ledRecordPROGRAM);
            this.groupStatus.Location = new System.Drawing.Point(409, 100);
            this.groupStatus.Name = "groupStatus";
            this.groupStatus.Size = new System.Drawing.Size(158, 77);
            this.groupStatus.TabIndex = 17;
            this.groupStatus.TabStop = false;
            this.groupStatus.Text = "Status";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 54);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(87, 13);
            this.label5.TabIndex = 1;
            this.label5.Text = "Wide Recording:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 28);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(101, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Program Recording:";
            // 
            // ledRecordWIDE
            // 
            this.ledRecordWIDE.BackColor = System.Drawing.Color.Gray;
            this.ledRecordWIDE.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ledRecordWIDE.Location = new System.Drawing.Point(113, 54);
            this.ledRecordWIDE.Name = "ledRecordWIDE";
            this.ledRecordWIDE.Size = new System.Drawing.Size(32, 13);
            this.ledRecordWIDE.TabIndex = 0;
            // 
            // ledRecordPROGRAM
            // 
            this.ledRecordPROGRAM.BackColor = System.Drawing.Color.Gray;
            this.ledRecordPROGRAM.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ledRecordPROGRAM.Location = new System.Drawing.Point(113, 28);
            this.ledRecordPROGRAM.Name = "ledRecordPROGRAM";
            this.ledRecordPROGRAM.Size = new System.Drawing.Size(32, 13);
            this.ledRecordPROGRAM.TabIndex = 0;
            // 
            // btnOpenRecordings
            // 
            this.btnOpenRecordings.Location = new System.Drawing.Point(12, 390);
            this.btnOpenRecordings.Name = "btnOpenRecordings";
            this.btnOpenRecordings.Size = new System.Drawing.Size(182, 23);
            this.btnOpenRecordings.TabIndex = 18;
            this.btnOpenRecordings.Text = "Open Recordings Folder";
            this.btnOpenRecordings.UseVisualStyleBackColor = true;
            this.btnOpenRecordings.Click += new System.EventHandler(this.btnOpenRecordings_Click);
            // 
            // btnUpload
            // 
            this.btnUpload.Location = new System.Drawing.Point(409, 183);
            this.btnUpload.Name = "btnUpload";
            this.btnUpload.Size = new System.Drawing.Size(158, 23);
            this.btnUpload.TabIndex = 19;
            this.btnUpload.Text = "Upload to Youtube";
            this.btnUpload.UseVisualStyleBackColor = true;
            this.btnUpload.Click += new System.EventHandler(this.btnUpload_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 425);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(126, 13);
            this.label1.TabIndex = 20;
            this.label1.Text = "Program Recording Path:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 449);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(112, 13);
            this.label6.TabIndex = 21;
            this.label6.Text = "Wide Recording Path:";
            // 
            // groupOBS
            // 
            this.groupOBS.Controls.Add(this.btnConnectProgram);
            this.groupOBS.Controls.Add(this.btnConnectWide);
            this.groupOBS.Location = new System.Drawing.Point(12, 40);
            this.groupOBS.Name = "groupOBS";
            this.groupOBS.Size = new System.Drawing.Size(391, 54);
            this.groupOBS.TabIndex = 22;
            this.groupOBS.TabStop = false;
            this.groupOBS.Text = "OBS Connections";
            // 
            // lblProgramPath
            // 
            this.lblProgramPath.AutoSize = true;
            this.lblProgramPath.Location = new System.Drawing.Point(144, 425);
            this.lblProgramPath.Name = "lblProgramPath";
            this.lblProgramPath.Size = new System.Drawing.Size(55, 13);
            this.lblProgramPath.TabIndex = 23;
            this.lblProgramPath.TabStop = true;
            this.lblProgramPath.Text = "linkLabel1";
            this.lblProgramPath.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblProgramPath_Click);
            // 
            // lblWidePath
            // 
            this.lblWidePath.AutoSize = true;
            this.lblWidePath.Location = new System.Drawing.Point(144, 449);
            this.lblWidePath.Name = "lblWidePath";
            this.lblWidePath.Size = new System.Drawing.Size(55, 13);
            this.lblWidePath.TabIndex = 24;
            this.lblWidePath.TabStop = true;
            this.lblWidePath.Text = "linkLabel2";
            this.lblWidePath.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblWidePath_Click);
            // 
            // frmMain
            // 
            this.AcceptButton = this.btnStartRecording;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnStopRecording;
            this.ClientSize = new System.Drawing.Size(579, 483);
            this.Controls.Add(this.lblWidePath);
            this.Controls.Add(this.lblProgramPath);
            this.Controls.Add(this.groupOBS);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnUpload);
            this.Controls.Add(this.btnOpenRecordings);
            this.Controls.Add(this.groupStatus);
            this.Controls.Add(this.lblElapsedTime);
            this.Controls.Add(this.groupMatch);
            this.Controls.Add(this.groupEvent);
            this.Controls.Add(this.btnStopRecording);
            this.Controls.Add(this.btnStartRecording);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FIRSTWA Recording";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.groupMatchTypes.ResumeLayout(false);
            this.groupMatchTypes.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMatchNumber)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numReplayNumber)).EndInit();
            this.groupEvent.ResumeLayout(false);
            this.groupEvent.PerformLayout();
            this.groupMatch.ResumeLayout(false);
            this.groupMatch.PerformLayout();
            this.groupStatus.ResumeLayout(false);
            this.groupStatus.PerformLayout();
            this.groupOBS.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnStartRecording;
        private System.Windows.Forms.Button btnStopRecording;
        private System.Windows.Forms.RadioButton radioBtnQual;
        private System.Windows.Forms.RadioButton radioBtnQuarter;
        private System.Windows.Forms.RadioButton radioBtnSemi;
        private System.Windows.Forms.RadioButton radioBtnFinal;
        private System.Windows.Forms.GroupBox groupMatchTypes;
        private System.Windows.Forms.NumericUpDown numMatchNumber;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.RadioButton radioBtnPractice;
        private System.Windows.Forms.Label label3;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.CheckBox chkReplay;
        private System.Windows.Forms.NumericUpDown numReplayNumber;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.GroupBox groupEvent;
        private System.Windows.Forms.GroupBox groupMatch;
        private System.Windows.Forms.Timer timerElapsed;
        private System.Windows.Forms.Label lblElapsedTime;
        private System.Windows.Forms.Button btnConnectProgram;
        private System.Windows.Forms.Button btnConnectWide;
        private System.Windows.Forms.GroupBox groupStatus;
        private System.Windows.Forms.Panel ledRecordPROGRAM;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel ledRecordWIDE;
        private System.Windows.Forms.CheckBox chkRecordWide;
        private System.Windows.Forms.CheckBox chkProgramRecord;
        private System.Windows.Forms.Button btnOpenRecordings;
        private System.Windows.Forms.ToolStripMenuItem recordingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem uploadsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem streamingToolStripMenuItem;
        private System.Windows.Forms.Button btnUpload;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox comboEventName;
        private System.Windows.Forms.GroupBox groupOBS;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.LinkLabel lblProgramPath;
        private System.Windows.Forms.LinkLabel lblWidePath;
    }
}

