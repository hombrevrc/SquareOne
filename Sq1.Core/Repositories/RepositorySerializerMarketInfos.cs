using System;
using System.Collections.Generic;
using System.IO;

using CsvHelper;
using CsvHelper.Configuration;

using Sq1.Core.DataTypes;
using Sq1.Core.Serializers;

namespace Sq1.Core.Repositories {
	public class RepositorySerializerMarketInfos : Serializer<Dictionary<string, MarketInfo>> {
		public	static string				MarketDefaultName = "FORTS";
		const	string						fnameTimeZonesCsv = "MicrosoftTimeZones.csv";
		public	string						AbsFilenameTimeZonesCsv	{ get { return Path.Combine(base.RootPath, fnameTimeZonesCsv); } }
		public	Dictionary<string, string>	TimeZonesWithUTC		{ get; protected set; }

		public	Dictionary<string, MarketInfo>	MarketsByName { get { return base.EntityDeserialized; } }

		public RepositorySerializerMarketInfos() : base() {}
		public new bool Initialize(string rootPath, string relFname,
					string subfolder = "Workspaces", string workspaceName = "Default",
					bool createNonExistingPath = true, bool createNonExistingFile = true) {
			bool createdNewFile = base.Initialize(rootPath, relFname, subfolder, workspaceName, createNonExistingPath, createNonExistingFile);
			this.TimeZonesWithUTC = this.parseCsv_MicrosoftTimeZones(this.AbsFilenameTimeZonesCsv, ",");
			return createdNewFile;
		}
		public override int Serialize() {
			base.Serialize();
			string errorsFixed = this.FixPotentialErrors(true);
			return base.EntityDeserialized.Count;
		}
		public override Dictionary<string, MarketInfo> Deserialize() {
			base.Deserialize();
			if (base.EntityDeserialized.Count == 0) base.EntityDeserialized = this.MarketsDefault;
			string errorsFixed = this.FixPotentialErrors();
			return base.EntityDeserialized;
		}
		public string FixPotentialErrors(bool popup = false) {
			string ret = "";
			foreach (MarketInfo marketInfo in base.EntityDeserialized.Values) {
				marketInfo.FixPotentialErrors(popup);
			}
			return ret;
		}


