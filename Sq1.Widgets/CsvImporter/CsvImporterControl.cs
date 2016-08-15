﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

using BrightIdeasSoftware;
using CsvHelper;

using Sq1.Core;
using Sq1.Core.DataFeed;
using Sq1.Core.DataTypes;
using Sq1.Core.Repositories;
using Sq1.Core.Serializers;

using Sq1.Widgets.RangeBar;

namespace Sq1.Widgets.CsvImporter {
	public partial class CsvImporterControl : UserControl {
		string								btnImportOriginalMessage;
		CsvImporterDataSnapshot				csvDataSnapshot;
		Serializer<CsvImporterDataSnapshot> csvDataSnapshotSerializer;
		RepositoryJsonDataSources			DataSourceRepository;

		string								targetSymbolClicked;
		DataSource							targetDataSource;
		
		public CsvImporterControl() {
			InitializeComponent();
			this.btnImportOriginalMessage			= this.btnImport.Text;
			this.olvColGenParsedRaw					= new Dictionary<OLVColumn, ColumnCatcher>();
			this.olvColGenFieldSetup				= new Dictionary<OLVColumn, ColumnCatcher>();
			this.csvDataSnapshotSerializer			= new Serializer<CsvImporterDataSnapshot>();
			this.rangeBar.OnValueMaxChanged			+= new EventHandler<RangeArgs<DateTime>>(rangeBar_OnValueMaxChanged);
			this.rangeBar.OnValueMinChanged			+= new EventHandler<RangeArgs<DateTime>>(rangeBar_OnValueMaxChanged);
			this.rangeBar.OnValuesMinAndMaxChanged	+= new EventHandler<RangeArgs<DateTime>>(rangeBar_OnValueMaxChanged);
			this.olvParsedByFormat_customize();
		}
		public void Initialize(RepositoryJsonDataSources dataSourceRepository) {
			this.DataSourceRepository = dataSourceRepository;
			bool createdNewFile = this.csvDataSnapshotSerializer.Initialize(this.DataSourceRepository.RootPath,
												   "Sq1.Widgets.CsvImporter.CsvImporterDataSnapshot.json", "Workspaces",
												   Assembler.InstanceInitialized.AssemblerDataSnapshot.WorkspaceCurrentlyLoaded, true, true);

			//this.dataSnapshot = new CsvImporterDataSnapshot();
			this.csvDataSnapshot = this.csvDataSnapshotSerializer.Deserialize();

			LinkLabel.Link link = new LinkLabel.Link();
			link.LinkData = this.csvDataSnapshot.DownloadButtonURL;
			this.lnkDownload.Links.Clear();
			this.lnkDownload.Links.Add(link);				

			foreach (var each in this.csvDataSnapshot.FieldSetupCurrent) {
				each.DataSnapshot = this.csvDataSnapshot;
			}
			this.mniltbCsvSeparator.InputFieldValue = this.csvDataSnapshot.CsvConfiguration.Delimiter;

			this.importSourceFileBrowser.PopulateListFromCsvPath(this.csvDataSnapshot.PathCsv);

			if (this.csvDataSnapshot.FileSelected != null) {
				this.stepsAllparseFromDataSnapshot();
				this.importSourceFileBrowser.SelectFile(this.csvDataSnapshot.FileSelected);
			}
			this.dataSourcesTree.Initialize(this.DataSourceRepository, false);
			this.dataSourcesTree.TreeFirstColumnNameText = "Import To (Symbol / DataSource):";
		}

		bool stepsAllparseFromDataSnapshot() {
			Cursor.Current = Cursors.WaitCursor;
			try {
				var fsi = new FileInfo(this.csvDataSnapshot.FileSelectedAbsname);
				var pi = new ImportSourcePathInfo(fsi);
				this.step1parseFileInfo(pi);
			} catch (Exception ex) {
				this.olvCsvParsedRaw.Reset();
				this.olvCsvParsedRaw.EmptyListMsg = "step1parseFileInfo\r\n[" + ex.Message + "]"
					+ "\r\nin new ImportSourcePathInfo(new FileInfo(" + this.csvDataSnapshot.FileSelected + "))";
				//Assembler.PopupException("step1parseFileInfo()", ex);
				this.olvParsedByFormat.SetObjects(null);
				this.olvParsedByFormat.EmptyListMsg = "";
				this.disableRangeBarAndBtnImport();
				Cursor.Current = Cursors.Default;
				return false;
			}

			try {
				this.step2syncCsvRawToFieldSetup();
			} catch (Exception ex) {
				this.olvParsedByFormat.SetObjects(null);
				this.olvParsedByFormat.EmptyListMsg = "step2syncCsvRawToFieldSetup\r\n[" + ex.Message + "]"
					+ "\r\nfor [" + this.csvDataSnapshot.FileSelected + "]";
				Assembler.PopupException("step2syncCsvRawToFieldSetup()", ex);
				this.disableRangeBarAndBtnImport();
				Cursor.Current = Cursors.Default;
				return false;
			}

			bool step3success = this.step3safe();
			if (step3success == false) return false;
			
			Cursor.Current = Cursors.Default;
			return true;
		}

