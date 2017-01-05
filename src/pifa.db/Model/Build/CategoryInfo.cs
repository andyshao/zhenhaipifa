using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using pifa.BLL;

namespace pifa.Model {

	[JsonObject(MemberSerialization.OptIn)]
	public partial class CategoryInfo {
		#region fields
		private uint? _Id;
		private uint? _Parent_id;
		private CategoryInfo _obj_category;
		private string _Title;
		#endregion

		public CategoryInfo() { }

		#region 序列化，反序列化
		protected static readonly string StringifySplit = "@<Category(Info]?#>";
		public string Stringify() {
			return string.Concat(
				_Id == null ? "null" : _Id.ToString(), "|",
				_Parent_id == null ? "null" : _Parent_id.ToString(), "|",
				_Title == null ? "null" : _Title.Replace("|", StringifySplit));
		}
		public static CategoryInfo Parse(string stringify) {
			string[] ret = stringify.Split(new char[] { '|' }, 3, StringSplitOptions.None);
			if (ret.Length != 3) throw new Exception("格式不正确，CategoryInfo：" + stringify);
			CategoryInfo item = new CategoryInfo();
			if (string.Compare("null", ret[0]) != 0) item.Id = uint.Parse(ret[0]);
			if (string.Compare("null", ret[1]) != 0) item.Parent_id = uint.Parse(ret[1]);
			if (string.Compare("null", ret[2]) != 0) item.Title = ret[2].Replace(StringifySplit, "|");
			return item;
		}
		#endregion

