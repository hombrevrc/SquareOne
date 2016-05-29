using System;
using System.Windows.Forms;

using Sq1.Core;
using Sq1.Core.Execution;

namespace Sq1.Reporters {
	public partial class Positions {
		void mniCopyToClipboard_Click(object sender, EventArgs e) {
			string text = this.generateTextScreenshot();
			Clipboard.SetText(text);
		}
		void mniColorify_Click(object sender, EventArgs e) {
			try {
				this.snap.Colorify = this.mniColorify.Checked;
				this.olv_customizeColors();
				//this.olvPositions.Refresh();
				this.olvPositions.BuildList(true);	// otherwize RestoreState() doesn't restore after restart?
				//this.rebuildOLVproperly();
				base.RaiseContextScriptChangedContainerShouldSerialize();
			} catch (Exception ex) {
				Assembler.PopupException(ex.Message);
			}
		}
		void olvPositions_DoubleClick(object sender, EventArgs e) {
			try {
				if (this.olvPositions.SelectedItems.Count == 0) return;
				int selected = this.olvPositions.SelectedIndex;
				if (selected < 0) {
					string msg = "HOW_CAN_YOU_DOUBLE_CLICK_ON_SOMETHING_NOT_SELECTED??? olvPositions.SelectedIndex < 0";
					Assembler.PopupException(msg);
					return;
				}
				if (this.positionsAll_reversedCached.Count < selected) {
					string msg = "SELECTED_INDEX_OUT_OF_RANGE positionsAllReversedCached.Count[" + this.positionsAll_reversedCached.Count + "] < selected[" + selected + "]";
					Assembler.PopupException(msg);
					return;
				}
				Position pos = this.positionsAll_reversedCached[selected];
				if (pos == null) {
					string msg = "POSITION_STORED_IN_REVERSED_CACHED_AS_NULL positionsAllReversedCached[" + selected + "]=null";
					Assembler.PopupException(msg);
					return;
				}
				base.Chart.SelectPosition(pos);
			} catch (Exception ex) {
				Assembler.PopupException(ex.Message);
			}
		}
		protected override void SymbolInfo_PriceDecimalsChanged(object sender, EventArgs e) {
			this.olvReCustomize_OnPriceDecimalsChanged();
			this.olvPositions.RebuildColumns();
		}
	}
}