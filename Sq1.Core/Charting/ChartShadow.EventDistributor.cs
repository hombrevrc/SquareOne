using System;

using Sq1.Core.DataTypes;

namespace Sq1.Core.Charting {
	public partial class ChartShadow {
		public event EventHandler<EventArgs> ChartSettingsChangedContainerShouldSerialize;
		public event EventHandler<EventArgs> ContextScriptChangedContainerShouldSerialize;

		public event EventHandler<BarEventArgs> BarStreamingUpdatedMerged;

		public event EventHandler<EventArgs> OnPumpPaused;
		public event EventHandler<EventArgs> OnPumpUnPaused;

		public void RaiseBarStreamingUpdatedMerged(BarEventArgs e) {
			if (this.BarStreamingUpdatedMerged == null) return;
			try {
				this.BarStreamingUpdatedMerged(this, e);
			} catch (Exception ex) {
				string msg = "RaiseBarStreamingUpdatedMerged(bar[" + e.Bar + "])";
				Assembler.PopupException(msg, ex, false);
			}
		}
		
		public void RaiseChartSettingsChangedContainerShouldSerialize() {
			if (this.ChartSettingsChangedContainerShouldSerialize == null) return;
			try {
				this.ChartSettingsChangedContainerShouldSerialize(this, null);
			} catch (Exception ex) {
				Assembler.PopupException("RaiseChartSettingsChangedContainerShouldSerialize()", ex);
			}
		}
		public void RaiseContextScriptChangedContainerShouldSerialize() {
			if (this.ContextScriptChangedContainerShouldSerialize == null) return;
			try {
				this.ContextScriptChangedContainerShouldSerialize(this, null);
			} catch (Exception ex) {
				Assembler.PopupException("RaiseContextScriptChangedContainerShouldSerialize()", ex);
			}
		}

		void raiseOnPumpPaused() {
			if (this.OnPumpPaused == null) return;
			try {
				this.OnPumpPaused(this, null);
			} catch (Exception ex) {
				string msg = "RaiseOnPumpPaused()";
				Assembler.PopupException(msg, ex, false);
			}
		}

		void raiseOnPumpUnPaused() {
			if (this.OnPumpUnPaused == null) return;
			try {
				this.OnPumpUnPaused(this, null);
			} catch (Exception ex) {
				string msg = "RaiseOnPumpUnPaused()";
				Assembler.PopupException(msg, ex, false);
			}
		}

	}
}
