﻿namespace FIRSTWA_Recorder
{
    partial class MainForm
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
            this.radioBtnCeremony = new System.Windows.Forms.RadioButton();
            this.numMatchNumber = new System.Windows.Forms.NumericUpDown();
            this.lblMatchNumber = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.audioToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.recordingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uploadsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.version001ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label3 = new System.Windows.Forms.Label();
            this.groupEvent = new System.Windows.Forms.GroupBox();
            this.comboEventName = new System.Windows.Forms.ComboBox();
            this.chkRecordWide = new System.Windows.Forms.CheckBox();
            this.chkProgramRecord = new System.Windows.Forms.CheckBox();
            this.groupMatch = new System.Windows.Forms.GroupBox();
            this.lblCeremonyTitle = new System.Windows.Forms.Label();
            this.txtCeremonyTitle = new System.Windows.Forms.TextBox();
            this.numFinalNo = new System.Windows.Forms.NumericUpDown();
            this.lblFinalNo = new System.Windows.Forms.Label();
            this.timerElapsed = new System.Windows.Forms.Timer(this.components);
            this.lblElapsedTime = new System.Windows.Forms.Label();
            this.groupOBS = new System.Windows.Forms.GroupBox();
            this.btnConnectWide = new System.Windows.Forms.Button();
            this.btnConnectProgram = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.bgWorker_FTP_Program = new System.ComponentModel.BackgroundWorker();
            this.bgWorker_FTP_Wide = new System.ComponentModel.BackgroundWorker();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.ledProgram = new System.Windows.Forms.Panel();
            this.lblProgramReady = new System.Windows.Forms.Label();
            this.lblWideReady = new System.Windows.Forms.Label();
            this.ledWide = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblReportA = new System.Windows.Forms.Label();
            this.lblReportB = new System.Windows.Forms.Label();
            this.bgWorker_WD = new System.ComponentModel.BackgroundWorker();
            this.groupMatchTypes.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMatchNumber)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.groupEvent.SuspendLayout();
            this.groupMatch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numFinalNo)).BeginInit();
            this.groupOBS.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnStartRecording
            // 
            this.btnStartRecording.Location = new System.Drawing.Point(22, 526);
            this.btnStartRecording.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.btnStartRecording.Name = "btnStartRecording";
            this.btnStartRecording.Size = new System.Drawing.Size(334, 96);
            this.btnStartRecording.TabIndex = 0;
            this.btnStartRecording.Text = "Start Recording";
            this.btnStartRecording.UseVisualStyleBackColor = true;
            this.btnStartRecording.Click += new System.EventHandler(this.btnStartRecording_Click);
            // 
            // btnStopRecording
            // 
            this.btnStopRecording.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnStopRecording.Enabled = false;
            this.btnStopRecording.Location = new System.Drawing.Point(405, 526);
            this.btnStopRecording.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.btnStopRecording.Name = "btnStopRecording";
            this.btnStopRecording.Size = new System.Drawing.Size(334, 96);
            this.btnStopRecording.TabIndex = 0;
            this.btnStopRecording.Text = "Stop Recording";
            this.btnStopRecording.UseVisualStyleBackColor = true;
            this.btnStopRecording.Click += new System.EventHandler(this.btnStopRecording_Click);
            // 
            // radioBtnQual
            // 
            this.radioBtnQual.AutoSize = true;
            this.radioBtnQual.Checked = true;
            this.radioBtnQual.Location = new System.Drawing.Point(13, 35);
            this.radioBtnQual.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.radioBtnQual.Name = "radioBtnQual";
            this.radioBtnQual.Size = new System.Drawing.Size(144, 29);
            this.radioBtnQual.TabIndex = 2;
            this.radioBtnQual.TabStop = true;
            this.radioBtnQual.Text = "Qualification";
            this.radioBtnQual.UseVisualStyleBackColor = true;
            this.radioBtnQual.CheckedChanged += new System.EventHandler(this.radioBtnMatchType_CheckedChanged);
            // 
            // radioBtnQuarter
            // 
            this.radioBtnQuarter.AutoSize = true;
            this.radioBtnQuarter.Location = new System.Drawing.Point(171, 35);
            this.radioBtnQuarter.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.radioBtnQuarter.Name = "radioBtnQuarter";
            this.radioBtnQuarter.Size = new System.Drawing.Size(138, 29);
            this.radioBtnQuarter.TabIndex = 2;
            this.radioBtnQuarter.Text = "Quarterfinal";
            this.radioBtnQuarter.UseVisualStyleBackColor = true;
            this.radioBtnQuarter.CheckedChanged += new System.EventHandler(this.radioBtnMatchType_CheckedChanged);
            // 
            // radioBtnSemi
            // 
            this.radioBtnSemi.AutoSize = true;
            this.radioBtnSemi.Location = new System.Drawing.Point(332, 35);
            this.radioBtnSemi.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.radioBtnSemi.Name = "radioBtnSemi";
            this.radioBtnSemi.Size = new System.Drawing.Size(117, 29);
            this.radioBtnSemi.TabIndex = 2;
            this.radioBtnSemi.Text = "Semifinal";
            this.radioBtnSemi.UseVisualStyleBackColor = true;
            this.radioBtnSemi.CheckedChanged += new System.EventHandler(this.radioBtnMatchType_CheckedChanged);
            // 
            // radioBtnFinal
            // 
            this.radioBtnFinal.AutoSize = true;
            this.radioBtnFinal.Location = new System.Drawing.Point(460, 33);
            this.radioBtnFinal.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.radioBtnFinal.Name = "radioBtnFinal";
            this.radioBtnFinal.Size = new System.Drawing.Size(79, 29);
            this.radioBtnFinal.TabIndex = 2;
            this.radioBtnFinal.Text = "Final";
            this.radioBtnFinal.UseVisualStyleBackColor = true;
            this.radioBtnFinal.CheckedChanged += new System.EventHandler(this.radioBtnMatchType_CheckedChanged);
            // 
            // groupMatchTypes
            // 
            this.groupMatchTypes.Controls.Add(this.radioBtnCeremony);
            this.groupMatchTypes.Controls.Add(this.radioBtnQual);
            this.groupMatchTypes.Controls.Add(this.radioBtnFinal);
            this.groupMatchTypes.Controls.Add(this.radioBtnSemi);
            this.groupMatchTypes.Controls.Add(this.radioBtnQuarter);
            this.groupMatchTypes.Location = new System.Drawing.Point(11, 35);
            this.groupMatchTypes.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.groupMatchTypes.Name = "groupMatchTypes";
            this.groupMatchTypes.Padding = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.groupMatchTypes.Size = new System.Drawing.Size(693, 83);
            this.groupMatchTypes.TabIndex = 3;
            this.groupMatchTypes.TabStop = false;
            this.groupMatchTypes.Text = "Match Type";
            // 
            // radioBtnCeremony
            // 
            this.radioBtnCeremony.AutoSize = true;
            this.radioBtnCeremony.Enabled = false;
            this.radioBtnCeremony.Location = new System.Drawing.Point(550, 33);
            this.radioBtnCeremony.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.radioBtnCeremony.Name = "radioBtnCeremony";
            this.radioBtnCeremony.Size = new System.Drawing.Size(128, 29);
            this.radioBtnCeremony.TabIndex = 3;
            this.radioBtnCeremony.Text = "Ceremony";
            this.radioBtnCeremony.UseVisualStyleBackColor = true;
            this.radioBtnCeremony.CheckedChanged += new System.EventHandler(this.radioBtnMatchType_CheckedChanged);
            // 
            // numMatchNumber
            // 
            this.numMatchNumber.Location = new System.Drawing.Point(361, 126);
            this.numMatchNumber.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
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
            this.numMatchNumber.Size = new System.Drawing.Size(95, 29);
            this.numMatchNumber.TabIndex = 6;
            this.numMatchNumber.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lblMatchNumber
            // 
            this.lblMatchNumber.AutoSize = true;
            this.lblMatchNumber.Location = new System.Drawing.Point(248, 129);
            this.lblMatchNumber.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblMatchNumber.Name = "lblMatchNumber";
            this.lblMatchNumber.Size = new System.Drawing.Size(102, 25);
            this.lblMatchNumber.TabIndex = 7;
            this.lblMatchNumber.Text = "Match No:";
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(28, 28);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.settingsToolStripMenuItem,
            this.version001ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(11, 4, 0, 4);
            this.menuStrip1.Size = new System.Drawing.Size(763, 42);
            this.menuStrip1.TabIndex = 8;
            this.menuStrip1.Text = "Menu Strip";
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.audioToolStripMenuItem,
            this.recordingToolStripMenuItem,
            this.uploadsToolStripMenuItem});
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(99, 34);
            this.settingsToolStripMenuItem.Text = "Settings";
            // 
            // audioToolStripMenuItem
            // 
            this.audioToolStripMenuItem.Name = "audioToolStripMenuItem";
            this.audioToolStripMenuItem.Size = new System.Drawing.Size(199, 34);
            this.audioToolStripMenuItem.Text = "Audio";
            this.audioToolStripMenuItem.Click += new System.EventHandler(this.audioToolStripMenuItem_Click);
            // 
            // recordingToolStripMenuItem
            // 
            this.recordingToolStripMenuItem.Name = "recordingToolStripMenuItem";
            this.recordingToolStripMenuItem.Size = new System.Drawing.Size(199, 34);
            this.recordingToolStripMenuItem.Text = "Recording";
            this.recordingToolStripMenuItem.Click += new System.EventHandler(this.recordingToolStripMenuItem_Click);
            // 
            // uploadsToolStripMenuItem
            // 
            this.uploadsToolStripMenuItem.Enabled = false;
            this.uploadsToolStripMenuItem.Name = "uploadsToolStripMenuItem";
            this.uploadsToolStripMenuItem.Size = new System.Drawing.Size(199, 34);
            this.uploadsToolStripMenuItem.Text = "Uploading";
            // 
            // version001ToolStripMenuItem
            // 
            this.version001ToolStripMenuItem.Name = "version001ToolStripMenuItem";
            this.version001ToolStripMenuItem.Size = new System.Drawing.Size(142, 34);
            this.version001ToolStripMenuItem.Text = "Version 0.0.2";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(17, 52);
            this.label3.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(125, 25);
            this.label3.TabIndex = 9;
            this.label3.Text = "Event Name:";
            // 
            // groupEvent
            // 
            this.groupEvent.Controls.Add(this.comboEventName);
            this.groupEvent.Controls.Add(this.chkRecordWide);
            this.groupEvent.Controls.Add(this.chkProgramRecord);
            this.groupEvent.Controls.Add(this.label3);
            this.groupEvent.Location = new System.Drawing.Point(22, 185);
            this.groupEvent.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.groupEvent.Name = "groupEvent";
            this.groupEvent.Padding = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.groupEvent.Size = new System.Drawing.Size(717, 142);
            this.groupEvent.TabIndex = 12;
            this.groupEvent.TabStop = false;
            this.groupEvent.Text = "Event Specific";
            // 
            // comboEventName
            // 
            this.comboEventName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboEventName.FormattingEnabled = true;
            this.comboEventName.Location = new System.Drawing.Point(156, 46);
            this.comboEventName.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.comboEventName.Name = "comboEventName";
            this.comboEventName.Size = new System.Drawing.Size(549, 32);
            this.comboEventName.TabIndex = 12;
            this.comboEventName.SelectedIndexChanged += new System.EventHandler(this.comboEventName_SelectedIndexChanged);
            // 
            // chkRecordWide
            // 
            this.chkRecordWide.AutoSize = true;
            this.chkRecordWide.Checked = true;
            this.chkRecordWide.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkRecordWide.Location = new System.Drawing.Point(216, 100);
            this.chkRecordWide.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.chkRecordWide.Name = "chkRecordWide";
            this.chkRecordWide.Size = new System.Drawing.Size(151, 29);
            this.chkRecordWide.TabIndex = 11;
            this.chkRecordWide.Text = "Record Wide";
            this.chkRecordWide.UseVisualStyleBackColor = true;
            // 
            // chkProgramRecord
            // 
            this.chkProgramRecord.AutoSize = true;
            this.chkProgramRecord.Checked = true;
            this.chkProgramRecord.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkProgramRecord.Location = new System.Drawing.Point(17, 100);
            this.chkProgramRecord.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.chkProgramRecord.Name = "chkProgramRecord";
            this.chkProgramRecord.Size = new System.Drawing.Size(179, 29);
            this.chkProgramRecord.TabIndex = 11;
            this.chkProgramRecord.Text = "Record Program";
            this.chkProgramRecord.UseVisualStyleBackColor = true;
            // 
            // groupMatch
            // 
            this.groupMatch.Controls.Add(this.lblCeremonyTitle);
            this.groupMatch.Controls.Add(this.txtCeremonyTitle);
            this.groupMatch.Controls.Add(this.groupMatchTypes);
            this.groupMatch.Controls.Add(this.numFinalNo);
            this.groupMatch.Controls.Add(this.numMatchNumber);
            this.groupMatch.Controls.Add(this.lblFinalNo);
            this.groupMatch.Controls.Add(this.lblMatchNumber);
            this.groupMatch.Location = new System.Drawing.Point(22, 338);
            this.groupMatch.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.groupMatch.Name = "groupMatch";
            this.groupMatch.Padding = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.groupMatch.Size = new System.Drawing.Size(717, 177);
            this.groupMatch.TabIndex = 13;
            this.groupMatch.TabStop = false;
            this.groupMatch.Text = "Match Specific";
            // 
            // lblCeremonyTitle
            // 
            this.lblCeremonyTitle.AutoSize = true;
            this.lblCeremonyTitle.Location = new System.Drawing.Point(484, 131);
            this.lblCeremonyTitle.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblCeremonyTitle.Name = "lblCeremonyTitle";
            this.lblCeremonyTitle.Size = new System.Drawing.Size(55, 25);
            this.lblCeremonyTitle.TabIndex = 10;
            this.lblCeremonyTitle.Text = "Title:";
            this.lblCeremonyTitle.Visible = false;
            // 
            // txtCeremonyTitle
            // 
            this.txtCeremonyTitle.Location = new System.Drawing.Point(550, 124);
            this.txtCeremonyTitle.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.txtCeremonyTitle.Name = "txtCeremonyTitle";
            this.txtCeremonyTitle.Size = new System.Drawing.Size(140, 29);
            this.txtCeremonyTitle.TabIndex = 8;
            this.txtCeremonyTitle.Visible = false;
            this.txtCeremonyTitle.Leave += new System.EventHandler(this.txtCeremonyTitle_Leave);
            // 
            // numFinalNo
            // 
            this.numFinalNo.Location = new System.Drawing.Point(117, 126);
            this.numFinalNo.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.numFinalNo.Maximum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.numFinalNo.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numFinalNo.Name = "numFinalNo";
            this.numFinalNo.Size = new System.Drawing.Size(95, 29);
            this.numFinalNo.TabIndex = 6;
            this.numFinalNo.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numFinalNo.Visible = false;
            // 
            // lblFinalNo
            // 
            this.lblFinalNo.AutoSize = true;
            this.lblFinalNo.Location = new System.Drawing.Point(17, 129);
            this.lblFinalNo.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblFinalNo.Name = "lblFinalNo";
            this.lblFinalNo.Size = new System.Drawing.Size(78, 25);
            this.lblFinalNo.TabIndex = 7;
            this.lblFinalNo.Text = "Set No:";
            this.lblFinalNo.Visible = false;
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
            this.lblElapsedTime.Location = new System.Drawing.Point(257, 637);
            this.lblElapsedTime.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblElapsedTime.Name = "lblElapsedTime";
            this.lblElapsedTime.Size = new System.Drawing.Size(240, 48);
            this.lblElapsedTime.TabIndex = 14;
            this.lblElapsedTime.Text = "00:00:00.00";
            // 
            // groupOBS
            // 
            this.groupOBS.Controls.Add(this.btnConnectWide);
            this.groupOBS.Controls.Add(this.btnConnectProgram);
            this.groupOBS.Location = new System.Drawing.Point(22, 74);
            this.groupOBS.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.groupOBS.Name = "groupOBS";
            this.groupOBS.Padding = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.groupOBS.Size = new System.Drawing.Size(717, 100);
            this.groupOBS.TabIndex = 22;
            this.groupOBS.TabStop = false;
            this.groupOBS.Text = "HyperDeck Connections";
            // 
            // btnConnectWide
            // 
            this.btnConnectWide.Location = new System.Drawing.Point(383, 35);
            this.btnConnectWide.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.btnConnectWide.Name = "btnConnectWide";
            this.btnConnectWide.Size = new System.Drawing.Size(323, 42);
            this.btnConnectWide.TabIndex = 19;
            this.btnConnectWide.Text = "Connect to Wide HyperDeck";
            this.btnConnectWide.UseVisualStyleBackColor = true;
            this.btnConnectWide.Click += new System.EventHandler(this.btnConnectWide_Click);
            // 
            // btnConnectProgram
            // 
            this.btnConnectProgram.Location = new System.Drawing.Point(11, 35);
            this.btnConnectProgram.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.btnConnectProgram.Name = "btnConnectProgram";
            this.btnConnectProgram.Size = new System.Drawing.Size(323, 42);
            this.btnConnectProgram.TabIndex = 19;
            this.btnConnectProgram.Text = "Connect to Program HyperDeck";
            this.btnConnectProgram.UseVisualStyleBackColor = true;
            this.btnConnectProgram.Click += new System.EventHandler(this.btnConnectProgram_Click);
            // 
            // bgWorker_FTP_Program
            // 
            this.bgWorker_FTP_Program.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgWorker_FTP_Program_DoWork);
            this.bgWorker_FTP_Program.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgWorker_FTP_Program_RunWorkerCompleted);
            // 
            // bgWorker_FTP_Wide
            // 
            this.bgWorker_FTP_Wide.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgWorker_FTP_Wide_DoWork);
            this.bgWorker_FTP_Wide.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgWorker_FTP_Wide_RunWorkerCompleted);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(22, 702);
            this.progressBar1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.progressBar1.Maximum = 31;
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(717, 48);
            this.progressBar1.TabIndex = 27;
            // 
            // ledProgram
            // 
            this.ledProgram.BackColor = System.Drawing.Color.Red;
            this.ledProgram.Location = new System.Drawing.Point(127, 646);
            this.ledProgram.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ledProgram.Name = "ledProgram";
            this.ledProgram.Size = new System.Drawing.Size(62, 30);
            this.ledProgram.TabIndex = 28;
            // 
            // lblProgramReady
            // 
            this.lblProgramReady.AutoSize = true;
            this.lblProgramReady.Location = new System.Drawing.Point(28, 652);
            this.lblProgramReady.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblProgramReady.Name = "lblProgramReady";
            this.lblProgramReady.Size = new System.Drawing.Size(92, 25);
            this.lblProgramReady.TabIndex = 29;
            this.lblProgramReady.Text = "Program:";
            // 
            // lblWideReady
            // 
            this.lblWideReady.AutoSize = true;
            this.lblWideReady.Location = new System.Drawing.Point(607, 652);
            this.lblWideReady.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblWideReady.Name = "lblWideReady";
            this.lblWideReady.Size = new System.Drawing.Size(64, 25);
            this.lblWideReady.TabIndex = 31;
            this.lblWideReady.Text = "Wide:";
            // 
            // ledWide
            // 
            this.ledWide.BackColor = System.Drawing.Color.Red;
            this.ledWide.Location = new System.Drawing.Point(677, 646);
            this.ledWide.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ledWide.Name = "ledWide";
            this.ledWide.Size = new System.Drawing.Size(62, 30);
            this.ledWide.TabIndex = 30;
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Red;
            this.btnCancel.Enabled = false;
            this.btnCancel.Location = new System.Drawing.Point(266, 792);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(216, 63);
            this.btnCancel.TabIndex = 32;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblReportA
            // 
            this.lblReportA.AutoSize = true;
            this.lblReportA.Location = new System.Drawing.Point(488, 761);
            this.lblReportA.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblReportA.Name = "lblReportA";
            this.lblReportA.Size = new System.Drawing.Size(43, 25);
            this.lblReportA.TabIndex = 33;
            this.lblReportA.Text = "Idle";
            // 
            // lblReportB
            // 
            this.lblReportB.AutoSize = true;
            this.lblReportB.Location = new System.Drawing.Point(28, 761);
            this.lblReportB.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblReportB.Name = "lblReportB";
            this.lblReportB.Size = new System.Drawing.Size(43, 25);
            this.lblReportB.TabIndex = 34;
            this.lblReportB.Text = "Idle";
            // 
            // bgWorker_WD
            // 
            this.bgWorker_WD.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgWorker_WD_DoWork);
            // 
            // MainForm
            // 
            this.AcceptButton = this.btnStartRecording;
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnStopRecording;
            this.ClientSize = new System.Drawing.Size(763, 855);
            this.Controls.Add(this.lblReportB);
            this.Controls.Add(this.lblReportA);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lblWideReady);
            this.Controls.Add(this.ledWide);
            this.Controls.Add(this.lblProgramReady);
            this.Controls.Add(this.ledProgram);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.groupOBS);
            this.Controls.Add(this.lblElapsedTime);
            this.Controls.Add(this.groupMatch);
            this.Controls.Add(this.groupEvent);
            this.Controls.Add(this.btnStopRecording);
            this.Controls.Add(this.btnStartRecording);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FIRSTWA Recording";
            this.groupMatchTypes.ResumeLayout(false);
            this.groupMatchTypes.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMatchNumber)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.groupEvent.ResumeLayout(false);
            this.groupEvent.PerformLayout();
            this.groupMatch.ResumeLayout(false);
            this.groupMatch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numFinalNo)).EndInit();
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
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupEvent;
        private System.Windows.Forms.GroupBox groupMatch;
        private System.Windows.Forms.Timer timerElapsed;
        private System.Windows.Forms.Label lblElapsedTime;
        private System.Windows.Forms.CheckBox chkRecordWide;
        private System.Windows.Forms.CheckBox chkProgramRecord;
        private System.Windows.Forms.ToolStripMenuItem recordingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem uploadsToolStripMenuItem;
        private System.Windows.Forms.ComboBox comboEventName;
        private System.Windows.Forms.GroupBox groupOBS;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label lblMatchNumber;
        private System.Windows.Forms.NumericUpDown numFinalNo;
        private System.Windows.Forms.Label lblFinalNo;
        private System.Windows.Forms.Button btnConnectWide;
        private System.Windows.Forms.Button btnConnectProgram;
        private System.ComponentModel.BackgroundWorker bgWorker_FTP_Program;
        private System.ComponentModel.BackgroundWorker bgWorker_FTP_Wide;
        private System.ComponentModel.BackgroundWorker bgWorker_Youtube;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Panel ledProgram;
        private System.Windows.Forms.Label lblProgramReady;
        private System.Windows.Forms.Label lblWideReady;
        private System.Windows.Forms.Panel ledWide;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ToolStripMenuItem audioToolStripMenuItem;
        private System.Windows.Forms.RadioButton radioBtnCeremony;
        private System.Windows.Forms.Label lblCeremonyTitle;
        private System.Windows.Forms.TextBox txtCeremonyTitle;
        private System.Windows.Forms.Label lblReportA;
        private System.Windows.Forms.Label lblReportB;
        private System.Windows.Forms.ToolStripMenuItem version001ToolStripMenuItem;
        private System.ComponentModel.BackgroundWorker bgWorker_WD;
    }
}

