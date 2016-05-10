﻿using System;

using Sq1.Core.DataTypes;

namespace Sq1.Core.Backtesting {
	public class BacktestSpreadModelerPercentage : BacktestSpreadModeler {
		double Percentage;
		double PartOfPrice;
		
		public BacktestSpreadModelerPercentage(double percentage, bool alignToPriceLevel = true) : base(alignToPriceLevel) {
			Percentage = percentage;
			PartOfPrice = percentage / 100;
		}
		public override double FillBidAsk_symmetrically(QuoteGenerated quote, double openOrClosePrice, Bar barSimulated) {
			string msig = " " + this.GetType().Name + ".GenerateFillBidAskSymmetrically(" + quote.ToString() + ")";
			SymbolInfo symbolInfo;
			try {
				symbolInfo = base.CheckThrowCanReachSymbolInfo(quote);
			} catch (Exception ex) {
				Assembler.PopupException(ex.Message + msig);
				return double.NaN;
			}

			double spread = openOrClosePrice * this.PartOfPrice;
			double spreadAligned = symbolInfo.AlignToPriceLevel(spread, PriceLevelRoundingMode.RoundToClosest);	//changed to RoundToClosest and checking below; RoundUp so I wont' get spread = 0
			if (spreadAligned == 0) {
				//string msg = "you can't use RoundDown here";
				//Debugger.Break();
				spreadAligned = symbolInfo.PriceStep;
			}
			
			if (barSimulated.HighLowDistance == 0) {
				double halfSpread = spreadAligned / 2d;
				halfSpread = symbolInfo.AlignToPriceLevel(halfSpread, PriceLevelRoundingMode.RoundToClosest); 
				quote.Bid = openOrClosePrice - halfSpread;
				quote.Ask = quote.Bid + spreadAligned;
			} else {
				if (barSimulated.HighLowDistance < spreadAligned) {
					string msg = "barSimulated.HighLowDistance[" + barSimulated.HighLowDistance + "] < spreadAligned[" + spreadAligned + "]...";
					Assembler.PopupException(msg, null, false);
				}
				if (openOrClosePrice - spreadAligned >= barSimulated.Low) {
					quote.Ask = openOrClosePrice; 
					quote.Bid = quote.Ask - spreadAligned;
				} else {
					quote.Bid = openOrClosePrice; 
					quote.Ask = quote.Bid + spreadAligned;
					if (openOrClosePrice + spreadAligned > barSimulated.High) {
						string msg = "openOrClosePrice[" + openOrClosePrice + "] +- spreadAligned[" + spreadAligned + "]"
							+ " is beyond Low[" + barSimulated.Low + "]...High[" + barSimulated.High + "]";
						Assembler.PopupException(msg, null, false);
					}
				}
			}

			this.AlignBidAsk_toPriceLevel(quote, PriceLevelRoundingMode.RoundToClosest, spreadAligned);
			if (double.IsNaN(quote.Spread)) {
				string msg = "SPREAD_MUST_NOT_BE_NAN for " + quote;
				Assembler.PopupException(msg);
			}
			if (quote.Spread == 0) {
				string msg = "SPREAD_MUST_NOT_BE_ZERO for " + quote;
				Assembler.PopupException(msg);
			}
			return spreadAligned;
		}
		public override void FillAsk_basedOnBid_aligned(QuoteGenerated quote) {
			string msig = " " + this.GetType().Name + ".GenerateFillBidAskSymmetrically(" + quote.ToString() + ")";
			SymbolInfo symbolInfo;
			try {
				symbolInfo = base.CheckThrowCanReachSymbolInfo(quote);
			} catch (Exception ex) {
				Assembler.PopupException(ex.Message + msig);
				return;
			}

			double spread = quote.Bid * this.PartOfPrice;
			double spreadAligned = symbolInfo.AlignToPriceLevel(spread, PriceLevelRoundingMode.RoundToClosest);	//changed to RoundToClosest and checking below; RoundUp so I wont' get spread = 0
			if (spreadAligned == 0) {
				string msg = "you can't use RoundDown here";
				//Debugger.Break();
				spreadAligned = symbolInfo.PriceStep;
			}
			
			quote.Ask = quote.Bid + spreadAligned;

			// attempt to make generatedBar.Low,Hight exactly the same as originalBar.Low,Height; I check upstack if I succeeded
			//base.AlignBidAskToPriceLevel(quote, PriceLevelRoundingMode.RoundUp, spreadAligned);
			//if (quote.Spread == 0) Debugger.Break();
		}
		public override void FillBid_basedOnAsk_aligned(QuoteGenerated quote) {
			string msig = " " + this.GetType().Name + ".GenerateFillBidAskSymmetrically(" + quote.ToString() + ")";
			SymbolInfo symbolInfo;
			try {
				symbolInfo = base.CheckThrowCanReachSymbolInfo(quote);
			} catch (Exception ex) {
				Assembler.PopupException(ex.Message + msig);
				return;
			}

			double spread = quote.Ask * this.PartOfPrice;
			double spreadAligned = symbolInfo.AlignToPriceLevel(spread, PriceLevelRoundingMode.RoundToClosest);	//changed to RoundToClosest and checking below; RoundUp so I wont' get spread = 0
			if (spreadAligned == 0) {
				//string msg = "you can't use RoundDown here";
				//Debugger.Break();
				spreadAligned = symbolInfo.PriceStep;
			}
			
			quote.Bid = quote.Ask - spreadAligned;
			// attempt to make generatedBar.Low,Hight exactly the same as originalBar.Low,Height; I check upstack if I succeeded 
			//base.AlignBidAskToPriceLevel(quote, PriceLevelRoundingMode.RoundDown, spreadAligned);
			//if (quote.Spread == 0) Debugger.Break();
		}
		public override string ToString() {
			return "BacktestSpreadModelerPercentage[" + Percentage + "]%";
		}

	}
}
