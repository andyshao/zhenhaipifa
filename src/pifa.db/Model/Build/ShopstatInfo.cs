using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using pifa.BLL;

namespace pifa.Model {

	public partial class ShopstatInfo {
		#region fields
		private uint? _Shop_id;
		private ShopInfo _obj_shop;
		private uint? _Today_fav;
		private uint? _Today_session;
		private uint? _Today_share;
		private uint? _Total_fav;
		private uint? _Total_session;
		private uint? _Total_share;
		#endregion

		public ShopstatInfo() { }

		#region 独创的序列化，反序列化
		protected static readonly string StringifySplit = "@<Shopstat(Info]?#>";
		public string Stringify() {
			return string.Concat(
				_Shop_id == null ? "null" : _Shop_id.ToString(), "|",
				_Today_fav == null ? "null" : _Today_fav.ToString(), "|",
				_Today_session == null ? "null" : _Today_session.ToString(), "|",
				_Today_share == null ? "null" : _Today_share.ToString(), "|",
				_Total_fav == null ? "null" : _Total_fav.ToString(), "|",
				_Total_session == null ? "null" : _Total_session.ToString(), "|",
				_Total_share == null ? "null" : _Total_share.ToString());
		}
		public ShopstatInfo(string stringify) {
			string[] ret = stringify.Split(new char[] { '|' }, 7, StringSplitOptions.None);
			if (ret.Length != 7) throw new Exception("格式不正确，ShopstatInfo：" + stringify);
			if (string.Compare("null", ret[0]) != 0) _Shop_id = uint.Parse(ret[0]);
			if (string.Compare("null", ret[1]) != 0) _Today_fav = uint.Parse(ret[1]);
			if (string.Compare("null", ret[2]) != 0) _Today_session = uint.Parse(ret[2]);
			if (string.Compare("null", ret[3]) != 0) _Today_share = uint.Parse(ret[3]);
			if (string.Compare("null", ret[4]) != 0) _Total_fav = uint.Parse(ret[4]);
			if (string.Compare("null", ret[5]) != 0) _Total_session = uint.Parse(ret[5]);
			if (string.Compare("null", ret[6]) != 0) _Total_share = uint.Parse(ret[6]);
		}
		#endregion

		#region override
		private static Dictionary<string, bool> __jsonIgnore;
		private static object __jsonIgnore_lock = new object();
		public override string ToString() {
			this.Init__jsonIgnore();
			string json = string.Concat(
				__jsonIgnore.ContainsKey("Shop_id") ? string.Empty : string.Format(", Shop_id : {0}", Shop_id == null ? "null" : Shop_id.ToString()), 
				__jsonIgnore.ContainsKey("Today_fav") ? string.Empty : string.Format(", Today_fav : {0}", Today_fav == null ? "null" : Today_fav.ToString()), 
				__jsonIgnore.ContainsKey("Today_session") ? string.Empty : string.Format(", Today_session : {0}", Today_session == null ? "null" : Today_session.ToString()), 
				__jsonIgnore.ContainsKey("Today_share") ? string.Empty : string.Format(", Today_share : {0}", Today_share == null ? "null" : Today_share.ToString()), 
				__jsonIgnore.ContainsKey("Total_fav") ? string.Empty : string.Format(", Total_fav : {0}", Total_fav == null ? "null" : Total_fav.ToString()), 
				__jsonIgnore.ContainsKey("Total_session") ? string.Empty : string.Format(", Total_session : {0}", Total_session == null ? "null" : Total_session.ToString()), 
				__jsonIgnore.ContainsKey("Total_share") ? string.Empty : string.Format(", Total_share : {0}", Total_share == null ? "null" : Total_share.ToString()), " }");
			return string.Concat("{", json.Substring(1));
		}
		public IDictionary ToBson() {
			this.Init__jsonIgnore();
			IDictionary ht = new Hashtable();
			if (!__jsonIgnore.ContainsKey("Shop_id")) ht["Shop_id"] = Shop_id;
			if (!__jsonIgnore.ContainsKey("Today_fav")) ht["Today_fav"] = Today_fav;
			if (!__jsonIgnore.ContainsKey("Today_session")) ht["Today_session"] = Today_session;
			if (!__jsonIgnore.ContainsKey("Today_share")) ht["Today_share"] = Today_share;
			if (!__jsonIgnore.ContainsKey("Total_fav")) ht["Total_fav"] = Total_fav;
			if (!__jsonIgnore.ContainsKey("Total_session")) ht["Total_session"] = Total_session;
			if (!__jsonIgnore.ContainsKey("Total_share")) ht["Total_share"] = Total_share;
			return ht;
		}
		private void Init__jsonIgnore() {
			if (__jsonIgnore == null) {
				lock (__jsonIgnore_lock) {
					if (__jsonIgnore == null) {
						FieldInfo field = typeof(ShopstatInfo).GetField("JsonIgnore");
						__jsonIgnore = new Dictionary<string, bool>();
						if (field != null) {
							string[] fs = string.Concat(field.GetValue(null)).Split(',');
							foreach (string f in fs) if (!string.IsNullOrEmpty(f)) __jsonIgnore[f] = true;
						}
					}
				}
			}
		}
		public override bool Equals(object obj) {
			ShopstatInfo item = obj as ShopstatInfo;
			if (item == null) return false;
			return this.ToString().Equals(item.ToString());
		}
		public override int GetHashCode() {
			return this.ToString().GetHashCode();
		}
		public static bool operator ==(ShopstatInfo op1, ShopstatInfo op2) {
			if (object.Equals(op1, null)) return object.Equals(op2, null);
			return op1.Equals(op2);
		}
		public static bool operator !=(ShopstatInfo op1, ShopstatInfo op2) {
			return !(op1 == op2);
		}
		public object this[string key] {
			get { return this.GetType().GetProperty(key).GetValue(this); }
			set { this.GetType().GetProperty(key).SetValue(this, value); }
		}
		#endregion

		#region properties
		public uint? Shop_id {
			get { return _Shop_id; }
			set {
				if (_Shop_id != value) _obj_shop = null;
				_Shop_id = value;
			}
		}
		public ShopInfo Obj_shop {
			get {
				if (_obj_shop == null) _obj_shop = Shop.GetItem(_Shop_id);
				return _obj_shop;
			}
			internal set { _obj_shop = value; }
		}
		/// <summary>
		/// 今日收藏
		/// </summary>
		public uint? Today_fav {
			get { return _Today_fav; }
			set { _Today_fav = value; }
		}
		/// <summary>
		/// 今日访客
		/// </summary>
		public uint? Today_session {
			get { return _Today_session; }
			set { _Today_session = value; }
		}
		/// <summary>
		/// 今日分享
		/// </summary>
		public uint? Today_share {
			get { return _Today_share; }
			set { _Today_share = value; }
		}
		/// <summary>
		/// 总收藏
		/// </summary>
		public uint? Total_fav {
			get { return _Total_fav; }
			set { _Total_fav = value; }
		}
		/// <summary>
		/// 总访客
		/// </summary>
		public uint? Total_session {
			get { return _Total_session; }
			set { _Total_session = value; }
		}
		/// <summary>
		/// 总分享
		/// </summary>
		public uint? Total_share {
			get { return _Total_share; }
			set { _Total_share = value; }
		}
		#endregion

		public pifa.DAL.Shopstat.SqlUpdateBuild UpdateDiy {
			get { return Shopstat.UpdateDiy(this, _Shop_id); }
		}
	}
}

