using System;
using System.Collections.Generic;

using Newtonsoft.Json;

using Sq1.Core.DataFeed;

namespace Sq1.Core.DataTypes {
	//v1 public partial class Bars : BarsUnscaledSortedList {
	public partial class Bars : BarsUnscaled {
		[JsonIgnore]	public const string RANDOM_GENERATED_BARS = "RANDOM_GENERATED_BARS";

		[JsonIgnore]	public static int InstanceAbsno = 0;
		
		[JsonIgnore]	public	string				SymbolHumanReadable;
		[JsonIgnore]	public	string				SymbolAndDataSource			{ get {
			string ret = this.Symbol;
			if (this.DataSource != null) ret += " :: " + this.DataSource.Name;
			return ret;
		} }
		[JsonIgnore]	public	BarScaleInterval	ScaleInterval				{ get; private set; }

		[JsonIgnore]	public	MarketInfo			MarketInfo;
		[JsonIgnore]	public	DataSource			DataSource					{ get; internal set; }

		[JsonIgnore]	public	bool				IsIntraday					{ get { return this.ScaleInterval.IsIntraday; } }
		[JsonIgnore]	public	string				SymbolIntervalScaleDSN		{ get {
			string ret = this.Symbol + ":" + this.ScaleInterval.ToString();
			if (this.DataSource != null) ret += "/" + this.DataSource.Name;
			return "(" + ret + ")";
		} }
		[JsonIgnore]	public	string				SymbolIntervalScaleCount	{ get { return this.SymbolIntervalScaleDSN + " [" + base.Count + "]bars"; } }
		[JsonIgnore]	public	string				IntervalScaleCount			{ get { return "[" + this.ScaleInterval + "][" + base.Count + "]bars"; } }
		[JsonIgnore]	public	string				SymbolIntervalScaleCount_dataSourceName { get { return SymbolIntervalScaleCount + " :: " + this.DataSource.Name; } }

		[JsonIgnore]	public	Bar					BarStreaming_nullUnsafe		{ get; private set; }
		[JsonIgnore]	public	Bar					BarStreaming_nullUnsafeCloneReadonly { get {
				//v1
//				Bar lastStatic = this.BarStaticLast;
//				DateTime lastStaticOrServerNow = (lastStatic != null)
//					? lastStatic.DateTimeNextBarOpenUnconditional
//					: this.MarketInfo.ServerTimeNow;
//				Bar ret = new Bar(this.Symbol, this.ScaleInterval, lastStaticOrServerNow);
//				ret.SetParentForBackwardUpdate(this, base.Count);
//				if (BarStreaming != null) {
//					ret.AbsorbOHLCVfrom(BarStreaming);
//				} else {
//					int a = 1;
//				}
//				return ret;
				
				//v2
				if (this.BarStreaming_nullUnsafe == null) return null;
				return this.BarStreaming_nullUnsafe.Clone();
			} }

		[JsonIgnore]	public	int					MyInstance					{ get; private set; }
		[JsonIgnore]	public	string				MyInstanceAsString			{ get { return " //Instance#" + this.MyInstance; } }
		[JsonIgnore]	public	string				InstanceScaleCount			{ get {
			string ret = this.MyInstanceAsString;
			//if (string.IsNullOrEmpty(base.ReasonToExist) == false) ret += ":" + base.ReasonToExist;
			ret += this.IntervalScaleCount;
			return ret;
		} }

		[JsonIgnore]	public	string				ClonedFromInstanceName		{ get; private set; }
		[JsonIgnore]	public	string				InstanceAndReasonForClone	{ get {
			string ret = this.MyInstanceAsString;
			if (string.IsNullOrEmpty(this.ClonedFromInstanceName) == false) ret += ":" + this.ClonedFromInstanceName;
			return ret;
		} }

		[JsonIgnore]	public	List<Bar>			InnerBars_exposedOnlyForEditor_fromSafeCopy		{ get; private set; }

