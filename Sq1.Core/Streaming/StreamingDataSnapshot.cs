﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using Newtonsoft.Json;
using Sq1.Core.DataTypes;
using Sq1.Core.Execution;

namespace Sq1.Core.Streaming {
	public class StreamingDataSnapshot {
		[JsonIgnore]	StreamingProvider streamingProvider;
		[JsonProperty]	protected Dictionary<string, double> BestBid { get; private set; }
		[JsonProperty]	protected Dictionary<string, double> BestAsk { get; private set; }
		[JsonProperty]	protected Dictionary<string, Quote> LastQuotesReceived { get; private set; }

		[JsonIgnore]	protected Object LockLastQuote = new Object();
		[JsonIgnore]	protected Object LockBestBid = new Object();
		[JsonIgnore]	protected Object LockBestAsk = new Object();

		[JsonIgnore]	public string SymbolsSubscribedAndReceiving { get {
				string ret = "";
				foreach (string symbol in LastQuotesReceived.Keys) {
					if (ret.Length > 0) ret += ",";
					ret += symbol + ":" + ((LastQuotesReceived[symbol] == null) ? "NULL" : LastQuotesReceived[symbol].AbsnoPerSymbol.ToString());
				}
				return ret;
			} }
		public StreamingDataSnapshot(StreamingProvider streamingProvider) {
			this.streamingProvider = streamingProvider;
			this.LastQuotesReceived = new Dictionary<string, Quote>();
			this.BestBid = new Dictionary<string, double>();
			this.BestAsk = new Dictionary<string, double>();
		}

		public void InitializeLastQuoteReceived(List<string> symbols) {
			foreach (string symbol in symbols) {
				if (this.LastQuotesReceived.ContainsKey(symbol)) continue;
				this.LastQuotesReceived.Add(symbol, null);
			}
		}
		public void LastQuotePutNull(string symbol) {
			lock (LockLastQuote) {
				if (this.LastQuotesReceived.ContainsKey(symbol)) {
					this.LastQuotesReceived[symbol] = null;
				} else {
					this.LastQuotesReceived.Add(symbol, null);
				}
			}
		}
		protected void LastQuoteUpdate(Quote quote) {
			string msig = " StreamingDataSnapshot.LastQuoteUpdate(" + quote.ToString() + ")";
			Quote last = this.LastQuotesReceived[quote.Symbol];
			if (last == null) {
				this.LastQuotesReceived[quote.Symbol] = quote;
				return;
			}
			if (last == quote) {
				string msg = "How come you update twice to the same quote?";
				Assembler.PopupException(msg + msig, null);
				return;
			}
			if (last.AbsnoPerSymbol >= quote.AbsnoPerSymbol) {
				string msg = "DONT_FEED_ME_WITH_OLD_QUOTES (????QuoteQuik #-1/0 AUTOGEN)";
				Assembler.PopupException(msg + msig, null, false);
				return;
			}
			this.LastQuotesReceived[quote.Symbol] = quote;
		}
		public Quote LastQuoteGetForSymbol(string Symbol) {
			lock (this.LockLastQuote) {
				if (this.LastQuotesReceived.ContainsKey(Symbol) == false) return null;
				return this.LastQuotesReceived[Symbol];
			}
		}
		public double LastQuoteGetPriceForMarketOrder(string Symbol) {
			Quote lastQuote = LastQuoteGetForSymbol(Symbol);
			if (lastQuote == null) return 0;
			if (lastQuote.LastDealBidOrAsk == BidOrAsk.UNKNOWN) {
				Debugger.Break();
				return 0;
			}
			return lastQuote.LastDealPrice;
		}

		public virtual void UpdateLastBidAskSnapFromQuote(Quote quote) {
			this.LastQuoteUpdate(quote);

			if (double.IsNaN(quote.Bid) || double.IsNaN(quote.Ask)) {
				if (false) throw new Exception("You seem to process Bars.LastBar with Partials=NaN");
				return;
			}
			if (quote.Bid != 0 && quote.Ask != 0) {
				this.BestBidAskPutForSymbol(quote.Symbol, quote.Bid, quote.Ask);
			}
		}
		public void BestBidAskPutForSymbol(string Symbol, double bid, double ask) {
			lock (this.BestBid) {
				this.BestBid[Symbol] = bid;
			}
			lock (this.BestAsk) {
				this.BestAsk[Symbol] = ask;
			}
		}
		public double BidOrAskFor(string Symbol, PositionLongShort direction) {
			if (direction == PositionLongShort.Unknown) {
				string msg = "BidOrAskFor(" + Symbol + ", " + direction + "): Bid and Ask are wrong to return for [" + direction + "]";
				throw new Exception(msg);
			}
			double price = (direction == PositionLongShort.Long)
				? this.BestBidGetForMarketOrder(Symbol) : this.BestAskGetForMarketOrder(Symbol);
			return price;
		}
		public double BestBidGetForMarketOrder(string Symbol) {
			double ret = 0;
			lock (this.BestBid) {
				if (this.BestBid.ContainsKey(Symbol)) {
					ret = this.BestBid[Symbol];
				}
			}
			return ret;
		}
		public double BestAskGetForMarketOrder(string Symbol) {
			double ret = 0;
			lock (this.BestAsk) {
				if (this.BestAsk.ContainsKey(Symbol)) {
					ret = this.BestAsk[Symbol];
				}
			}
			return ret;
		}