		//this.improveColumnTypeGuess();
		bool step3safe() {
			try {
				this.step3syncCsvRawAndFieldSetupToParsedByFormat();
			} catch (Exception ex) {
				this.olvParsedByFormat.SetObjects(null);
				this.olvParsedByFormat.EmptyListMsg = "step3syncCsvRawAndFieldSetupToParsedByFormat\r\n[" + ex.Message + "]" + "\r\nfor [" + this.csvDataSnapshot.FileSelected + "]";
				Assembler.PopupException("step3syncCsvRawAndFieldSetupToParsedByFormat()", ex);
				this.disableRangeBarAndBtnImport();
				Cursor.Current = Cursors.Default;
				return false;
			}
			return true;
		}
		public List<List<string>> readCsvToMatrix(string filename) {
			List<List<string>> ret = new List<List<string>>();
			using (StreamReader textReader = new StreamReader(filename)) {
				var csv = new CsvReader(textReader, this.csvDataSnapshot.CsvConfiguration);
				while (csv.Read()) {
					string[] fields = csv.CurrentRecord;
					ret.Add(new List<string>(fields));
				}
			}
			return ret;
		}
		
		List<List<string>> csvUnnamed;
		Dictionary<OLVColumn, ColumnCatcher> olvColGenParsedRaw;
		void step1parseFileInfo(ImportSourcePathInfo e) {
			this.csvDataSnapshot.FileSelected = e.FSI.Name;
			this.csvUnnamed = readCsvToMatrix(e.FSI.FullName);
			string status = " Rows[" + this.csvUnnamed.Count + "]";
			if (this.csvUnnamed.Count == 0) {
				string msg = "File [" + e.FSI.Name + "] isn't a CSV file;\r\nDouble Click here to open and adjust your settings";
				throw new Exception(msg);
			}
			int maxColumns = 0;
			foreach (var row in this.csvUnnamed) {
				if (row.Count > maxColumns) maxColumns = row.Count;
			}
			if (maxColumns == 0) {
				string msg = "File [" + e.FSI.Name + "] is CSV with ZERO columns (???);\r\nDouble Click here to open and adjust your settings";
				throw new Exception(msg);
			}
			status += " Columns[" + maxColumns + "] found in File[" + e.FSI.Name + "]";
			
			string statusOld = this.grpPreviewParsedRaw.Text;
			int posToAppend = statusOld.IndexOf('|');
			//if (posToAppend < 0) posToAppend = 0;
			string prefix = statusOld.Substring(0, posToAppend+1); // no exception thrown even if posToAppend=-1 (not found)
			this.grpPreviewParsedRaw.Text = prefix + status;
			

			//this.csvPreviewParsedRaw.Columns.Clear();
			//this.csvPreviewParsedRaw.AllColumns.Clear();
			//this.csvPreviewParsedRaw.Clear();
			this.olvCsvParsedRaw.Reset();
			this.olvColGenParsedRaw.Clear();
			//RELIES_ON_CLASS_PROPERTIES_WHICH_LIST<LIST<STRING>>_DOESNT_HAVE Generator.GenerateColumns(this.csvPreviewParsedRaw, csvUnnamed);

			//int width = this.csvPreviewParsedRaw.Width / maxColumns;
			for (int i = 0; i < maxColumns; i++) {
				var olvColumn = new OLVColumn();
				olvColumn.Name = "AnonymousColumn_" + (i+1);
				olvColumn.Text = "Anonymous " + (i + 1);
				//olvColumn.Width = width;
				//if (i == maxColumns-1) olvColumn.FillsFreeSpace = true;
				ColumnCatcher iCatcher;
				if (this.csvDataSnapshot.FieldSetupCurrent.Count >= i+1) {
					iCatcher = this.csvDataSnapshot.FieldSetupCurrent[i];
				} else {
					iCatcher = new ColumnCatcher(i, this.csvDataSnapshot);
					this.csvDataSnapshot.FieldSetupCurrent.Add(iCatcher);
				}
				olvColumn.AspectGetter = iCatcher.AspectGetterParsedRaw;
				this.olvCsvParsedRaw.Columns.Add(olvColumn);
				//this.csvPreviewParsedRaw.AllColumns.Add(olvColumn);
				this.olvColGenParsedRaw.Add(olvColumn, iCatcher);
			}
			//MAKES_THE_TABLE_EMPTY_EVEN_WITHOUT_HEADERS_this.csvPreviewParsedRaw.RebuildColumns();
			this.olvCsvParsedRaw.SetObjects(this.csvUnnamed);
			this.olvCsvParsedRaw.AutoResizeColumns();
		}

