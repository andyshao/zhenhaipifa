using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using pifa.BLL;

namespace pifa.Model {

	public partial class Area_categoryInfo {
		#region fields
		private uint? _Area_id;
		private AreaInfo _obj_area;
		private uint? _Category_id;
		private CategoryInfo _obj_category;
		#endregion

		public Area_categoryInfo() { }

		#region 独创的序列化，反序列化
		protected static readonly string StringifySplit = "@<Area_category(Info]?#>";
		public string Stringify() {
			return string.Concat(
				_Area_id == null ? "null" : _Area_id.ToString(), "|",
				_Category_id == null ? "null" : _Category_id.ToString());
		}
		public Area_categoryInfo(string stringify) {
			string[] ret = stringify.Split(new char[] { '|' }, 2, StringSplitOptions.None);
			if (ret.Length != 2) throw new Exception("格式不正确，Area_categoryInfo：" + stringify);
			if (string.Compare("null", ret[0]) != 0) _Area_id = uint.Parse(ret[0]);
			if (string.Compare("null", ret[1]) != 0) _Category_id = uint.Parse(ret[1]);
		}
		#endregion

		#region override
		private static Dictionary<string, bool> __jsonIgnore;
		private static object __jsonIgnore_lock = new object();
		public override string ToString() {
			this.Init__jsonIgnore();
			string json = string.Concat(
				__jsonIgnore.ContainsKey("Area_id") ? string.Empty : string.Format(", Area_id : {0}", Area_id == null ? "null" : Area_id.ToString()), 
				__jsonIgnore.ContainsKey("Category_id") ? string.Empty : string.Format(", Category_id : {0}", Category_id == null ? "null" : Category_id.ToString()), " }");
			return string.Concat("{", json.Substring(1));
		}
		public IDictionary ToBson() {
			this.Init__jsonIgnore();
			IDictionary ht = new Hashtable();
			if (!__jsonIgnore.ContainsKey("Area_id")) ht["Area_id"] = Area_id;
			if (!__jsonIgnore.ContainsKey("Category_id")) ht["Category_id"] = Category_id;
			return ht;
		}
		private void Init__jsonIgnore() {
			if (__jsonIgnore == null) {
				lock (__jsonIgnore_lock) {
					if (__jsonIgnore == null) {
						FieldInfo field = typeof(Area_categoryInfo).GetField("JsonIgnore");
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
			Area_categoryInfo item = obj as Area_categoryInfo;
			if (item == null) return false;
			return this.ToString().Equals(item.ToString());
		}
		public override int GetHashCode() {
			return this.ToString().GetHashCode();
		}
		public static bool operator ==(Area_categoryInfo op1, Area_categoryInfo op2) {
			if (object.Equals(op1, null)) return object.Equals(op2, null);
			return op1.Equals(op2);
		}
		public static bool operator !=(Area_categoryInfo op1, Area_categoryInfo op2) {
			return !(op1 == op2);
		}
		public object this[string key] {
			get { return this.GetType().GetProperty(key).GetValue(this); }
			set { this.GetType().GetProperty(key).SetValue(this, value); }
		}
		#endregion

		#region properties
		public uint? Area_id {
			get { return _Area_id; }
			set {
				if (_Area_id != value) _obj_area = null;
				_Area_id = value;
			}
		}
		public AreaInfo Obj_area {
			get {
				if (_obj_area == null) _obj_area = Area.GetItem(_Area_id);
				return _obj_area;
			}
			internal set { _obj_area = value; }
		}
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
		#endregion

		public pifa.DAL.Area_category.SqlUpdateBuild UpdateDiy {
			get { return Area_category.UpdateDiy(this, _Area_id, _Category_id); }
		}
	}
}