		#region override
		private static Lazy<Dictionary<string, bool>> __jsonIgnoreLazy = new Lazy<Dictionary<string, bool>>(() => {
			FieldInfo field = typeof(CategoryInfo).GetField("JsonIgnore");
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
				__jsonIgnore.ContainsKey("Parent_id") ? string.Empty : string.Format(", Parent_id : {0}", Parent_id == null ? "null" : Parent_id.ToString()), 
				__jsonIgnore.ContainsKey("Title") ? string.Empty : string.Format(", Title : {0}", Title == null ? "null" : string.Format("'{0}'", Title.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), " }");
			return string.Concat("{", json.Substring(1));
		}
		public IDictionary ToBson(bool allField = false) {
			IDictionary ht = new Hashtable();
			if (allField || !__jsonIgnore.ContainsKey("Id")) ht["Id"] = Id;
			if (allField || !__jsonIgnore.ContainsKey("Parent_id")) ht["Parent_id"] = Parent_id;
			if (allField || !__jsonIgnore.ContainsKey("Title")) ht["Title"] = Title;
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
		/// 父
		/// </summary>
		[JsonProperty] public uint? Parent_id {
			get { return _Parent_id; }
			set {
				if (_Parent_id != value) _obj_category = null;
				_Parent_id = value;
			}
		}
		public CategoryInfo Obj_category {
			get {
				if (_obj_category == null) _obj_category = Category.GetItem(_Parent_id.Value);
				return _obj_category;
			}
			internal set { _obj_category = value; }
		}
		/// <summary>
		/// 分类名称
		/// </summary>
		[JsonProperty] public string Title {
			get { return _Title; }
			set { _Title = value; }
		}
		private List<AreaInfo> _obj_areas;
		public List<AreaInfo> Obj_areas {
			get {
				if (_obj_areas == null) _obj_areas = Area.SelectByCategory_id(_Id.Value).ToList();
				return _obj_areas;
			}
		}
		private List<CategoryInfo> _obj_categorys;
		public List<CategoryInfo> Obj_categorys {
			get {
				if (_obj_categorys == null) _obj_categorys = Category.SelectByParent_id(_Id).Limit(500).ToList();
				return _obj_categorys;
			}
		}
		private List<MarkettypeInfo> _obj_markettypes;
		public List<MarkettypeInfo> Obj_markettypes {
			get {
				if (_obj_markettypes == null) _obj_markettypes = Markettype.SelectByCategory_id(_Id.Value).ToList();
				return _obj_markettypes;
			}
		}
		private List<PattrInfo> _obj_pattrs;
		public List<PattrInfo> Obj_pattrs {
			get {
				if (_obj_pattrs == null) _obj_pattrs = Pattr.SelectByCategory_id(_Id).Limit(500).ToList();
				return _obj_pattrs;
			}
		}
		private List<ProductInfo> _obj_products;
		public List<ProductInfo> Obj_products {
			get {
				if (_obj_products == null) _obj_products = Product.SelectByCategory_id(_Id).Limit(500).ToList();
				return _obj_products;
			}
		}
		#endregion

		public pifa.DAL.Category.SqlUpdateBuild UpdateDiy {
			get { return Category.UpdateDiy(this, _Id.Value); }
		}
		public CategoryInfo Save() {
			if (this.Id != null) {
				Category.Update(this);
				return this;
			}
			return Category.Insert(this);
		}
		public Area_categoryInfo FlagArea(AreaInfo Area) => FlagArea(Area.Id);
		public Area_categoryInfo FlagArea(uint? Area_id) {
			Area_categoryInfo item = Area_category.GetItem(Area_id.Value, this.Id.Value);
			if (item == null) item = Area_category.Insert(new Area_categoryInfo {
				Area_id = Area_id, 
			Category_id = this.Id});
			return item;
		}

		public int UnflagArea(AreaInfo Area) => UnflagArea(Area.Id);
		public int UnflagArea(uint? Area_id) => Area_category.Delete(Area_id.Value, this.Id.Value);
		public int UnflagAreaALL() => Area_category.DeleteByCategory_id(this.Id);

		public CategoryInfo AddCategory(string Title) => Category.Insert(new CategoryInfo {
			Parent_id = this.Id, 
				Title = Title});
		public CategoryInfo AddCategory(CategoryInfo item) {
			item.Parent_id = this.Id;
			return item.Save();
		}

		public Markettype_categoryInfo FlagMarkettype(MarkettypeInfo Markettype) => FlagMarkettype(Markettype.Id);
		public Markettype_categoryInfo FlagMarkettype(uint? Markettype_id) {
			Markettype_categoryInfo item = Markettype_category.GetItem(this.Id.Value, Markettype_id.Value);
			if (item == null) item = Markettype_category.Insert(new Markettype_categoryInfo {
			Category_id = this.Id, 
				Markettype_id = Markettype_id});
			return item;
		}

		public int UnflagMarkettype(MarkettypeInfo Markettype) => UnflagMarkettype(Markettype.Id);
		public int UnflagMarkettype(uint? Markettype_id) => Markettype_category.Delete(this.Id.Value, Markettype_id.Value);
		public int UnflagMarkettypeALL() => Markettype_category.DeleteByCategory_id(this.Id);

		public PattrInfo AddPattr(PattrInfo Pattr, bool? Is_filter, string Name) => AddPattr(Pattr.Id, Is_filter, Name);
		public PattrInfo AddPattr(uint? Parent_id, bool? Is_filter, string Name) => Pattr.Insert(new PattrInfo {
			Category_id = this.Id, 
				Parent_id = Parent_id, 
				Is_filter = Is_filter, 
				Name = Name});
		public PattrInfo AddPattr(PattrInfo item) {
			item.Category_id = this.Id;
			return item.Save();
		}

		public ProductInfo AddProduct(ShopInfo Shop, DateTime? Create_time, ProductICON? Icon, decimal? Price, uint? Stock, string Title, string Unit) => AddProduct(Shop.Id, Create_time, Icon, Price, Stock, Title, Unit);
		public ProductInfo AddProduct(uint? Shop_id, DateTime? Create_time, ProductICON? Icon, decimal? Price, uint? Stock, string Title, string Unit) => Product.Insert(new ProductInfo {
			Category_id = this.Id, 
				Shop_id = Shop_id, 
				Create_time = Create_time, 
				Icon = Icon, 
				Price = Price, 
				Stock = Stock, 
				Title = Title, 
				Unit = Unit});
		public ProductInfo AddProduct(ProductInfo item) {
			item.Category_id = this.Id;
			return item.Save();
		}

	}
}

