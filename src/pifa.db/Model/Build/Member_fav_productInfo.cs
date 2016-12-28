using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using pifa.BLL;

namespace pifa.Model {

	public partial class Member_fav_productInfo {
		#region fields
		private uint? _Member_id;
		private MemberInfo _obj_member;
		private uint? _Product_id;
		private ProductInfo _obj_product;
		private DateTime? _Create_time;
		#endregion

		public Member_fav_productInfo() { }

		#region 独创的序列化，反序列化
		protected static readonly string StringifySplit = "@<Member_fav_product(Info]?#>";
		public string Stringify() {
			return string.Concat(
				_Member_id == null ? "null" : _Member_id.ToString(), "|",
				_Product_id == null ? "null" : _Product_id.ToString(), "|",
				_Create_time == null ? "null" : _Create_time.Value.Ticks.ToString());
		}
		public Member_fav_productInfo(string stringify) {
			string[] ret = stringify.Split(new char[] { '|' }, 3, StringSplitOptions.None);
			if (ret.Length != 3) throw new Exception("格式不正确，Member_fav_productInfo：" + stringify);
			if (string.Compare("null", ret[0]) != 0) _Member_id = uint.Parse(ret[0]);
			if (string.Compare("null", ret[1]) != 0) _Product_id = uint.Parse(ret[1]);
			if (string.Compare("null", ret[2]) != 0) _Create_time = new DateTime(long.Parse(ret[2]));
		}
		#endregion

		#region override
		private static Dictionary<string, bool> __jsonIgnore;
		private static object __jsonIgnore_lock = new object();
		public override string ToString() {
			this.Init__jsonIgnore();
			string json = string.Concat(
				__jsonIgnore.ContainsKey("Member_id") ? string.Empty : string.Format(", Member_id : {0}", Member_id == null ? "null" : Member_id.ToString()), 
				__jsonIgnore.ContainsKey("Product_id") ? string.Empty : string.Format(", Product_id : {0}", Product_id == null ? "null" : Product_id.ToString()), 
				__jsonIgnore.ContainsKey("Create_time") ? string.Empty : string.Format(", Create_time : {0}", Create_time == null ? "null" : Create_time.Value.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds.ToString()), " }");
			return string.Concat("{", json.Substring(1));
		}
		public IDictionary ToBson() {
			this.Init__jsonIgnore();
			IDictionary ht = new Hashtable();
			if (!__jsonIgnore.ContainsKey("Member_id")) ht["Member_id"] = Member_id;
			if (!__jsonIgnore.ContainsKey("Product_id")) ht["Product_id"] = Product_id;
			if (!__jsonIgnore.ContainsKey("Create_time")) ht["Create_time"] = Create_time;
			return ht;
		}
		private void Init__jsonIgnore() {
			if (__jsonIgnore == null) {
				lock (__jsonIgnore_lock) {
					if (__jsonIgnore == null) {
						FieldInfo field = typeof(Member_fav_productInfo).GetField("JsonIgnore");
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
			Member_fav_productInfo item = obj as Member_fav_productInfo;
			if (item == null) return false;
			return this.ToString().Equals(item.ToString());
		}
		public override int GetHashCode() {
			return this.ToString().GetHashCode();
		}
		public static bool operator ==(Member_fav_productInfo op1, Member_fav_productInfo op2) {
			if (object.Equals(op1, null)) return object.Equals(op2, null);
			return op1.Equals(op2);
		}
		public static bool operator !=(Member_fav_productInfo op1, Member_fav_productInfo op2) {
			return !(op1 == op2);
		}
		public object this[string key] {
			get { return this.GetType().GetProperty(key).GetValue(this); }
			set { this.GetType().GetProperty(key).SetValue(this, value); }
		}
		#endregion

		#region properties
		public uint? Member_id {
			get { return _Member_id; }
			set {
				if (_Member_id != value) _obj_member = null;
				_Member_id = value;
			}
		}
		public MemberInfo Obj_member {
			get {
				if (_obj_member == null) _obj_member = Member.GetItem(_Member_id);
				return _obj_member;
			}
			internal set { _obj_member = value; }
		}
		public uint? Product_id {
			get { return _Product_id; }
			set {
				if (_Product_id != value) _obj_product = null;
				_Product_id = value;
			}
		}
		public ProductInfo Obj_product {
			get {
				if (_obj_product == null) _obj_product = Product.GetItem(_Product_id);
				return _obj_product;
			}
			internal set { _obj_product = value; }
		}
		/// <summary>
		/// 创建时间
		/// </summary>
		public DateTime? Create_time {
			get { return _Create_time; }
			set { _Create_time = value; }
		}
		#endregion

		public pifa.DAL.Member_fav_product.SqlUpdateBuild UpdateDiy {
			get { return Member_fav_product.UpdateDiy(this, _Member_id, _Product_id); }
		}
	}
}

