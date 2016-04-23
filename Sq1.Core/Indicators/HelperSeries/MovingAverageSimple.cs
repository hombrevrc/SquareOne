﻿using System;

using Sq1.Core.DataTypes;

namespace Sq1.Core.Indicators.HelperSeries {
	public class MovingAverageSimple : DataSeriesTimeBased {
		public DataSeriesTimeBased AverageFor;	// could be Bars, DataSeriesProxyBars or Indicator.OwnValues
		public int Period;
			
		public MovingAverageSimple(DataSeriesTimeBased averageFor, int period, int decimals = 2) : base(averageFor.ScaleInterval, decimals) {
			AverageFor = averageFor;
			Period = period;
		}
		public double Calculate_appendOwnValue_forNewStaticBarFormed_NanUnsafe(Bar newStaticBar, bool allowExistingValueSame = false) {
			double valueCalculated = this.CalculateOwnValue(newStaticBar);
			if (base.ContainsDate(newStaticBar.DateTimeOpen)) {
				double valueWeAlreadyHave = base[newStaticBar.DateTimeOpen];
				if (valueCalculated == valueWeAlreadyHave && allowExistingValueSame) {
					return valueCalculated;
				}
				string msg = "PROHIBITED_TO_CALCULATE_EACH_QUOTE_SLOW DONT_INVOKE_ME_TWICE on[" + newStaticBar.DateTimeOpen + "]"
					+ " thisBarValue[" + valueCalculated.ToString(base.Format) + "] valueWeAlreadyHave[" + valueWeAlreadyHave + "]";
				Assembler.PopupException(msg);
				//v1 return double.NaN;
				//v2 ALREADY_HAVE_ADDED  STILL_ADD_NAN_TO_KEEP_INDEXES_SYNCED_WITH_OWN_VALUES 
				//valueCalculated = double.NaN;
				return valueCalculated;
			}
			base.Append(newStaticBar.DateTimeOpen, valueCalculated);
			return valueCalculated;
		}

		public void SubstituteBars_withoutRecalculation(DataSeriesTimeBased averageFor) {
			this.AverageFor = averageFor;
		}

		public double CalculateOwnValue(Bar newStaticBar) {
			string msig = " // CalculateOwnValue(" + newStaticBar + ") " + this.ToString();
			// COPYPASTE_FROM_IndicatorAverageMovingSimple:Indicator BEGIN
			if (this.Period <= 0) return double.NaN;
			if (this.AverageFor.Count - 1 < this.Period) return double.NaN;
			if (newStaticBar.ParentBarsIndex  < this.Period - 1) return double.NaN;
			
			DataSeriesProxyBars barsBehind = this.AverageFor as DataSeriesProxyBars;
			if (barsBehind != null) {
				if (barsBehind.BarsBeingProxied != newStaticBar.ParentBars) {
					string msg = "YOU_FORGOT_TO_RESTORE_ORIGINAL_BARS_BEFORE_UNPAUSING_QUOTE_PUMP";
					if (newStaticBar.ParentBarsIndex >= barsBehind.Count) {
						msg = "AVOIDING_OUT_OF_BOUNDARY_EXCEPTION_FOR_this.AverageFor[i] " + msg;
					}
					Assembler.PopupException(msg + msig);
				}
			}

			double sum = 0;
			int slidingWindowRightBar = newStaticBar.ParentBarsIndex;
			int slidingWindowLeftBar = slidingWindowRightBar - this.Period + 1;	// FirstValidBarIndex must be Period+1
			int barsProcessedCheck = 0;
			for (int i = slidingWindowLeftBar; i <= slidingWindowRightBar; i++) {
				double eachBarInSlidingWindow = this.AverageFor[i];
				if (double.IsNaN(eachBarInSlidingWindow)) {
					string msg = "IGNORING_NAN_BAR_CLOSE_FOR_SMA.AverageFor[" + i + "]";
					Assembler.PopupException(msg + msig);
					continue;
				}
				sum += eachBarInSlidingWindow;
				barsProcessedCheck++;
			}
			if (barsProcessedCheck != this.Period) {
				string msg = "FYI barsProcessedCheck[" + barsProcessedCheck + "] != this.Period[" + this.Period + "]";
				Assembler.PopupException(msg + msig, null, false);
			}
			double ret = sum / this.Period;
			return ret;
			// COPYPASTE_FROM_IndicatorAverageMovingSimple:Indicator END
		}
	}
}