		public MarketInfo FindMarketInfo(string marketName) {
			if (string.IsNullOrEmpty(marketName)) return null;
			foreach (string market in EntityDeserialized.Keys) {
				if (market.ToUpper() == marketName.ToUpper()) return EntityDeserialized[market];
			}
			return null;
		}
		public MarketInfo FindMarketInfoOrNew(string marketName) {
			MarketInfo ret = this.FindMarketInfo(marketName);
			if (ret == null) ret = new MarketInfo();
			return ret;
		}	
		public Dictionary<string, MarketInfo> MarketsDefault {
			get {
				Dictionary<string, MarketInfo> ret = new Dictionary<string, MarketInfo>();
				MarketInfo mockMarket = new MarketInfo() {
					Name = "MOCK",
					Description = "23:59 hours open",
					MarketOpen_serverTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0),// 10, 30, 0);
					MarketClose_serverTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59)
					,TimeZoneName = "Russian Standard Time"
					//,TimeZoneInfo = TimeZoneInfo.CreateCustomTimeZone("MSK_CUSTOM", new TimeSpan(4, 0, 0),
					//	"Moscow Daylight (UTC+04:00)", "Moscow Standard non-auto-adjusting-for-winter")
				};
				mockMarket.ClearingTimespans.Add(new MarketClearingTimespan() {
					SuspendServerTimeOfDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 14, 00, 0),
					ResumeServerTimeOfDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 14, 03, 0)
				});
				ret.Add(mockMarket.Name, mockMarket);
				MarketInfo russianMarket = new MarketInfo() {
					Name = MarketDefaultName,
					Description = "RTS Index Futures and Options Market (RIM3)",
					MarketOpen_serverTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 10, 0, 0),
					MarketClose_serverTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 50, 0)
					,TimeZoneName = "Russian Standard Time"
					//,TimeZoneInfo = TimeZoneInfo.CreateCustomTimeZone("MSK_CUSTOM", new TimeSpan(4, 0, 0),
					//	"Moscow Daylight (UTC+04:00)", "Moscow Standard non-auto-adjusting-for-winter")
				};
				// 10.00 - 14.00	������ �������� �������� ������
				// 14.00 - 14.03	������������� ����������� ����� (������� �������)
				// 14.03 - 18.45	��������� �������� �������� ������
				// 18.45 - 19.00	�������� ����������� �����
				// 19.00 - 23.50	�������� �������� ������
				russianMarket.ClearingTimespans.Add(new MarketClearingTimespan() {
					SuspendServerTimeOfDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 14, 00, 0),
					ResumeServerTimeOfDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 14, 03, 0)
				});
				russianMarket.ClearingTimespans.Add(new MarketClearingTimespan() {
					SuspendServerTimeOfDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 18, 45, 0),
					ResumeServerTimeOfDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 19, 00, 0)
				});
				ret.Add(russianMarket.Name, russianMarket);
				MarketInfo usMarket = new MarketInfo() {
					Name = "US",
					Description = "NYSE, Nasdaq and Amex equities",
					MarketOpen_serverTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 9, 30, 0),
					MarketClose_serverTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 16, 0, 0),
					TimeZoneName = "Eastern Standard Time"
				};
				for (int i = 2007; i < 2020; i++) {
					usMarket.HolidaysYMD000.Add(new DateTime(i, 7, 4));
					usMarket.HolidaysYMD000.Add(new DateTime(i, 12, 25));
					for (int j = 1 ; j < 30 ; j++) {
						DateTime item = new DateTime(i, 9, j);
						if (item.DayOfWeek == DayOfWeek.Monday) {
							usMarket.HolidaysYMD000.Add(item);
							break;
						}
					}
				}
				ret.Add(usMarket.Name, usMarket);
				return ret;
			}
		}
		public MarketInfo MarketDefault {
			get {
				if (base.EntityDeserialized.ContainsKey(RepositorySerializerMarketInfos.MarketDefaultName) == false) {
					throw new Exception("please add Market.Name=[" + RepositorySerializerMarketInfos.MarketDefaultName + "]"
						+ " into your [" + Path.Combine(base.RootPath, base.FnameRelpath) + "]"
						+ " for MarketInfoRepository.MarketDefault");
				}
				return EntityDeserialized[RepositorySerializerMarketInfos.MarketDefaultName];
			}
		}
		public static List<DayOfWeek> ParseDaysOfWeekCsv(string csv, string separator) {
			List<DayOfWeek> ret = null;
			if (string.IsNullOrEmpty(csv)) return ret;
			if (string.IsNullOrEmpty(separator)) return ret;
			string[] csvSplitted = csv.Split(separator.ToCharArray());
			if (csvSplitted.Length == 0) return ret;
			foreach (string token in csvSplitted) {
				bool dowParsedOk = false;
				DayOfWeek parsed = DayOfWeek.Sunday;
				foreach (DayOfWeek everyDow in Enum.GetValues(typeof(DayOfWeek))) {
					if (everyDow.ToString().ToUpper().IndexOf(token.ToUpper()) >= 0) {
						parsed = everyDow;
						dowParsedOk = true;
						break;
					}
				}
				if (dowParsedOk == false) throw new Exception("[" + token + "] doesn't look like a DayOfWeek");
				if (ret == null) ret = new List<DayOfWeek>();
				if (ret.Contains(parsed)) throw new Exception("[" + token + "] parsed[" + parsed + "] is mentioned twice");
				ret.Add(parsed);
			}
			return ret;
		}
		public Dictionary<string, string> parseCsv_MicrosoftTimeZones(string filename, string delimeters, int skipFirstLines = 0) {
			string msig = " //parseCsv_MicrosoftTimeZones(" + filename + ")";
			Dictionary<string, string> ret = new Dictionary<string, string>();

			// #http://msdn.microsoft.com/en-us/library/ms912391(v=winembedded.11).aspx
			// #Index,Name of Time Zone,Time,
			// 0,Dateline Standard Time,(GMT-12:00) International Date Line West
			// 1,Samoa Standard Time,"(GMT-11:00) Midway Island, Samoa"
			
			StreamReader textReader = null;
			try {
				textReader = new StreamReader(filename);
				CsvConfiguration cfg = new CsvConfiguration();
				cfg.Delimiter = delimeters;
				cfg.AllowComments = true;
				var csv = new CsvReader(textReader, cfg);
				while (csv.Read()) {
					string[] fields = csv.CurrentRecord;
					if (fields.Length < 3) continue;
					//cfg.AllowComments = true if (fields[0].TrimStart(" ".ToCharArray()).StartsWith("#")) continue;

					string timeZoneWindowsName = fields[1].Trim();		// Central Standard Time
					string timeZoneGmtCity = fields[2].Trim();			// (GMT-06:00) Central Time (US and Canada)

					if (ret.ContainsKey(timeZoneGmtCity)) continue;
					ret.Add(timeZoneGmtCity, timeZoneWindowsName);
				}
			} catch (Exception ex) {
				string msg = "FILE_PERMISSION__OR_CsvHelper_ERROR";
				Assembler.PopupException(msg + msig, ex, false);
			} finally {
				if (textReader != null) textReader.Dispose();
			}

			return ret;
		}
		public void RenameMarketInfoRearrangeDictionary(MarketInfo marketInfo) {
			if (base.EntityDeserialized.ContainsKey(marketInfo.Name)) {
				throw new Exception("the Market[" + marketInfo.Name + "] is already defined, choose another name");
			}
			string oldMarketName = null;
			foreach (string marketName in EntityDeserialized.Keys) {
				if (base.EntityDeserialized[marketName] == marketInfo) {
					oldMarketName = marketName;
					break;
				}
			}
			if (string.IsNullOrEmpty(oldMarketName)) {
				throw new Exception("CRAZY#86 the Market[" + marketInfo.Name + "] wasn't found in Markets");
			}
			base.EntityDeserialized.Remove(oldMarketName);
			base.EntityDeserialized.Add(marketInfo.Name, marketInfo);
		}
	}
}