using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using pifa.BLL;

namespace pifa.Model {

	public partial class Markettype_categoryInfo {
		#region fields
		private uint? _Category_id;
		private CategoryInfo _obj_category;
		private uint? _Markettype_id;
		private MarkettypeInfo _obj_markettype;
		#endregion

		public Markettype_categoryInfo() { }

		#region 独创的序列化，反序列化
		protected static readonly string StringifySplit = "@<Markettype_category(Info]?#>";
		public string Stringify() {
			return string.Concat(
				_Category_id == null ? "null" : _Category_id.ToString(), "|",
				_Markettype_id == null ? "null" : _Markettype_id.ToString());
		}
		public Markettype_categoryInfo(string stringify) {
			string[] ret = stringify.Split(new char[] { '|' }, 2, StringSplitOptions.None);
			if (ret.Length != 2) throw new Exception("格式不正确，Markettype_categoryInfo：" + stringify);
			if (string.Compare("null", ret[0]) != 0) _Category_id = uint.Parse(ret[0]);
			if (string.Compare("null", ret[1]) != 0) _Markettype_id = uint.Parse(ret[1]);
		}
		#endregion

		#region override
		private static Dictionary<string, bool> __jsonIgnore;
		private static object __jsonIgnore_lock = new object();
		public override string ToString() {
			this.Init__jsonIgnore();
			string json = string.Concat(
				__jsonIgnore.ContainsKey("Category_id") ? string.Empty : string.Format(", Category_id : {0}", Category_id == null ? "null" : Category_id.ToString()), 
				__jsonIgnore.ContainsKey("Markettype_id") ? string.Empty : string.Format(", Markettype_id : {0}", Markettype_id == null ? "null" : Markettype_id.ToString()), " }");
			return string.Concat("{", json.Substring(1));
		}
		public IDictionary ToBson() {
			this.Init__jsonIgnore();
			IDictionary ht = new Hashtable();
			if (!__jsonIgnore.ContainsKey("Category_id")) ht["Category_id"] = Category_id;
			if (!__jsonIgnore.ContainsKey("Markettype_id")) ht["Markettype_id"] = Markettype_id;
			return ht;
		}
		private void Init__jsonIgnore() {
			if (__jsonIgnore == null) {
				lock (__jsonIgnore_lock) {
					if (__jsonIgnore == null) {
						FieldInfo field = typeof(Markettype_categoryInfo).GetField("JsonIgnore");
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
			Markettype_categoryInfo item = obj as Markettype_categoryInfo;
			if (item == null) return false;
			return this.ToString().Equals(item.ToString());
		}
		public override int GetHashCode() {
			return this.ToString().GetHashCode();
		}
		public static bool operator ==(Markettype_categoryInfo op1, Markettype_categoryInfo op2) {
			if (object.Equals(op1, null)) return object.Equals(op2, null);
			return op1.Equals(op2);
		}
		public static bool operator !=(Markettype_categoryInfo op1, Markettype_categoryInfo op2) {
			return !(op1 == op2);
		}
		public object this[string key] {
			get { return this.GetType().GetProperty(key).GetValue(this); }
			set { this.GetType().GetProperty(key).SetValue(this, value); }
		}
		#endregion

		#region properties
		public uint? Category_id {
			get { return _Category_id; }
			set {
				if (_Category_id != value) _obj_category = null;
				_Category_id = value;
			}
		}
		public CategoryInfo Obj_category {
			get {
				if (_obj_category == null) _obj_category = Category.GetItem(_Category_id);
				return _obj_category;
			}
			internal set { _obj_category = value; }
		}
		public uint? Markettype_id {
			get { return _Markettype_id; }
			set {
				if (_Markettype_id != value) _obj_markettype = null;
				_Markettype_id = value;
			}
		}
		public MarkettypeInfo Obj_markettype {
			get {
				if (_obj_markettype == null) _obj_markettype = Markettype.GetItem(_Markettype_id);
				return _obj_markettype;
			}
			internal set { _obj_markettype = value; }
		}
		#endregion

		public pifa.DAL.Markettype_category.SqlUpdateBuild UpdateDiy {
			get { return Markettype_category.UpdateDiy(this, _Category_id, _Markettype_id); }
		}
	}
}

