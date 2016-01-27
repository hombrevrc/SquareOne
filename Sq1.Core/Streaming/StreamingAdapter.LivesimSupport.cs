using System;
using System.Collections.Generic;

using Newtonsoft.Json;

using Sq1.Core.DataFeed;
using Sq1.Core.Livesim;
using Sq1.Core.DataTypes;
using Sq1.Core.Charting;
using Sq1.Core.StrategyBase;

namespace Sq1.Core.Streaming {
	public partial class StreamingAdapter {
		[JsonIgnore]	DataDistributor		dataDistributor_preLivesimForSymbolLivesimming;
		[JsonIgnore]	DataDistributor		dataDistributorSolidifiers_preLivesimForSymbolLivesimming;
		[JsonIgnore]	LivesimStreaming	livesimStreamingForWhomDataDistributorsAreReplaced;

		[JsonIgnore]	public bool			DataDistributorsAreReplacedByLivesim_dontPauseNeighborsOnBacktestContextInitRestore {
			get { return this.livesimStreamingForWhomDataDistributorsAreReplaced != null; } }

		internal void SubstituteDistributorForSymbolsLivesimming_extractChartIntoSeparateDistributor(LivesimStreaming livesimStreaming) {
			this.livesimStreamingForWhomDataDistributorsAreReplaced = livesimStreaming;

			ScriptExecutor executor = this.livesimStreamingForWhomDataDistributorsAreReplaced.Livesimulator.Executor;
			string reasonForNewDistributor = "LIVESIM_STARTED:"
				+ this.livesimStreamingForWhomDataDistributorsAreReplaced.Name
				+ "/" + executor.StrategyName
				// NO_IT_DOES_CONTAIN_GARBAGE!!! + "@" + executor.Bars.ToString()	// should not contain Static/Streaming bars since Count=0
				+ executor.Bars.SymbolIntervalScale
				;

			this.dataDistributor_preLivesimForSymbolLivesimming = this.DataDistributor_replacedForLivesim;
			this.dataDistributor_preLivesimForSymbolLivesimming.AllQuotePumps_Pause(reasonForNewDistributor);
			this.DataDistributor_replacedForLivesim = new DataDistributor(this, reasonForNewDistributor);

			string symbol					= livesimStreaming.Livesimulator.BarsSimulating.Symbol;
			BarScaleInterval scaleInterval	= livesimStreaming.Livesimulator.BarsSimulating.ScaleInterval;
			string symbolIntervalScale		= livesimStreaming.Livesimulator.BarsSimulating.SymbolIntervalScale;
			StreamingConsumer chartShadow	= livesimStreaming.Livesimulator.Executor.ChartShadow.ChartStreamingConsumer;

			bool willPushUsingPumpInSeparateThread = true;	// I wanna know which thread is going to be used; if DDE-client then cool; YES_IT_WAS_DDE_THREAD
			//bool willPushUsingPumpInSeparateThread = true;			// and now I wanna Livesim just like it will be working with Real Quik
			//if (this.distributorCharts_preLivesimForSymbolLivesimming.ConsumerQuoteIsSubscribed(symbol, scaleInterval, chartShadow) == false) {
			//    string msg = "EXECUTOR'S_CHART_SHADOW_WASNT_QUOTECONSUMING_WHAT_YOU_GONNA_LIVESIM NONSENSE " + symbolIntervalScale;
			//    Assembler.PopupException(msg, null, false);
			//}
			// the chart will be subscribed twice to the same Symbol+ScaleInterval, yes! but the original distributor is backed up and PushQuoteReceived will only push to the new DataDistributor(this) with one chart only
			this.DataDistributor_replacedForLivesim.ConsumerQuoteSubscribe(symbol, scaleInterval, chartShadow, willPushUsingPumpInSeparateThread);

			//if (this.distributorCharts_preLivesimForSymbolLivesimming.ConsumerBarIsSubscribed(symbol, scaleInterval, chartShadow) == false) {
			//    string msg = "EXECUTOR'S_CHART_SHADOW_WASNT_BARCONSUMING_WHAT_YOU_GONNA_LIVESIM NONSENSE " + symbolIntervalScale;
			//    Assembler.PopupException(msg, null, false);
			//}
			// the chart will be subscribed twice to the same Symbol+ScaleInterval, yes! but the original distributor is backed up and PushBarReceived will only push to the new DataDistributor(this) with one chart only
			this.DataDistributor_replacedForLivesim.ConsumerBarSubscribe(symbol, scaleInterval, chartShadow, willPushUsingPumpInSeparateThread);

			this.dataDistributorSolidifiers_preLivesimForSymbolLivesimming = this.DataDistributorSolidifiers_replacedForLivesim;
			this.dataDistributorSolidifiers_preLivesimForSymbolLivesimming.AllQuotePumps_Pause(reasonForNewDistributor);
			this.DataDistributorSolidifiers_replacedForLivesim = new DataDistributor(this, reasonForNewDistributor);		// EMPTY!!! exactly what I wanted

			this.DataDistributor_replacedForLivesim.SetQuotePumpThreadName_sinceNoMoreSubscribersWillFollowFor(symbol, scaleInterval);

			string msg1 = "THESE_STREAMING_CONSUMERS_LOST_INCOMING_QUOTES_FOR_THE_DURATION_OF_LIVESIM: ";
			string msg2= this.dataDistributor_preLivesimForSymbolLivesimming.ToString();
			Assembler.PopupException(msg1 + msg2, null, false);
		}

