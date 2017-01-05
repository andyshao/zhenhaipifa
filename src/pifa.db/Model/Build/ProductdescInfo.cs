using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using pifa.BLL;

namespace pifa.Model {

	[JsonObject(MemberSerialization.OptIn)]
	public partial class ProductdescInfo {
		#region fields
		private uint? _Product_id;
		private ProductInfo _obj_product;
		private string _Content;
		#endregion

		public ProductdescInfo() { }

		#region 序列化，反序列化
		protected static readonly string StringifySplit = "@<Productdesc(Info]?#>";
		public string Stringify() {
			return string.Concat(
				_Product_id == null ? "null" : _Product_id.ToString(), "|",
				_Content == null ? "null" : _Content.Replace("|", StringifySplit));
		}
		public static ProductdescInfo Parse(string stringify) {
			string[] ret = stringify.Split(new char[] { '|' }, 2, StringSplitOptions.None);
			if (ret.Length != 2) throw new Exception("格式不正确，ProductdescInfo：" + stringify);
			ProductdescInfo item = new ProductdescInfo();
			if (string.Compare("null", ret[0]) != 0) item.Product_id = uint.Parse(ret[0]);
			if (string.Compare("null", ret[1]) != 0) item.Content = ret[1].Replace(StringifySplit, "|");
			return item;
		}
		#endregion

		#region override
		private static Lazy<Dictionary<string, bool>> __jsonIgnoreLazy = new Lazy<Dictionary<string, bool>>(() => {
			FieldInfo field = typeof(ProductdescInfo).GetField("JsonIgnore");
			Dictionary<string, bool> ret = new Dictionary<string, bool>();
			if (field != null) string.Concat(field.GetValue(null)).Split(',').ToList().ForEach(f => {
				if (!string.IsNullOrEmpty(f)) ret[f] = true;
			});
			return ret;
		});
		private static Dictionary<string, bool> __jsonIgnore => __jsonIgnoreLazy.Value;
		public override string ToString() {
			string json = string.Concat(
				__jsonIgnore.ContainsKey("Product_id") ? string.Empty : string.Format(", Product_id : {0}", Product_id == null ? "null" : Product_id.ToString()), 
				__jsonIgnore.ContainsKey("Content") ? string.Empty : string.Format(", Content : {0}", Content == null ? "null" : string.Format("'{0}'", Content.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), " }");
			return string.Concat("{", json.Substring(1));
		}
		public IDictionary ToBson(bool allField = false) {
			IDictionary ht = new Hashtable();
			if (allField || !__jsonIgnore.ContainsKey("Product_id")) ht["Product_id"] = Product_id;
			if (allField || !__jsonIgnore.ContainsKey("Content")) ht["Content"] = Content;
			return ht;
		}
		public object this[string key] {
			get { return this.GetType().GetProperty(key).GetValue(this); }
			set { this.GetType().GetProperty(key).SetValue(this, value); }
		}
		#endregion

		#region properties
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
		/// 介绍
		/// </summary>
		[JsonProperty] public string Content {
			get { return _Content; }
			set { _Content = value; }
		}
		#endregion

		public pifa.DAL.Productdesc.SqlUpdateBuild UpdateDiy {
			get { return Productdesc.UpdateDiy(this, _Product_id.Value); }
		}
		public ProductdescInfo Save() {
			if (this.Product_id != null) {
				Productdesc.Update(this);
				return this;
			}
			return Productdesc.Insert(this);
		}
	}
}

