﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;

using Sq1.Core;
using Sq1.Core.Charting;

namespace Sq1.Charting {
	public partial class ChartSettingsEditorControl : UserControl {
		Dictionary<ChartSettings, ChartControl> chartSettings;
		ChartSettings							chartSettingsSelected_nullUnsafe { get { return this.cbxSettings.ComboBox.SelectedItem as ChartSettings; } }
		bool									rebuildingDropdown;
		bool									openDropDownAfterSelected;

		public ChartSettingsEditorControl() {
			InitializeComponent();

			// DESIGNER_RESETS_TO_EDITABLE__LAZY_TO_TUNNEL_PROPERTIES_AND_EVENTS_IN_ToolStripItemComboBox.cs
			this.cbxSettings.ComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
			this.cbxSettings.ComboBox.Sorted = true;
			this.cbxSettings.ComboBox.SelectedIndexChanged += new EventHandler(this.cbxSettings_SelectedIndexChanged);
		}

		public void Initialize(Dictionary<ChartSettings, ChartControl> chartControlsPassed) {
			this.chartSettings = chartControlsPassed;	// I_EXPECT_LIST_OF_CHART_FORMS_TO_REMAIN_SAME_INSTANCE__SO_EACH_NEXT_CHART_IS_OPEN_I_JUST_rebuildDropdown()
			this.RebuildDropdown();
			//if (this.repositorySerializerSymbolInfo.SymbolInfos.Count > 0) {
			//	this.Initialize(this.repositorySerializerSymbolInfo.SymbolInfos[0]);
			//}
		}

		public void RebuildDropdown() {
			if (this.chartSettings == null) {
				string msg = "DONT_INVOKE_REBUILD()_PRIOR_TO_INITIALIZE() //ChartSettingsEditorControl.RebuildDropdown()";
				Assembler.PopupException(msg);
				return;
			}
			this.rebuildingDropdown = true;
			try {
				this.cbxSettings.ComboBox.Items.Clear();
				foreach (ChartSettings chartSettings in this.chartSettings.Keys) {
					this.cbxSettings.ComboBox.Items.Add(chartSettings);
				}
			} finally {
				this.rebuildingDropdown = false;
			}
		}
		public void PopulateWithChartSettings(ChartSettings chartSettings = null, bool forceRebuild = false) {
			if (chartSettings == null) {
				chartSettings = this.chartSettingsSelected_nullUnsafe;
			}
			if (chartSettings == null) {
				string msg = "I_REFUSE_TO_INITIALIZE_WITH_NULL_ChartSettings";
				Assembler.PopupException(msg);
				return;
			}
			Form parent = base.Parent as Form;
			if (parent != null) {
				parent.Text = "Chart Editor :: " + chartSettings.ToString();
			}

			this.propertyGrid1.SelectedObject = chartSettings;
			if (forceRebuild) this.RebuildDropdown();

			ChartSettings selected = this.chartSettingsSelected_nullUnsafe;
			if (selected == null) {
				this.cbxSettings.ComboBox.SelectedItem = chartSettings;
				return;
			} else {
				if (selected.ToString() == chartSettings.ToString()) {
					return;
				}
			}
			foreach (ChartSettings eachChartSettings in this.cbxSettings.ComboBox.Items) {
				if (eachChartSettings.ToString() != chartSettings.ToString()) continue;
				this.openDropDownAfterSelected = false;
				this.cbxSettings.ComboBox.SelectedItem = eachChartSettings;	// triggering event to invoke toolStripComboBox1_SelectedIndexChanged => testing chartSettingsSelected_nullUnsafe + Initialize()
				break;
			}
		}
	}
}
