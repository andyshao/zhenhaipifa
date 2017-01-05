using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using pifa.BLL;

namespace pifa.Model {

	[JsonObject(MemberSerialization.OptIn)]
	public partial class Area_categoryInfo {
		#region fields
		private uint? _Area_id;
		private AreaInfo _obj_area;
		private uint? _Category_id;
		private CategoryInfo _obj_category;
		#endregion

		public Area_categoryInfo() { }

		#region 序列化，反序列化
		protected static readonly string StringifySplit = "@<Area_category(Info]?#>";
		public string Stringify() {
			return string.Concat(
				_Area_id == null ? "null" : _Area_id.ToString(), "|",
				_Category_id == null ? "null" : _Category_id.ToString());
		}
		public static Area_categoryInfo Parse(string stringify) {
			string[] ret = stringify.Split(new char[] { '|' }, 2, StringSplitOptions.None);
			if (ret.Length != 2) throw new Exception("格式不正确，Area_categoryInfo：" + stringify);
			Area_categoryInfo item = new Area_categoryInfo();
			if (string.Compare("null", ret[0]) != 0) item.Area_id = uint.Parse(ret[0]);
			if (string.Compare("null", ret[1]) != 0) item.Category_id = uint.Parse(ret[1]);
			return item;
		}
		#endregion

		#region override
		private static Lazy<Dictionary<string, bool>> __jsonIgnoreLazy = new Lazy<Dictionary<string, bool>>(() => {
			FieldInfo field = typeof(Area_categoryInfo).GetField("JsonIgnore");
			Dictionary<string, bool> ret = new Dictionary<string, bool>();
			if (field != null) string.Concat(field.GetValue(null)).Split(',').ToList().ForEach(f => {
				if (!string.IsNullOrEmpty(f)) ret[f] = true;
			});
			return ret;
		});
		private static Dictionary<string, bool> __jsonIgnore => __jsonIgnoreLazy.Value;
		public override string ToString() {
			string json = string.Concat(
				__jsonIgnore.ContainsKey("Area_id") ? string.Empty : string.Format(", Area_id : {0}", Area_id == null ? "null" : Area_id.ToString()), 
				__jsonIgnore.ContainsKey("Category_id") ? string.Empty : string.Format(", Category_id : {0}", Category_id == null ? "null" : Category_id.ToString()), " }");
			return string.Concat("{", json.Substring(1));
		}
		public IDictionary ToBson(bool allField = false) {
			IDictionary ht = new Hashtable();
			if (allField || !__jsonIgnore.ContainsKey("Area_id")) ht["Area_id"] = Area_id;
			if (allField || !__jsonIgnore.ContainsKey("Category_id")) ht["Category_id"] = Category_id;
			return ht;
		}
		public object this[string key] {
			get { return this.GetType().GetProperty(key).GetValue(this); }
			set { this.GetType().GetProperty(key).SetValue(this, value); }
		}
		#endregion

		#region properties
		[JsonProperty] public uint? Area_id {
			get { return _Area_id; }
			set {
				if (_Area_id != value) _obj_area = null;
				_Area_id = value;
			}
		}
		public AreaInfo Obj_area {
			get {
				if (_obj_area == null) _obj_area = Area.GetItem(_Area_id.Value);
				return _obj_area;
			}
			internal set { _obj_area = value; }
		}
		[JsonProperty] public uint? Category_id {
			get { return _Category_id; }
			set {
				if (_Category_id != value) _obj_category = null;
				_Category_id = value;
			}
		}
		public CategoryInfo Obj_category {
			get {
				if (_obj_category == null) _obj_category = Category.GetItem(_Category_id.Value);
				return _obj_category;
			}
			internal set { _obj_category = value; }
		}
		#endregion

		public pifa.DAL.Area_category.SqlUpdateBuild UpdateDiy {
			get { return Area_category.UpdateDiy(this, _Area_id.Value, _Category_id.Value); }
		}
	}
}