		public Bars SafeCopy_forEachReusableExecutor(string reasonToExist, bool exposeInnerBars_forEditor = false, bool reverseInnerBars_forEditor = false) { lock (base.BarsLock) {
			if (reverseInnerBars_forEditor) reasonToExist = "REVERTED " + reasonToExist;
			//Bars ret = new Bars(this.Symbol, this.ScaleInterval, "SafeCopy_oneCopyForEachDisposableExecutors");
			Bars clone = new Bars(this.Symbol, this.ScaleInterval, reasonToExist);
			foreach (Bar each in this.BarsList) {
				Bar cloneBar = each.CloneDetached();
				clone.BarAppendAttach(cloneBar);
			}
			clone.DataSource = this.DataSource;
			clone.MarketInfo = this.MarketInfo;
			//base.ReasonToExist = reasonToExist + "_CLONED_FROM_" + base.ReasonToExist;
			if (exposeInnerBars_forEditor) {
				clone.InnerBars_exposedOnlyForEditor_fromSafeCopy = clone.BarsList;
				if (reverseInnerBars_forEditor) {
					clone.Reverse();
				}
			}
			return clone;
		} }

		Bars(string symbol, string reasonToExist = "NOREASON") : base(symbol, reasonToExist) {
			ScaleInterval = new BarScaleInterval(BarScale.Unknown, 0);
			SymbolHumanReadable = "";
			MyInstance = ++InstanceAbsno;
		}
		public Bars(string symbol, BarScaleInterval scaleInterval, string reasonToExist) : this(symbol, reasonToExist) {
			this.ScaleInterval = scaleInterval;
			// it's a flashing tail but ALWAYS added into Bars for easy enumeration/charting/serialization;
			// ALWAYS ADDED, it is either still streaming (incomplete) OR it's complete (same instance becomes LastStaticBar);
			// while in streaming, you use AbsorbIntoStreaming(), when complete use CreateNewStreaming 
			//this.BarStreaming = new Bar(this.Symbol, this.ScaleInterval, DateTime.MinValue);
		}
		public Bars CloneBars_firstBarInside_avoidingLastBarNull(string reasonToExist = null, BarScaleInterval scaleIntervalConvertingTo = null) {
			Bars ret = this.CloneBars_zeroBarsInside_sameDataSource(reasonToExist, scaleIntervalConvertingTo);
			if (this.Count == 0) {
				string msg = "I_REFUSE_TO_ADD_FIRST_BAR CLONE_EMPTY_BARS__WITH_CloneBars_zeroBarsInside()_INSTEAD";
				Assembler.PopupException(msg);
				return ret;
			}
			Bar firstBar_noParentBackRef = this[0].CloneDetached();
			ret.BarStatic_appendAttach(firstBar_noParentBackRef);
			return ret;
		}
		public Bars CloneBars_zeroBarsInside_sameDataSource(string reasonToExist = null, BarScaleInterval scaleIntervalConvertingTo = null, bool exposedInnerBars_forEditor = false) {
			if (scaleIntervalConvertingTo == null) scaleIntervalConvertingTo = this.ScaleInterval;
			if (string.IsNullOrEmpty(reasonToExist)) reasonToExist = "InitializedFrom(" + this.ReasonToExist + ")";
			reasonToExist += this.InstanceScaleCount;
			Bars ret = new Bars(this.Symbol, scaleIntervalConvertingTo, reasonToExist);
			ret.SymbolHumanReadable = this.SymbolHumanReadable;
			ret.MarketInfo = this.MarketInfo;
			ret.SymbolInfo = this.SymbolInfo;
			ret.DataSource = this.DataSource;
			ret.ClonedFromInstanceName = this.InstanceScaleCount;
			return ret;
		}
		public Bar BarStreaming_createNewAttach_orAbsorb(Bar barToMerge_intoStreaming) { lock (base.BarsLock) {
			bool shouldAppend = this.BarLast == null || barToMerge_intoStreaming.DateTimeOpen >= this.BarLast.DateTime_nextBarOpen_unconditional;
			barToMerge_intoStreaming.CheckThrowFix_valuesOkay();
			if (shouldAppend) {	// if this.BarStreaming == null I'll have just one bar in Bars which will be streaming and no static 
				Bar barAdding = new Bar(this.Symbol, this.ScaleInterval, barToMerge_intoStreaming.DateTimeOpen);
				barAdding.Set_OHLCV_aligned(barToMerge_intoStreaming.Open, barToMerge_intoStreaming.High,
					barToMerge_intoStreaming.Low, barToMerge_intoStreaming.Close, barToMerge_intoStreaming.Volume, this.SymbolInfo);
				this.BarAppendAttach(barAdding);
				this.BarStreaming_nullUnsafe = barAdding;
				//OBSOLETE_NOW__USE_STREAMING_CONSUMERS_INSTEAD this.raiseOnBarStreamingAdded(barAdding);
			} else {
				if (this.BarStreaming_nullUnsafe == null) {
					this.BarStreaming_nullUnsafe = this.BarLast;
				}
				//base.BarAbsorbAppend(this.StreamingBar, open, high, low, close, volume);
				this.BarStreaming_nullUnsafe.MergeExpandHLCV_whileCompressing_manyBarsToOne(barToMerge_intoStreaming, false);	// duplicated volume for just added bar; moved up
				//OBSOLETE_NOW__USE_STREAMING_CONSUMERS_INSTEAD this.raiseOnBarStreamingUpdated(barToMerge_intoStreaming);
			}
			return this.BarStreaming_nullUnsafe;
		} }
		public Bar BarStatic_createAppendAttach(DateTime dateTime, double open, double high, double low, double close, double volume, bool throwError = false) { lock (base.BarsLock) {
			Bar barAdding = new Bar(this.Symbol, this.ScaleInterval, dateTime);
			barAdding.Set_OHLCV_aligned(open, high, low, close, volume, this.SymbolInfo);
			this.BarStatic_appendAttach(barAdding, throwError);
			return barAdding;
		} }
		public void BarStatic_appendAttach(Bar barAdding, bool throwError = false) { lock (base.BarsLock) {
			barAdding.CheckThrowFix_valuesOkay(throwError);
			this.BarStreaming_nullUnsafe = null;
			this.BarAppendAttach(barAdding);
			//OBSOLETE_NOW__USE_STREAMING_CONSUMERS_INSTEAD this.raiseOnBarStaticAdded(barAdding);
		} }
		protected override void CheckThrow_dateIsNotLess_thanScaleDictates(DateTime dateAdding) {
			if (this.Count == 0) return;
			if (dateAdding >= this.BarLast.DateTime_nextBarOpen_unconditional) return;
			throw new Exception("DATE_ADDING_IS_CLOSER_THAN_SCALEINTERVAL_DICTATES"
				+ ": dateAdding[" + dateAdding + "]<this.BarStaticLast.DateTimeNextBarOpenUnconditional["
				+ this.BarLast.DateTime_nextBarOpen_unconditional + "]");
		}
		protected void BarAppendAttach(Bar barAdding) { lock (base.BarsLock) {
			try {
				base.BarAppend(barAdding);
			} catch (Exception e) {
				string msg = "BARS_UNSCALED_IS_NOT_SATISFIED Bars.BarAppendBind[" + barAdding + "] to " + this;
				Assembler.PopupException(msg, e);
				return;
			}
			try {
				barAdding.SetParent_forBackwardUpdate(this, base.Count - 1);
			} catch (Exception e) {
				string msg = "BACKWARD_UPDATE_FAILED adding bar[" + barAdding + "] to " + this;
				Assembler.PopupException(msg, e);
				return;
			}
		} }
		public void BarStreaming_overrideDOHLCVwith(Bar bar) {
			if (bar == null) {
				string msg = "I_DONT_ACCEPT_NULL_BARS_TO OverrideStreamingDOHLCVwith(" + bar + ")";
				throw new Exception(msg);
			}
			if (this.BarStreaming_nullUnsafe == null) {
				string msg = "CAN_ONLY_OVERRIDE_STREAMING_NOT_NULL_WHILE_NOW_IT_IS_NULL OverrideStreamingDOHLCVwith(" + bar + "): this.streamingBar == null";
				throw new Exception(msg);
			}
			string msgSame = "BARS_IDENTICAL";
			bool sameDOHLCV = this.BarStreaming_nullUnsafe.HasSameDOHLCVas(bar, "barAbsorbed", "BarStreaming", ref msgSame);
			if (sameDOHLCV) {
				string msg = "IN_BAR_QUOTE NO_NEED_TO_ABSORB_ANYTHING__DESTINATION_HasSameDOHLCV msgSame[" + msgSame + "]";
				//Assembler.PopupException(msg, null, false);
				return;
			} else {
				string msg = "THERE_IS_NEED_TO_ABSORB_ANYTHING__DESTINATION_HasSameDOHLCV msgSame[" + msgSame + "]";
				//Assembler.PopupException(msg, null, false);
			}
			//this.streamingBar.DateTimeOpen = bar.DateTimeOpen;
			this.BarStreaming_nullUnsafe.AbsorbOHLCVfrom(bar);
			// IMPORTANT!! this.BarStreamingCloneReadonly freezes changes in the clone so that subscribers get the same StreamingBar
			//OBSOLETE_NOW__USE_STREAMING_CONSUMERS_INSTEAD this.raiseOnBarStreamingUpdated(this.BarStreaming_nullUnsafeCloneReadonly);
		}
		public override string ToString() {
			string ret = this.Symbol + "-" + this.IntervalScaleCount + this.MyInstanceAsString;
			if (base.Count > 0) {
				//try {
					string barLastStaticAsString = "BAR_STATIC_NULL";
 					Bar barLastStatic = this.BarStaticLast_nullUnsafe;
					if (barLastStatic != null) {
						barLastStaticAsString = this.ValueFormatted(barLastStatic.Close) + "] @[" + barLastStatic.DateTimeOpen;
					}
					ret += " LastStaticClose=[" + barLastStaticAsString + "]";
				//} catch (Exception e) {
				//	ret += " BARS_STATIC[" + (base.Count - 1) + "]_EXCEPTION";
				//}
				//try {
					string barStreamingAsString = "BAR_STREAMING_NULL";
 					Bar barStreaming = this.BarStreaming_nullUnsafeCloneReadonly;
					if (barStreaming != null) {
						barStreamingAsString = this.ValueFormatted(barStreaming.Close) + "] @[" + barStreaming.DateTimeOpen;
					}
					ret += " StreamingClose=[" + barStreamingAsString + "]";
				//} catch (Exception e) {
				//	ret += " BARS_STREAMING[" + base.Count + "]_EXCEPTION";
				//}
			}
			ret += " " + this.ReasonToExist;

			return ret;
		}
		public string ValueFormatted(double ohlc) {
			return ohlc.ToString(this.SymbolInfo.PriceFormat);
		}
		public override bool Equals(object another) {
			Bars bars = (Bars)another;
			string barsAsString = bars.ToString();
			string thisAsString = this.ToString();
			bool identicalContent = (
				bars.Symbol == this.Symbol
				&& bars.ScaleInterval == this.ScaleInterval
				&& bars.Count == base.Count
				&& bars.BarStaticFirst_nullUnsafe.ToString() == this.BarStaticFirst_nullUnsafe.ToString()
				&& bars.BarStaticLast_nullUnsafe.ToString() == this.BarStaticLast_nullUnsafe.ToString()
			);
			return identicalContent;
			//return (barsAsString == thisAsString);
		}

