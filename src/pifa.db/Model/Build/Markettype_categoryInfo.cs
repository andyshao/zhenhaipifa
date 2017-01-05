using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using pifa.BLL;

namespace pifa.Model {

	[JsonObject(MemberSerialization.OptIn)]
	public partial class Markettype_categoryInfo {
		#region fields
		private uint? _Category_id;
		private CategoryInfo _obj_category;
		private uint? _Markettype_id;
		private MarkettypeInfo _obj_markettype;
		#endregion

		public Markettype_categoryInfo() { }

		#region 序列化，反序列化
		protected static readonly string StringifySplit = "@<Markettype_category(Info]?#>";
		public string Stringify() {
			return string.Concat(
				_Category_id == null ? "null" : _Category_id.ToString(), "|",
				_Markettype_id == null ? "null" : _Markettype_id.ToString());
		}
		public static Markettype_categoryInfo Parse(string stringify) {
			string[] ret = stringify.Split(new char[] { '|' }, 2, StringSplitOptions.None);
			if (ret.Length != 2) throw new Exception("格式不正确，Markettype_categoryInfo：" + stringify);
			Markettype_categoryInfo item = new Markettype_categoryInfo();
			if (string.Compare("null", ret[0]) != 0) item.Category_id = uint.Parse(ret[0]);
			if (string.Compare("null", ret[1]) != 0) item.Markettype_id = uint.Parse(ret[1]);
			return item;
		}
		#endregion

		#region override
		private static Lazy<Dictionary<string, bool>> __jsonIgnoreLazy = new Lazy<Dictionary<string, bool>>(() => {
			FieldInfo field = typeof(Markettype_categoryInfo).GetField("JsonIgnore");
			Dictionary<string, bool> ret = new Dictionary<string, bool>();
			if (field != null) string.Concat(field.GetValue(null)).Split(',').ToList().ForEach(f => {
				if (!string.IsNullOrEmpty(f)) ret[f] = true;
			});
			return ret;
		});
		private static Dictionary<string, bool> __jsonIgnore => __jsonIgnoreLazy.Value;
		public override string ToString() {
			string json = string.Concat(
				__jsonIgnore.ContainsKey("Category_id") ? string.Empty : string.Format(", Category_id : {0}", Category_id == null ? "null" : Category_id.ToString()), 
				__jsonIgnore.ContainsKey("Markettype_id") ? string.Empty : string.Format(", Markettype_id : {0}", Markettype_id == null ? "null" : Markettype_id.ToString()), " }");
			return string.Concat("{", json.Substring(1));
		}
		public IDictionary ToBson(bool allField = false) {
			IDictionary ht = new Hashtable();
			if (allField || !__jsonIgnore.ContainsKey("Category_id")) ht["Category_id"] = Category_id;
			if (allField || !__jsonIgnore.ContainsKey("Markettype_id")) ht["Markettype_id"] = Markettype_id;
			return ht;
		}
		public object this[string key] {
			get { return this.GetType().GetProperty(key).GetValue(this); }
			set { this.GetType().GetProperty(key).SetValue(this, value); }
		}
		#endregion

		#region properties
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
		[JsonProperty] public uint? Markettype_id {
			get { return _Markettype_id; }
			set {
				if (_Markettype_id != value) _obj_markettype = null;
				_Markettype_id = value;
			}
		}
		public MarkettypeInfo Obj_markettype {
			get {
				if (_obj_markettype == null) _obj_markettype = Markettype.GetItem(_Markettype_id.Value);
				return _obj_markettype;
			}
			internal set { _obj_markettype = value; }
		}
		#endregion

		public pifa.DAL.Markettype_category.SqlUpdateBuild UpdateDiy {
			get { return Markettype_category.UpdateDiy(this, _Category_id.Value, _Markettype_id.Value); }
		}
	}
}

