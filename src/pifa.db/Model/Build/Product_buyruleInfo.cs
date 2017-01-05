using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using pifa.BLL;

namespace pifa.Model {

	[JsonObject(MemberSerialization.OptIn)]
	public partial class Product_buyruleInfo {
		#region fields
		private uint? _Id;
		private uint? _Product_id;
		private ProductInfo _obj_product;
		private uint? _Discount;
		private uint? _Ordering_end;
		private uint? _Ordering_start;
		#endregion

		public Product_buyruleInfo() { }

		#region 序列化，反序列化
		protected static readonly string StringifySplit = "@<Product_buyrule(Info]?#>";
		public string Stringify() {
			return string.Concat(
				_Id == null ? "null" : _Id.ToString(), "|",
				_Product_id == null ? "null" : _Product_id.ToString(), "|",
				_Discount == null ? "null" : _Discount.ToString(), "|",
				_Ordering_end == null ? "null" : _Ordering_end.ToString(), "|",
				_Ordering_start == null ? "null" : _Ordering_start.ToString());
		}
		public static Product_buyruleInfo Parse(string stringify) {
			string[] ret = stringify.Split(new char[] { '|' }, 5, StringSplitOptions.None);
			if (ret.Length != 5) throw new Exception("格式不正确，Product_buyruleInfo：" + stringify);
			Product_buyruleInfo item = new Product_buyruleInfo();
			if (string.Compare("null", ret[0]) != 0) item.Id = uint.Parse(ret[0]);
			if (string.Compare("null", ret[1]) != 0) item.Product_id = uint.Parse(ret[1]);
			if (string.Compare("null", ret[2]) != 0) item.Discount = uint.Parse(ret[2]);
			if (string.Compare("null", ret[3]) != 0) item.Ordering_end = uint.Parse(ret[3]);
			if (string.Compare("null", ret[4]) != 0) item.Ordering_start = uint.Parse(ret[4]);
			return item;
		}
		#endregion

		#region override
		private static Lazy<Dictionary<string, bool>> __jsonIgnoreLazy = new Lazy<Dictionary<string, bool>>(() => {
			FieldInfo field = typeof(Product_buyruleInfo).GetField("JsonIgnore");
			Dictionary<string, bool> ret = new Dictionary<string, bool>();
			if (field != null) string.Concat(field.GetValue(null)).Split(',').ToList().ForEach(f => {
				if (!string.IsNullOrEmpty(f)) ret[f] = true;
			});
			return ret;
		});
		private static Dictionary<string, bool> __jsonIgnore => __jsonIgnoreLazy.Value;
		public override string ToString() {
			string json = string.Concat(
				__jsonIgnore.ContainsKey("Id") ? string.Empty : string.Format(", Id : {0}", Id == null ? "null" : Id.ToString()), 
				__jsonIgnore.ContainsKey("Product_id") ? string.Empty : string.Format(", Product_id : {0}", Product_id == null ? "null" : Product_id.ToString()), 
				__jsonIgnore.ContainsKey("Discount") ? string.Empty : string.Format(", Discount : {0}", Discount == null ? "null" : Discount.ToString()), 
				__jsonIgnore.ContainsKey("Ordering_end") ? string.Empty : string.Format(", Ordering_end : {0}", Ordering_end == null ? "null" : Ordering_end.ToString()), 
				__jsonIgnore.ContainsKey("Ordering_start") ? string.Empty : string.Format(", Ordering_start : {0}", Ordering_start == null ? "null" : Ordering_start.ToString()), " }");
			return string.Concat("{", json.Substring(1));
		}
		public IDictionary ToBson(bool allField = false) {
			IDictionary ht = new Hashtable();
			if (allField || !__jsonIgnore.ContainsKey("Id")) ht["Id"] = Id;
			if (allField || !__jsonIgnore.ContainsKey("Product_id")) ht["Product_id"] = Product_id;
			if (allField || !__jsonIgnore.ContainsKey("Discount")) ht["Discount"] = Discount;
			if (allField || !__jsonIgnore.ContainsKey("Ordering_end")) ht["Ordering_end"] = Ordering_end;
			if (allField || !__jsonIgnore.ContainsKey("Ordering_start")) ht["Ordering_start"] = Ordering_start;
			return ht;
		}
		public object this[string key] {
			get { return this.GetType().GetProperty(key).GetValue(this); }
			set { this.GetType().GetProperty(key).SetValue(this, value); }
		}
		#endregion

		#region properties
		[JsonProperty] public uint? Id {
			get { return _Id; }
			set { _Id = value; }
		}
		/// <summary>
		/// 产品
		/// </summary>
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
		/// 折扣(0-100)
		/// </summary>
		[JsonProperty] public uint? Discount {
			get { return _Discount; }
			set { _Discount = value; }
		}
		/// <summary>
		/// 始订量
		/// </summary>
		[JsonProperty] public uint? Ordering_end {
			get { return _Ordering_end; }
			set { _Ordering_end = value; }
		}
		/// <summary>
		/// 起订量
		/// </summary>
		[JsonProperty] public uint? Ordering_start {
			get { return _Ordering_start; }
			set { _Ordering_start = value; }
		}
		#endregion

		public pifa.DAL.Product_buyrule.SqlUpdateBuild UpdateDiy {
			get { return Product_buyrule.UpdateDiy(this, _Id.Value); }
		}
		public Product_buyruleInfo Save() {
			if (this.Id != null) {
				Product_buyrule.Update(this);
				return this;
			}
			return Product_buyrule.Insert(this);
		}
	}
}