		Dictionary<OLVColumn, ColumnCatcher> olvColGenFieldSetup;
		void step2syncCsvRawToFieldSetup() {
			this.olvFieldSetup.Reset();
			this.olvColGenFieldSetup.Clear();
			foreach (OLVColumn olvRaw in this.olvColGenParsedRaw.Keys) {
				ColumnCatcher iCatcher = this.olvColGenParsedRaw[olvRaw];
				OLVColumn olvColumnSetup = new OLVColumn();
				olvColumnSetup.Name = "FieldSetupColumn_" + (iCatcher.ColumnSerno + 1);
				olvColumnSetup.Text = "FieldSetup " + (iCatcher.ColumnSerno + 1);
				olvColumnSetup.Width = olvRaw.Width;
				olvColumnSetup.AspectGetter = iCatcher.AspectGetterFieldSetup;
				this.olvFieldSetup.Columns.Add(olvColumnSetup);
				this.olvColGenFieldSetup.Add(olvColumnSetup, iCatcher);

				//olvRaw.Wi
			}
			this.olvFieldSetup.SetObjects(this.csvDataSnapshot.OLVModel);
			//this.olvColGenParsedRaw.
		}
		void step3syncCsvRawAndFieldSetupToParsedByFormat() {
			List<CsvBar> csvParsedByFormat = new List<CsvBar>();
			string symbol = null;
			BarScaleInterval scaleInterval = null;
			bool allCsvBarsHaveDOHLCV = true;
			string dateAndTimeFormatsGlued = this.csvDataSnapshot.FieldSetupCurrent.DateAndTimeFormatsGlued;
			
			foreach (OLVColumn each in this.olvParsedByFormat.Columns) {
				each.HeaderForeColor = Color.Black;
			}

			foreach (List<string> row in this.csvUnnamed) {
				try {
					CsvBar newCsvBar = new CsvBar();
					//foreach (string cell in row) {
					for (int i=0; i<row.Count; i++) {
						string cell = row[i];
						var iColumn = this.csvDataSnapshot.FieldSetupCurrent[i];
						try {
							iColumn.Parser.ParseAndPushThrows(cell, newCsvBar, dateAndTimeFormatsGlued);
						} catch (Exception ex) {
							//YOULL_RESET_COLUMNS_DESIGNER_this.olvParsedByFormat.Reset();
							//UPSTACK_CATCH_DOES_THIS this.olvParsedByFormat.SetObjects(null);
							//UPSTACK_CATCH_DOES_THIS this.olvParsedByFormat.EmptyListMsg = msg;
							string msg = "value[" + cell + "] column#[" + i + "] type[" + iColumn.Parser.CsvType + "]"
								+ "\r\n" + ex.Message;
							throw new Exception(msg, ex);
						}
						switch (iColumn.Parser.CsvType) {
							case CsvFieldType.Symbol:
								if (String.IsNullOrEmpty(symbol) == false) break;
								symbol = cell;
								break;
							case CsvFieldType.Interval_Min:
								if (scaleInterval != null) break;
								int minutes = 0;
								bool minutesParsedOk = int.TryParse(cell, out minutes);
								if (minutesParsedOk == false) break;
								if (minutes <= 0) break;
								scaleInterval = new BarScaleInterval(BarScale.Minute, minutes);
								break;
							case CsvFieldType.Interval_Hrs:
								if (scaleInterval != null) break;
								int hours = 0;
								bool hoursParsedOk = int.TryParse(cell, out hours);
								if (hoursParsedOk == false) break;
								if (hours <= 0) break;
								scaleInterval = new BarScaleInterval(BarScale.Hour, hours);
								break;
							case CsvFieldType.Interval_Days:
								if (scaleInterval != null) break;
								int days = 0;
								bool daysParsedOk = int.TryParse(cell, out days);
								if (daysParsedOk == false) break;
								if (days <= 0) break;
								scaleInterval = new BarScaleInterval(BarScale.Daily, days);
								break;
						}
					}
					allCsvBarsHaveDOHLCV &= newCsvBar.IsFullyFilledDOHLCV;
					csvParsedByFormat.Add(newCsvBar);
				} catch (Exception ex) {
					this.olvCsvParsedRaw.SelectObject(row);
					throw ex;
				}
			}
			// MOVED_TO_STEP4=>bars_selectRange_flushToGui this.olvParsedByFormat.SetObjects(csvParsedByFormat);
			
			if (allCsvBarsHaveDOHLCV == false || csvParsedByFormat.Count == 0) {
				this.disableRangeBarAndBtnImport();
				return;
			}

			this.csvParsedByFormat = csvParsedByFormat;
			this.symbolDetectedInCsv = symbol;
			this.scaleIntervalDetectedInCsv = scaleInterval;
			this.step4createBarsDisplayChart();
		}
		void disableRangeBarAndBtnImport() {
			this.rangeBar.Reset();
			this.rangeBar.Enabled = false;
			//this.btnImport.Enabled = true;	//make btnImport.Text and .BackColor repaint when was disabled before changing
			//this.btnImport.Text = this.btnImportOriginalMessage;
			//this.btnImport.BackColor = SystemColors.Control;
			//this.btnImport.Enabled = false;
			//this.btnImport.Invalidate();		//btnImport.Enabled = true works better
			this.barsParsed = null;
			this.populateImportButton();
		}
		BarScaleInterval findMinimalInterval(List<CsvBar> csvParsedByFormat, int takeFirstBarsLimit = 10) {
			BarScaleInterval ret = BarScaleInterval.MaxValue;
			if (takeFirstBarsLimit <= 1) takeFirstBarsLimit = csvParsedByFormat.Count;
			DateTime prevDateTime = csvParsedByFormat[0].DateTime;
			for (int i = 1; i < takeFirstBarsLimit; i++) {
				CsvBar bar = csvParsedByFormat[i];
				TimeSpan ts = bar.DateTime - prevDateTime;
				prevDateTime = bar.DateTime;
				BarScaleInterval bsi = BarScaleInterval.FromTimeSpan(ts);
				if (bsi.LessGranularThan(ret)) ret = bsi; 
			}
			return ret;
		}


