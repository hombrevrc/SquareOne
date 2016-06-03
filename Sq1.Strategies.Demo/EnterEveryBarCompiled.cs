﻿using System;
using System.Diagnostics;
using System.Drawing;

using Sq1.Core;
using Sq1.Core.DataTypes;
using Sq1.Core.Execution;
using Sq1.Core.Indicators;
using Sq1.Core.StrategyBase;

using Sq1.Indicators;

namespace Sq1.Strategies.Demo {
	public class EnterEveryBarCompiled : Script {
		// if an indicator is NULL (isn't initialized in this.ctor()) you'll see INDICATOR_DECLARED_BUT_NOT_CREATED+ASSIGNED_IN_CONSTRUCTOR in ExceptionsForm 
		IndicatorMovingAverageSimple	MAfast;
		ScriptParameter					test;
		ScriptParameter					verbose;
		Font							fontConsolas8bold;
		Font							fontConsolas7;

		public EnterEveryBarCompiled() {
			MAfast = new IndicatorMovingAverageSimple();
			MAfast.ParamPeriod = new IndicatorParameter("Period", 15, 10, 20, 1);
			MAfast.LineWidth = 2;
			MAfast.LineColor = Color.LightSeaGreen;
			
			//base.ScriptParameterCreateRegister(1, "test", 0, 0, 10, 1);
			test = new ScriptParameter(1, "test", 0, 0, 10, 1, "hopefully this will go to tooltip");

			//base.ScriptParameterCreateRegister(2, "verbose", 0, 0, 1, 1, "set to 0 if you don't want log() to spam your Exceptions window");
			verbose = new ScriptParameter(2, "verbose", 0, 0, 10, 1, "set to 0 if you don't want log() to spam your Exceptions window");

			// trying to reduce {HANDLES(after 121 sequencer runs) => 61k} in taskmgr32.exe; I don't want to dispose reusable GDI+ objects
			fontConsolas8bold	= new Font("Consolas", 8, FontStyle.Bold);
			fontConsolas7		= new Font("Consolas", 7);
		}
		