		internal void SubstituteDistributorForSymbolsLivesimming_restoreOriginalDistributor() {
			ScriptExecutor executor = this.livesimStreamingForWhomDataDistributorsAreReplaced.Livesimulator.Executor;
			string reasonForStoppingReplacedDistributor = this.livesimStreamingForWhomDataDistributorsAreReplaced.Name
				+ "==RESTORING_AFTER_LIVESIM" + executor.StrategyName + "@" + executor.Bars.ToString();	// should not contain Static/Streaming bars since Count=0

			this.DataDistributor_replacedForLivesim.AllQuotePumps_Stop(reasonForStoppingReplacedDistributor);

			this.DataDistributor_replacedForLivesim				= this.dataDistributor_preLivesimForSymbolLivesimming;
			this.DataDistributorSolidifiers_replacedForLivesim	= this.dataDistributorSolidifiers_preLivesimForSymbolLivesimming;

			this.DataDistributor_replacedForLivesim				.AllQuotePumps_Unpause(reasonForStoppingReplacedDistributor);
			this.DataDistributorSolidifiers_replacedForLivesim	.AllQuotePumps_Unpause(reasonForStoppingReplacedDistributor);

			string msg1 = "STREAMING_CONSUMERS_RESTORED_CONNECTIVITY_TO_STREAMING_ADAPTER_AFTER_LIVESIM: "
				+ this.DataDistributor_replacedForLivesim.ToString() + " SOLIDIFIERS:" + this.DataDistributorSolidifiers_replacedForLivesim	;
			Assembler.PopupException(msg1, null, false);

			this.livesimStreamingForWhomDataDistributorsAreReplaced = null;
		}

		public string ReasonWhyLivesimCanNotBeStartedForSymbol(string symbol, ChartShadow chartShadow) {
			string ret = null;
			List<SymbolScaleDistributionChannel> channelsCloned = this.DataDistributor_replacedForLivesim
				.GetDistributionChannels_forSymbol_exceptForChartLivesimming(symbol, null, chartShadow.ChartStreamingConsumer);
			if (channelsCloned == null) {
				ret = "YOU_MUST_HAVE_CHART_SUBSCRIBED_TO_SYMBOL_BEFORE_STARTING_LIVESIM";
			} else {
				foreach(SymbolScaleDistributionChannel channelCloned in channelsCloned) {
					if (ret != null) ret += ", ";
					ret += channelCloned.ToString();
				}
				if (ret != null) ret = "STREAMING[" + this.ToString() + "] HAS_CONSUMERS[" + ret + "]";
			}
			return ret;
		}

	}
}