		public virtual double GetAlignedBidOrAskForTidalOrCrossMarketFromStreaming(string symbol, Direction direction
				, out OrderSpreadSide oss, bool forceCrossMarket) {
			string msig = " GetAlignedBidOrAskForTidalOrCrossMarketFromStreaming(" + symbol + ", " + direction + ")";
			double priceLastQuote = this.LastQuoteGetPriceForMarketOrder(symbol);
			if (priceLastQuote == 0) {
				string msg = "QuickCheck ZERO priceLastQuote=" + priceLastQuote + " for Symbol=[" + symbol + "]"
					+ " from streamingProvider[" + this.streamingProvider.Name + "].StreamingDataSnapshot";
				Assembler.PopupException(msg);
				//throw new Exception(msg);
			}
			double currentBid = this.BestBidGetForMarketOrder(symbol);
			double currentAsk = this.BestAskGetForMarketOrder(symbol);
			if (currentBid == 0) {
				string msg = "ZERO currentBid=" + currentBid + " for Symbol=[" + symbol + "]"
					+ " while priceLastQuote=[" + priceLastQuote + "]"
					+ " from streamingProvider[" + this.streamingProvider.Name + "].StreamingDataSnapshot";
				;
				Assembler.PopupException(msg);
				//throw new Exception(msg);
			}
			if (currentAsk == 0) {
				string msg = "ZERO currentAsk=" + currentAsk + " for Symbol=[" + symbol + "]"
					+ " while priceLastQuote=[" + priceLastQuote + "]"
					+ " from streamingProvider[" + this.streamingProvider.Name + "].StreamingDataSnapshot";
				Assembler.PopupException(msg);
				//throw new Exception(msg);
			}

			double price = 0;
			oss = OrderSpreadSide.ERROR;

			SymbolInfo symbolInfo = Assembler.InstanceInitialized.RepositorySymbolInfo.FindSymbolInfo(symbol);
			MarketOrderAs spreadSide;
			if (forceCrossMarket) {
				spreadSide = MarketOrderAs.LimitCrossMarket;
			} else {
				spreadSide = (symbolInfo == null) ? MarketOrderAs.LimitCrossMarket : symbolInfo.MarketOrderAs;
			}
			if (spreadSide == MarketOrderAs.ERROR || spreadSide == MarketOrderAs.Unknown) {
				string msg = "Set Symbol[" + symbol + "].SymbolInfo.LimitCrossMarket; should not be spreadSide[" + spreadSide + "]";
				Assembler.PopupException(msg);
				throw new Exception(msg);
				//return;
			}

			switch (direction) {
				case Direction.Buy:
				case Direction.Cover:
					switch (spreadSide) {
						case MarketOrderAs.LimitTidal:
							oss = OrderSpreadSide.AskTidal;
							price = currentAsk;
							break;
						case MarketOrderAs.LimitCrossMarket:
							oss = OrderSpreadSide.BidCrossed;
							price = currentBid;		// Unknown (Order default) becomes CrossMarket
							break;
						case MarketOrderAs.MarketMinMaxSentToBroker:
							oss = OrderSpreadSide.MaxPrice;
							price = currentAsk;
							break;
						case MarketOrderAs.MarketZeroSentToBroker:
							oss = OrderSpreadSide.MarketPrice;
							price = currentAsk;		// looks like default, must be crossmarket to fill it right now
							break;
						default:
							string msg2 = "no handler for spreadSide[" + spreadSide + "] direction[" + direction + "]";
							throw new Exception(msg2);
					}
					break;
				case Direction.Short:
				case Direction.Sell:
					switch (spreadSide) {
						case MarketOrderAs.LimitTidal:
							oss = OrderSpreadSide.BidTidal;
							price = currentBid;
							break;
						case MarketOrderAs.LimitCrossMarket:
							oss = OrderSpreadSide.AskCrossed;
							price = currentAsk;		// Unknown (Order default) becomes CrossMarket
							break;
						case MarketOrderAs.MarketMinMaxSentToBroker:
							oss = OrderSpreadSide.MinPrice;
							price = currentBid;		// Unknown (Order default) becomes CrossMarket
							break;
						case MarketOrderAs.MarketZeroSentToBroker:
							oss = OrderSpreadSide.MarketPrice;
							price = currentBid;		// looks like default, must be crossmarket to fill it right now
							break;
						default:
							string msg2 = "no handler for spreadSide[" + spreadSide + "] direction[" + direction + "]";
							throw new Exception(msg2);
					}
					break;
				default:
					string msg = "no handler for direction[" + direction + "]";
					throw new Exception(msg);
			}

			if (double.IsNaN(price)) {
				Debugger.Break();
			}
			symbolInfo = Assembler.InstanceInitialized.RepositorySymbolInfo.FindSymbolInfoOrNew(symbol);
			//v2
			price = symbolInfo.AlignAlertToPriceLevelSimplified(price, direction, MarketLimitStop.Market);

			//v1
			#if DEBUG	// REMOVE_ONCE_NEW_ALIGNMENT_MATURES_DECEMBER_15TH_2014
			double price1 = symbolInfo.AlignOrderToPriceLevel(price, direction, MarketLimitStop.Market);
			if (price1 != price) {
				string msg3 = "FIX_DEFINITELY_DIFFERENT_POSTPONE_TILL_ORDER_EXECUTOR_BACK_FOR_QUIK_BROKER";
				Assembler.PopupException(msg3 + msig, null);
			}
			#endif
			
			return price;
		}
	}
}
