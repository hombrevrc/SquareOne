﻿using System;
using System.Windows.Forms;

using Sq1.Core;
using Sq1.Core.Sequencing;

namespace Sq1.Gui.Forms {
	public partial class CorrelatorForm {
		void correlatorForm_FormClosing(object sender, FormClosingEventArgs e) {
			// only when user closed => allow scriptEditorForm_FormClosed() to serialize
			if (this.chartFormManager.MainForm.MainFormClosing_skipChartFormsRemoval_serializeExceptionsToPopupInNotepad) {
				e.Cancel = true;
				return;
			}
			if (Assembler.InstanceInitialized.MainFormClosingIgnoreReLayoutDockedForms) {
				e.Cancel = true;
				return;
			}
		}
		void correlatorForm_FormClosed(object sender, FormClosedEventArgs e) {
			// both at FormCloseByX and MainForm.onClose()
			this.chartFormManager.ChartForm.MniShowCorrelator.Checked = false;
			this.chartFormManager.MainForm.MainFormSerialize();
			this.chartFormManager.SequencerFormSingletonized_nullUnsafe.SequencerControl.BacktestsShowAll_regardlessWhatIsChosenInCorrelator();
			// NOT_MY_JOB__FORM.CLOSE()_SENDS_MESSAGE_WHICH_DISPOSES_ALL_INNER_COMPONENTS/CONTROLS this.Dispose(true); this.Dispose();

			this.chartFormManager.CorrelatorForm = null;
		}
		void correlator_OnSequencedBacktestsOriginalMinusParameterValuesUnchosenIsRebuilt(object sender, SequencedBacktestsEventArgs e) {
			this.chartFormManager.SequencerFormSingletonized_nullUnsafe.SequencerControl.BacktestsReplaceWithCorrelated(e.SequencedBacktests);
		}

	}
}
