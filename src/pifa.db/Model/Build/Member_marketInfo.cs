﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using pifa.BLL;

namespace pifa.Model {

	[JsonObject(MemberSerialization.OptIn)]
	public partial class Member_marketInfo {
		#region fields
		private uint? _Market_id;
		private MarketInfo _obj_market;
		private uint? _Member_id;
		private MemberInfo _obj_member;
		private DateTime? _Create_time;
		#endregion

		public Member_marketInfo() { }

		#region 序列化，反序列化
		protected static readonly string StringifySplit = "@<Member_market(Info]?#>";
		public string Stringify() {
			return string.Concat(
				_Market_id == null ? "null" : _Market_id.ToString(), "|",
				_Member_id == null ? "null" : _Member_id.ToString(), "|",
				_Create_time == null ? "null" : _Create_time.Value.Ticks.ToString());
		}
		public static Member_marketInfo Parse(string stringify) {
			string[] ret = stringify.Split(new char[] { '|' }, 3, StringSplitOptions.None);
			if (ret.Length != 3) throw new Exception("格式不正确，Member_marketInfo：" + stringify);
			Member_marketInfo item = new Member_marketInfo();
			if (string.Compare("null", ret[0]) != 0) item.Market_id = uint.Parse(ret[0]);
			if (string.Compare("null", ret[1]) != 0) item.Member_id = uint.Parse(ret[1]);
			if (string.Compare("null", ret[2]) != 0) item.Create_time = new DateTime(long.Parse(ret[2]));
			return item;
		}
		#endregion

		#region override
		private static Lazy<Dictionary<string, bool>> __jsonIgnoreLazy = new Lazy<Dictionary<string, bool>>(() => {
			FieldInfo field = typeof(Member_marketInfo).GetField("JsonIgnore");
			Dictionary<string, bool> ret = new Dictionary<string, bool>();
			if (field != null) string.Concat(field.GetValue(null)).Split(',').ToList().ForEach(f => {
				if (!string.IsNullOrEmpty(f)) ret[f] = true;
			});
			return ret;
		});
		private static Dictionary<string, bool> __jsonIgnore => __jsonIgnoreLazy.Value;
		public override string ToString() {
			string json = string.Concat(
				__jsonIgnore.ContainsKey("Market_id") ? string.Empty : string.Format(", Market_id : {0}", Market_id == null ? "null" : Market_id.ToString()), 
				__jsonIgnore.ContainsKey("Member_id") ? string.Empty : string.Format(", Member_id : {0}", Member_id == null ? "null" : Member_id.ToString()), 
				__jsonIgnore.ContainsKey("Create_time") ? string.Empty : string.Format(", Create_time : {0}", Create_time == null ? "null" : Create_time.Value.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds.ToString()), " }");
			return string.Concat("{", json.Substring(1));
		}
		public IDictionary ToBson(bool allField = false) {
			IDictionary ht = new Hashtable();
			if (allField || !__jsonIgnore.ContainsKey("Market_id")) ht["Market_id"] = Market_id;
			if (allField || !__jsonIgnore.ContainsKey("Member_id")) ht["Member_id"] = Member_id;
			if (allField || !__jsonIgnore.ContainsKey("Create_time")) ht["Create_time"] = Create_time;
			return ht;
		}
		public object this[string key] {
			get { return this.GetType().GetProperty(key).GetValue(this); }
			set { this.GetType().GetProperty(key).SetValue(this, value); }
		}
		#endregion

		#region properties
		[JsonProperty] public uint? Market_id {
			get { return _Market_id; }
			set {
				if (_Market_id != value) _obj_market = null;
				_Market_id = value;
			}
		}
		public MarketInfo Obj_market {
			get {
				if (_obj_market == null) _obj_market = Market.GetItem(_Market_id.Value);
				return _obj_market;
			}
			internal set { _obj_market = value; }
		}
		[JsonProperty] public uint? Member_id {
			get { return _Member_id; }
			set {
				if (_Member_id != value) _obj_member = null;
				_Member_id = value;
			}
		}
		public MemberInfo Obj_member {
			get {
				if (_obj_member == null) _obj_member = Member.GetItem(_Member_id.Value);
				return _obj_member;
			}
			internal set { _obj_member = value; }
		}
		/// <summary>
		/// 创建时间
		/// </summary>
		[JsonProperty] public DateTime? Create_time {
			get { return _Create_time; }
			set { _Create_time = value; }
		}
		#endregion

		public pifa.DAL.Member_market.SqlUpdateBuild UpdateDiy {
			get { return Member_market.UpdateDiy(this, _Market_id.Value, _Member_id.Value); }
		}
		public Member_marketInfo Save() {
			if (this.Market_id != null && this.Member_id != null) {
				Member_market.Update(this);
				return this;
			}
			this.Create_time = DateTime.Now;
			return Member_market.Insert(this);
		}
	}
}

