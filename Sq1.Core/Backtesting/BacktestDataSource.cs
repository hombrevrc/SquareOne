﻿using Sq1.Core.DataTypes;
using Sq1.Core.DataFeed;

namespace Sq1.Core.Backtesting {
	public class BacktestDataSource : DataSource {
		public BacktestStreaming StreamingAsBacktestNullUnsafe	{ get { return base.StreamingAdapter	as BacktestStreaming; } }
		public BacktestBroker	    BrokerAsBacktestNullUnsafe	{ get { return base   .BrokerAdapter	as BacktestBroker; } }

		public BacktestDataSource() {
			base.Name = "BacktestDataSource";
			base.StreamingAdapter	= new BacktestStreaming	("USED_FOR_BACKTESTS__CHARTLESS_NO_ORDEREXEC");
			base.BrokerAdapter		= new BacktestBroker	("USED_FOR_BACKTESTS__CHARTLESS_NO_ORDEREXEC");
		}
		public void Initialize(Bars bars, BacktestSpreadModeler spreadModeler) {
			base.MarketInfo = bars.MarketInfo;
			base.ScaleInterval = bars.ScaleInterval;
			base.Symbols.Clear();
			base.Symbols.Add(bars.Symbol);
			base.StreamingAdapter.InitializeFromDataSource(this);
			this.StreamingAsBacktestNullUnsafe.SpreadModeler = spreadModeler;
			base.BrokerAdapter.InitializeDataSource_inverse(this, base.StreamingAdapter, null);
		}

		public override string ToString() {
			string ret = this.Name;
			if (this.ScaleInterval != null) ret += "(" + this.ScaleInterval.ToString() + ")";
			ret += this.SymbolsCSV;
			ret += " {" + this.StreamingAdapterName + ":" + this.BrokerAdapterName + "}";
			return ret;
		}
	}
}