		[Obsolete("Designer uses reflection which doesn't feel static methods; instead, use new BarsBasic().GenerateAppend()")]
		public static Bars GenerateRandom(BarScaleInterval scaleInt,  int howManyBars = 10,
			string symbol = "SAMPLE", string reasonToExist = "test-ChartControl-DesignMode") {
			Bars ret = new Bars(symbol, scaleInt, reasonToExist);
			ret.GenerateAppend(howManyBars);
			return ret;
		}
		public void GenerateAppend(int howManyBars = 10) {
			int lowest = 1000;
			int highest = 9999;
			int volumeMax = 1000;
			float closeAwayFromOpenPotentialRange = 0.1f;		// how big candle bodies are, max
			float shadowsLengthRelativelyToCandleBody = 0.3f;	// how big candle shadows are, max
			DateTime dateCurrent = new DateTime(2011, 7, 2, 13, 26, 0);	//three years from now
			Random rand = new Random();
			int open = rand.Next(lowest, highest);
			for (int i = 0; i < howManyBars; i++) {
				int closeLowest = open - (int)Math.Round(open * closeAwayFromOpenPotentialRange);
				int closeHighest = open + (int)Math.Round(open * closeAwayFromOpenPotentialRange);
				if (closeLowest < lowest)
					closeLowest = lowest;
				if (closeHighest > highest)
					closeHighest = highest;
				int close = rand.Next(closeLowest, closeHighest);
				int candleBodyLow = open;
				int candleBodyHigh = close;
				if (open > close) {
					candleBodyLow = close;
					candleBodyHigh = open;
				}
				int candleBody = Math.Abs(close - open);
				int shadowLimit = (int)Math.Round(candleBody * shadowsLengthRelativelyToCandleBody);
				int high = rand.Next(candleBodyHigh, candleBodyHigh + shadowLimit);
				int low = rand.Next(candleBodyLow - shadowLimit, candleBodyLow);
				int volume = rand.Next(volumeMax);
				this.BarStatic_createAppendAttach(dateCurrent, open, high, low, close, volume);
				dateCurrent = dateCurrent.AddSeconds(this.ScaleInterval.AsTimeSpanInSeconds);
				open = close;
			}
		}
		public Bars Clone_selectRange(BarDataRange dataRangeRq, bool exposeInnerBars_forEditor = false) {
			DateTime startDate = DateTime.MinValue;
			DateTime endDate = DateTime.MaxValue;
			dataRangeRq.FillStartEndDate(out startDate, out endDate);
			if (startDate == DateTime.MinValue && endDate == DateTime.MaxValue && dataRangeRq.Range != BarRange.AllData && dataRangeRq.RecentBars == 0) return this;

			//v1 string reasonForClone = this.ReasonToExist + " [" + dataRangeRq.ToString() + "]";
			string reasonForCloning = "RANGE_SELECTED[" + dataRangeRq.ToString() + "]";
			Bars clone = this.CloneBars_zeroBarsInside_sameDataSource(reasonForCloning, this.ScaleInterval, exposeInnerBars_forEditor);
			int recentIndexStart = 0;
			if (dataRangeRq.RecentBars > 0) recentIndexStart = this.Count - dataRangeRq.RecentBars;  
			for (int i=0; i<this.Count; i++) {
				if (recentIndexStart > 0 && i < recentIndexStart) continue;
				Bar barAdding = this[i];
				bool skipThisBar = false;
				if (startDate > DateTime.MinValue && barAdding.DateTimeOpen < startDate) skipThisBar = true; 
				if (endDate < DateTime.MaxValue && barAdding.DateTimeOpen > endDate) skipThisBar = true;
				if (skipThisBar) continue;
				clone.BarStatic_appendAttach(barAdding.CloneDetached());
			}
			if (exposeInnerBars_forEditor) {
				clone.InnerBars_exposedOnlyForEditor_fromSafeCopy = clone.BarsList;
			}
			return clone;
		}
		
