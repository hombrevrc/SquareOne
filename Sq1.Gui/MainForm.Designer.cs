﻿namespace Sq1.Gui {
	partial class MainForm {
		private System.ComponentModel.IContainer components = null;

		protected override void Dispose(bool disposing) {
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent() {
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			WeifenLuo.WinFormsUI.Docking.DockPanelSkin dockPanelSkin1 = new WeifenLuo.WinFormsUI.Docking.DockPanelSkin();
			WeifenLuo.WinFormsUI.Docking.AutoHideStripSkin autoHideStripSkin1 = new WeifenLuo.WinFormsUI.Docking.AutoHideStripSkin();
			WeifenLuo.WinFormsUI.Docking.DockPanelGradient dockPanelGradient1 = new WeifenLuo.WinFormsUI.Docking.DockPanelGradient();
			WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient1 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
			WeifenLuo.WinFormsUI.Docking.DockPaneStripSkin dockPaneStripSkin1 = new WeifenLuo.WinFormsUI.Docking.DockPaneStripSkin();
			WeifenLuo.WinFormsUI.Docking.DockPaneStripGradient dockPaneStripGradient1 = new WeifenLuo.WinFormsUI.Docking.DockPaneStripGradient();
			WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient2 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
			WeifenLuo.WinFormsUI.Docking.DockPanelGradient dockPanelGradient2 = new WeifenLuo.WinFormsUI.Docking.DockPanelGradient();
			WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient3 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
			WeifenLuo.WinFormsUI.Docking.DockPaneStripToolWindowGradient dockPaneStripToolWindowGradient1 = new WeifenLuo.WinFormsUI.Docking.DockPaneStripToolWindowGradient();
			WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient4 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
			WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient5 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
			WeifenLuo.WinFormsUI.Docking.DockPanelGradient dockPanelGradient3 = new WeifenLuo.WinFormsUI.Docking.DockPanelGradient();
			WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient6 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
			WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient7 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
			this.ctxTools = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.mniDllVersions = new System.Windows.Forms.ToolStripMenuItem();
			this.ctxDllVersions = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.menuItemDllVersions = new Sq1.Widgets.DllVersions.MenuItemDllVersions();
			this.mniSaveScreenshot = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
			this.mniDistributedBacktester = new System.Windows.Forms.ToolStripMenuItem();
			this.mniFuturesMerger = new System.Windows.Forms.ToolStripMenuItem();
			this.mniBarsEditor = new System.Windows.Forms.ToolStripMenuItem();
			this.mniChartSettingsEditor = new System.Windows.Forms.ToolStripMenuItem();
			this.mniSymbolInfoEditor = new System.Windows.Forms.ToolStripMenuItem();
			this.mniCsvImporter = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.mniExecution = new System.Windows.Forms.ToolStripMenuItem();
			this.mniSliders = new System.Windows.Forms.ToolStripMenuItem();
			this.mniStrategies = new System.Windows.Forms.ToolStripMenuItem();
			this.mniDataSources = new System.Windows.Forms.ToolStripMenuItem();
			this.mniExceptions = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.mniExit = new System.Windows.Forms.ToolStripMenuItem();
			this.mniMainMenu = new System.Windows.Forms.ToolStripDropDownButton();
			this.DockPanel = new WeifenLuo.WinFormsUI.Docking.DockPanel();
			this.mainFormStatusStrip = new System.Windows.Forms.StatusStrip();
			this.mniWindows = new System.Windows.Forms.ToolStripDropDownButton();
			this.ctxWindows = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.mniWorkSpaces = new System.Windows.Forms.ToolStripDropDownButton();
			this.CtxWorkspaces = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.btnFullScreen = new System.Windows.Forms.ToolStripButton();
			this.lblSpace = new System.Windows.Forms.ToolStripStatusLabel();
			this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
			this.CtxWorkspacesModify = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.MniltbWorklspaceDuplicateTo = new Sq1.Widgets.LabeledTextBox.MenuItemLabeledTextBox();
			this.MniltbWorklspaceRenameTo = new Sq1.Widgets.LabeledTextBox.MenuItemLabeledTextBox();
			this.MniWorkspacesToolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
			this.MniltbWorklspaceNewBlank = new Sq1.Widgets.LabeledTextBox.MenuItemLabeledTextBox();
			this.MniWorkspaceDelete = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.ctxTools.SuspendLayout();
			this.ctxDllVersions.SuspendLayout();
			this.mainFormStatusStrip.SuspendLayout();
			this.CtxWorkspacesModify.SuspendLayout();
			this.SuspendLayout();
			// 
			// ctxTools
			// 
			this.ctxTools.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mniDllVersions,
            this.mniSaveScreenshot,
            this.toolStripSeparator4,
            this.mniDistributedBacktester,
            this.mniFuturesMerger,
            this.mniBarsEditor,
            this.mniChartSettingsEditor,
            this.mniSymbolInfoEditor,
            this.mniCsvImporter,
            this.toolStripSeparator2,
            this.mniExecution,
            this.mniSliders,
            this.mniStrategies,
            this.mniDataSources,
            this.mniExceptions,
            this.toolStripSeparator1,
            this.mniExit});
			this.ctxTools.Name = "ctxmsTools";
			this.ctxTools.Size = new System.Drawing.Size(230, 352);
			// 
			// mniVersionsAndCredits
			// 
			this.mniDllVersions.DropDown = this.ctxDllVersions;
			this.mniDllVersions.Name = "mniDllVersions";
			this.mniDllVersions.Size = new System.Drawing.Size(229, 22);
			this.mniDllVersions.Text = "DLL Versions";
			// 
			// ctxDllVersions
			// 
			this.ctxDllVersions.AutoSize = false;
			this.ctxDllVersions.BackColor = System.Drawing.SystemColors.ControlDark;
			this.ctxDllVersions.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemDllVersions});
			this.ctxDllVersions.Name = "ctxDllVersions";
			this.ctxDllVersions.OwnerItem = this.mniDllVersions;
			this.ctxDllVersions.ShowImageMargin = false;
			this.ctxDllVersions.Size = new System.Drawing.Size(816, 306);
			// 
			// menuItemDllVersions
			// 
			this.menuItemDllVersions.AutoSize = false;
			this.menuItemDllVersions.Name = "menuItemDllVersions";
			this.menuItemDllVersions.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
			this.menuItemDllVersions.Size = new System.Drawing.Size(800, 300);
			this.menuItemDllVersions.Text = "menuItemDllVersions";
			// 
			// mniSaveScreenshot
			// 
			this.mniSaveScreenshot.Enabled = false;
			this.mniSaveScreenshot.Name = "mniSaveScreenshot";
			this.mniSaveScreenshot.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.Pause)));
			this.mniSaveScreenshot.Size = new System.Drawing.Size(229, 22);
			this.mniSaveScreenshot.Text = "Save Screenshot...";
			// 
			// toolStripSeparator4
			// 
			this.toolStripSeparator4.Name = "toolStripSeparator4";
			this.toolStripSeparator4.Size = new System.Drawing.Size(226, 6);
			// 
			// mniDistributedBacktester
			// 
			this.mniDistributedBacktester.Enabled = false;
			this.mniDistributedBacktester.Name = "mniDistributedBacktester";
			this.mniDistributedBacktester.Size = new System.Drawing.Size(229, 22);
			this.mniDistributedBacktester.Text = "Distributed Backtester";
			// 
			// mniFuturesMerger
			// 
			this.mniFuturesMerger.Name = "mniFuturesMerger";
			this.mniFuturesMerger.Size = new System.Drawing.Size(229, 22);
			this.mniFuturesMerger.Text = "Futures Merger";
			this.mniFuturesMerger.Click += new System.EventHandler(this.mniFuturesMerger_Click);
			// 
			// mniBarsEditor
			// 
			this.mniBarsEditor.Name = "mniBarsEditor";
			this.mniBarsEditor.Size = new System.Drawing.Size(229, 22);
			this.mniBarsEditor.Text = "Bars Editor";
			this.mniBarsEditor.Click += new System.EventHandler(this.mniBarsEditor_Click);
			// 
			// mniChartSettingsEditor
			// 
			this.mniChartSettingsEditor.Name = "mniChartSettingsEditor";
			this.mniChartSettingsEditor.Size = new System.Drawing.Size(229, 22);
			this.mniChartSettingsEditor.Text = "Chart Settings Editor";
			this.mniChartSettingsEditor.Click += new System.EventHandler(this.mniChartSettingsEditor_Click);
			// 
			// mniSymbolInfoEditor
			// 
			this.mniSymbolInfoEditor.Name = "mniSymbolInfoEditor";
			this.mniSymbolInfoEditor.Size = new System.Drawing.Size(229, 22);
			this.mniSymbolInfoEditor.Text = "Symbol Info Editor";
			this.mniSymbolInfoEditor.Click += new System.EventHandler(this.mniSymbolInfoEditor_Click);
			// 
			// mniCsvImporter
			// 
			this.mniCsvImporter.Name = "mniCsvImporter";
			this.mniCsvImporter.Size = new System.Drawing.Size(229, 22);
			this.mniCsvImporter.Text = "CSV Importer";
			this.mniCsvImporter.Click += new System.EventHandler(this.mniCsvImporter_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(226, 6);
			// 
			// mniExecution
			// 
			this.mniExecution.Name = "mniExecution";
			this.mniExecution.ShortcutKeys = System.Windows.Forms.Keys.F3;
			this.mniExecution.Size = new System.Drawing.Size(229, 22);
			this.mniExecution.Text = "Order Execution";
			this.mniExecution.Click += new System.EventHandler(this.mniExecution_Click);
			// 
			// mniSliders
			// 
			this.mniSliders.Name = "mniSliders";
			this.mniSliders.Size = new System.Drawing.Size(229, 22);
			this.mniSliders.Text = "Script Parameters";
			this.mniSliders.Click += new System.EventHandler(this.mniSliders_Click);
			// 
			// mniStrategies
			// 
			this.mniStrategies.Name = "mniStrategies";
			this.mniStrategies.ShortcutKeys = System.Windows.Forms.Keys.F1;
			this.mniStrategies.Size = new System.Drawing.Size(229, 22);
			this.mniStrategies.Text = "Strategies";
			this.mniStrategies.Click += new System.EventHandler(this.mniStrategies_Click);
			// 
			// mniDataSources
			// 
			this.mniDataSources.Name = "mniDataSources";
			this.mniDataSources.Size = new System.Drawing.Size(229, 22);
			this.mniDataSources.Text = "Data Sources";
			this.mniDataSources.Click += new System.EventHandler(this.mniSymbols_Click);
			// 
			// mniExceptions
			// 
			this.mniExceptions.Name = "mniExceptions";
			this.mniExceptions.Size = new System.Drawing.Size(229, 22);
			this.mniExceptions.Text = "Exceptions";
			this.mniExceptions.Click += new System.EventHandler(this.mniExceptions_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(226, 6);
			// 
			// mniExit
			// 
			this.mniExit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.mniExit.Name = "mniExit";
			this.mniExit.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
			this.mniExit.Size = new System.Drawing.Size(229, 22);
			this.mniExit.Text = "Exit";
			this.mniExit.Click += new System.EventHandler(this.mniExit_Click);
			// 
			// mniMainMenu
			// 
			this.mniMainMenu.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.mniMainMenu.DropDown = this.ctxTools;
			this.mniMainMenu.Image = ((System.Drawing.Image)(resources.GetObject("mniMainMenu.Image")));
			this.mniMainMenu.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.mniMainMenu.Name = "mniMainMenu";
			this.mniMainMenu.Size = new System.Drawing.Size(49, 20);
			this.mniMainMenu.Text = "Tools";
			// 
			// DockPanel
			// 
			this.DockPanel.BackColor = System.Drawing.SystemColors.ControlDark;
			this.DockPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.DockPanel.DockBottomPortion = 0.4D;
			this.DockPanel.DockLeftPortion = 0.15D;
			this.DockPanel.DockRightPortion = 0.35D;
			this.DockPanel.DockTopPortion = 0.3D;
			this.DockPanel.DocumentTabStripLocation = WeifenLuo.WinFormsUI.Docking.DocumentTabStripLocation.Bottom;
			this.DockPanel.Location = new System.Drawing.Point(0, 0);
			this.DockPanel.Name = "DockPanel";
			this.DockPanel.Size = new System.Drawing.Size(774, 423);
			dockPanelGradient1.EndColor = System.Drawing.SystemColors.ControlLight;
			dockPanelGradient1.StartColor = System.Drawing.SystemColors.ControlLight;
			autoHideStripSkin1.DockStripGradient = dockPanelGradient1;
			tabGradient1.EndColor = System.Drawing.SystemColors.Control;
			tabGradient1.StartColor = System.Drawing.SystemColors.Control;
			tabGradient1.TextColor = System.Drawing.SystemColors.ControlDarkDark;
			autoHideStripSkin1.TabGradient = tabGradient1;
			autoHideStripSkin1.TextFont = new System.Drawing.Font("Segoe UI", 9F);
			dockPanelSkin1.AutoHideStripSkin = autoHideStripSkin1;
			tabGradient2.EndColor = System.Drawing.SystemColors.ControlLightLight;
			tabGradient2.StartColor = System.Drawing.SystemColors.ControlLightLight;
			tabGradient2.TextColor = System.Drawing.SystemColors.ControlText;
			dockPaneStripGradient1.ActiveTabGradient = tabGradient2;
			dockPanelGradient2.EndColor = System.Drawing.SystemColors.Control;
			dockPanelGradient2.StartColor = System.Drawing.SystemColors.Control;
			dockPaneStripGradient1.DockStripGradient = dockPanelGradient2;
			tabGradient3.EndColor = System.Drawing.SystemColors.ControlLight;
			tabGradient3.StartColor = System.Drawing.SystemColors.ControlLight;
			tabGradient3.TextColor = System.Drawing.SystemColors.ControlText;
			dockPaneStripGradient1.InactiveTabGradient = tabGradient3;
			dockPaneStripSkin1.DocumentGradient = dockPaneStripGradient1;
			dockPaneStripSkin1.TextFont = new System.Drawing.Font("Segoe UI", 9F);
			tabGradient4.EndColor = System.Drawing.SystemColors.ActiveCaption;
			tabGradient4.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
			tabGradient4.StartColor = System.Drawing.SystemColors.GradientActiveCaption;
			tabGradient4.TextColor = System.Drawing.SystemColors.ActiveCaptionText;
			dockPaneStripToolWindowGradient1.ActiveCaptionGradient = tabGradient4;
			tabGradient5.EndColor = System.Drawing.SystemColors.Control;
			tabGradient5.StartColor = System.Drawing.SystemColors.Control;
			tabGradient5.TextColor = System.Drawing.SystemColors.ControlText;
			dockPaneStripToolWindowGradient1.ActiveTabGradient = tabGradient5;
			dockPanelGradient3.EndColor = System.Drawing.SystemColors.ControlLight;
			dockPanelGradient3.StartColor = System.Drawing.SystemColors.ControlLight;
			dockPaneStripToolWindowGradient1.DockStripGradient = dockPanelGradient3;
			tabGradient6.EndColor = System.Drawing.SystemColors.InactiveCaption;
			tabGradient6.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
			tabGradient6.StartColor = System.Drawing.SystemColors.GradientInactiveCaption;
			tabGradient6.TextColor = System.Drawing.SystemColors.InactiveCaptionText;
			dockPaneStripToolWindowGradient1.InactiveCaptionGradient = tabGradient6;
			tabGradient7.EndColor = System.Drawing.Color.Transparent;
			tabGradient7.StartColor = System.Drawing.Color.Transparent;
			tabGradient7.TextColor = System.Drawing.SystemColors.ControlDarkDark;
			dockPaneStripToolWindowGradient1.InactiveTabGradient = tabGradient7;
			dockPaneStripSkin1.ToolWindowGradient = dockPaneStripToolWindowGradient1;
			dockPanelSkin1.DockPaneStripSkin = dockPaneStripSkin1;
			this.DockPanel.Skin = dockPanelSkin1;
			this.DockPanel.TabIndex = 2;
			// 
			// mainFormStatusStrip
			// 
			this.mainFormStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mniMainMenu,
            this.mniWindows,
            this.mniWorkSpaces,
            this.btnFullScreen,
            this.lblSpace,
            this.lblStatus});
			this.mainFormStatusStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
			this.mainFormStatusStrip.Location = new System.Drawing.Point(0, 401);
			this.mainFormStatusStrip.Name = "mainFormStatusStrip";
			this.mainFormStatusStrip.Size = new System.Drawing.Size(774, 22);
			this.mainFormStatusStrip.SizingGrip = false;
			this.mainFormStatusStrip.TabIndex = 5;
			this.mainFormStatusStrip.Text = "mainFormStatusStrip";
			// 
			// mniWindows
			// 
			this.mniWindows.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.mniWindows.DropDown = this.ctxWindows;
			this.mniWindows.Image = ((System.Drawing.Image)(resources.GetObject("mniWindows.Image")));
			this.mniWindows.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.mniWindows.Name = "mniWindows";
			this.mniWindows.Size = new System.Drawing.Size(69, 20);
			this.mniWindows.Text = "Windows";
			// 
			// ctxWindows
			// 
			this.ctxWindows.Name = "ctxWindows";
			this.ctxWindows.OwnerItem = this.mniWindows;
			this.ctxWindows.ShowImageMargin = false;
			this.ctxWindows.Size = new System.Drawing.Size(36, 4);
			this.ctxWindows.Opening += new System.ComponentModel.CancelEventHandler(this.ctxWindows_opening);
			// 
			// mniWorkSpaces
			// 
			this.mniWorkSpaces.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.mniWorkSpaces.DropDown = this.CtxWorkspaces;
			this.mniWorkSpaces.Image = ((System.Drawing.Image)(resources.GetObject("mniWorkSpaces.Image")));
			this.mniWorkSpaces.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.mniWorkSpaces.Name = "mniWorkSpaces";
			this.mniWorkSpaces.Size = new System.Drawing.Size(84, 20);
			this.mniWorkSpaces.Text = "WorkSpaces";
			this.mniWorkSpaces.DropDownOpening += new System.EventHandler(this.mniWorkSpaces_DropDownOpening);
			// 
			// CtxWorkspaces
			// 
			this.CtxWorkspaces.Name = "ctxWorkspaces";
			this.CtxWorkspaces.OwnerItem = this.mniWorkSpaces;
			this.CtxWorkspaces.Size = new System.Drawing.Size(61, 4);
			// 
			// btnFullScreen
			// 
			this.btnFullScreen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.btnFullScreen.Name = "btnFullScreen";
			this.btnFullScreen.Size = new System.Drawing.Size(65, 20);
			this.btnFullScreen.Text = "FullScreen";
			this.btnFullScreen.Click += new System.EventHandler(this.btnFullScreen_Click);
			// 
			// lblSpace
			// 
			this.lblSpace.Name = "lblSpace";
			this.lblSpace.Size = new System.Drawing.Size(28, 17);
			this.lblSpace.Text = "   |   ";
			// 
			// lblStatus
			// 
			this.lblStatus.Name = "lblStatus";
			this.lblStatus.Size = new System.Drawing.Size(16, 17);
			this.lblStatus.Text = "...";
			// 
			// CtxWorkspacesModify
			// 
			this.CtxWorkspacesModify.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MniltbWorklspaceDuplicateTo,
            this.MniltbWorklspaceRenameTo,
            this.MniWorkspacesToolStripSeparator,
            this.MniltbWorklspaceNewBlank,
            this.MniWorkspaceDelete});
			this.CtxWorkspacesModify.Name = "ctxWorkspacesModify";
			this.CtxWorkspacesModify.Size = new System.Drawing.Size(311, 101);
			// 
			// MniltbWorklspaceDuplicateTo
			// 
			this.MniltbWorklspaceDuplicateTo.BackColor = System.Drawing.Color.Transparent;
			this.MniltbWorklspaceDuplicateTo.InputFieldAlignedRight = false;
			this.MniltbWorklspaceDuplicateTo.InputFieldBackColor = System.Drawing.SystemColors.Info;
			this.MniltbWorklspaceDuplicateTo.InputFieldEditable = true;
			this.MniltbWorklspaceDuplicateTo.InputFieldMultiline = true;
			this.MniltbWorklspaceDuplicateTo.InputFieldOffsetX = 80;
			this.MniltbWorklspaceDuplicateTo.InputFieldValue = "";
			this.MniltbWorklspaceDuplicateTo.InputFieldWidth = 160;
			this.MniltbWorklspaceDuplicateTo.Name = "MniltbWorklspaceDuplicateTo";
			this.MniltbWorklspaceDuplicateTo.OffsetTop = 0;
			this.MniltbWorklspaceDuplicateTo.Size = new System.Drawing.Size(250, 20);
			this.MniltbWorklspaceDuplicateTo.TextLeft = "Duplicate To:";
			this.MniltbWorklspaceDuplicateTo.TextLeftOffsetX = 0;
			this.MniltbWorklspaceDuplicateTo.TextLeftWidth = 79;
			this.MniltbWorklspaceDuplicateTo.TextRed = false;
			this.MniltbWorklspaceDuplicateTo.TextRight = "";
			this.MniltbWorklspaceDuplicateTo.TextRightOffsetX = 243;
			this.MniltbWorklspaceDuplicateTo.TextRightWidth = 4;
			// 
			// MniltbWorklspaceRenameTo
			// 
			this.MniltbWorklspaceRenameTo.BackColor = System.Drawing.Color.Transparent;
			this.MniltbWorklspaceRenameTo.InputFieldAlignedRight = false;
			this.MniltbWorklspaceRenameTo.InputFieldBackColor = System.Drawing.SystemColors.Info;
			this.MniltbWorklspaceRenameTo.InputFieldEditable = true;
			this.MniltbWorklspaceRenameTo.InputFieldMultiline = true;
			this.MniltbWorklspaceRenameTo.InputFieldOffsetX = 80;
			this.MniltbWorklspaceRenameTo.InputFieldValue = "";
			this.MniltbWorklspaceRenameTo.InputFieldWidth = 160;
			this.MniltbWorklspaceRenameTo.Name = "MniltbWorklspaceRenameTo";
			this.MniltbWorklspaceRenameTo.OffsetTop = 0;
			this.MniltbWorklspaceRenameTo.Size = new System.Drawing.Size(250, 20);
			this.MniltbWorklspaceRenameTo.TextLeft = "Rename To:";
			this.MniltbWorklspaceRenameTo.TextLeftOffsetX = 0;
			this.MniltbWorklspaceRenameTo.TextLeftWidth = 72;
			this.MniltbWorklspaceRenameTo.TextRed = false;
			this.MniltbWorklspaceRenameTo.TextRight = "";
			this.MniltbWorklspaceRenameTo.TextRightOffsetX = 243;
			this.MniltbWorklspaceRenameTo.TextRightWidth = 4;
			// 
			// MniWorkspacesToolStripSeparator
			// 
			this.MniWorkspacesToolStripSeparator.Name = "MniWorkspacesToolStripSeparator";
			this.MniWorkspacesToolStripSeparator.Size = new System.Drawing.Size(307, 6);
			// 
			// MniltbWorklspaceNewBlank
			// 
			this.MniltbWorklspaceNewBlank.BackColor = System.Drawing.Color.Transparent;
			this.MniltbWorklspaceNewBlank.InputFieldAlignedRight = false;
			this.MniltbWorklspaceNewBlank.InputFieldBackColor = System.Drawing.SystemColors.Info;
			this.MniltbWorklspaceNewBlank.InputFieldEditable = true;
			this.MniltbWorklspaceNewBlank.InputFieldMultiline = true;
			this.MniltbWorklspaceNewBlank.InputFieldOffsetX = 80;
			this.MniltbWorklspaceNewBlank.InputFieldValue = "";
			this.MniltbWorklspaceNewBlank.InputFieldWidth = 160;
			this.MniltbWorklspaceNewBlank.Name = "MniltbWorklspaceNewBlank";
			this.MniltbWorklspaceNewBlank.OffsetTop = 0;
			this.MniltbWorklspaceNewBlank.Size = new System.Drawing.Size(250, 20);
			this.MniltbWorklspaceNewBlank.TextLeft = "New Blank:";
			this.MniltbWorklspaceNewBlank.TextLeftOffsetX = 0;
			this.MniltbWorklspaceNewBlank.TextLeftWidth = 68;
			this.MniltbWorklspaceNewBlank.TextRed = false;
			this.MniltbWorklspaceNewBlank.TextRight = "";
			this.MniltbWorklspaceNewBlank.TextRightOffsetX = 243;
			this.MniltbWorklspaceNewBlank.TextRightWidth = 4;
			// 
			// MniWorkspaceDelete
			// 
			this.MniWorkspaceDelete.Name = "MniWorkspaceDelete";
			this.MniWorkspaceDelete.Size = new System.Drawing.Size(310, 22);
			this.MniWorkspaceDelete.Text = "Delete [TO_BE_REPLACED]";
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(182, 6);
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(229, 22);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(774, 423);
			this.Controls.Add(this.mainFormStatusStrip);
			this.Controls.Add(this.DockPanel);
			this.IsMdiContainer = true;
			this.Name = "MainForm";
			this.Text = "SquareOne";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.mainForm_FormClosing);
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.mainForm_FormClosed);
			this.Load += new System.EventHandler(this.mainForm_Load);
			this.ResizeEnd += new System.EventHandler(this.mainForm_ResizeEnd);
			this.LocationChanged += new System.EventHandler(this.mainForm_LocationChanged);
			this.ctxTools.ResumeLayout(false);
			this.ctxDllVersions.ResumeLayout(false);
			this.mainFormStatusStrip.ResumeLayout(false);
			this.mainFormStatusStrip.PerformLayout();
			this.CtxWorkspacesModify.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		private System.Windows.Forms.ToolStripMenuItem mniCsvImporter;
		private System.Windows.Forms.ContextMenuStrip ctxWindows;
		private System.Windows.Forms.ToolStripDropDownButton mniWindows;
		public System.Windows.Forms.ContextMenuStrip CtxWorkspaces;

		private System.Windows.Forms.ContextMenuStrip ctxTools;
		private System.Windows.Forms.ToolStripMenuItem mniExceptions;
		private System.Windows.Forms.ToolStripMenuItem mniExecution;
		private System.Windows.Forms.ToolStripMenuItem mniSliders;
		private System.Windows.Forms.ToolStripMenuItem mniDataSources;
		public WeifenLuo.WinFormsUI.Docking.DockPanel DockPanel;
		private System.Windows.Forms.ToolStripMenuItem mniStrategies;
		private System.Windows.Forms.StatusStrip mainFormStatusStrip;
		private System.Windows.Forms.ToolStripDropDownButton mniMainMenu;
		private System.Windows.Forms.ToolStripStatusLabel lblSpace;
		private System.Windows.Forms.ToolStripButton btnFullScreen;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem mniExit;
		private System.Windows.Forms.ToolStripDropDownButton mniWorkSpaces;
		private System.Windows.Forms.ToolStripStatusLabel lblStatus;
		private System.Windows.Forms.ToolStripMenuItem mniSymbolInfoEditor;
		private System.Windows.Forms.ToolStripMenuItem mniChartSettingsEditor;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripMenuItem mniFuturesMerger;
		public System.Windows.Forms.ContextMenuStrip CtxWorkspacesModify;
		public Widgets.LabeledTextBox.MenuItemLabeledTextBox MniltbWorklspaceRenameTo;
		public Widgets.LabeledTextBox.MenuItemLabeledTextBox MniltbWorklspaceDuplicateTo;
		public System.Windows.Forms.ToolStripSeparator MniWorkspacesToolStripSeparator;
		public Widgets.LabeledTextBox.MenuItemLabeledTextBox MniltbWorklspaceNewBlank;
		public System.Windows.Forms.ToolStripMenuItem MniWorkspaceDelete;
		private System.Windows.Forms.ToolStripMenuItem mniDllVersions;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
		private System.Windows.Forms.ContextMenuStrip ctxDllVersions;
		private Widgets.DllVersions.MenuItemDllVersions menuItemDllVersions;
		private System.Windows.Forms.ToolStripMenuItem mniSaveScreenshot;
		private System.Windows.Forms.ToolStripMenuItem mniBarsEditor;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem mniDistributedBacktester;
	}
}
