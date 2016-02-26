﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using BrightIdeasSoftware;

using Sq1.Core;
using Sq1.Core.Execution;
using Sq1.Core.Support;

namespace Sq1.Widgets.Execution {
	public partial class ExecutionTreeControl {
		void oLVColumn_VisibilityChanged(object sender, EventArgs e) {
			OLVColumn oLVColumn = sender as OLVColumn;
			if (oLVColumn == null) return;
			
				//v1 prior to using this.OrdersTreeOLV.SaveState();
//			if (this.DataSnapshot.ColumnsShown.ContainsKey(oLVColumn.Text) == false) {
//				this.DataSnapshot.ColumnsShown.Add(oLVColumn.Text, oLVColumn.IsVisible);
//			} else {
//				this.DataSnapshot.ColumnsShown[oLVColumn.Text] = oLVColumn.IsVisible;
//			}
			byte[] olvStateBinary = this.OrdersTreeOLV.SaveState();
			this.DataSnapshot.OrdersTreeOlvStateBase64 = ObjectListViewStateSerializer.Base64Encode(olvStateBinary);
			if (Assembler.InstanceInitialized.MainFormDockFormsFullyDeserializedLayoutComplete == false) return;
			this.DataSnapshotSerializer.Serialize();
		}

		void orderTreeListViewCustomize() {
			//v2
			// adds columns to filter in the header (right click - unselect garbage columns); there might be some BrightIdeasSoftware.SyncColumnsToAllColumns()?...
			List<OLVColumn> allColumns = new List<OLVColumn>();
			foreach (ColumnHeader columnHeader in this.OrdersTreeOLV.Columns) {
				OLVColumn oLVColumn = columnHeader as OLVColumn; 
				oLVColumn.VisibilityChanged += oLVColumn_VisibilityChanged;
				if (oLVColumn == null) continue;
				//THROWS_ADDING_ALL_REGARDLESS_AFTER_OrdersTreeOLV.RestoreState(base64Decoded)_ADDED_FILTER_IN_OUTER_LOOP 
				if (this.OrdersTreeOLV.AllColumns.Contains(oLVColumn)) continue;
				allColumns.Add(oLVColumn);
			}
			if (allColumns.Count > 0) {
				//THROWS_ADDING_ALL_REGARDLESS_AFTER_OrdersTreeOLV.RestoreState(base64Decoded)_ADDED_FILTER_IN_OUTER_LOOP 
				this.OrdersTreeOLV.AllColumns.AddRange(allColumns);
			}

			//	http://stackoverflow.com/questions/9802724/how-to-create-a-multicolumn-treeview-like-this-in-c-sharp-winforms-app/9802753#9802753
			this.OrdersTreeOLV.CanExpandGetter = delegate(object o) {
				var order = o as Order;
				if (order == null) {
					Assembler.PopupException("treeListView.CanExpandGetter: order=null");
					return false;
				}
				return order.DerivedOrders.Count > 0;
			};
			this.OrdersTreeOLV.ChildrenGetter = delegate(object o) {
				var order = o as Order;
				if (order == null) {
					Assembler.PopupException("treeListView.ChildrenGetter: order=null");
					return null;
				}
				return order.DerivedOrders;
			};

			this.olvcAccount.AspectGetter = delegate(object o) {
				var order = o as Order;
				if (order == null) return "olvcAccount.AspectGetter: order=null";
				return order.Alert.AccountNumber;
			};
			this.olvcBarNum.AspectGetter = delegate(object o) {
				var order = o as Order;
				if (order == null) return "olvcBarNum.AspectGetter: order=null";
				return order.Alert.PlacedBarIndex.ToString();
			};
			this.olvcOrderCreated.AspectGetter = delegate(object o) {
				var order = o as Order;
				if (order == null) return "olvcDatetime.AspectGetter: order=null";
				DateTime orderCreated;
				if (this.mniToggleBrokerTime.Checked) {
					orderCreated = (order.Alert.QuoteCreatedThisAlertServerTime != DateTime.MinValue)
						? order.Alert.QuoteCreatedThisAlertServerTime : order.CreatedBrokerTime;
				} else {
					if (order.Alert.Bars != null) {
						orderCreated = order.Alert.Bars.MarketInfo.ConvertServerTimeToLocal(order.CreatedBrokerTime);
					} else {
						orderCreated = order.CreatedBrokerTime;
					}
				}
				return orderCreated.ToString(Assembler.DateTimeFormatLong);
			};
			this.olvcSymbol.AspectGetter = delegate(object o) {
				var order = o as Order;
				if (order == null) return "olvcSymbol.AspectGetter: order=null";
				return order.Alert.Symbol;
			};
			this.olvcDirection.AspectGetter = delegate(object o) {
				var order = o as Order;
				if (order == null) return "olvcDirection.AspectGetter: order=null";
				return order.IsKiller ? "KILLER" : order.Alert.Direction.ToString();
			};
			this.olvcDirection.ImageGetter = delegate(object o) {
				var order = o as Order;
				if (order == null) return "olvcDirection.ImageGetter: order=null";
				return (int)order.Alert.Direction - 1;
			};
			this.olvcOrderType.AspectGetter = delegate(object o) {
				var order = o as Order;
				if (order == null) return "olvcOrderType.AspectGetter: order=null";
				return order.IsKiller ? "" : order.Alert.MarketLimitStop.ToString();
			};
			this.olvcSpreadSide.AspectGetter = delegate(object o) {
				var order = o as Order;
				if (order == null) return "olvcSpreadSide.AspectGetter: order=null";
				return order.IsKiller ? "" : formatOrderPriceSpreadSide(order, this.DataSnapshot.PricingDecimalForSymbol);
			};
			this.olvcPriceScript.AspectGetter = delegate(object o) {
				var order = o as Order;
				if (order == null) return "olvcPriceScript.AspectGetter: order=null";
				return order.IsKiller ? "" : order.Alert.PriceScript.ToString("N" + this.DataSnapshot.PricingDecimalForSymbol);
			};
			this.olvcSlippage.AspectGetter = delegate(object o) {
				var order = o as Order;
				if (order == null) return "olvcSlippage.AspectGetter: order=null";
				return order.IsKiller ? "" : order.SlippageFill.ToString();
			};
			this.olvcPriceScriptRequested.AspectGetter = delegate(object o) {
				var order = o as Order;
				if (order == null) return "olvcPriceScriptRequested.AspectGetter: order=null";
				return order.IsKiller ? "" : order.PriceRequested.ToString("N" + this.DataSnapshot.PricingDecimalForSymbol);
			};
			this.olvcPriceFilled.AspectGetter = delegate(object o) {
				var order = o as Order;
				if (order == null) return "olvcPriceFilled.AspectGetter: order=null";
				return order.IsKiller ? "" : order.PriceFill.ToString("N" + this.DataSnapshot.PricingDecimalForSymbol);
			};
			this.olvcStateTime.AspectGetter = delegate(object o) {
				var order = o as Order;
				if (order == null) return "olvcStateTime.AspectGetter: order=null";
				return order.StateUpdateLastTimeLocal.ToString(Assembler.DateTimeFormatLong);
			};
			this.olvcState.AspectGetter = delegate(object o) {
				var order = o as Order;
				if (order == null) return "olvcState.AspectGetter: order=null";
				//return (order.InStateExpectingCallbackFromBroker ? "* " : "") + order.State.ToString();
				return order.State.ToString();
			};
//			this.olvcState.FontGetter = delegate(object o) {
//				var order = o as Order;
//				if (order == null) {
//					Assembler.PopupException("olvcState.FontGetter: order=null");
//					return null;
//				}
//				return (order.ExpectingCallbackFromBroker) ? this.fontBold : this.fontNormal;
//			};
			
			this.olvcPriceDeposited.AspectGetter = delegate(object o) {
				var order = o as Order;
				if (order == null) return "olvcPriceDeposited.AspectGetter: order=null";
				return (order.QtyFill == 0) ? "0" : order.Alert.PriceDeposited.ToString("N" + this.DataSnapshot.PricingDecimalForSymbol);
			};
			this.olvcQtyRequested.AspectGetter = delegate(object o) {
				var order = o as Order;
				if (order == null) return "olvcQtyRequested.AspectGetter: order=null";
				return order.IsKiller ? "" : order.QtyRequested.ToString();
			};
			this.olvcQtyFilled.AspectGetter = delegate(object o) {
				var order = o as Order;
				if (order == null) return "olvcQtyFilled.AspectGetter: order=null";
				return order.IsKiller ? "" : order.QtyFill.ToString();
			};
			this.olvcSernoSession.AspectGetter = delegate(object o) {
				var order = o as Order;
				if (order == null) return "olvcSernoSession.AspectGetter: order=null";
				return order.SernoSession.ToString();
			};
			this.olvcSernoExchange.AspectGetter = delegate(object o) {
				var order = o as Order;
				if (order == null) return "olvcSernoExchange.AspectGetter: order=null";
				return order.SernoExchange.ToString();
			};
			this.olvcGUID.AspectGetter = delegate(object o) {
				var order = o as Order;
				if (order == null) return "olvcGUID.AspectGetter: order=null";
				return order.GUID;
			};
			this.olvcKilledByGUID.AspectGetter = delegate(object o) {
				var order = o as Order;
				if (order == null) return "olvcKilledByGUID.AspectGetter: order=null";
				return order.KillerGUID;
			};
			this.olvcReplacedByGUID.AspectGetter = delegate(object o) {
				var order = o as Order;
				if (order == null) return "olvcReplacedByGUID.AspectGetter: order=null";
				return order.ReplacedByGUID;
			};
			this.olvcBrokerName.AspectGetter = delegate(object o) {
				var order = o as Order;
				if (order == null) return "olvcBrokerName.AspectGetter: order=null";
				return order.BrokerName;
			};
			this.olvcStrategyName.AspectGetter = delegate(object o) {
				var order = o as Order;
				if (order == null) return "olvcStrategyName.AspectGetter: order=null";
				return order.Alert.StrategyName;
			};
			this.olvcSignalName.AspectGetter = delegate(object o) {
				var order = o as Order;
				if (order == null) return "olvcSignalName.AspectGetter: order=null";
				return order.Alert.SignalName;
			};
			this.olvcScale.AspectGetter = delegate(object o) {
				var order = o as Order;
				if (order == null) return "olvcScale.AspectGetter: order=null";
				return (order.Alert.BarsScaleInterval == null)
							? "Alert.BarsScaleInterval == null"
							: order.Alert.BarsScaleInterval.ToString();
			};
			this.olvcLastMessage.AspectGetter = delegate(object o) {
				var order = o as Order;
				if (order == null) return "olvcLastMessage.AspectGetter: order=null";
				return order.LastMessage;
			};
		}
		string formatOrderPriceSpreadSide(Order order, int pricingDecimalForSymbol) {
			string ret = "";
			switch (order.SpreadSide) {
				case OrderSpreadSide.AskCrossed:
				case OrderSpreadSide.AskTidal:
					ret = order.CurrentAsk.ToString("N" + pricingDecimalForSymbol) + " " + order.SpreadSide;
					break;
				case OrderSpreadSide.BidCrossed:
				case OrderSpreadSide.BidTidal:
					ret = order.CurrentBid.ToString("N" + pricingDecimalForSymbol) + " " + order.SpreadSide;
					break;
				default:
					ret = order.SpreadSide + " bid[" + order.CurrentBid + "] ask[" + order.CurrentAsk + "]";
					break;
			}
			return ret;
		}
		void messagesListViewCustomize() {
			// adds columns to filter in the header (right click - unselect garbage columns); there might be some BrightIdeasSoftware.SyncColumnsToAllColumns()?...
			List<OLVColumn> allColumns = new List<OLVColumn>();
			foreach (ColumnHeader columnHeader in this.olvMessages.Columns) {
				OLVColumn oLVColumn = columnHeader as OLVColumn; 
				if (oLVColumn == null) continue;
				allColumns.Add(oLVColumn);
			}
			if (allColumns.Count > 0) {
				this.olvMessages.AllColumns.AddRange(allColumns);
			}

			this.olvcMessageText.AspectGetter = delegate(object o) {
				var omsg = o as OrderStateMessage;
				if (omsg == null) return "olvcMessageText.AspectGetter: omsg=null";
				return omsg.Message;
			};
			this.olvcMessageState.AspectGetter = delegate(object o) {
				var omsg = o as OrderStateMessage;
				if (omsg == null) return "olvcMessageState.AspectGetter: omsg=null";
				return omsg.State.ToString();
			};
			this.olvcMessageDateTime.AspectGetter = delegate(object o) {
				var omsg = o as OrderStateMessage;
				if (omsg == null) return "olvcMessageDateTime.AspectGetter: omsg=null";
				return omsg.DateTime.ToString(Assembler.DateTimeFormatLong);
			};
		}

