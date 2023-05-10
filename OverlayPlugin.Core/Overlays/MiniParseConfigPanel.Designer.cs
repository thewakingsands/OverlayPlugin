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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MiniParseConfigPanel));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabGeneral = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnClearCache = new System.Windows.Forms.Button();
            this.checkMiniParseVisible = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.cbEnableOverlay = new System.Windows.Forms.CheckBox();
            this.cbMuteHidden = new System.Windows.Forms.CheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cbWhiteBg = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.checkMiniParseClickthru = new System.Windows.Forms.CheckBox();
            this.cbHideOutOfCombat = new System.Windows.Forms.CheckBox();
            this.label10 = new System.Windows.Forms.Label();
            this.checkLock = new System.Windows.Forms.CheckBox();
            this.label9 = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonMiniParseSelectFile = new System.Windows.Forms.Button();
            this.textMiniParseUrl = new System.Windows.Forms.TextBox();
            this.applyPresetCombo = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabHotkeys = new System.Windows.Forms.TabPage();
            this.btnApplyHotkeyChanges = new System.Windows.Forms.Button();
            this.btnRemoveHotkey = new System.Windows.Forms.Button();
            this.btnAddHotkey = new System.Windows.Forms.Button();
            this.hotkeyGridView = new System.Windows.Forms.DataGridView();
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
            this.hotkeyColEnabled = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.hotkeyColHotkey = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.hotkeyColAction = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.tabControl1.SuspendLayout();
            this.tabGeneral.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
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
            resources.ApplyResources(this.tabControl1, "tabControl1");
            this.tabControl1.Controls.Add(this.tabGeneral);
            this.tabControl1.Controls.Add(this.tabHotkeys);
            this.tabControl1.Controls.Add(this.tabAdvanced);
            this.tabControl1.Controls.Add(this.tabACTWS);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            // 
            // tabGeneral
            // 
            resources.ApplyResources(this.tabGeneral, "tabGeneral");
            this.tabGeneral.Controls.Add(this.tableLayoutPanel1);
            this.tabGeneral.Name = "tabGeneral";
            this.tabGeneral.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.btnClearCache, 1, 12);
            this.tableLayoutPanel1.Controls.Add(this.checkMiniParseVisible, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label7, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label14, 0, 11);
            this.tableLayoutPanel1.Controls.Add(this.cbEnableOverlay, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.cbMuteHidden, 1, 10);
            this.tableLayoutPanel1.Controls.Add(this.label8, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.label13, 0, 10);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.cbWhiteBg, 1, 8);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 8);
            this.tableLayoutPanel1.Controls.Add(this.label12, 1, 9);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.checkMiniParseClickthru, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.cbHideOutOfCombat, 1, 11);
            this.tableLayoutPanel1.Controls.Add(this.label10, 1, 7);
            this.tableLayoutPanel1.Controls.Add(this.checkLock, 1, 6);
            this.tableLayoutPanel1.Controls.Add(this.label9, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.applyPresetCombo, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // btnClearCache
            // 
            resources.ApplyResources(this.btnClearCache, "btnClearCache");
            this.btnClearCache.Name = "btnClearCache";
            this.btnClearCache.UseVisualStyleBackColor = true;
            this.btnClearCache.Click += new System.EventHandler(this.btnClearCache_Click);
            // 
            // checkMiniParseVisible
            // 
            resources.ApplyResources(this.checkMiniParseVisible, "checkMiniParseVisible");
            this.checkMiniParseVisible.Name = "checkMiniParseVisible";
            this.checkMiniParseVisible.UseVisualStyleBackColor = true;
            this.checkMiniParseVisible.CheckedChanged += new System.EventHandler(this.checkWindowVisible_CheckedChanged);
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // label14
            // 
            resources.ApplyResources(this.label14, "label14");
            this.label14.Name = "label14";
            // 
            // cbEnableOverlay
            // 
            resources.ApplyResources(this.cbEnableOverlay, "cbEnableOverlay");
            this.cbEnableOverlay.Name = "cbEnableOverlay";
            this.cbEnableOverlay.UseVisualStyleBackColor = true;
            this.cbEnableOverlay.CheckedChanged += new System.EventHandler(this.cbEnableOverlay_CheckedChanged);
            // 
            // cbMuteHidden
            // 
            resources.ApplyResources(this.cbMuteHidden, "cbMuteHidden");
            this.cbMuteHidden.Name = "cbMuteHidden";
            this.cbMuteHidden.UseVisualStyleBackColor = true;
            this.cbMuteHidden.CheckedChanged += new System.EventHandler(this.cbMuteHidden_CheckedChanged);
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // label13
            // 
            resources.ApplyResources(this.label13, "label13");
            this.label13.Name = "label13";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // cbWhiteBg
            // 
            resources.ApplyResources(this.cbWhiteBg, "cbWhiteBg");
            this.cbWhiteBg.Name = "cbWhiteBg";
            this.cbWhiteBg.UseVisualStyleBackColor = true;
            this.cbWhiteBg.CheckedChanged += new System.EventHandler(this.cbWhiteBg_CheckedChanged);
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // label12
            // 
            resources.ApplyResources(this.label12, "label12");
            this.label12.Name = "label12";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // checkMiniParseClickthru
            // 
            resources.ApplyResources(this.checkMiniParseClickthru, "checkMiniParseClickthru");
            this.checkMiniParseClickthru.Name = "checkMiniParseClickthru";
            this.checkMiniParseClickthru.UseVisualStyleBackColor = true;
            this.checkMiniParseClickthru.CheckedChanged += new System.EventHandler(this.checkMouseClickthru_CheckedChanged);
            // 
            // cbHideOutOfCombat
            // 
            resources.ApplyResources(this.cbHideOutOfCombat, "cbHideOutOfCombat");
            this.cbHideOutOfCombat.Name = "cbHideOutOfCombat";
            this.cbHideOutOfCombat.UseVisualStyleBackColor = true;
            this.cbHideOutOfCombat.CheckedChanged += new System.EventHandler(this.cbHideOutOfCombat_CheckedChanged);
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            // 
            // checkLock
            // 
            resources.ApplyResources(this.checkLock, "checkLock");
            this.checkLock.Name = "checkLock";
            this.checkLock.UseVisualStyleBackColor = true;
            this.checkLock.CheckedChanged += new System.EventHandler(this.checkLock_CheckedChanged);
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // tableLayoutPanel2
            // 
            resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
            this.tableLayoutPanel2.Controls.Add(this.buttonMiniParseSelectFile, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.textMiniParseUrl, 0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            // 
            // buttonMiniParseSelectFile
            // 
            resources.ApplyResources(this.buttonMiniParseSelectFile, "buttonMiniParseSelectFile");
            this.buttonMiniParseSelectFile.Name = "buttonMiniParseSelectFile";
            this.buttonMiniParseSelectFile.UseVisualStyleBackColor = true;
            this.buttonMiniParseSelectFile.Click += new System.EventHandler(this.buttonMiniParseSelectFile_Click);
            // 
            // textMiniParseUrl
            // 
            resources.ApplyResources(this.textMiniParseUrl, "textMiniParseUrl");
            this.textMiniParseUrl.Name = "textMiniParseUrl";
            this.textMiniParseUrl.TextChanged += new System.EventHandler(this.textMiniParseUrl_TextChanged);
            // 
            // applyPresetCombo
            // 
            this.applyPresetCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.applyPresetCombo.FormattingEnabled = true;
            resources.ApplyResources(this.applyPresetCombo, "applyPresetCombo");
            this.applyPresetCombo.Name = "applyPresetCombo";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // tabHotkeys
            // 
            this.tabHotkeys.Controls.Add(this.btnApplyHotkeyChanges);
            this.tabHotkeys.Controls.Add(this.btnRemoveHotkey);
            this.tabHotkeys.Controls.Add(this.btnAddHotkey);
            this.tabHotkeys.Controls.Add(this.hotkeyGridView);
            resources.ApplyResources(this.tabHotkeys, "tabHotkeys");
            this.tabHotkeys.Name = "tabHotkeys";
            this.tabHotkeys.UseVisualStyleBackColor = true;
            // 
            // btnApplyHotkeyChanges
            // 
            resources.ApplyResources(this.btnApplyHotkeyChanges, "btnApplyHotkeyChanges");
            this.btnApplyHotkeyChanges.Name = "btnApplyHotkeyChanges";
            this.btnApplyHotkeyChanges.UseVisualStyleBackColor = true;
            this.btnApplyHotkeyChanges.Click += new System.EventHandler(this.btnApplyHotkeyChanges_Click);
            // 
            // btnRemoveHotkey
            // 
            resources.ApplyResources(this.btnRemoveHotkey, "btnRemoveHotkey");
            this.btnRemoveHotkey.Name = "btnRemoveHotkey";
            this.btnRemoveHotkey.UseVisualStyleBackColor = true;
            this.btnRemoveHotkey.Click += new System.EventHandler(this.btnRemoveHotkey_Click);
            // 
            // btnAddHotkey
            // 
            resources.ApplyResources(this.btnAddHotkey, "btnAddHotkey");
            this.btnAddHotkey.Name = "btnAddHotkey";
            this.btnAddHotkey.UseVisualStyleBackColor = true;
            this.btnAddHotkey.Click += new System.EventHandler(this.btnAddHotkey_Click);
            // 
            // hotkeyGridView
            // 
            this.hotkeyGridView.AllowUserToAddRows = false;
            this.hotkeyGridView.AllowUserToResizeRows = false;
            resources.ApplyResources(this.hotkeyGridView, "hotkeyGridView");
            this.hotkeyGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.hotkeyGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.hotkeyColEnabled,
            this.hotkeyColHotkey,
            this.hotkeyColAction});
            this.hotkeyGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.hotkeyGridView.GridColor = System.Drawing.SystemColors.ControlDarkDark;
            this.hotkeyGridView.Name = "hotkeyGridView";
            this.hotkeyGridView.RowHeadersVisible = false;
            this.hotkeyGridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.hotkeyGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.hotkeyGridView.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.hotkeyGridView_CellFormatting);
            this.hotkeyGridView.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.hotkeyGridView_CellMouseClick);
            this.hotkeyGridView.CellValidated += new System.Windows.Forms.DataGridViewCellEventHandler(this.hotkeyGridView_CellValidated);
            this.hotkeyGridView.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.hotkeyGridView_EditingControlShowing);
            // 
            // tabAdvanced
            // 
            resources.ApplyResources(this.tabAdvanced, "tabAdvanced");
            this.tabAdvanced.Controls.Add(this.label15);
            this.tabAdvanced.Controls.Add(this.tableLayoutPanel4);
            this.tabAdvanced.Controls.Add(this.checkLogConsoleMessages);
            this.tabAdvanced.Controls.Add(this.label6);
            this.tabAdvanced.Controls.Add(this.nudMaxFrameRate);
            this.tabAdvanced.Name = "tabAdvanced";
            this.tabAdvanced.UseVisualStyleBackColor = true;
            // 
            // label15
            // 
            resources.ApplyResources(this.label15, "label15");
            this.label15.Name = "label15";
            // 
            // tableLayoutPanel4
            // 
            resources.ApplyResources(this.tableLayoutPanel4, "tableLayoutPanel4");
            this.tableLayoutPanel4.Controls.Add(this.tbZoom, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.btnResetZoom, 1, 0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            // 
            // tbZoom
            // 
            resources.ApplyResources(this.tbZoom, "tbZoom");
            this.tbZoom.Maximum = 400;
            this.tbZoom.Minimum = -200;
            this.tbZoom.Name = "tbZoom";
            this.tbZoom.Value = 100;
            this.tbZoom.ValueChanged += new System.EventHandler(this.tbZoom_ValueChanged);
            // 
            // btnResetZoom
            // 
            resources.ApplyResources(this.btnResetZoom, "btnResetZoom");
            this.btnResetZoom.Name = "btnResetZoom";
            this.btnResetZoom.UseVisualStyleBackColor = true;
            this.btnResetZoom.Click += new System.EventHandler(this.btnResetZoom_Click);
            // 
            // checkLogConsoleMessages
            // 
            resources.ApplyResources(this.checkLogConsoleMessages, "checkLogConsoleMessages");
            this.checkLogConsoleMessages.Name = "checkLogConsoleMessages";
            this.checkLogConsoleMessages.UseVisualStyleBackColor = true;
            this.checkLogConsoleMessages.CheckedChanged += new System.EventHandler(this.checkLogConsoleMessages_CheckedChanged);
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // nudMaxFrameRate
            // 
            resources.ApplyResources(this.nudMaxFrameRate, "nudMaxFrameRate");
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
            this.nudMaxFrameRate.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudMaxFrameRate.ValueChanged += new System.EventHandler(this.nudMaxFrameRate_ValueChanged);
            // 
            // tabACTWS
            // 
            resources.ApplyResources(this.tabACTWS, "tabACTWS");
            this.tabACTWS.Controls.Add(this.label5);
            this.tabACTWS.Controls.Add(this.checkActwsCompatbility);
            this.tabACTWS.Controls.Add(this.label11);
            this.tabACTWS.Controls.Add(this.checkNoFocus);
            this.tabACTWS.Controls.Add(this.lblNoFocus);
            this.tabACTWS.Name = "tabACTWS";
            this.tabACTWS.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // checkActwsCompatbility
            // 
            resources.ApplyResources(this.checkActwsCompatbility, "checkActwsCompatbility");
            this.checkActwsCompatbility.Name = "checkActwsCompatbility";
            this.checkActwsCompatbility.UseVisualStyleBackColor = true;
            this.checkActwsCompatbility.CheckedChanged += new System.EventHandler(this.CheckActwsCompatbility_CheckedChanged);
            // 
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.label11.Name = "label11";
            // 
            // checkNoFocus
            // 
            resources.ApplyResources(this.checkNoFocus, "checkNoFocus");
            this.checkNoFocus.Name = "checkNoFocus";
            this.checkNoFocus.UseVisualStyleBackColor = true;
            this.checkNoFocus.CheckedChanged += new System.EventHandler(this.checkNoFocus_CheckedChanged);
            // 
            // lblNoFocus
            // 
            resources.ApplyResources(this.lblNoFocus, "lblNoFocus");
            this.lblNoFocus.Name = "lblNoFocus";
            // 
            // tableLayoutPanel3
            // 
            resources.ApplyResources(this.tableLayoutPanel3, "tableLayoutPanel3");
            this.tableLayoutPanel3.Controls.Add(this.buttonMiniParseOpenDevTools, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.buttonMiniParseReloadBrowser, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.buttonResetOverlayPosition, 2, 0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            // 
            // buttonMiniParseOpenDevTools
            // 
            resources.ApplyResources(this.buttonMiniParseOpenDevTools, "buttonMiniParseOpenDevTools");
            this.buttonMiniParseOpenDevTools.Name = "buttonMiniParseOpenDevTools";
            this.buttonMiniParseOpenDevTools.UseVisualStyleBackColor = true;
            this.buttonMiniParseOpenDevTools.Click += new System.EventHandler(this.buttonMiniParseOpenDevTools_Click);
            this.buttonMiniParseOpenDevTools.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonMiniParseOpenDevTools_RClick);
            // 
            // buttonMiniParseReloadBrowser
            // 
            resources.ApplyResources(this.buttonMiniParseReloadBrowser, "buttonMiniParseReloadBrowser");
            this.buttonMiniParseReloadBrowser.Name = "buttonMiniParseReloadBrowser";
            this.buttonMiniParseReloadBrowser.UseVisualStyleBackColor = true;
            this.buttonMiniParseReloadBrowser.Click += new System.EventHandler(this.buttonReloadBrowser_Click);
            // 
            // buttonResetOverlayPosition
            // 
            resources.ApplyResources(this.buttonResetOverlayPosition, "buttonResetOverlayPosition");
            this.buttonResetOverlayPosition.Name = "buttonResetOverlayPosition";
            this.buttonResetOverlayPosition.UseVisualStyleBackColor = true;
            this.buttonResetOverlayPosition.Click += new System.EventHandler(this.buttonResetOverlayPosition_Click);
            // 
            // hotkeyColEnabled
            // 
            this.hotkeyColEnabled.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            resources.ApplyResources(this.hotkeyColEnabled, "hotkeyColEnabled");
            this.hotkeyColEnabled.Name = "hotkeyColEnabled";
            // 
            // hotkeyColHotkey
            // 
            this.hotkeyColHotkey.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            resources.ApplyResources(this.hotkeyColHotkey, "hotkeyColHotkey");
            this.hotkeyColHotkey.Name = "hotkeyColHotkey";
            this.hotkeyColHotkey.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // hotkeyColAction
            // 
            this.hotkeyColAction.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            resources.ApplyResources(this.hotkeyColAction, "hotkeyColAction");
            this.hotkeyColAction.Name = "hotkeyColAction";
            // 
            // MiniParseConfigPanel
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Controls.Add(this.tableLayoutPanel3);
            this.Controls.Add(this.tabControl1);
            this.Name = "MiniParseConfigPanel";
            this.tabControl1.ResumeLayout(false);
            this.tabGeneral.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
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
        private System.Windows.Forms.TabPage tabHotkeys;
        private System.Windows.Forms.CheckBox checkNoFocus;
        private System.Windows.Forms.Label lblNoFocus;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox checkActwsCompatbility;
        private System.Windows.Forms.NumericUpDown nudMaxFrameRate;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Button buttonMiniParseOpenDevTools;
        private System.Windows.Forms.Button buttonMiniParseReloadBrowser;
        private System.Windows.Forms.Button buttonResetOverlayPosition;
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
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnClearCache;
        private System.Windows.Forms.CheckBox checkMiniParseVisible;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.CheckBox cbEnableOverlay;
        private System.Windows.Forms.CheckBox cbMuteHidden;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox cbWhiteBg;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox checkMiniParseClickthru;
        private System.Windows.Forms.CheckBox cbHideOutOfCombat;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.CheckBox checkLock;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button buttonMiniParseSelectFile;
        private System.Windows.Forms.TextBox textMiniParseUrl;
        private System.Windows.Forms.ComboBox applyPresetCombo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn hotkeyColEnabled;
        private System.Windows.Forms.DataGridViewTextBoxColumn hotkeyColHotkey;
        private System.Windows.Forms.DataGridViewComboBoxColumn hotkeyColAction;
    }
}