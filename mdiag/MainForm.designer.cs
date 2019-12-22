namespace mdiag
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.captureLogsButton = new System.Windows.Forms.Button();
            this.serviceGroup = new System.Windows.Forms.GroupBox();
            this.serviceStopButton = new System.Windows.Forms.Button();
            this.serviceRestartButton = new System.Windows.Forms.Button();
            this.serviceStartButton = new System.Windows.Forms.Button();
            this.processGroup = new System.Windows.Forms.GroupBox();
            this.shutdownButton = new System.Windows.Forms.Button();
            this.killIconButton = new System.Windows.Forms.Button();
            this.killMorphicButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.keyInText = new System.Windows.Forms.ComboBox();
            this.keyInMethod = new System.Windows.Forms.ComboBox();
            this.keyOutButton = new System.Windows.Forms.Button();
            this.keyInButton = new System.Windows.Forms.Button();
            this.logTabs = new System.Windows.Forms.TabControl();
            this.appLogTab = new System.Windows.Forms.TabPage();
            this.logText = new System.Windows.Forms.TextBox();
            this.morphicLogTab = new System.Windows.Forms.TabPage();
            this.morphicLogText = new System.Windows.Forms.TextBox();
            this.serviceLogTab = new System.Windows.Forms.TabPage();
            this.serviceLogText = new System.Windows.Forms.TextBox();
            this.metricsTab = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.metricsFilterCheckbox = new System.Windows.Forms.CheckBox();
            this.metricsSaveFileLink = new System.Windows.Forms.LinkLabel();
            this.metricsSaveCheckbox = new System.Windows.Forms.CheckBox();
            this.enableMetricsCheckbox = new System.Windows.Forms.CheckBox();
            this.metricsLogText = new System.Windows.Forms.TextBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.saveLogsDialog = new System.Windows.Forms.SaveFileDialog();
            this.metricsSaveDialog = new System.Windows.Forms.SaveFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.serviceGroup.SuspendLayout();
            this.processGroup.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.logTabs.SuspendLayout();
            this.appLogTab.SuspendLayout();
            this.morphicLogTab.SuspendLayout();
            this.serviceLogTab.SuspendLayout();
            this.metricsTab.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.captureLogsButton);
            this.splitContainer1.Panel1.Controls.Add(this.serviceGroup);
            this.splitContainer1.Panel1.Controls.Add(this.processGroup);
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            this.splitContainer1.Panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.splitContainer1_Panel1_Paint);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.logTabs);
            this.splitContainer1.Size = new System.Drawing.Size(481, 522);
            this.splitContainer1.SplitterDistance = 270;
            this.splitContainer1.TabIndex = 0;
            // 
            // captureLogsButton
            // 
            this.captureLogsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.captureLogsButton.Location = new System.Drawing.Point(375, 210);
            this.captureLogsButton.Name = "captureLogsButton";
            this.captureLogsButton.Size = new System.Drawing.Size(85, 36);
            this.captureLogsButton.TabIndex = 3;
            this.captureLogsButton.Text = "Save Logs";
            this.toolTip1.SetToolTip(this.captureLogsButton, "Collect all Morphic logs into a zip file");
            this.captureLogsButton.UseVisualStyleBackColor = true;
            this.captureLogsButton.Click += new System.EventHandler(this.captureLogsButton_Click);
            // 
            // serviceGroup
            // 
            this.serviceGroup.Controls.Add(this.serviceStopButton);
            this.serviceGroup.Controls.Add(this.serviceRestartButton);
            this.serviceGroup.Controls.Add(this.serviceStartButton);
            this.serviceGroup.Location = new System.Drawing.Point(12, 94);
            this.serviceGroup.Name = "serviceGroup";
            this.serviceGroup.Size = new System.Drawing.Size(256, 76);
            this.serviceGroup.TabIndex = 2;
            this.serviceGroup.TabStop = false;
            this.serviceGroup.Text = "Morphic Service";
            // 
            // serviceStopButton
            // 
            this.serviceStopButton.Location = new System.Drawing.Point(169, 29);
            this.serviceStopButton.Name = "serviceStopButton";
            this.serviceStopButton.Size = new System.Drawing.Size(75, 23);
            this.serviceStopButton.TabIndex = 0;
            this.serviceStopButton.Text = "Stop";
            this.toolTip1.SetToolTip(this.serviceStopButton, "Stop the morphic service");
            this.serviceStopButton.UseVisualStyleBackColor = true;
            this.serviceStopButton.Click += new System.EventHandler(this.serviceStopButton_Click);
            // 
            // serviceRestartButton
            // 
            this.serviceRestartButton.Location = new System.Drawing.Point(7, 29);
            this.serviceRestartButton.Name = "serviceRestartButton";
            this.serviceRestartButton.Size = new System.Drawing.Size(75, 23);
            this.serviceRestartButton.TabIndex = 0;
            this.serviceRestartButton.Text = "Restart";
            this.toolTip1.SetToolTip(this.serviceRestartButton, "Restarts the morphic service");
            this.serviceRestartButton.UseVisualStyleBackColor = true;
            this.serviceRestartButton.Click += new System.EventHandler(this.serviceRestartButton_Click);
            // 
            // serviceStartButton
            // 
            this.serviceStartButton.Location = new System.Drawing.Point(88, 29);
            this.serviceStartButton.Name = "serviceStartButton";
            this.serviceStartButton.Size = new System.Drawing.Size(75, 23);
            this.serviceStartButton.TabIndex = 0;
            this.serviceStartButton.Text = "Start";
            this.toolTip1.SetToolTip(this.serviceStartButton, "Start the morphic service");
            this.serviceStartButton.UseVisualStyleBackColor = true;
            this.serviceStartButton.Click += new System.EventHandler(this.serviceStartButton_Click);
            // 
            // processGroup
            // 
            this.processGroup.Controls.Add(this.shutdownButton);
            this.processGroup.Controls.Add(this.killIconButton);
            this.processGroup.Controls.Add(this.killMorphicButton);
            this.processGroup.Location = new System.Drawing.Point(12, 176);
            this.processGroup.Name = "processGroup";
            this.processGroup.Size = new System.Drawing.Size(256, 70);
            this.processGroup.TabIndex = 1;
            this.processGroup.TabStop = false;
            this.processGroup.Text = "Morphic Process";
            // 
            // shutdownButton
            // 
            this.shutdownButton.Location = new System.Drawing.Point(88, 32);
            this.shutdownButton.Name = "shutdownButton";
            this.shutdownButton.Size = new System.Drawing.Size(75, 23);
            this.shutdownButton.TabIndex = 1;
            this.shutdownButton.Text = "Shutdown";
            this.toolTip1.SetToolTip(this.shutdownButton, "Shutdown morphic - service will not restart it");
            this.shutdownButton.UseVisualStyleBackColor = true;
            this.shutdownButton.Click += new System.EventHandler(this.shutdownButton_Click);
            // 
            // killIconButton
            // 
            this.killIconButton.Location = new System.Drawing.Point(169, 32);
            this.killIconButton.Name = "killIconButton";
            this.killIconButton.Size = new System.Drawing.Size(75, 23);
            this.killIconButton.TabIndex = 1;
            this.killIconButton.Text = "Kill Icon";
            this.toolTip1.SetToolTip(this.killIconButton, "Kill the tray button (morphic should restart it)");
            this.killIconButton.UseVisualStyleBackColor = true;
            this.killIconButton.Click += new System.EventHandler(this.killIconButton_Click);
            // 
            // killMorphicButton
            // 
            this.killMorphicButton.Location = new System.Drawing.Point(7, 32);
            this.killMorphicButton.Name = "killMorphicButton";
            this.killMorphicButton.Size = new System.Drawing.Size(75, 23);
            this.killMorphicButton.TabIndex = 1;
            this.killMorphicButton.Text = "Kill";
            this.toolTip1.SetToolTip(this.killMorphicButton, "Kills the Morphic process (service should restart it)");
            this.killMorphicButton.UseVisualStyleBackColor = true;
            this.killMorphicButton.Click += new System.EventHandler(this.killMorphicButton_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.keyInText);
            this.groupBox1.Controls.Add(this.keyInMethod);
            this.groupBox1.Controls.Add(this.keyOutButton);
            this.groupBox1.Controls.Add(this.keyInButton);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(351, 75);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Key in";
            // 
            // keyInText
            // 
            this.keyInText.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.keyInText.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.keyInText.FormattingEnabled = true;
            this.keyInText.Location = new System.Drawing.Point(7, 20);
            this.keyInText.Name = "keyInText";
            this.keyInText.Size = new System.Drawing.Size(168, 21);
            this.keyInText.TabIndex = 1;
            // 
            // keyInMethod
            // 
            this.keyInMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.keyInMethod.FormattingEnabled = true;
            this.keyInMethod.Items.AddRange(new object[] {
            "USB",
            "FlowManager"});
            this.keyInMethod.Location = new System.Drawing.Point(7, 47);
            this.keyInMethod.Name = "keyInMethod";
            this.keyInMethod.Size = new System.Drawing.Size(121, 21);
            this.keyInMethod.TabIndex = 2;
            this.toolTip1.SetToolTip(this.keyInMethod, "Key-in method");
            // 
            // keyOutButton
            // 
            this.keyOutButton.Location = new System.Drawing.Point(262, 18);
            this.keyOutButton.Name = "keyOutButton";
            this.keyOutButton.Size = new System.Drawing.Size(75, 23);
            this.keyOutButton.TabIndex = 1;
            this.keyOutButton.Text = "Key Out";
            this.keyOutButton.UseVisualStyleBackColor = true;
            this.keyOutButton.Click += new System.EventHandler(this.keyOutButton_Click);
            // 
            // keyInButton
            // 
            this.keyInButton.Location = new System.Drawing.Point(181, 18);
            this.keyInButton.Name = "keyInButton";
            this.keyInButton.Size = new System.Drawing.Size(75, 23);
            this.keyInButton.TabIndex = 1;
            this.keyInButton.Text = "Key In";
            this.keyInButton.UseVisualStyleBackColor = true;
            this.keyInButton.Click += new System.EventHandler(this.keyInButton_Click);
            // 
            // logTabs
            // 
            this.logTabs.Controls.Add(this.appLogTab);
            this.logTabs.Controls.Add(this.morphicLogTab);
            this.logTabs.Controls.Add(this.serviceLogTab);
            this.logTabs.Controls.Add(this.metricsTab);
            this.logTabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logTabs.Location = new System.Drawing.Point(0, 0);
            this.logTabs.Name = "logTabs";
            this.logTabs.SelectedIndex = 0;
            this.logTabs.Size = new System.Drawing.Size(481, 248);
            this.logTabs.TabIndex = 1;
            this.logTabs.SelectedIndexChanged += new System.EventHandler(this.logTabs_SelectedIndexChanged);
            // 
            // appLogTab
            // 
            this.appLogTab.Controls.Add(this.logText);
            this.appLogTab.Location = new System.Drawing.Point(4, 22);
            this.appLogTab.Name = "appLogTab";
            this.appLogTab.Padding = new System.Windows.Forms.Padding(3);
            this.appLogTab.Size = new System.Drawing.Size(473, 222);
            this.appLogTab.TabIndex = 0;
            this.appLogTab.Text = "mdiag log";
            this.toolTip1.SetToolTip(this.appLogTab, "aa");
            this.appLogTab.ToolTipText = "vv";
            this.appLogTab.UseVisualStyleBackColor = true;
            // 
            // logText
            // 
            this.logText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logText.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.logText.Location = new System.Drawing.Point(3, 3);
            this.logText.Multiline = true;
            this.logText.Name = "logText";
            this.logText.ReadOnly = true;
            this.logText.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.logText.Size = new System.Drawing.Size(467, 216);
            this.logText.TabIndex = 0;
            this.logText.WordWrap = false;
            // 
            // morphicLogTab
            // 
            this.morphicLogTab.Controls.Add(this.morphicLogText);
            this.morphicLogTab.Location = new System.Drawing.Point(4, 22);
            this.morphicLogTab.Name = "morphicLogTab";
            this.morphicLogTab.Padding = new System.Windows.Forms.Padding(3);
            this.morphicLogTab.Size = new System.Drawing.Size(389, 183);
            this.morphicLogTab.TabIndex = 1;
            this.morphicLogTab.Text = "Morphic Log";
            this.morphicLogTab.UseVisualStyleBackColor = true;
            // 
            // morphicLogText
            // 
            this.morphicLogText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.morphicLogText.Location = new System.Drawing.Point(3, 3);
            this.morphicLogText.Multiline = true;
            this.morphicLogText.Name = "morphicLogText";
            this.morphicLogText.ReadOnly = true;
            this.morphicLogText.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.morphicLogText.Size = new System.Drawing.Size(383, 177);
            this.morphicLogText.TabIndex = 1;
            this.morphicLogText.WordWrap = false;
            // 
            // serviceLogTab
            // 
            this.serviceLogTab.Controls.Add(this.serviceLogText);
            this.serviceLogTab.Location = new System.Drawing.Point(4, 22);
            this.serviceLogTab.Name = "serviceLogTab";
            this.serviceLogTab.Size = new System.Drawing.Size(389, 183);
            this.serviceLogTab.TabIndex = 2;
            this.serviceLogTab.Text = "Service Log";
            this.serviceLogTab.UseVisualStyleBackColor = true;
            // 
            // serviceLogText
            // 
            this.serviceLogText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.serviceLogText.Location = new System.Drawing.Point(0, 0);
            this.serviceLogText.Multiline = true;
            this.serviceLogText.Name = "serviceLogText";
            this.serviceLogText.ReadOnly = true;
            this.serviceLogText.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.serviceLogText.Size = new System.Drawing.Size(389, 183);
            this.serviceLogText.TabIndex = 1;
            this.serviceLogText.WordWrap = false;
            // 
            // metricsTab
            // 
            this.metricsTab.Controls.Add(this.panel1);
            this.metricsTab.Controls.Add(this.metricsLogText);
            this.metricsTab.Location = new System.Drawing.Point(4, 22);
            this.metricsTab.Name = "metricsTab";
            this.metricsTab.Padding = new System.Windows.Forms.Padding(3);
            this.metricsTab.Size = new System.Drawing.Size(473, 222);
            this.metricsTab.TabIndex = 3;
            this.metricsTab.Text = "Metrics";
            this.metricsTab.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.metricsFilterCheckbox);
            this.panel1.Controls.Add(this.metricsSaveFileLink);
            this.panel1.Controls.Add(this.metricsSaveCheckbox);
            this.panel1.Controls.Add(this.enableMetricsCheckbox);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(467, 22);
            this.panel1.TabIndex = 3;
            // 
            // metricsFilterCheckbox
            // 
            this.metricsFilterCheckbox.AutoSize = true;
            this.metricsFilterCheckbox.Location = new System.Drawing.Point(68, 3);
            this.metricsFilterCheckbox.Name = "metricsFilterCheckbox";
            this.metricsFilterCheckbox.Size = new System.Drawing.Size(48, 17);
            this.metricsFilterCheckbox.TabIndex = 2;
            this.metricsFilterCheckbox.Text = "Filter";
            this.toolTip1.SetToolTip(this.metricsFilterCheckbox, "Remove less interesting fields");
            this.metricsFilterCheckbox.UseVisualStyleBackColor = true;
            // 
            // metricsSaveFileLink
            // 
            this.metricsSaveFileLink.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.metricsSaveFileLink.AutoEllipsis = true;
            this.metricsSaveFileLink.Enabled = false;
            this.metricsSaveFileLink.Location = new System.Drawing.Point(200, 4);
            this.metricsSaveFileLink.Name = "metricsSaveFileLink";
            this.metricsSaveFileLink.Size = new System.Drawing.Size(262, 16);
            this.metricsSaveFileLink.TabIndex = 1;
            this.metricsSaveFileLink.TabStop = true;
            this.metricsSaveFileLink.Text = "Browse...";
            this.metricsSaveFileLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.metricsSaveFileLink_LinkClicked);
            // 
            // metricsSaveCheckbox
            // 
            this.metricsSaveCheckbox.AutoSize = true;
            this.metricsSaveCheckbox.Location = new System.Drawing.Point(138, 3);
            this.metricsSaveCheckbox.Name = "metricsSaveCheckbox";
            this.metricsSaveCheckbox.Size = new System.Drawing.Size(66, 17);
            this.metricsSaveCheckbox.TabIndex = 0;
            this.metricsSaveCheckbox.Text = "Save to:";
            this.toolTip1.SetToolTip(this.metricsSaveCheckbox, "Write the output to a file");
            this.metricsSaveCheckbox.UseVisualStyleBackColor = true;
            this.metricsSaveCheckbox.CheckedChanged += new System.EventHandler(this.metricsSaveCheckbox_CheckedChanged);
            // 
            // enableMetricsCheckbox
            // 
            this.enableMetricsCheckbox.AutoSize = true;
            this.enableMetricsCheckbox.Location = new System.Drawing.Point(3, 3);
            this.enableMetricsCheckbox.Name = "enableMetricsCheckbox";
            this.enableMetricsCheckbox.Size = new System.Drawing.Size(59, 17);
            this.enableMetricsCheckbox.TabIndex = 0;
            this.enableMetricsCheckbox.Text = "Enable";
            this.toolTip1.SetToolTip(this.enableMetricsCheckbox, "Enables capturing the metrics log (disables filebeat service)");
            this.enableMetricsCheckbox.UseVisualStyleBackColor = true;
            this.enableMetricsCheckbox.CheckedChanged += new System.EventHandler(this.enableMetricsCheckbox_CheckedChanged);
            // 
            // metricsLogText
            // 
            this.metricsLogText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.metricsLogText.Location = new System.Drawing.Point(0, 26);
            this.metricsLogText.Margin = new System.Windows.Forms.Padding(0);
            this.metricsLogText.Multiline = true;
            this.metricsLogText.Name = "metricsLogText";
            this.metricsLogText.ReadOnly = true;
            this.metricsLogText.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.metricsLogText.Size = new System.Drawing.Size(473, 196);
            this.metricsLogText.TabIndex = 2;
            this.metricsLogText.WordWrap = false;
            // 
            // saveLogsDialog
            // 
            this.saveLogsDialog.DefaultExt = "zip";
            this.saveLogsDialog.Filter = "Zip files|*.zip|All files|*.*";
            this.saveLogsDialog.RestoreDirectory = true;
            // 
            // metricsSaveDialog
            // 
            this.metricsSaveDialog.DefaultExt = "txt";
            this.metricsSaveDialog.Filter = "Text files|*.txt|All files|*.*";
            this.metricsSaveDialog.RestoreDirectory = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(481, 522);
            this.Controls.Add(this.splitContainer1);
            this.Name = "MainForm";
            this.Text = "Morphic Diagnostics";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.serviceGroup.ResumeLayout(false);
            this.processGroup.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.logTabs.ResumeLayout(false);
            this.appLogTab.ResumeLayout(false);
            this.appLogTab.PerformLayout();
            this.morphicLogTab.ResumeLayout(false);
            this.morphicLogTab.PerformLayout();
            this.serviceLogTab.ResumeLayout(false);
            this.serviceLogTab.PerformLayout();
            this.metricsTab.ResumeLayout(false);
            this.metricsTab.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox logText;
        private System.Windows.Forms.ComboBox keyInMethod;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button keyOutButton;
        private System.Windows.Forms.Button keyInButton;
        private System.Windows.Forms.ComboBox keyInText;
        private System.Windows.Forms.GroupBox processGroup;
        private System.Windows.Forms.Button killMorphicButton;
        private System.Windows.Forms.Button killIconButton;
        private System.Windows.Forms.Button shutdownButton;
        private System.Windows.Forms.GroupBox serviceGroup;
        private System.Windows.Forms.Button serviceStopButton;
        private System.Windows.Forms.Button serviceStartButton;
        private System.Windows.Forms.Button serviceRestartButton;
        private System.Windows.Forms.TabControl logTabs;
        private System.Windows.Forms.TabPage appLogTab;
        private System.Windows.Forms.TabPage morphicLogTab;
        private System.Windows.Forms.TextBox morphicLogText;
        private System.Windows.Forms.TabPage serviceLogTab;
        private System.Windows.Forms.TextBox serviceLogText;
        private System.Windows.Forms.Button captureLogsButton;
        private System.Windows.Forms.SaveFileDialog saveLogsDialog;
        private System.Windows.Forms.TabPage metricsTab;
        private System.Windows.Forms.TextBox metricsLogText;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox enableMetricsCheckbox;
        private System.Windows.Forms.CheckBox metricsSaveCheckbox;
        private System.Windows.Forms.SaveFileDialog metricsSaveDialog;
        private System.Windows.Forms.LinkLabel metricsSaveFileLink;
        private System.Windows.Forms.CheckBox metricsFilterCheckbox;
    }
}

