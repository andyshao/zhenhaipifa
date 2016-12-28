using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using pifa.BLL;

namespace pifa.Model {

	public partial class Shop_friendly_linksInfo {
		#region fields
		private uint? _Id;
		private uint? _Shop_id;
		private ShopInfo _obj_shop;
		private DateTime? _Create_time;
		private byte? _Sort;
		private string _Title;
		private string _Url;
		#endregion

		public Shop_friendly_linksInfo() { }

		#region 独创的序列化，反序列化
		protected static readonly string StringifySplit = "@<Shop_friendly_links(Info]?#>";
		public string Stringify() {
			return string.Concat(
				_Id == null ? "null" : _Id.ToString(), "|",
				_Shop_id == null ? "null" : _Shop_id.ToString(), "|",
				_Create_time == null ? "null" : _Create_time.Value.Ticks.ToString(), "|",
				_Sort == null ? "null" : _Sort.ToString(), "|",
				_Title == null ? "null" : _Title.Replace("|", StringifySplit), "|",
				_Url == null ? "null" : _Url.Replace("|", StringifySplit));
		}
		public Shop_friendly_linksInfo(string stringify) {
			string[] ret = stringify.Split(new char[] { '|' }, 6, StringSplitOptions.None);
			if (ret.Length != 6) throw new Exception("格式不正确，Shop_friendly_linksInfo：" + stringify);
			if (string.Compare("null", ret[0]) != 0) _Id = uint.Parse(ret[0]);
			if (string.Compare("null", ret[1]) != 0) _Shop_id = uint.Parse(ret[1]);
			if (string.Compare("null", ret[2]) != 0) _Create_time = new DateTime(long.Parse(ret[2]));
			if (string.Compare("null", ret[3]) != 0) _Sort = byte.Parse(ret[3]);
			if (string.Compare("null", ret[4]) != 0) _Title = ret[4].Replace(StringifySplit, "|");
			if (string.Compare("null", ret[5]) != 0) _Url = ret[5].Replace(StringifySplit, "|");
		}
		#endregion

		#region override
		private static Dictionary<string, bool> __jsonIgnore;
		private static object __jsonIgnore_lock = new object();
		public override string ToString() {
			this.Init__jsonIgnore();
			string json = string.Concat(
				__jsonIgnore.ContainsKey("Id") ? string.Empty : string.Format(", Id : {0}", Id == null ? "null" : Id.ToString()), 
				__jsonIgnore.ContainsKey("Shop_id") ? string.Empty : string.Format(", Shop_id : {0}", Shop_id == null ? "null" : Shop_id.ToString()), 
				__jsonIgnore.ContainsKey("Create_time") ? string.Empty : string.Format(", Create_time : {0}", Create_time == null ? "null" : Create_time.Value.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds.ToString()), 
				__jsonIgnore.ContainsKey("Sort") ? string.Empty : string.Format(", Sort : {0}", Sort == null ? "null" : Sort.ToString()), 
				__jsonIgnore.ContainsKey("Title") ? string.Empty : string.Format(", Title : {0}", Title == null ? "null" : string.Format("'{0}'", Title.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), 
				__jsonIgnore.ContainsKey("Url") ? string.Empty : string.Format(", Url : {0}", Url == null ? "null" : string.Format("'{0}'", Url.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), " }");
			return string.Concat("{", json.Substring(1));
		}
		public IDictionary ToBson() {
			this.Init__jsonIgnore();
			IDictionary ht = new Hashtable();
			if (!__jsonIgnore.ContainsKey("Id")) ht["Id"] = Id;
			if (!__jsonIgnore.ContainsKey("Shop_id")) ht["Shop_id"] = Shop_id;
			if (!__jsonIgnore.ContainsKey("Create_time")) ht["Create_time"] = Create_time;
			if (!__jsonIgnore.ContainsKey("Sort")) ht["Sort"] = Sort;
			if (!__jsonIgnore.ContainsKey("Title")) ht["Title"] = Title;
			if (!__jsonIgnore.ContainsKey("Url")) ht["Url"] = Url;
			return ht;
		}
		private void Init__jsonIgnore() {
			if (__jsonIgnore == null) {
				lock (__jsonIgnore_lock) {
					if (__jsonIgnore == null) {
						FieldInfo field = typeof(Shop_friendly_linksInfo).GetField("JsonIgnore");
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
			Shop_friendly_linksInfo item = obj as Shop_friendly_linksInfo;
			if (item == null) return false;
			return this.ToString().Equals(item.ToString());
		}
		public override int GetHashCode() {
			return this.ToString().GetHashCode();
		}
		public static bool operator ==(Shop_friendly_linksInfo op1, Shop_friendly_linksInfo op2) {
			if (object.Equals(op1, null)) return object.Equals(op2, null);
			return op1.Equals(op2);
		}
		public static bool operator !=(Shop_friendly_linksInfo op1, Shop_friendly_linksInfo op2) {
			return !(op1 == op2);
		}
		public object this[string key] {
			get { return this.GetType().GetProperty(key).GetValue(this); }
			set { this.GetType().GetProperty(key).SetValue(this, value); }
		}
		#endregion

		#region properties
		public uint? Id {
			get { return _Id; }
			set { _Id = value; }
		}
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
		/// 创建时间
		/// </summary>
		public DateTime? Create_time {
			get { return _Create_time; }
			set { _Create_time = value; }
		}
		/// <summary>
		/// 排序
		/// </summary>
		public byte? Sort {
			get { return _Sort; }
			set { _Sort = value; }
		}
		/// <summary>
		/// 文本
		/// </summary>
		public string Title {
			get { return _Title; }
			set { _Title = value; }
		}
		/// <summary>
		/// 链接
		/// </summary>
		public string Url {
			get { return _Url; }
			set { _Url = value; }
		}
		#endregion

		public pifa.DAL.Shop_friendly_links.SqlUpdateBuild UpdateDiy {
			get { return Shop_friendly_links.UpdateDiy(this, _Id); }
		}
	}
}

