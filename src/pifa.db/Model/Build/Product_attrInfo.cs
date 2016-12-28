using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using pifa.BLL;

namespace pifa.Model {

	public partial class Product_attrInfo {
		#region fields
		private uint? _Pattr_id;
		private PattrInfo _obj_pattr;
		private uint? _Product_id;
		private ProductInfo _obj_product;
		private string _Value;
		#endregion

		public Product_attrInfo() { }

		#region 独创的序列化，反序列化
		protected static readonly string StringifySplit = "@<Product_attr(Info]?#>";
		public string Stringify() {
			return string.Concat(
				_Pattr_id == null ? "null" : _Pattr_id.ToString(), "|",
				_Product_id == null ? "null" : _Product_id.ToString(), "|",
				_Value == null ? "null" : _Value.Replace("|", StringifySplit));
		}
		public Product_attrInfo(string stringify) {
			string[] ret = stringify.Split(new char[] { '|' }, 3, StringSplitOptions.None);
			if (ret.Length != 3) throw new Exception("格式不正确，Product_attrInfo：" + stringify);
			if (string.Compare("null", ret[0]) != 0) _Pattr_id = uint.Parse(ret[0]);
			if (string.Compare("null", ret[1]) != 0) _Product_id = uint.Parse(ret[1]);
			if (string.Compare("null", ret[2]) != 0) _Value = ret[2].Replace(StringifySplit, "|");
		}
		#endregion

		#region override
		private static Dictionary<string, bool> __jsonIgnore;
		private static object __jsonIgnore_lock = new object();
		public override string ToString() {
			this.Init__jsonIgnore();
			string json = string.Concat(
				__jsonIgnore.ContainsKey("Pattr_id") ? string.Empty : string.Format(", Pattr_id : {0}", Pattr_id == null ? "null" : Pattr_id.ToString()), 
				__jsonIgnore.ContainsKey("Product_id") ? string.Empty : string.Format(", Product_id : {0}", Product_id == null ? "null" : Product_id.ToString()), 
				__jsonIgnore.ContainsKey("Value") ? string.Empty : string.Format(", Value : {0}", Value == null ? "null" : string.Format("'{0}'", Value.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), " }");
			return string.Concat("{", json.Substring(1));
		}
		public IDictionary ToBson() {
			this.Init__jsonIgnore();
			IDictionary ht = new Hashtable();
			if (!__jsonIgnore.ContainsKey("Pattr_id")) ht["Pattr_id"] = Pattr_id;
			if (!__jsonIgnore.ContainsKey("Product_id")) ht["Product_id"] = Product_id;
			if (!__jsonIgnore.ContainsKey("Value")) ht["Value"] = Value;
			return ht;
		}
		private void Init__jsonIgnore() {
			if (__jsonIgnore == null) {
				lock (__jsonIgnore_lock) {
					if (__jsonIgnore == null) {
						FieldInfo field = typeof(Product_attrInfo).GetField("JsonIgnore");
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
			Product_attrInfo item = obj as Product_attrInfo;
			if (item == null) return false;
			return this.ToString().Equals(item.ToString());
		}
		public override int GetHashCode() {
			return this.ToString().GetHashCode();
		}
		public static bool operator ==(Product_attrInfo op1, Product_attrInfo op2) {
			if (object.Equals(op1, null)) return object.Equals(op2, null);
			return op1.Equals(op2);
		}
		public static bool operator !=(Product_attrInfo op1, Product_attrInfo op2) {
			return !(op1 == op2);
		}
		public object this[string key] {
			get { return this.GetType().GetProperty(key).GetValue(this); }
			set { this.GetType().GetProperty(key).SetValue(this, value); }
		}
		#endregion

		#region properties
		public uint? Pattr_id {
			get { return _Pattr_id; }
			set {
				if (_Pattr_id != value) _obj_pattr = null;
				_Pattr_id = value;
			}
		}
		public PattrInfo Obj_pattr {
			get {
				if (_obj_pattr == null) _obj_pattr = Pattr.GetItem(_Pattr_id);
				return _obj_pattr;
			}
			internal set { _obj_pattr = value; }
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
		/// 属性值
		/// </summary>
		public string Value {
			get { return _Value; }
			set { _Value = value; }
		}
		#endregion

		public pifa.DAL.Product_attr.SqlUpdateBuild UpdateDiy {
			get { return Product_attr.UpdateDiy(this, _Pattr_id, _Product_id); }
		}
	}
}

