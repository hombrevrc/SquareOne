﻿using System;

using Sq1.Core.DataTypes;

namespace Sq1.Core.Indicators.HelperSeries {
	public class TrueRangeSeries : DataSeriesTimeBased {
		public TrueRangeSeries(BarScaleInterval scaleInterval) : base(scaleInterval) {
			base.Description = "TrueRangeSeries";
		}
		
		public double CalculateOwnValue_onNewStaticBarFormed_invokedAtEachBarNoExceptions_NoPeriodWaiting(Bar newStaticBar) {
			if (base.ContainsDate(newStaticBar.DateTimeOpen)) {
				string msg = "DONT_INVOKE_ME_TWICE on[" + newStaticBar.DateTimeOpen + "]";
				Assembler.PopupException(msg, null, false);
				return double.NaN;
			}
			double thisBarValue = this.calculateOwnValue(newStaticBar);
			base.Append(newStaticBar.DateTimeOpen, thisBarValue);
			return thisBarValue;
		}

		double calculateOwnValue(Bar newStaticBar) {
			//https://www.tradingview.com/stock-charts-support/index.php/Average_True_Range_(ATR)
			//The True Range is the largest of the following:
			//1) The Current Period High minus (-) Current Period Low
			//2) The Absolute Value (abs) of the Current Period High minus (-) The Previous Period Close
			//3) The Absolute Value (abs) of the Current Period Low minus (-) The Previous Period Close
			//true range=max[(high - low), abs(high - previous close), abs (low - previous close)]
			
			double hiLo = newStaticBar.HighLowDistance;
			if (newStaticBar.ParentBarsIndex == 0) return hiLo;

			Bar prevBar = newStaticBar.BarPrevious_nullUnsafe;
			if (prevBar == null) return hiLo;
			
			double hiPrevClose = Math.Abs(newStaticBar.High - prevBar.Close);
			double loPrevClose = Math.Abs(newStaticBar.Low  - prevBar.Close);
			
			double ret = Math.Max(hiPrevClose, loPrevClose);
			ret = Math.Max(ret, hiLo);
			return ret;
		}
		
	}
}