		protected void log(string msg) {
			return;

			if (this.verbose.ValueCurrent == 0) {
				return;
			}
			string whereIam = "\n\r\n\rEnterEveryBar.cs now=[" + DateTime.Now.ToString("ddd dd-MMM-yyyy HH:mm:ss.fff") + "]";
			//this.Executor.PopupException(msg + whereIam);
		}
		public override void InitializeBacktest() {
			//Debugger.Break();
			//this.PadBars(0);
			if (base.Strategy == null) {
				log("CANT_SET_EXCEPTIONS_LIMIT: base.Strategy == null");
				#if DEBUG
				Debugger.Break();
				#endif
				return;
			}
			base.Strategy.ExceptionsLimitToAbortBacktest = 10;
			//this.MAslow.NotOnChartSymbol = "SANDP-FUT";
			//this.MAslow.NotOnChartBarScaleInterval = new BarScaleInterval(BarScale.Hour, 1);
			//this.MAslow.NotOnChartBarScaleInterval = new BarScaleInterval(BarScale.Minute, 15);
			//this.MAslow.LineWidth = 2;
			//this.MAslow.LineColor = System.Drawing.Color.LightCoral;

			//if (this.Executor.Sequencer.IsRunningNow == false) {
			//	string msg = "SEQUENCER_IS_ALREADY_RUN_KOZ_4CORES-SPAWNED_EXECUTORS(WHOS_MY_FATER)_ARE_POINTING_TO_SAME_SEQUENCER";
			//	Assembler.PopupException(msg);
			//}
			testChartLabelDrawOnNextLineModify();
		}
		void testChartLabelDrawOnNextLineModify() {
			if (this.Executor.Sequencer.IsRunningNow) return;
			//DISPOSE_OR_TURN_TO_CLASS_VAR Font font = new Font(FontFamily.GenericMonospace, 8, FontStyle.Bold);
			//base.Executor.ChartConditionalChartLabelDrawOnNextLineModify("labelTest", "test[" + test+ "]", font, Color.Brown, Color.Empty);
			base.Executor.ChartConditional_chartLabelDrawOnNextLineModify("labelTest", "test["
				+ this.test.ValueCurrent + "]", this.fontConsolas8bold, Color.Brown, Color.Beige);
		}
		public override void OnNewQuoteOfStreamingBar_callback(Quote quote) {
			//double slowStreaming = this.MAslow.BarClosesProxied.StreamingValue;
			//double slowStatic = this.MAslow.ClosesProxyEffective.LastStaticValue;
			//DateTime slowStaticDate = this.MAslow.ClosesProxyEffective.LastStaticDate;

			if (this.Executor.Sequencer.IsRunningNow) return;

			if (this.Executor.BacktesterOrLivesimulator.ImRunningChartless_backtestOrSequencing == false) {
				Bar bar = quote.ParentBarStreaming;
				int barNo = bar.ParentBarsIndex;
				if (barNo <= 0) return;
				DateTime lastStaticBarDateTime = bar.ParentBars.BarStaticLast_nullUnsafe.DateTimeOpen;
				DateTime streamingBarDateTime = bar.DateTimeOpen;
				Bar barNormalizedDateTimes = new Bar(bar.Symbol, bar.ScaleInterval, quote.ServerTime);
				DateTime thisBarDateTimeOpen = barNormalizedDateTimes.DateTimeOpen;
				int a = 1;
			}
			//log("OnNewQuoteCallback(): [" + quote.ToString() + "]");
			#if VERBOSE_STRINGS_SLOW
			string msg = "OnNewQuoteCallback(): [" + quote.ToString() + "]";
			log("EnterEveryBar.cs now=[" + DateTime.Now.ToString("ddd dd-MMM-yyyy HH:mm:ss.fff" + "]: " + msg));
			#endif

			if (quote.IntraBarSerno == 0) {
				return;
			}
		}
		public override void OnBarStaticLastFormed_whileStreamingBarWithOneQuoteAlreadyAppended_callback(Bar barStaticFormed) {
			//this.testBarAnnotations(barStaticFormed);
			//Thread.Sleep(500);

			Bar barStreaming = base.Bars.BarStreaming_nullUnsafe;
			if (this.Executor.BacktesterOrLivesimulator.ImRunningChartless_backtestOrSequencing == false) {
				//Debugger.Break();
			}
			if (barStaticFormed.ParentBarsIndex <= 2) return;
			if (barStaticFormed.IsBarStreaming) {
				string msg = "SHOULD_NEVER_HAPPEN triggered@barStaticFormed.IsBarStreaming[" + barStaticFormed + "] while Streaming[" + barStreaming + "]";
				#if DEBUG
				Debugger.Break();
				#endif
			}

			//Position lastPos_fromMaster_canBeAnything = base.LastPosition_fromMaster_nullUnsafe;
			Position lastPos_OpenNow_nullUnsafe = base.LastPosition_OpenNow_nullUnsafe;
			//v1 if (base.IsLastPosition_OpenNow) {
			//v2 if (base.HasPositions_OpenNow) {
			if (lastPos_OpenNow_nullUnsafe != null) {
				if (lastPos_OpenNow_nullUnsafe.EntryFilledBarIndex > barStaticFormed.ParentBarsIndex) {
					string msg1 = "NOTIFIED_ABOUT_LAST_FORMED_WHILE_LAST_POST_FILLED_AT_STREAMING__LOOKS_OK";
					//Debugger.Break();
				}

				if (lastPos_OpenNow_nullUnsafe.ExitAlert != null && lastPos_OpenNow_nullUnsafe.ExitAlert.IsKilled == false) {
					string msg1 = "YES_IM_LIVESIMMING_WITH_BROKER_SPOILING_EXECUTION_FOR_3000ms"
						//+ " you want to avoid POSITION_ALREADY_HAS_AN_EXIT_ALERT_REPLACE_INSTEAD_OF_ADDING_SECOND"
						//+ " ExitAtMarket by throwing [can't have two closing alerts for one positionExit]"
						+ " Strategy[" + this.Strategy.ToString() + "]";
					#if DEBUG
					//Debugger.Break();
					#endif
					return;
				}

				//if (barStaticFormed.ParentBarsIndex == 163) {
				//	#if DEBUG
				//	Debugger.Break();
				//	#endif
				//	StreamingDataSnapshot streaming = this.Executor.DataSource.StreamingAdapter.StreamingDataSnapshot;
				//	Quote quoteLast = streaming.LastQuoteCloneGetForSymbol(barStaticFormed.Symbol);
				//	double priceForMarketOrder = streaming.LastQuoteGetPriceForMarketOrder(barStaticFormed.Symbol);
				//}

				string msg = "ExitAtMarket@" + barStaticFormed.ParentBarsIdent;
				//return;

				//this.Executor.ExecutionDataSnapshot.IsScriptRunningOnBarStaticLastNonBlockingRead = false;
				Alert exitPlaced = base.ExitAtMarket(barStreaming, lastPos_OpenNow_nullUnsafe, msg);
				//this.Executor.ExecutionDataSnapshot.IsScriptRunningOnBarStaticLastNonBlockingRead = true;
				log("Execute(): " + msg);
			}

			if (base.HasAlertsUnfilled) return;

			ExecutorDataSnapshot snap = base.Executor.ExecutionDataSnapshot;

			//if (base.HasAlertsPendingOrBeingReplaced_orPositionsOpenNow) {
			if (base.HasPositions_OpenNow) {
			//if (base.HasAlertsPendingAndPositionsOpenNow) {
				if (snap.AlertsUnfilled.Count > 0) {
					//GOT_OUT_OF_BOUNDADRY_EXCEPTION_ONCE Alert firstPendingAlert = snap.AlertsPending.InnerList[0];
					Alert firstPendingAlert = snap.AlertsUnfilled.Last_nullUnsafe(this, "OnBarStaticLastFormedWhileStreamingBarWithOneQuoteAlreadyAppendedCallback(WAIT)");
					Alert lastPosEntryAlert = lastPos_OpenNow_nullUnsafe != null ? lastPos_OpenNow_nullUnsafe.EntryAlert : null;
					Alert lastPosExitAlert  = lastPos_OpenNow_nullUnsafe != null ? lastPos_OpenNow_nullUnsafe.ExitAlert : null;
					if (firstPendingAlert == lastPosEntryAlert) {
						string msg = "EXPECTED: I don't have open positions but I have an unfilled firstPendingAlert from lastPosition.EntryAlert=alertsPending[0]";
						this.log(msg);
					} else if (firstPendingAlert == lastPosExitAlert) {
						string msg = "EXPECTED: I have and open lastPosition with .ExitAlert=alertsPending[0]";
						this.log(msg);
					} else {
						string msg = "UNEXPECTED: firstPendingAlert alert doesn't relate to lastPosition; who is here?";
						this.log(msg);
					}
				}
				if (snap.Positions_OpenNow.Count > 1) {
					string msg = "EXPECTED: I got multiple positions[" + snap.Positions_OpenNow.Count + "]";
					if (snap.Positions_OpenNow.First_nullUnsafe(this, "OnBarStaticLastFormedWhileStreamingBarWithOneQuoteAlreadyAppendedCallback(WAIT)") == lastPos_OpenNow_nullUnsafe) {
						msg += "50/50: positionsMaster.Last = positionsOpenNow.First";
					}
					this.log(msg);
				}
				return;
			}

			if (barStaticFormed.Close > barStaticFormed.Open) {
				string msg = "BuyAtMarket@" + barStaticFormed.ParentBarsIdent;
				//this.Executor.ExecutionDataSnapshot.IsScriptRunningOnBarStaticLastNonBlockingRead = false;
				Alert buyPlaced = base.BuyAtMarket(barStreaming, msg);
				//this.Executor.ExecutionDataSnapshot.IsScriptRunningOnBarStaticLastNonBlockingRead = true;
				//Debugger.Break();
				this.log(msg);
			} else {
				string msg = "ShortAtMarket@" + barStaticFormed.ParentBarsIdent;
				//this.Executor.ExecutionDataSnapshot.IsScriptRunningOnBarStaticLastNonBlockingRead = false;
				Alert shortPlaced = base.ShortAtMarket(barStreaming, msg);
				//this.Executor.ExecutionDataSnapshot.IsScriptRunningOnBarStaticLastNonBlockingRead = true;
				//Debugger.Break();
				this.log(msg);
			}
		}
		