		List<CsvBar>		csvParsedByFormat;
		Bars				barsParsed;
		Bars				barsParsed_rangeSelectedForImport;
		string				symbolDetectedInCsv;
		BarScaleInterval	scaleIntervalDetectedInCsv;
		BarScaleInterval	scaleIntervalMinimalScanned;

		void step4createBarsDisplayChart() {
			string msig = " //CsvImporterControl.btnImport_Click()";
			int			barsRead_total = 0;
			List<int>	barsIndexes_failedOHLCVcheck = new List<int>();
			string		msg_barsFailed = "";

			this.barsParsed = new Bars(this.symbolDetectedInCsv, this.scaleIntervalDetectedInCsv, this.csvDataSnapshot.FileSelectedAbsname);
			//BarsUnscaled barsParsed = new BarsUnscaled(symbol, this.dataSnapshot.FileSelectedAbsname);
			foreach (CsvBar csvBar in this.csvParsedByFormat) {
				barsRead_total++;
				try {
					Bar barAdded = this.barsParsed.BarStatic_createAppendAttach(csvBar.DateTime,
						csvBar.Open, csvBar.High, csvBar.Low, csvBar.Close, csvBar.Volume, true, true);
				} catch (Exception exception_DateOHLCV_NaNs__orZeroes) {
					barsIndexes_failedOHLCVcheck.Add(barsRead_total-1);
				}
			}

			if (barsIndexes_failedOHLCVcheck.Count > 0) {
				string barsIndexes_failedOHLCVcheck_asString = string.Join(",", barsIndexes_failedOHLCVcheck);
				msg_barsFailed = "CSV_PARTIALLY_UNPARSEABLE BARS_NANs_OR_ZEROes"
					+ " indexes[" + barsIndexes_failedOHLCVcheck_asString + "] of barsReadTotal[" + barsRead_total + "]";
				Assembler.PopupException(msg_barsFailed + msig, null, false);
			}

			this.bars_selectRange_flushToGui(new BarDataRange());
		}

