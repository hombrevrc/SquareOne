﻿using System;
using System.Windows.Forms;

using Sq1.Core;
using Sq1.Core.Repositories;
using Sq1.Core.DataTypes;
using Sq1.Core.DataFeed;

namespace Sq1.Widgets.SymbolEditor {
	public partial class SymbolInfoEditorControl : UserControl {
		const string					noSymbolSelected_symbol = "SELECT_SYMBOL";
		SymbolInfo						noSymbolSelected_symbolInfo;

		RepositorySerializerSymbolInfos	repositorySerializerSymbolInfo;
		RepositoryJsonDataSources		repositoryJsonDataSource;
		SymbolInfo						symbolInfoSelected_nullUnsafe { get { return this.tsiCbxSymbols.ComboBox.SelectedItem as SymbolInfo; } }
		bool							rebuildingDropdown;
		bool							openDropDownAfterSelected;
		bool							ignoreEvent_SelectedIndexChanged_resetInHandler;

		public SymbolInfoEditorControl() {
			InitializeComponent();

			//MOVED_TO_DESIGNER_AFTER_TUNNELING DESIGNER_RESETS_TO_EDITABLE__LAZY_TO_TUNNEL_PROPERTIES_AND_EVENTS_IN_ToolStripItemComboBox.cs
			//this.toolStripItemComboBox1.ComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
			//this.toolStripItemComboBox1.ComboBox.Sorted = true;
			//this.toolStripItemComboBox1.ComboBox.SelectedIndexChanged	+= new EventHandler(this.toolStripItemComboBox1_SelectedIndexChanged);
			//this.toolStripItemComboBox1.ComboBox.DropDown				+= new EventHandler(this.toolStripItemComboBox1_DropDown);

			this.noSymbolSelected_symbolInfo = new SymbolInfo();
			this.noSymbolSelected_symbolInfo.Symbol = noSymbolSelected_symbol;
		}
		public void Initialize(RepositorySerializerSymbolInfos repositorySerializerSymbolInfo, RepositoryJsonDataSources repositoryJsonDataSource) {
			this.repositorySerializerSymbolInfo = repositorySerializerSymbolInfo;
			this.repositoryJsonDataSource = repositoryJsonDataSource;
			this.rebuildDropdown_select();
			if (this.repositorySerializerSymbolInfo.SymbolInfos.Count > 0) {
				this.PopulateWithSymbolInfo(this.repositorySerializerSymbolInfo.SymbolInfos[0]);
			}

			//repositoryJsonDataSource.OnItemRemovedDone -= new EventHandler<NamedObjectJsonEventArgs<DataSource>>(repositoryJsonDataSource_OnDataSourceDeleted_closeSymbolInfoEditor);
			//repositoryJsonDataSource.OnItemRemovedDone += new EventHandler<NamedObjectJsonEventArgs<DataSource>>(repositoryJsonDataSource_OnDataSourceDeleted_closeSymbolInfoEditor);

			//repositoryJsonDataSource.OnItemRenamed -= new EventHandler<NamedObjectJsonEventArgs<DataSource>>(repositoryJsonDataSource_OnDataSourceRenamed_refreshTitle);
			//repositoryJsonDataSource.OnItemRenamed += new EventHandler<NamedObjectJsonEventArgs<DataSource>>(repositoryJsonDataSource_OnDataSourceRenamed_refreshTitle);

			repositoryJsonDataSource.OnSymbolRemovedDone -= new EventHandler<DataSourceSymbolEventArgs>(repositoryJsonDataSource_OnSymbolRemoved_clean);
			repositoryJsonDataSource.OnSymbolRemovedDone += new EventHandler<DataSourceSymbolEventArgs>(repositoryJsonDataSource_OnSymbolRemoved_clean);

			repositoryJsonDataSource.OnSymbolRenamed -= new EventHandler<DataSourceSymbolRenamedEventArgs>(repositoryJsonDataSource_OnSymbolRenamed_refresh);
			repositoryJsonDataSource.OnSymbolRenamed += new EventHandler<DataSourceSymbolRenamedEventArgs>(repositoryJsonDataSource_OnSymbolRenamed_refresh);
		}