		public override void OnStreamingTriggeringScript_turnedOn_callback() {
			string msg = "SCRIPT_IS_NOW_AWARE_THAT_STREAMING_ADAPDER_WILL_TRIGGER_SCRIPT_METHODS"
				+ " ScriptContextCurrent.IsStreamingTriggeringScript[" + this.Strategy.ScriptContextCurrent.StreamingIsTriggeringScript+ "]";
			Assembler.PopupException(msg, null, false);
			
			//if (base.HasAlertsPendingOrBeingReplaced_orPositionsOpenNow == false) return;
			if (base.HasPositions_OpenNow == false) return;

			string msg2 = "here you can probably sync your actual open positions on the broker side with backtest-opened ghosts";
			Assembler.PopupException(msg2, null, false);
		}
		public override void OnStreamingTriggeringScript_turnedOff_callback() {
			string msg = "SCRIPT_IS_NOW_AWARE_THAT_STREAMING_ADAPDER_WILL_NOT_TRIGGER_SCRIPT_METHODS"
				+ " ScriptContextCurrent.IsStreamingTriggeringScript[" + this.Strategy.ScriptContextCurrent.StreamingIsTriggeringScript+ "]";
			Assembler.PopupException(msg, null, false);
		}
		
		public override void OnStrategyEmittingOrders_turnedOn_callback() {
			string msg = "SCRIPT_IS_NOW_AWARE_THAT_ORDERS_WILL_START_SHOOTING_THROUGH_BROKER_ADAPDER"
				+ " ScriptContextCurrent.StrategyEmittingOrders[" + this.Strategy.ScriptContextCurrent.StrategyEmittingOrders+ "]";
			Assembler.PopupException(msg, null, false);
		}
		public override void OnStrategyEmittingOrders_turnedOff_callback() {
			string msg = "SCRIPT_IS_NOW_AWARE_THAT_ORDERS_WILL_STOP_SHOOTING_THROUGH_BROKER_ADAPDER"
				+ " ScriptContextCurrent.StrategyEmittingOrders[" + this.Strategy.ScriptContextCurrent.StrategyEmittingOrders+ "]";
			Assembler.PopupException(msg, null, false);
		}

		
		public override void OnAlertFilled_callback(Alert alertFilled) {
			if (alertFilled.FilledBarIndex == 12) {
				//Debugger.Break();
			}
		}
		public override void OnAlertKilled_callback(Alert alertKilled) {
			int ordersNumber_thatTried_toFillAlert = alertKilled.OrderFollowed == null ? 0 : 1;
			ordersNumber_thatTried_toFillAlert += alertKilled.OrdersFollowed_killedAndReplaced.Count;
			string msg = "OnAlertKilled_callback ordersNumber_thatTried_toFillAlert[" + ordersNumber_thatTried_toFillAlert + "]";
			Assembler.PopupException(msg, null, false);
		}
		public override void OnOrderReplaced_callback(Order orderKilled, Order orderReplacement) {
			//int ordersNumber_thatTried_toFillAlert = alertKilled.OrderFollowed == null ? 0 : 1;
			//ordersNumber_thatTried_toFillAlert += alertKilled.OrdersFollowed_killedAndReplaced.Count;
			string msg = "OnOrderReplaced_callback";		// ordersNumber_thatTried_toFillAlert[" + ordersNumber_thatTried_toFillAlert + "]";
			Assembler.PopupException(msg, null, false);
		}
		public override void OnAlertNotSubmitted_callback(Alert alertNotSubmitted, int barNotSubmittedRelno) {
			string msg = "OnAlertNotSubmitted_callback";
			Assembler.PopupException(msg, null, false);
		}
		public override void OnPositionOpened_callback(Position positionOpened) {
			//if (positionOpened.EntryFilledBarIndex == 37) {
			//	#if DEBUG
			//	Debugger.Break();
			//	#endif
			//}
		}
		public override void OnPositionOpened_prototypeSlTpPlaced_callback(Position positionOpenedByPrototype) {
			string msg = "OnPositionOpened_prototypeSlTpPlaced_callback";
			Assembler.PopupException(msg, null, false);
		}
		public override void OnPositionClosed_callback(Position positionClosed) {
			//if (positionClosed.EntryFilledBarIndex == 37) {
			//	Debugger.Break();
			//}
		}
		//void testBarAnnotations(Bar barStaticFormed) {
		//	int barIndex = barStaticFormed.ParentBarsIndex;
		//	string labelText = barStaticFormed.DateTimeOpen.ToString("HH:mm");
		//	labelText += " " + barStaticFormed.BarIndexAfterMidnightReceived + "/";
		//	labelText += barStaticFormed.BarIndexExpectedSinceTodayMarketOpen + ":" + barStaticFormed.BarIndexExpectedMarketClosesTodaySinceMarketOpen;
		//	//bool evenAboveOddBelow = true;
		//	bool evenAboveOddBelow = (barStaticFormed.ParentBarsIndex % 2) == 0;
		//	base.Executor.ChartConditionalBarAnnotationDrawModify(
		//		barIndex, "ann" + barIndex, labelText, this.fontConsolas7, Color.ForestGreen, Color.Empty, evenAboveOddBelow);
		//	// checking labels stacking next upon (underneath) the previous
		//	base.Executor.ChartConditionalBarAnnotationDrawModify(
		//		barIndex, "ann2" + barIndex, labelText, this.fontConsolas7, Color.ForestGreen, Color.LightGray, evenAboveOddBelow);
		//}
	}
}