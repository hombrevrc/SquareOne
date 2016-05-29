﻿using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;

using BrightIdeasSoftware;

using Sq1.Core;
using Sq1.Core.DataTypes;
using Sq1.Core.Support;
using Sq1.Core.Streaming;

namespace Sq1.Widgets.Level2 {
	public partial class LevelTwoUserControl {	
		void olvDomCustomize() {
			this.olvDomCustomize_cellBackgound();

			this.olvAskCumulative.AspectGetter = delegate(object o) {
				LevelTwoEachLine askPriceBid = o as LevelTwoEachLine;
				if (askPriceBid == null) return "olvAskCumulative.AspectGetter: askPriceBid=null";
				if (double.IsNaN(askPriceBid.AskVolume)) return null;
				string formatVolume = this.symbolInfo.VolumeFormat;
				return askPriceBid.AskCumulative.ToString(formatVolume);
			};
			this.olvAsk.AspectGetter = delegate(object o) {
				LevelTwoEachLine askPriceBid = o as LevelTwoEachLine;
				if (askPriceBid == null) return "olvAsk.AspectGetter: askPriceBid=null";
				if (double.IsNaN(askPriceBid.AskVolume)) return null;
				string formatVolume = this.symbolInfo.VolumeFormat;
				return askPriceBid.AskVolume.ToString(formatVolume);
			};
			this.olvPrice.AspectGetter = delegate(object o) {
				LevelTwoEachLine askPriceBid = o as LevelTwoEachLine;
				if (askPriceBid == null) return "olvPrice.AspectGetter: askPriceBid=null";
				string formatPrice = this.symbolInfo.PriceFormat;
				string priceFormatted = askPriceBid.PriceLevel.ToString(formatPrice);
				if (askPriceBid.BidOrAsk == BidOrAsk.UNKNOWN) {
					priceFormatted = "spread: " + priceFormatted;
				}
				return priceFormatted;
			};
			this.olvBid.AspectGetter = delegate(object o) {
				LevelTwoEachLine askPriceBid = o as LevelTwoEachLine;
				if (askPriceBid == null) return "olvBid.AspectGetter: askPriceBid=null";
				if (double.IsNaN(askPriceBid.BidVolume)) return null;
				string formatVolume = this.symbolInfo.VolumeFormat;
				return askPriceBid.BidVolume.ToString(formatVolume);
			};
			this.olvBidCumulative.AspectGetter = delegate(object o) {
				LevelTwoEachLine askPriceBid = o as LevelTwoEachLine;
				if (askPriceBid == null) return "olvBidCumulative.AspectGetter: askPriceBid=null";
				if (double.IsNaN(askPriceBid.BidVolume)) return null;
				string formatVolume = this.symbolInfo.VolumeFormat;
				return askPriceBid.BidCumulative.ToString(formatVolume);
			};
		}
		
		//Color	LevelTwoLotsColorForeground;
		Color	LevelTwoAskColorBackground;
		//Color	LevelTwoAskColorContour;
		Color	LevelTwoBidColorBackground;
		//Color	LevelTwoBidColorContour;
		Color	LevelTwoSpreadColorBackground;

		Color	LevelTwoLessThanPriceStepColorBackground;
		Color	LevelTwoLessThanZeroColorBackground;

		void olvDomCustomize_cellBackgound() {
			// colors copypasted from ChartSettings.cs
			//this.LevelTwoLotsColorForeground	= Color.Black;
			//this.LevelTwoAskColorBackground		= Color.FromArgb(255, 230, 230);
			this.LevelTwoAskColorBackground		= Assembler.InstanceInitialized.ColorBackgroundRed_forPositionLoss;
			//this.LevelTwoAskColorContour		= Color.FromArgb(this.LevelTwoAskColorBackground.R - 50, this.LevelTwoAskColorBackground.G - 50, this.LevelTwoAskColorBackground.B - 50);
			//this.LevelTwoBidColorBackground		= Color.FromArgb(230, 255, 230);
			this.LevelTwoBidColorBackground		= Assembler.InstanceInitialized.ColorBackgroundGreen_forPositionProfit;
			//this.LevelTwoBidColorContour		= Color.FromArgb(this.LevelTwoBidColorBackground.R - 50, this.LevelTwoBidColorBackground.G - 50, this.LevelTwoBidColorBackground.B - 50);
			this.LevelTwoSpreadColorBackground	= Color.Gainsboro;

			this.LevelTwoLessThanPriceStepColorBackground	= Color.FromArgb(90, 200, 255);		// lightblue
			this.LevelTwoLessThanZeroColorBackground		= Color.FromArgb(255, 160, 120);	// orange

			this.OlvLevelTwo.FormatCell += new EventHandler<FormatCellEventArgs>(olvcLevelTwo_FormatCell);
			this.OlvLevelTwo.UseCellFormatEvents = true;
		}

		void olvcLevelTwo_FormatCell(object sender, FormatCellEventArgs e) {
			if (e.Model == null) return;
			LevelTwoEachLine askPriceBid = e.Model as LevelTwoEachLine;
			if (askPriceBid == null) {
				string msg = "MUST_BE_LevelTwoEachLine e.Model[" + e.Model + "] //olvcLevelTwo_FormatCell()";
				Assembler.PopupException(msg, null, false);
				return;
			}
			if (askPriceBid.BidOrAsk == BidOrAsk.UNKNOWN) {
				e.SubItem.BackColor = this.LevelTwoSpreadColorBackground;

				//it's SPREAD row that I inserted in LevelTwo.cs:138
				double spread = askPriceBid.PriceLevel;

				if (spread <= 0) {
					e.SubItem.BackColor = this.LevelTwoLessThanZeroColorBackground;					// again orange - something is definitely wrong
				} else {
					if (this.symbolInfo.PriceStep > 0) {
						if (spread < this.symbolInfo.PriceStep) {
							e.SubItem.BackColor = this.LevelTwoLessThanPriceStepColorBackground;	// lightblue
						}
					} else {
						e.SubItem.BackColor = this.LevelTwoLessThanZeroColorBackground;				// again orange - something is definitely wrong
					}
				}
				return;
			}
			if (e.Column == this.olvAskCumulative) {
				if (askPriceBid == null) return;
				if (askPriceBid.BidOrAsk != BidOrAsk.Ask) return;
				if (askPriceBid.Colorify == false) return;
				e.SubItem.BackColor = this.LevelTwoAskColorBackground;
				return;
			}
			if (e.Column == this.olvAsk) {
				if (askPriceBid == null) return;
				if (askPriceBid.BidOrAsk != BidOrAsk.Ask) return;
				if (askPriceBid.Colorify == false) return;
				e.SubItem.BackColor = this.LevelTwoAskColorBackground;
				return;
			}
			if (e.Column == this.olvBid) {
				if (askPriceBid == null) return;
				if (askPriceBid.BidOrAsk != BidOrAsk.Bid) return;
				if (askPriceBid.Colorify == false) return;
				e.SubItem.BackColor = this.LevelTwoBidColorBackground;
				return;
			}
			if (e.Column == this.olvBidCumulative) {
				if (askPriceBid == null) return;
				if (askPriceBid.BidOrAsk != BidOrAsk.Bid) return;
				if (askPriceBid.Colorify == false) return;
				e.SubItem.BackColor = this.LevelTwoBidColorBackground;
				return;
			}
		}
	}
}