		void checkThrowCanConvert(BarScaleInterval scaleIntervalTo) {
			string msig = "checkThrowCanConvert(" + this.ScaleInterval + "=>" + scaleIntervalTo + ") for " + this + " datasource[" + this.DataSource + "]";
			string msg = "";
			bool canConvert = this.ScaleInterval.CanConvertTo(scaleIntervalTo);
			if (canConvert == false) msg += "CANNOT_CONVERT_TO_LARGER_SCALE_INTERVAL";
			if (this.Count == 0) msg += " EMPTY_BARS_FROM";
			//if (barsFrom.ScaleInterval.Scale == BarScale.Tick) msg += " TICKS_CAN_NOT_BE_CONVERTED_TO_ANYTHING";
			if (string.IsNullOrEmpty(msg)) return;
			throw new Exception(msg + msig);
		}
		public Bars ToLarger_scaleInterval(BarScaleInterval scaleIntervalTo) {
			if (this.ScaleInterval == scaleIntervalTo) return this;
			this.checkThrowCanConvert(scaleIntervalTo);

			//v1 string reasonForClone = this.ReasonToExist + "=>[" + scaleIntervalTo + "]";
			string reasonForClone = "COMPRESSED_CLONE_OF_" + this.IntervalScaleCount + "=>[" + scaleIntervalTo + "]";
			Bars barsConverted = this.CloneBars_zeroBarsInside_sameDataSource(reasonForClone, scaleIntervalTo);
			if (this.Count == 0) return barsConverted;
			
			Bar barFromFirst = this[0];
			Bar barCompressing = new Bar(this.Symbol, scaleIntervalTo, barFromFirst.DateTimeOpen);	// I'm happy with RoundDateDownInitTwoAuxDates()
			barCompressing.AbsorbOHLCVfrom(barFromFirst);

			for (int i = 1; i < this.Count; i++) {
				Bar barEach = this[i];
				if (barEach.DateTimeOpen >= barCompressing.DateTime_nextBarOpen_unconditional) {
					barsConverted.BarStatic_appendAttach(barCompressing);
					barCompressing = new Bar(this.Symbol, scaleIntervalTo, barEach.DateTimeOpen);
					barCompressing.AbsorbOHLCVfrom(barEach);
				} else {
					barCompressing.MergeExpandHLCV_whileCompressing_manyBarsToOne(barEach, true);
				}
			}
			return barsConverted;
		}

		public string Save() {
			string millisElapsed = "DIDNT_EVEN_START_WRITING_TO_FILE";

			if (this.DataSource == null) {
				string msg = "";
				Assembler.PopupException(msg, null, false);
				return millisElapsed;
			}
			this.DataSource.BarsSave(this, out millisElapsed);
			return millisElapsed;
		}

		public void SubstituteDataSource_forBarsSimulating(DataSource dataSource_livesim) {
			this.DataSource = dataSource_livesim;
		}
	}
}