		void rebuildDropdown_select(SymbolInfo symbolInfo_toSelect = null) {
			this.rebuildingDropdown = true;
			try {
				this.tsiCbxSymbols.ComboBox.Items.Clear();
				foreach (SymbolInfo symbolInfo in this.repositorySerializerSymbolInfo.SymbolInfos) {
					this.tsiCbxSymbols.ComboBox.Items.Add(symbolInfo);
				}

				if (symbolInfo_toSelect != null) {
					SymbolInfo symbolInfo_found = null;
					foreach (SymbolInfo eachSymbolInfo in this.tsiCbxSymbols.ComboBox.Items) {
						if (eachSymbolInfo.ToString() != symbolInfo_toSelect.ToString()) continue;
						symbolInfo_found = eachSymbolInfo;
						break;
					}

					this.openDropDownAfterSelected = false;
					if (symbolInfo_found == null) {
						string msg = "I_REFUSE_TO_SELECT_NOT_YET_ADDED";
						Assembler.PopupException(msg);
						return;
					}
					if (this.tsiCbxSymbols.ComboBox.SelectedItem == symbolInfo_found) return;
					this.ignoreEvent_SelectedIndexChanged_resetInHandler = true;
					this.tsiCbxSymbols.ComboBox.SelectedItem = symbolInfo_found;	// triggering event to invoke toolStripComboBox1_SelectedIndexChanged => testing chartSettingsSelected_nullUnsafe + Initialize()
				}
			} finally {
				this.rebuildingDropdown = false;
			}
		}
		public void CleanPropertyEditor() {
			this.tsiCbxSymbols.ComboBox.Items.Add(this.noSymbolSelected_symbolInfo);
			this.PopulateWithSymbolInfo(this.noSymbolSelected_symbolInfo);
			// that's it! nothing else is needed to be done: once any other symbol is selected,
			// toolStripItemComboBox1_SelectedIndexChanged() will do this.PopulateWithSymbolInfo(this.symbolInfoSelected_nullUnsafe, true);
		}
		public void PopulateWithSymbolInfo(SymbolInfo symbolInfo, bool rebuildDropdown = false, bool selectPopulated_afterRebuild = true) {
			if (symbolInfo == null) {
				string msg = "SHOULD_CLEAR_PROPERTY_EDITOR";
				Assembler.PopupException(msg, null, false);
				//return;
			}

			if (this.tsiCbxSymbols.ComboBox.SelectedItem != symbolInfo) {
				this.ignoreEvent_SelectedIndexChanged_resetInHandler = true;
				this.tsiCbxSymbols.ComboBox.SelectedItem = symbolInfo;
			}

			Form parent = base.Parent as Form;
			if (parent != null) {
				parent.Text = "Symbol Editor :: " + symbolInfo.ToString();
			}

			this.mniDeleteSymbol.Text				= "Delete [" + symbolInfo.Symbol + "]";
			this.mniltbAddNew.InputFieldValue		= symbolInfo.Symbol;
			this.mniltbDuplicate.InputFieldValue	= symbolInfo.Symbol;
			this.mniltbRename.InputFieldValue		= symbolInfo.Symbol;

			if (symbolInfo == noSymbolSelected_symbolInfo) {
				this.propertyGrid1.SelectedObject = null;
				return;
			}

			this.propertyGrid1.SelectedObject = symbolInfo;

			if (rebuildDropdown == false) return;
			SymbolInfo symbolInfo_toSelect = selectPopulated_afterRebuild ? symbolInfo : null;
			this.rebuildDropdown_select(symbolInfo_toSelect);
		}
		public void PopulateRenamedSymbol_rebuildDropdown(DataSourceSymbolRenamedEventArgs e) {
			string msig = " //PopulateRenamedSymbol_rebuildDropdown(" + e.SymbolOld + "=>" + e.Symbol + ")";
			SymbolInfo symbolInfo = this.repositorySerializerSymbolInfo.FindSymbolInfo_nullUnsafe(e.Symbol);
			if (symbolInfo == null) {
				string msg = "RENAME_IN_REPOSITORY_FIRST__EDITOR_DEALS_WITH_EXISTING_DATA";
				Assembler.PopupException(msg + msig);
				return;
			}
			this.PopulateWithSymbolInfo(symbolInfo, true);
		}

		public void PopulateWithSymbol_findOrCreateSymbolInfo(string symbol) {
			string msig = " //PopulateWithSymbol_findOrCreateSymbolInfo(" + symbol + ")";
			SymbolInfo symbolInfo = this.repositorySerializerSymbolInfo.FindSymbolInfo_nullUnsafe(symbol);
			if (symbolInfo == null) {
				string msg = "HACKY!!!!_RENAME_IN_REPOSITORY_FIRST__EDITOR_DEALS_WITH_EXISTING_DATA";
				Assembler.PopupException(msg + msig);
				symbolInfo = this.repositorySerializerSymbolInfo.FindSymbolInfoOrNew(symbol);
			}
			this.PopulateWithSymbolInfo(symbolInfo, true);
		}
	}
}
