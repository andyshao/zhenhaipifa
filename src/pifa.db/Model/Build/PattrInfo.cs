using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using pifa.BLL;

namespace pifa.Model {

	public partial class PattrInfo {
		#region fields
		private uint? _Id;
		private uint? _Category_id;
		private CategoryInfo _obj_category;
		private uint? _Parent_id;
		private PattrInfo _obj_pattr;
		private bool? _Is_filter;
		private string _Name;
		#endregion

		public PattrInfo() { }

		#region 独创的序列化，反序列化
		protected static readonly string StringifySplit = "@<Pattr(Info]?#>";
		public string Stringify() {
			return string.Concat(
				_Id == null ? "null" : _Id.ToString(), "|",
				_Category_id == null ? "null" : _Category_id.ToString(), "|",
				_Parent_id == null ? "null" : _Parent_id.ToString(), "|",
				_Is_filter == null ? "null" : (_Is_filter == true ? "1" : "0"), "|",
				_Name == null ? "null" : _Name.Replace("|", StringifySplit));
		}
		public PattrInfo(string stringify) {
			string[] ret = stringify.Split(new char[] { '|' }, 5, StringSplitOptions.None);
			if (ret.Length != 5) throw new Exception("格式不正确，PattrInfo：" + stringify);
			if (string.Compare("null", ret[0]) != 0) _Id = uint.Parse(ret[0]);
			if (string.Compare("null", ret[1]) != 0) _Category_id = uint.Parse(ret[1]);
			if (string.Compare("null", ret[2]) != 0) _Parent_id = uint.Parse(ret[2]);
			if (string.Compare("null", ret[3]) != 0) _Is_filter = ret[3] == "1";
			if (string.Compare("null", ret[4]) != 0) _Name = ret[4].Replace(StringifySplit, "|");
		}
		#endregion

		#region override
		private static Dictionary<string, bool> __jsonIgnore;
		private static object __jsonIgnore_lock = new object();
		public override string ToString() {
			this.Init__jsonIgnore();
			string json = string.Concat(
				__jsonIgnore.ContainsKey("Id") ? string.Empty : string.Format(", Id : {0}", Id == null ? "null" : Id.ToString()), 
				__jsonIgnore.ContainsKey("Category_id") ? string.Empty : string.Format(", Category_id : {0}", Category_id == null ? "null" : Category_id.ToString()), 
				__jsonIgnore.ContainsKey("Parent_id") ? string.Empty : string.Format(", Parent_id : {0}", Parent_id == null ? "null" : Parent_id.ToString()), 
				__jsonIgnore.ContainsKey("Is_filter") ? string.Empty : string.Format(", Is_filter : {0}", Is_filter == null ? "null" : (Is_filter == true ? "true" : "false")), 
				__jsonIgnore.ContainsKey("Name") ? string.Empty : string.Format(", Name : {0}", Name == null ? "null" : string.Format("'{0}'", Name.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), " }");
			return string.Concat("{", json.Substring(1));
		}
		public IDictionary ToBson() {
			this.Init__jsonIgnore();
			IDictionary ht = new Hashtable();
			if (!__jsonIgnore.ContainsKey("Id")) ht["Id"] = Id;
			if (!__jsonIgnore.ContainsKey("Category_id")) ht["Category_id"] = Category_id;
			if (!__jsonIgnore.ContainsKey("Parent_id")) ht["Parent_id"] = Parent_id;
			if (!__jsonIgnore.ContainsKey("Is_filter")) ht["Is_filter"] = Is_filter;
			if (!__jsonIgnore.ContainsKey("Name")) ht["Name"] = Name;
			return ht;
		}
		private void Init__jsonIgnore() {
			if (__jsonIgnore == null) {
				lock (__jsonIgnore_lock) {
					if (__jsonIgnore == null) {
						FieldInfo field = typeof(PattrInfo).GetField("JsonIgnore");
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
			PattrInfo item = obj as PattrInfo;
			if (item == null) return false;
			return this.ToString().Equals(item.ToString());
		}
		public override int GetHashCode() {
			return this.ToString().GetHashCode();
		}
		public static bool operator ==(PattrInfo op1, PattrInfo op2) {
			if (object.Equals(op1, null)) return object.Equals(op2, null);
			return op1.Equals(op2);
		}
		public static bool operator !=(PattrInfo op1, PattrInfo op2) {
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
		/// <summary>
		/// 分类
		/// </summary>
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
		/// <summary>
		/// 父
		/// </summary>
		public uint? Parent_id {
			get { return _Parent_id; }
			set {
				if (_Parent_id != value) _obj_pattr = null;
				_Parent_id = value;
			}
		}
		public PattrInfo Obj_pattr {
			get {
				if (_obj_pattr == null) _obj_pattr = Pattr.GetItem(_Parent_id);
				return _obj_pattr;
			}
			internal set { _obj_pattr = value; }
		}
		/// <summary>
		/// 是否过滤
		/// </summary>
		public bool? Is_filter {
			get { return _Is_filter; }
			set { _Is_filter = value; }
		}
		/// <summary>
		/// 属性名称
		/// </summary>
		public string Name {
			get { return _Name; }
			set { _Name = value; }
		}
		private List<PattrInfo> _obj_pattrs;
		public List<PattrInfo> Obj_pattrs {
			get {
				if (_obj_pattrs == null) _obj_pattrs = Pattr.SelectByParent_id(_Id).Limit(500).ToList();
				return _obj_pattrs;
			}
		}
		public Product_attrInfo Obj_product_attr { set; get; }
		private List<ProductInfo> _obj_product_attrs;
		/// <summary>
		/// 遍历时，可通过 Obj_product_attr 可获取中间表数据
		/// </summary>
		public List<ProductInfo> Obj_product_attrs {
			get {
				if (_obj_product_attrs == null) _obj_product_attrs = Product.Select.InnerJoin<Product_attr>("b", "b.`product_id` = a.`id`").Where("b.`pattr_id` = {0}", _Id.Value).ToList();
				return _obj_product_attrs;
			}
		}
		#endregion

		public pifa.DAL.Pattr.SqlUpdateBuild UpdateDiy {
			get { return Pattr.UpdateDiy(this, _Id); }
		}
		public PattrInfo AddPattr(CategoryInfo Category, bool? Is_filter, string Name) {
			return AddPattr(Category.Id, Is_filter, Name);
		}
		public PattrInfo AddPattr(uint? Category_id, bool? Is_filter, string Name) {
			return Pattr.Insert(new PattrInfo {
				Category_id = Category_id, 
				Parent_id = this.Id, 
				Is_filter = Is_filter, 
				Name = Name});
		}

		public Product_attrInfo FlagProduct_attr(ProductInfo Product, string Value) {
			return FlagProduct_attr(Product.Id, Value);
		}
		public Product_attrInfo FlagProduct_attr(uint? Product_id, string Value) {
			Product_attrInfo item = Product_attr.GetItem(this.Id, Product_id);
			if (item == null) item = Product_attr.Insert(new Product_attrInfo {
				Pattr_id = this.Id, 
				Product_id = Product_id, 
				Value = Value});
			else item.UpdateDiy
					.SetValue(Value).ExecuteNonQuery();
			return item;
		}

		public int UnflagProduct_attr(ProductInfo Product) {
			return UnflagProduct_attr(Product.Id);
		}
		public int UnflagProduct_attr(uint? Product_id) {
			return Product_attr.Delete(this.Id, Product_id);
		}
		public int UnflagProduct_attrALL() {
			return Product_attr.DeleteByPattr_id(this.Id);
		}

	}
}