		FontCache fontCache;

		void tree_FormatRow(object sender, BrightIdeasSoftware.FormatRowEventArgs e) {
			Order order = e.Model as Order;
			if (order == null) return;
			if (order.InStateExpectingCallbackFromBroker) {
				//v1 e.Item.Font = new Font(e.Item.Font, FontStyle.Bold);
				e.Item.Font = this.fontCache.Bolden();
			}

			//v1 if (Assembler.InstanceInitialized.AlertsForChart.IsItemRegisteredForAnyContainer(order.Alert)) return;
			//v2 ORDERS_RESTORED_AFTER_APP_RESTART_HAVE_ALERT.STRATEGY=NULL,BARS=NULL
			if (order.Alert.Bars == null) e.Item.ForeColor = Color.DimGray;
			// replaced with new column if (order.Alert.MyBrokerIsLivesim) e.Item.BackColor = Color.Gainsboro;
		}
		// WRONG WAY TO MOVE COLUMNS AROUND: AFTER I did RestoreState(), Column(3) is not State and I add State twice => exception
//		public void MoveStateColumnToLeftmost() {
//			//moving State as we drag-n-dropped it; tree will grow in second column
//			//NOT_NEEDED this.OrdersTree.BuildList();
//			//NOT_NEEDED this.RebuildAllTreeFocusOnTopmost();
//			this.OrdersTreeOLV.SetObjects(this.ordersTree.InnerOrderList);
//			//NOT_NEEDED this.OrdersTree.RebuildAll(true);
//			this.OrdersTreeOLV.Columns.RemoveAt(3);
//			this.OrdersTreeOLV.Columns.Insert(0, this.olvcState);
//			//NOT_NEEDED this.OrdersTree.BuildList();
//			//NOT_NEEDED this.RebuildAllTreeFocusOnTopmost();
//		}
	}
}
