namespace RainbowMage.OverlayPlugin.Overlays
{
    partial class MiniParseConfigPanel
    {
        /// <summary> 
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region コンポーネント デザイナーで生成されたコード

        /// <summary> 
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を 
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabGeneral = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.cbWhiteBg = new System.Windows.Forms.CheckBox();
            this.label12 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.applyPresetCombo = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.textMiniParseUrl = new System.Windows.Forms.TextBox();
            this.buttonMiniParseSelectFile = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label10 = new System.Windows.Forms.Label();
            this.checkLock = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cbEnableOverlay = new System.Windows.Forms.CheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cbHideOutOfCombat = new System.Windows.Forms.CheckBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.cbMuteHidden = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.checkMiniParseClickthru = new System.Windows.Forms.CheckBox();
            this.checkMiniParseVisible = new System.Windows.Forms.CheckBox();
            this.label9 = new System.Windows.Forms.Label();
            this.tabHotkeys = new System.Windows.Forms.TabPage();
            this.btnApplyHotkeyChanges = new System.Windows.Forms.Button();
            this.btnRemoveHotkey = new System.Windows.Forms.Button();
            this.btnAddHotkey = new System.Windows.Forms.Button();
            this.hotkeyGridView = new System.Windows.Forms.DataGridView();
            this.hotkeyColEnabled = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.hotkeyColHotkey = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.hotkeyColAction = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.tabAdvanced = new System.Windows.Forms.TabPage();
            this.label15 = new System.Windows.Forms.Label();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.tbZoom = new System.Windows.Forms.TrackBar();
            this.btnResetZoom = new System.Windows.Forms.Button();
            this.checkLogConsoleMessages = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.nudMaxFrameRate = new System.Windows.Forms.NumericUpDown();
            this.tabACTWS = new System.Windows.Forms.TabPage();
            this.label5 = new System.Windows.Forms.Label();
            this.checkActwsCompatbility = new System.Windows.Forms.CheckBox();
            this.label11 = new System.Windows.Forms.Label();
            this.checkNoFocus = new System.Windows.Forms.CheckBox();
            this.lblNoFocus = new System.Windows.Forms.Label();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonMiniParseOpenDevTools = new System.Windows.Forms.Button();
            this.buttonMiniParseReloadBrowser = new System.Windows.Forms.Button();
            this.buttonResetOverlayPosition = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabGeneral.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabHotkeys.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.hotkeyGridView)).BeginInit();
            this.tabAdvanced.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbZoom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMaxFrameRate)).BeginInit();
            this.tabACTWS.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabGeneral);
            this.tabControl1.Controls.Add(this.tabHotkeys);
            this.tabControl1.Controls.Add(this.tabAdvanced);
            this.tabControl1.Controls.Add(this.tabACTWS);
            this.tabControl1.Location = new System.Drawing.Point(0, 4);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1169, 507);
            this.tabControl1.TabIndex = 42;
            // 
            // tabGeneral
            // 
            this.tabGeneral.Controls.Add(this.tableLayoutPanel1);
            this.tabGeneral.Location = new System.Drawing.Point(4, 28);
            this.tabGeneral.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabGeneral.Name = "tabGeneral";
            this.tabGeneral.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabGeneral.Size = new System.Drawing.Size(1161, 475);
            this.tabGeneral.TabIndex = 0;
            this.tabGeneral.Text = "通用";
            this.tabGeneral.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoScroll = true;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 300F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panel4, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.panel3, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.cbHideOutOfCombat, 1, 7);
            this.tableLayoutPanel1.Controls.Add(this.label14, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.label7, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.cbMuteHidden, 1, 6);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.label13, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.checkMiniParseClickthru, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.checkMiniParseVisible, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label9, 0, 5);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(4, 4);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 8;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1153, 467);
            this.tableLayoutPanel1.TabIndex = 44;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.cbWhiteBg);
            this.panel4.Controls.Add(this.label12);
            this.panel4.Location = new System.Drawing.Point(300, 250);
            this.panel4.Margin = new System.Windows.Forms.Padding(0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(629, 69);
            this.panel4.TabIndex = 45;
            // 
            // cbWhiteBg
            // 
            this.cbWhiteBg.AutoSize = true;
            this.cbWhiteBg.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.cbWhiteBg.Location = new System.Drawing.Point(4, 4);
            this.cbWhiteBg.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbWhiteBg.Name = "cbWhiteBg";
            this.cbWhiteBg.Size = new System.Drawing.Size(22, 21);
            this.cbWhiteBg.TabIndex = 33;
            this.cbWhiteBg.UseVisualStyleBackColor = true;
            this.cbWhiteBg.CheckedChanged += new System.EventHandler(this.cbWhiteBg_CheckedChanged);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label12.Location = new System.Drawing.Point(4, 30);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(584, 18);
            this.label12.TabIndex = 34;
            this.label12.Text = "如果您找不到悬浮窗或由于某种原因无法调整其大小时，请启用此选项。";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.applyPresetCombo);
            this.panel3.Controls.Add(this.tableLayoutPanel2);
            this.panel3.Location = new System.Drawing.Point(300, 158);
            this.panel3.Margin = new System.Windows.Forms.Padding(0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(629, 92);
            this.panel3.TabIndex = 45;
            // 
            // applyPresetCombo
            // 
            this.applyPresetCombo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.applyPresetCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.applyPresetCombo.FormattingEnabled = true;
            this.applyPresetCombo.Location = new System.Drawing.Point(4, 44);
            this.applyPresetCombo.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.applyPresetCombo.Name = "applyPresetCombo";
            this.applyPresetCombo.Size = new System.Drawing.Size(621, 26);
            this.applyPresetCombo.TabIndex = 41;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 68F));
            this.tableLayoutPanel2.Controls.Add(this.textMiniParseUrl, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.buttonMiniParseSelectFile, 1, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(4, 0);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(623, 26);
            this.tableLayoutPanel2.TabIndex = 27;
            // 
            // textMiniParseUrl
            // 
            this.textMiniParseUrl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textMiniParseUrl.Location = new System.Drawing.Point(0, 0);
            this.textMiniParseUrl.Margin = new System.Windows.Forms.Padding(0);
            this.textMiniParseUrl.Name = "textMiniParseUrl";
            this.textMiniParseUrl.Size = new System.Drawing.Size(555, 28);
            this.textMiniParseUrl.TabIndex = 3;
            this.textMiniParseUrl.Leave += new System.EventHandler(this.textMiniParseUrl_Leave);
            // 
            // buttonMiniParseSelectFile
            // 
            this.buttonMiniParseSelectFile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonMiniParseSelectFile.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.buttonMiniParseSelectFile.Location = new System.Drawing.Point(555, 0);
            this.buttonMiniParseSelectFile.Margin = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.buttonMiniParseSelectFile.Name = "buttonMiniParseSelectFile";
            this.buttonMiniParseSelectFile.Size = new System.Drawing.Size(68, 24);
            this.buttonMiniParseSelectFile.TabIndex = 4;
            this.buttonMiniParseSelectFile.Text = "浏览...";
            this.buttonMiniParseSelectFile.UseVisualStyleBackColor = true;
            this.buttonMiniParseSelectFile.Click += new System.EventHandler(this.buttonSelectFile_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label10);
            this.panel2.Controls.Add(this.checkLock);
            this.panel2.Location = new System.Drawing.Point(300, 319);
            this.panel2.Margin = new System.Windows.Forms.Padding(0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(629, 86);
            this.panel2.TabIndex = 45;
            // 
            // label10
            // 
            this.label10.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label10.Location = new System.Drawing.Point(0, 40);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(629, 46);
            this.label10.TabIndex = 38;
            this.label10.Text = "如果某悬浮窗被锁定，其位置将固定不变，只有将其再次解锁才能移动。\r\n某些悬浮窗(如cactbot)锁定后方可正常工作。";
            // 
            // checkLock
            // 
            this.checkLock.AutoSize = true;
            this.checkLock.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.checkLock.Location = new System.Drawing.Point(4, 4);
            this.checkLock.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.checkLock.Name = "checkLock";
            this.checkLock.Size = new System.Drawing.Size(22, 21);
            this.checkLock.TabIndex = 25;
            this.checkLock.UseVisualStyleBackColor = true;
            this.checkLock.CheckedChanged += new System.EventHandler(this.checkLock_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cbEnableOverlay);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Location = new System.Drawing.Point(300, 29);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(629, 100);
            this.panel1.TabIndex = 45;
            // 
            // cbEnableOverlay
            // 
            this.cbEnableOverlay.AutoSize = true;
            this.cbEnableOverlay.Location = new System.Drawing.Point(4, 4);
            this.cbEnableOverlay.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbEnableOverlay.Name = "cbEnableOverlay";
            this.cbEnableOverlay.Size = new System.Drawing.Size(22, 21);
            this.cbEnableOverlay.TabIndex = 36;
            this.cbEnableOverlay.UseVisualStyleBackColor = true;
            this.cbEnableOverlay.CheckedChanged += new System.EventHandler(this.cbEnableOverlay_CheckedChanged);
            // 
            // label8
            // 
            this.label8.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label8.Location = new System.Drawing.Point(0, 34);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(629, 66);
            this.label8.TabIndex = 37;
            this.label8.Text = "禁用某个悬浮窗会减少CPU使用率，但也会停止它的所有工作（例如声音）。\r\n隐藏某个悬浮窗只会使其不可见，它仍然在后台运行，并仍可以播放声音、收集统计信息等。\r\n";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label3.Location = new System.Drawing.Point(4, 0);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(98, 18);
            this.label3.TabIndex = 0;
            this.label3.Text = "显示悬浮窗";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cbHideOutOfCombat
            // 
            this.cbHideOutOfCombat.AutoSize = true;
            this.cbHideOutOfCombat.Location = new System.Drawing.Point(304, 438);
            this.cbHideOutOfCombat.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbHideOutOfCombat.Name = "cbHideOutOfCombat";
            this.cbHideOutOfCombat.Size = new System.Drawing.Size(22, 21);
            this.cbHideOutOfCombat.TabIndex = 42;
            this.cbHideOutOfCombat.UseVisualStyleBackColor = true;
            this.cbHideOutOfCombat.CheckedChanged += new System.EventHandler(this.cbHideOutOfCombat_CheckedChanged);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label14.Location = new System.Drawing.Point(4, 434);
            this.label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(152, 18);
            this.label14.TabIndex = 43;
            this.label14.Text = "脱战时隐藏悬浮窗";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(4, 29);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(98, 18);
            this.label7.TabIndex = 35;
            this.label7.Text = "启用悬浮窗";
            // 
            // cbMuteHidden
            // 
            this.cbMuteHidden.AutoSize = true;
            this.cbMuteHidden.Location = new System.Drawing.Point(304, 409);
            this.cbMuteHidden.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbMuteHidden.Name = "cbMuteHidden";
            this.cbMuteHidden.Size = new System.Drawing.Size(22, 21);
            this.cbMuteHidden.TabIndex = 40;
            this.cbMuteHidden.UseVisualStyleBackColor = true;
            this.cbMuteHidden.CheckedChanged += new System.EventHandler(this.cbMuteHidden_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label2.Location = new System.Drawing.Point(4, 129);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 18);
            this.label2.TabIndex = 0;
            this.label2.Text = "鼠标穿透";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label1.Location = new System.Drawing.Point(4, 158);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 18);
            this.label1.TabIndex = 26;
            this.label1.Text = "悬浮窗路径";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label4.Location = new System.Drawing.Point(4, 250);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(242, 18);
            this.label4.TabIndex = 32;
            this.label4.Text = "在悬浮窗下强制启用白色背景";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(4, 405);
            this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(170, 18);
            this.label13.TabIndex = 39;
            this.label13.Text = "隐藏时使悬浮窗静音";
            // 
            // checkMiniParseClickthru
            // 
            this.checkMiniParseClickthru.AutoSize = true;
            this.checkMiniParseClickthru.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.checkMiniParseClickthru.Location = new System.Drawing.Point(304, 133);
            this.checkMiniParseClickthru.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.checkMiniParseClickthru.Name = "checkMiniParseClickthru";
            this.checkMiniParseClickthru.Size = new System.Drawing.Size(22, 21);
            this.checkMiniParseClickthru.TabIndex = 2;
            this.checkMiniParseClickthru.UseVisualStyleBackColor = true;
            this.checkMiniParseClickthru.CheckedChanged += new System.EventHandler(this.checkMouseClickthru_CheckedChanged);
            // 
            // checkMiniParseVisible
            // 
            this.checkMiniParseVisible.AutoSize = true;
            this.checkMiniParseVisible.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.checkMiniParseVisible.Location = new System.Drawing.Point(304, 4);
            this.checkMiniParseVisible.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.checkMiniParseVisible.Name = "checkMiniParseVisible";
            this.checkMiniParseVisible.Size = new System.Drawing.Size(22, 21);
            this.checkMiniParseVisible.TabIndex = 1;
            this.checkMiniParseVisible.UseVisualStyleBackColor = true;
            this.checkMiniParseVisible.CheckedChanged += new System.EventHandler(this.checkWindowVisible_CheckedChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label9.Location = new System.Drawing.Point(4, 319);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(98, 18);
            this.label9.TabIndex = 23;
            this.label9.Text = "锁定悬浮窗";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tabHotkeys
            // 
            this.tabHotkeys.Controls.Add(this.btnApplyHotkeyChanges);
            this.tabHotkeys.Controls.Add(this.btnRemoveHotkey);
            this.tabHotkeys.Controls.Add(this.btnAddHotkey);
            this.tabHotkeys.Controls.Add(this.hotkeyGridView);
            this.tabHotkeys.Location = new System.Drawing.Point(4, 28);
            this.tabHotkeys.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabHotkeys.Name = "tabHotkeys";
            this.tabHotkeys.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabHotkeys.Size = new System.Drawing.Size(1171, 535);
            this.tabHotkeys.TabIndex = 1;
            this.tabHotkeys.Text = "快捷键";
            this.tabHotkeys.UseVisualStyleBackColor = true;
            // 
            // btnApplyHotkeyChanges
            // 
            this.btnApplyHotkeyChanges.Location = new System.Drawing.Point(544, 9);
            this.btnApplyHotkeyChanges.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnApplyHotkeyChanges.Name = "btnApplyHotkeyChanges";
            this.btnApplyHotkeyChanges.Size = new System.Drawing.Size(188, 39);
            this.btnApplyHotkeyChanges.TabIndex = 3;
            this.btnApplyHotkeyChanges.Text = "应用更改";
            this.btnApplyHotkeyChanges.UseVisualStyleBackColor = true;
            this.btnApplyHotkeyChanges.Click += new System.EventHandler(this.btnApplyHotkeyChanges_Click);
            // 
            // btnRemoveHotkey
            // 
            this.btnRemoveHotkey.Location = new System.Drawing.Point(246, 9);
            this.btnRemoveHotkey.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnRemoveHotkey.Name = "btnRemoveHotkey";
            this.btnRemoveHotkey.Size = new System.Drawing.Size(270, 39);
            this.btnRemoveHotkey.TabIndex = 2;
            this.btnRemoveHotkey.Text = "删除选定快捷键";
            this.btnRemoveHotkey.UseVisualStyleBackColor = true;
            this.btnRemoveHotkey.Click += new System.EventHandler(this.btnRemoveHotkey_Click);
            // 
            // btnAddHotkey
            // 
            this.btnAddHotkey.Location = new System.Drawing.Point(10, 9);
            this.btnAddHotkey.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnAddHotkey.Name = "btnAddHotkey";
            this.btnAddHotkey.Size = new System.Drawing.Size(208, 39);
            this.btnAddHotkey.TabIndex = 1;
            this.btnAddHotkey.Text = "添加新快捷键";
            this.btnAddHotkey.UseVisualStyleBackColor = true;
            this.btnAddHotkey.Click += new System.EventHandler(this.btnAddHotkey_Click);
            // 
            // hotkeyGridView
            // 
            this.hotkeyGridView.AllowUserToAddRows = false;
            this.hotkeyGridView.AllowUserToResizeRows = false;
            this.hotkeyGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.hotkeyGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.hotkeyGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.hotkeyGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.hotkeyColEnabled,
            this.hotkeyColHotkey,
            this.hotkeyColAction});
            this.hotkeyGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.hotkeyGridView.Location = new System.Drawing.Point(9, 57);
            this.hotkeyGridView.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.hotkeyGridView.Name = "hotkeyGridView";
            this.hotkeyGridView.RowHeadersVisible = false;
            this.hotkeyGridView.RowHeadersWidth = 62;
            this.hotkeyGridView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.hotkeyGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.hotkeyGridView.Size = new System.Drawing.Size(1004, 554);
            this.hotkeyGridView.TabIndex = 0;
            this.hotkeyGridView.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.hotkeyGridView_CellFormatting);
            this.hotkeyGridView.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.hotkeyGridView_CellMouseClick);
            this.hotkeyGridView.CellValidated += new System.Windows.Forms.DataGridViewCellEventHandler(this.hotkeyGridView_CellValidated);
            this.hotkeyGridView.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.hotkeyGridView_EditingControlShowing);
            // 
            // hotkeyColEnabled
            // 
            this.hotkeyColEnabled.HeaderText = "启用";
            this.hotkeyColEnabled.MinimumWidth = 50;
            this.hotkeyColEnabled.Name = "hotkeyColEnabled";
            this.hotkeyColEnabled.Width = 50;
            // 
            // hotkeyColHotkey
            // 
            this.hotkeyColHotkey.HeaderText = "热键";
            this.hotkeyColHotkey.MinimumWidth = 150;
            this.hotkeyColHotkey.Name = "hotkeyColHotkey";
            this.hotkeyColHotkey.Width = 150;
            // 
            // hotkeyColAction
            // 
            this.hotkeyColAction.HeaderText = "行为";
            this.hotkeyColAction.MinimumWidth = 150;
            this.hotkeyColAction.Name = "hotkeyColAction";
            this.hotkeyColAction.Width = 150;
            // 
            // tabAdvanced
            // 
            this.tabAdvanced.AutoScroll = true;
            this.tabAdvanced.Controls.Add(this.label15);
            this.tabAdvanced.Controls.Add(this.tableLayoutPanel4);
            this.tabAdvanced.Controls.Add(this.checkLogConsoleMessages);
            this.tabAdvanced.Controls.Add(this.label6);
            this.tabAdvanced.Controls.Add(this.nudMaxFrameRate);
            this.tabAdvanced.Location = new System.Drawing.Point(4, 28);
            this.tabAdvanced.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabAdvanced.Name = "tabAdvanced";
            this.tabAdvanced.Size = new System.Drawing.Size(1171, 535);
            this.tabAdvanced.TabIndex = 2;
            this.tabAdvanced.Text = "高级";
            this.tabAdvanced.UseVisualStyleBackColor = true;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label15.Location = new System.Drawing.Point(6, 86);
            this.label15.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(44, 18);
            this.label15.TabIndex = 37;
            this.label15.Text = "缩放";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 141F));
            this.tableLayoutPanel4.Controls.Add(this.tbZoom, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.btnResetZoom, 1, 0);
            this.tableLayoutPanel4.Location = new System.Drawing.Point(240, 81);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(759, 45);
            this.tableLayoutPanel4.TabIndex = 42;
            // 
            // tbZoom
            // 
            this.tbZoom.AutoSize = false;
            this.tbZoom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbZoom.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.tbZoom.Location = new System.Drawing.Point(4, 4);
            this.tbZoom.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbZoom.Maximum = 400;
            this.tbZoom.Minimum = -200;
            this.tbZoom.Name = "tbZoom";
            this.tbZoom.Size = new System.Drawing.Size(610, 37);
            this.tbZoom.TabIndex = 40;
            this.tbZoom.Value = 100;
            this.tbZoom.ValueChanged += new System.EventHandler(this.tbZoom_ValueChanged);
            // 
            // btnResetZoom
            // 
            this.btnResetZoom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnResetZoom.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnResetZoom.Location = new System.Drawing.Point(622, 4);
            this.btnResetZoom.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnResetZoom.Name = "btnResetZoom";
            this.btnResetZoom.Size = new System.Drawing.Size(133, 37);
            this.btnResetZoom.TabIndex = 41;
            this.btnResetZoom.Text = "重设";
            this.btnResetZoom.UseVisualStyleBackColor = true;
            this.btnResetZoom.Click += new System.EventHandler(this.btnResetZoom_Click);
            // 
            // checkLogConsoleMessages
            // 
            this.checkLogConsoleMessages.AutoSize = true;
            this.checkLogConsoleMessages.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.checkLogConsoleMessages.Location = new System.Drawing.Point(240, 50);
            this.checkLogConsoleMessages.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.checkLogConsoleMessages.Name = "checkLogConsoleMessages";
            this.checkLogConsoleMessages.Size = new System.Drawing.Size(358, 22);
            this.checkLogConsoleMessages.TabIndex = 40;
            this.checkLogConsoleMessages.Text = "在日志中显示来自此悬浮窗的控制台消息";
            this.checkLogConsoleMessages.UseVisualStyleBackColor = true;
            this.checkLogConsoleMessages.CheckedChanged += new System.EventHandler(this.checkLogConsoleMessages_CheckedChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label6.Location = new System.Drawing.Point(6, 15);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(98, 18);
            this.label6.TabIndex = 12;
            this.label6.Text = "最大帧速率";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // nudMaxFrameRate
            // 
            this.nudMaxFrameRate.Location = new System.Drawing.Point(240, 16);
            this.nudMaxFrameRate.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.nudMaxFrameRate.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.nudMaxFrameRate.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudMaxFrameRate.Name = "nudMaxFrameRate";
            this.nudMaxFrameRate.Size = new System.Drawing.Size(648, 28);
            this.nudMaxFrameRate.TabIndex = 13;
            this.nudMaxFrameRate.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudMaxFrameRate.ValueChanged += new System.EventHandler(this.nudMaxFrameRate_ValueChanged);
            // 
            // tabACTWS
            // 
            this.tabACTWS.AutoScroll = true;
            this.tabACTWS.Controls.Add(this.label5);
            this.tabACTWS.Controls.Add(this.checkActwsCompatbility);
            this.tabACTWS.Controls.Add(this.label11);
            this.tabACTWS.Controls.Add(this.checkNoFocus);
            this.tabACTWS.Controls.Add(this.lblNoFocus);
            this.tabACTWS.Location = new System.Drawing.Point(4, 28);
            this.tabACTWS.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabACTWS.Name = "tabACTWS";
            this.tabACTWS.Size = new System.Drawing.Size(1171, 535);
            this.tabACTWS.TabIndex = 3;
            this.tabACTWS.Text = "ACTWS";
            this.tabACTWS.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label5.Location = new System.Drawing.Point(4, 14);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(107, 18);
            this.label5.TabIndex = 30;
            this.label5.Text = "ACTWS兼容性";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // checkActwsCompatbility
            // 
            this.checkActwsCompatbility.AutoSize = true;
            this.checkActwsCompatbility.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.checkActwsCompatbility.Location = new System.Drawing.Point(232, 14);
            this.checkActwsCompatbility.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.checkActwsCompatbility.Name = "checkActwsCompatbility";
            this.checkActwsCompatbility.Size = new System.Drawing.Size(286, 22);
            this.checkActwsCompatbility.TabIndex = 28;
            this.checkActwsCompatbility.Text = "此悬浮窗需要使用ACTWebSocket";
            this.checkActwsCompatbility.UseVisualStyleBackColor = true;
            this.checkActwsCompatbility.CheckedChanged += new System.EventHandler(this.CheckActwsCompatbility_CheckedChanged);
            // 
            // label11
            // 
            this.label11.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label11.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label11.Location = new System.Drawing.Point(228, 42);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(906, 48);
            this.label11.TabIndex = 31;
            this.label11.Text = "仅当悬浮窗与OverlayPlugin不兼容时，才需要启用此选项。";
            // 
            // checkNoFocus
            // 
            this.checkNoFocus.AutoSize = true;
            this.checkNoFocus.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.checkNoFocus.Location = new System.Drawing.Point(232, 94);
            this.checkNoFocus.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.checkNoFocus.Name = "checkNoFocus";
            this.checkNoFocus.Size = new System.Drawing.Size(160, 22);
            this.checkNoFocus.TabIndex = 38;
            this.checkNoFocus.Text = "不焦点此悬浮窗";
            this.checkNoFocus.UseVisualStyleBackColor = true;
            this.checkNoFocus.CheckedChanged += new System.EventHandler(this.checkNoFocus_CheckedChanged);
            // 
            // lblNoFocus
            // 
            this.lblNoFocus.AutoSize = true;
            this.lblNoFocus.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblNoFocus.Location = new System.Drawing.Point(4, 96);
            this.lblNoFocus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblNoFocus.Name = "lblNoFocus";
            this.lblNoFocus.Size = new System.Drawing.Size(80, 18);
            this.lblNoFocus.TabIndex = 35;
            this.lblNoFocus.Text = "取消焦点";
            this.lblNoFocus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel3.ColumnCount = 3;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 358F));
            this.tableLayoutPanel3.Controls.Add(this.buttonMiniParseOpenDevTools, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.buttonMiniParseReloadBrowser, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.buttonResetOverlayPosition, 2, 0);
            this.tableLayoutPanel3.Location = new System.Drawing.Point(4, 519);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(1165, 60);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // buttonMiniParseOpenDevTools
            // 
            this.buttonMiniParseOpenDevTools.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonMiniParseOpenDevTools.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.buttonMiniParseOpenDevTools.Location = new System.Drawing.Point(4, 4);
            this.buttonMiniParseOpenDevTools.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonMiniParseOpenDevTools.Name = "buttonMiniParseOpenDevTools";
            this.buttonMiniParseOpenDevTools.Size = new System.Drawing.Size(395, 52);
            this.buttonMiniParseOpenDevTools.TabIndex = 2;
            this.buttonMiniParseOpenDevTools.Text = "打开 DevTools";
            this.buttonMiniParseOpenDevTools.UseVisualStyleBackColor = true;
            this.buttonMiniParseOpenDevTools.Click += new System.EventHandler(this.buttonMiniParseOpenDevTools_Click);
            this.buttonMiniParseOpenDevTools.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonMiniParseOpenDevTools_RClick);
            // 
            // buttonMiniParseReloadBrowser
            // 
            this.buttonMiniParseReloadBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonMiniParseReloadBrowser.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.buttonMiniParseReloadBrowser.Location = new System.Drawing.Point(407, 4);
            this.buttonMiniParseReloadBrowser.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonMiniParseReloadBrowser.Name = "buttonMiniParseReloadBrowser";
            this.buttonMiniParseReloadBrowser.Size = new System.Drawing.Size(395, 52);
            this.buttonMiniParseReloadBrowser.TabIndex = 0;
            this.buttonMiniParseReloadBrowser.Text = "刷新悬浮窗";
            this.buttonMiniParseReloadBrowser.UseVisualStyleBackColor = true;
            this.buttonMiniParseReloadBrowser.Click += new System.EventHandler(this.buttonReloadBrowser_Click);
            // 
            // buttonResetOverlayPosition
            // 
            this.buttonResetOverlayPosition.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonResetOverlayPosition.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.buttonResetOverlayPosition.Location = new System.Drawing.Point(810, 4);
            this.buttonResetOverlayPosition.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonResetOverlayPosition.Name = "buttonResetOverlayPosition";
            this.buttonResetOverlayPosition.Size = new System.Drawing.Size(351, 52);
            this.buttonResetOverlayPosition.TabIndex = 3;
            this.buttonResetOverlayPosition.Text = "重置悬浮窗位置";
            this.buttonResetOverlayPosition.UseVisualStyleBackColor = true;
            this.buttonResetOverlayPosition.Click += new System.EventHandler(this.buttonResetOverlayPosition_Click);
            // 
            // MiniParseConfigPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Controls.Add(this.tableLayoutPanel3);
            this.Controls.Add(this.tabControl1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "MiniParseConfigPanel";
            this.Size = new System.Drawing.Size(1184, 597);
            this.tabControl1.ResumeLayout(false);
            this.tabGeneral.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabHotkeys.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.hotkeyGridView)).EndInit();
            this.tabAdvanced.ResumeLayout(false);
            this.tabAdvanced.PerformLayout();
            this.tableLayoutPanel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tbZoom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMaxFrameRate)).EndInit();
            this.tabACTWS.ResumeLayout(false);
            this.tabACTWS.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabGeneral;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox checkMiniParseClickthru;
        private System.Windows.Forms.CheckBox checkMiniParseVisible;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TabPage tabHotkeys;
        private System.Windows.Forms.CheckBox checkNoFocus;
        private System.Windows.Forms.Label lblNoFocus;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.CheckBox cbWhiteBg;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox checkActwsCompatbility;
        private System.Windows.Forms.NumericUpDown nudMaxFrameRate;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Button buttonMiniParseOpenDevTools;
        private System.Windows.Forms.Button buttonMiniParseReloadBrowser;
        private System.Windows.Forms.Button buttonResetOverlayPosition;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TextBox textMiniParseUrl;
        private System.Windows.Forms.Button buttonMiniParseSelectFile;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkLock;
        private System.Windows.Forms.DataGridView hotkeyGridView;
        private System.Windows.Forms.TabPage tabAdvanced;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.TrackBar tbZoom;
        private System.Windows.Forms.Button btnResetZoom;
        private System.Windows.Forms.CheckBox checkLogConsoleMessages;
        private System.Windows.Forms.TabPage tabACTWS;
        private System.Windows.Forms.Button btnAddHotkey;
        private System.Windows.Forms.Button btnRemoveHotkey;
        private System.Windows.Forms.Button btnApplyHotkeyChanges;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox cbEnableOverlay;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox cbMuteHidden;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ComboBox applyPresetCombo;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.CheckBox cbHideOutOfCombat;
        private System.Windows.Forms.DataGridViewCheckBoxColumn hotkeyColEnabled;
        private System.Windows.Forms.DataGridViewTextBoxColumn hotkeyColHotkey;
        private System.Windows.Forms.DataGridViewComboBoxColumn hotkeyColAction;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel4;
    }
}
