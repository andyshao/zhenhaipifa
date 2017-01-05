using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using pifa.BLL;

namespace pifa.Model {

	[JsonObject(MemberSerialization.OptIn)]
	public partial class Product_attrInfo {
		#region fields
		private uint? _Pattr_id;
		private PattrInfo _obj_pattr;
		private uint? _Product_id;
		private ProductInfo _obj_product;
		private string _Value;
		#endregion

		public Product_attrInfo() { }

		#region 序列化，反序列化
		protected static readonly string StringifySplit = "@<Product_attr(Info]?#>";
		public string Stringify() {
			return string.Concat(
				_Pattr_id == null ? "null" : _Pattr_id.ToString(), "|",
				_Product_id == null ? "null" : _Product_id.ToString(), "|",
				_Value == null ? "null" : _Value.Replace("|", StringifySplit));
		}
		public static Product_attrInfo Parse(string stringify) {
			string[] ret = stringify.Split(new char[] { '|' }, 3, StringSplitOptions.None);
			if (ret.Length != 3) throw new Exception("格式不正确，Product_attrInfo：" + stringify);
			Product_attrInfo item = new Product_attrInfo();
			if (string.Compare("null", ret[0]) != 0) item.Pattr_id = uint.Parse(ret[0]);
			if (string.Compare("null", ret[1]) != 0) item.Product_id = uint.Parse(ret[1]);
			if (string.Compare("null", ret[2]) != 0) item.Value = ret[2].Replace(StringifySplit, "|");
			return item;
		}
		#endregion

		#region override
		private static Lazy<Dictionary<string, bool>> __jsonIgnoreLazy = new Lazy<Dictionary<string, bool>>(() => {
			FieldInfo field = typeof(Product_attrInfo).GetField("JsonIgnore");
			Dictionary<string, bool> ret = new Dictionary<string, bool>();
			if (field != null) string.Concat(field.GetValue(null)).Split(',').ToList().ForEach(f => {
				if (!string.IsNullOrEmpty(f)) ret[f] = true;
			});
			return ret;
		});
		private static Dictionary<string, bool> __jsonIgnore => __jsonIgnoreLazy.Value;
		public override string ToString() {
			string json = string.Concat(
				__jsonIgnore.ContainsKey("Pattr_id") ? string.Empty : string.Format(", Pattr_id : {0}", Pattr_id == null ? "null" : Pattr_id.ToString()), 
				__jsonIgnore.ContainsKey("Product_id") ? string.Empty : string.Format(", Product_id : {0}", Product_id == null ? "null" : Product_id.ToString()), 
				__jsonIgnore.ContainsKey("Value") ? string.Empty : string.Format(", Value : {0}", Value == null ? "null" : string.Format("'{0}'", Value.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), " }");
			return string.Concat("{", json.Substring(1));
		}
		public IDictionary ToBson(bool allField = false) {
			IDictionary ht = new Hashtable();
			if (allField || !__jsonIgnore.ContainsKey("Pattr_id")) ht["Pattr_id"] = Pattr_id;
			if (allField || !__jsonIgnore.ContainsKey("Product_id")) ht["Product_id"] = Product_id;
			if (allField || !__jsonIgnore.ContainsKey("Value")) ht["Value"] = Value;
			return ht;
		}
		public object this[string key] {
			get { return this.GetType().GetProperty(key).GetValue(this); }
			set { this.GetType().GetProperty(key).SetValue(this, value); }
		}
		#endregion

		#region properties
		[JsonProperty] public uint? Pattr_id {
			get { return _Pattr_id; }
			set {
				if (_Pattr_id != value) _obj_pattr = null;
				_Pattr_id = value;
			}
		}
		public PattrInfo Obj_pattr {
			get {
				if (_obj_pattr == null) _obj_pattr = Pattr.GetItem(_Pattr_id.Value);
				return _obj_pattr;
			}
			internal set { _obj_pattr = value; }
		}
		[JsonProperty] public uint? Product_id {
			get { return _Product_id; }
			set {
				if (_Product_id != value) _obj_product = null;
				_Product_id = value;
			}
		}
		public ProductInfo Obj_product {
			get {
				if (_obj_product == null) _obj_product = Product.GetItem(_Product_id.Value);
				return _obj_product;
			}
			internal set { _obj_product = value; }
		}
		/// <summary>
		/// 属性值
		/// </summary>
		[JsonProperty] public string Value {
			get { return _Value; }
			set { _Value = value; }
		}
		#endregion

		public pifa.DAL.Product_attr.SqlUpdateBuild UpdateDiy {
			get { return Product_attr.UpdateDiy(this, _Pattr_id.Value, _Product_id.Value); }
		}
		public Product_attrInfo Save() {
			if (this.Pattr_id != null && this.Product_id != null) {
				Product_attr.Update(this);
				return this;
			}
			return Product_attr.Insert(this);
		}
	}
}

