﻿using System;


namespace Sq1.Charting {
	public partial class PanelPrice : PanelBase {

		//public double	PriceRangeShown_cached					{ get { return this.VisibleMinDoubleMaxValueUnsafe - this.VisibleMaxDoubleMinValueUnsafe; } }
		public double	PriceRangeShownPlusSqueezers_cached		{ get { return base.VisibleMaxPlusBottomSqueezer_cached - base.VisibleMinMinusTopSqueezer_cached; } }
		public double	PriceLevelsShown_cached					{ get { return this.PriceRangeShownPlusSqueezers_cached / this.PriceStep; } }
		public double	PixelsPerPriceStep_cached				{ get { return base.Height / this.PriceLevelsShown_cached; } }
		public int		PixelsPerPriceStep3pxLeast_cached	{ get {
				int minimumPriceLevelThicknessRendered = 5;
				int ret = minimumPriceLevelThicknessRendered;
				if (double.IsInfinity(this.PixelsPerPriceStep_cached) == false) {
					double rounded = (int)Math.Round(this.PixelsPerPriceStep_cached);
					ret = (int)rounded;
				}
				if (ret < minimumPriceLevelThicknessRendered) ret = minimumPriceLevelThicknessRendered;
				return ret;
			} }
	}
}