		void populateGrpPreviewParsedByFormat(int barsCount) {
			string status = " Symbol[" + this.symbolDetectedInCsv + "]";

			BarScaleInterval csvScaleInterval = this.scaleIntervalDetectedInCsv;
			if (csvScaleInterval == null) {
				this.scaleIntervalMinimalScanned = this.findMinimalInterval(this.csvParsedByFormat);
				status += " ScaleIntervalMinFound[" + csvScaleInterval + "]";
			} else {
				status += " ScaleIntervalFromColumn[" + csvScaleInterval + "]";
			}
			
			string statusOld = this.grpPreviewParsedByFormat.Text;
			int posToAppend = statusOld.IndexOf('|');
			//if (posToAppend < 0) posToAppend = 0;
			string prefix = statusOld.Substring(0, posToAppend+1); // no exception thrown even if posToAppend=-1 (not found)

			//status += " BarsParsed.Count[" + this.BarsParsed.Count + "]";
			status += " Bars[" + barsCount + "]";
			this.grpPreviewParsedByFormat.Text = prefix + status;
		}
		void populateImportButton() {
			this.btnImport.Enabled = true;	//make btnImport.Text and .BackColor repaint when was disabled before changing
			if (this.barsParsed_rangeSelectedForImport == null) {
				this.btnImport.Text = this.btnImportOriginalMessage;
				this.btnImport.BackColor = SystemColors.Control;
				this.btnImport.Enabled = false;
				return;
			}
			string msg = "";
			if (this.targetDataSource == null) {
				msg = "SELECT_DATASOURCE";
				if (string.IsNullOrEmpty(this.symbolDetectedInCsv) && string.IsNullOrEmpty(this.targetSymbolClicked)) {
					msg += "_AND_SYMBOL";
					this.btnImport.Enabled = false;
				}
				msg += "_IN_RIGHT_PANEL to import";
				this.btnImport.Text = msg;
				this.btnImport.BackColor = Color.MistyRose;
				this.btnImport.Enabled = false;
				return;
			}
			if (this.targetDataSource.ScaleInterval != this.barsParsed_rangeSelectedForImport.ScaleInterval) {
				msg = "SELECT_DATASOURCE_WITH_SCALE_" + this.barsParsed_rangeSelectedForImport.ScaleInterval;
				msg += "_INSTEAD_OF_" + this.targetDataSource.ScaleInterval;
				this.btnImport.Text = msg;
				this.btnImport.BackColor = Color.MistyRose;
				this.btnImport.Enabled = false;
				return;
			}
			msg = "Import";
			msg += " [" + this.barsParsed_rangeSelectedForImport.Count;
			if (this.csvParsedByFormat.Count != this.barsParsed_rangeSelectedForImport.Count) {
				msg += "/" + this.csvParsedByFormat.Count;
			}
			msg += "] bars";
			msg += " into DataSource[" + this.targetDataSource.Name + "]";
			msg += "-[" + this.targetDataSource.ScaleInterval + "]";
			if (string.IsNullOrEmpty(this.targetSymbolClicked) == false) {
				msg += " to Symbol[" + this.targetSymbolClicked + "]";
			} else {
				msg += " SymbolDetected[" + this.symbolDetectedInCsv + "]";
			}
			//msg += " from [" + this.dataSnapshot.FileSelected + "]"
			//msg += " >>";
			this.btnImport.Text = msg;
			this.btnImport.Enabled = true;
			this.btnImport.BackColor = Color.LightGreen;
		}

		void bars_selectRange_flushToGui(BarDataRange rangeToShow) {
			this.barsParsed_rangeSelectedForImport = this.barsParsed.Clone_selectRange(rangeToShow, true);
			this.rangeBar.Initialize(this.barsParsed, this.barsParsed_rangeSelectedForImport);

			this.olvParsedByFormat.SetObjects(this.barsParsed_rangeSelectedForImport.InnerBars_exposedOnlyForEditor_fromSafeCopy);
			this.rangeBar.Enabled = true;

			this.populateGrpPreviewParsedByFormat(this.barsParsed_rangeSelectedForImport.Count);
			this.populateImportButton();
		}


//		void improveColumnTypeGuess(int linesToAnalyzeRQ = 10, bool distributedEvenly = false, bool highlightCheched = true) {
//			int firstLine = 0;
//			int firstLineOffsetToGetNext = 1;
//			int timesToGetNext = linesToAnalyzeRQ;
//			if (this.csvUnnamed.Count < linesToAnalyzeRQ) timesToGetNext = linesToAnalyzeRQ;
//			if (distributedEvenly) {
//				firstLineOffsetToGetNext = this.csvUnnamed.Count / timesToGetNext;
//			}
//
//			int OpenFoundTimes = 0;
//			foreach (this.olvColGenFieldSetup.Values) {
//
//			}
//
//			for (int i = firstLine; i < this.csvUnnamed.Count; i += firstLineOffsetToGetNext) {
//				List<string> row = this.csvUnnamed[i];
//
//			}
//		}
	}
}