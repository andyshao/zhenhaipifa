using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Reflection;
using Newtonsoft.Json;
using pifa.BLL;

namespace pifa.Model {

	[JsonObject(MemberSerialization.OptIn)]
	public partial class ProductInfo {
		#region fields
		private uint? _Id;
		private uint? _Category_id;
		private CategoryInfo _obj_category;
		private uint? _Shop_id;
		private ShopInfo _obj_shop;
		private DateTime? _Create_time;
		private ProductICON? _Icon;
		private decimal? _Price;
		private uint? _Stock;
		private string _Title;
		private string _Unit;
		#endregion

		public ProductInfo() { }

		#region 序列化，反序列化
		protected static readonly string StringifySplit = "@<Product(Info]?#>";
		public string Stringify() {
			return string.Concat(
				_Id == null ? "null" : _Id.ToString(), "|",
				_Category_id == null ? "null" : _Category_id.ToString(), "|",
				_Shop_id == null ? "null" : _Shop_id.ToString(), "|",
				_Create_time == null ? "null" : _Create_time.Value.Ticks.ToString(), "|",
				_Icon == null ? "null" : _Icon.ToInt64().ToString(), "|",
				_Price == null ? "null" : _Price.ToString(), "|",
				_Stock == null ? "null" : _Stock.ToString(), "|",
				_Title == null ? "null" : _Title.Replace("|", StringifySplit), "|",
				_Unit == null ? "null" : _Unit.Replace("|", StringifySplit));
		}
		public static ProductInfo Parse(string stringify) {
			string[] ret = stringify.Split(new char[] { '|' }, 9, StringSplitOptions.None);
			if (ret.Length != 9) throw new Exception("格式不正确，ProductInfo：" + stringify);
			ProductInfo item = new ProductInfo();
			if (string.Compare("null", ret[0]) != 0) item.Id = uint.Parse(ret[0]);
			if (string.Compare("null", ret[1]) != 0) item.Category_id = uint.Parse(ret[1]);
			if (string.Compare("null", ret[2]) != 0) item.Shop_id = uint.Parse(ret[2]);
			if (string.Compare("null", ret[3]) != 0) item.Create_time = new DateTime(long.Parse(ret[3]));
			if (string.Compare("null", ret[4]) != 0) item.Icon = (ProductICON)long.Parse(ret[4]);
			if (string.Compare("null", ret[5]) != 0) item.Price = decimal.Parse(ret[5]);
			if (string.Compare("null", ret[6]) != 0) item.Stock = uint.Parse(ret[6]);
			if (string.Compare("null", ret[7]) != 0) item.Title = ret[7].Replace(StringifySplit, "|");
			if (string.Compare("null", ret[8]) != 0) item.Unit = ret[8].Replace(StringifySplit, "|");
			return item;
		}
		#endregion

		#region override
		private static Lazy<Dictionary<string, bool>> __jsonIgnoreLazy = new Lazy<Dictionary<string, bool>>(() => {
			FieldInfo field = typeof(ProductInfo).GetField("JsonIgnore");
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
				__jsonIgnore.ContainsKey("Category_id") ? string.Empty : string.Format(", Category_id : {0}", Category_id == null ? "null" : Category_id.ToString()), 
				__jsonIgnore.ContainsKey("Shop_id") ? string.Empty : string.Format(", Shop_id : {0}", Shop_id == null ? "null" : Shop_id.ToString()), 
				__jsonIgnore.ContainsKey("Create_time") ? string.Empty : string.Format(", Create_time : {0}", Create_time == null ? "null" : Create_time.Value.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds.ToString()), 
				__jsonIgnore.ContainsKey("Icon") ? string.Empty : string.Format(", Icon : {0}", Icon == null ? "null" : string.Format("[ '{0}' ]", string.Join("', '", Icon.ToInt64().ToSet<ProductICON>().Select<ProductICON, string>(a => a.ToDescriptionOrString().Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))))), 
				__jsonIgnore.ContainsKey("Price") ? string.Empty : string.Format(", Price : {0}", Price == null ? "null" : Price.ToString()), 
				__jsonIgnore.ContainsKey("Stock") ? string.Empty : string.Format(", Stock : {0}", Stock == null ? "null" : Stock.ToString()), 
				__jsonIgnore.ContainsKey("Title") ? string.Empty : string.Format(", Title : {0}", Title == null ? "null" : string.Format("'{0}'", Title.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), 
				__jsonIgnore.ContainsKey("Unit") ? string.Empty : string.Format(", Unit : {0}", Unit == null ? "null" : string.Format("'{0}'", Unit.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), " }");
			return string.Concat("{", json.Substring(1));
		}
		public IDictionary ToBson(bool allField = false) {
			IDictionary ht = new Hashtable();
			if (allField || !__jsonIgnore.ContainsKey("Id")) ht["Id"] = Id;
			if (allField || !__jsonIgnore.ContainsKey("Category_id")) ht["Category_id"] = Category_id;
			if (allField || !__jsonIgnore.ContainsKey("Shop_id")) ht["Shop_id"] = Shop_id;
			if (allField || !__jsonIgnore.ContainsKey("Create_time")) ht["Create_time"] = Create_time;
			if (allField || !__jsonIgnore.ContainsKey("Icon")) ht["Icon"] = Icon?.ToInt64().ToSet<ProductICON>().Select(a => a.ToDescriptionOrString());
			if (allField || !__jsonIgnore.ContainsKey("Price")) ht["Price"] = Price;
			if (allField || !__jsonIgnore.ContainsKey("Stock")) ht["Stock"] = Stock;
			if (allField || !__jsonIgnore.ContainsKey("Title")) ht["Title"] = Title;
			if (allField || !__jsonIgnore.ContainsKey("Unit")) ht["Unit"] = Unit;
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
		/// 分类
		/// </summary>
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
		/// <summary>
		/// 店铺
		/// </summary>
		[JsonProperty] public uint? Shop_id {
			get { return _Shop_id; }
			set {
				if (_Shop_id != value) _obj_shop = null;
				_Shop_id = value;
			}
		}
		public ShopInfo Obj_shop {
			get {
				if (_obj_shop == null) _obj_shop = Shop.GetItem(_Shop_id.Value);
				return _obj_shop;
			}
			internal set { _obj_shop = value; }
		}
		/// <summary>
		/// 创建时间
		/// </summary>
		[JsonProperty] public DateTime? Create_time {
			get { return _Create_time; }
			set { _Create_time = value; }
		}
		/// <summary>
		/// 点亮图标
		/// </summary>
		[JsonProperty] public ProductICON? Icon {
			get { return _Icon; }
			set { _Icon = value; }
		}
		/// <summary>
		/// 价格
		/// </summary>
		[JsonProperty] public decimal? Price {
			get { return _Price; }
			set { _Price = value; }
		}
		/// <summary>
		/// 库存
		/// </summary>
		[JsonProperty] public uint? Stock {
			get { return _Stock; }
			set { _Stock = value; }
		}
		/// <summary>
		/// 产品名称
		/// </summary>
		[JsonProperty] public string Title {
			get { return _Title; }
			set { _Title = value; }
		}
		/// <summary>
		/// 单位
		/// </summary>
		[JsonProperty] public string Unit {
			get { return _Unit; }
			set { _Unit = value; }
		}
		public Member_productInfo Obj_member_product { set; get; }
		private List<MemberInfo> _obj_members;
		/// <summary>
		/// 遍历时，可通过 Obj_member_product 可获取中间表数据
		/// </summary>
		public List<MemberInfo> Obj_members {
			get {
				if (_obj_members == null) _obj_members = Member.Select.InnerJoin<Member_product>("b", "b.`member_id` = a.`id`").Where("b.`product_id` = {0}", _Id.Value).ToList();
				return _obj_members;
			}
		}
		public Product_attrInfo Obj_product_attr { set; get; }
		private List<PattrInfo> _obj_attrs;
		/// <summary>
		/// 遍历时，可通过 Obj_product_attr 可获取中间表数据
		/// </summary>
		public List<PattrInfo> Obj_attrs {
			get {
				if (_obj_attrs == null) _obj_attrs = Pattr.Select.InnerJoin<Product_attr>("b", "b.`pattr_id` = a.`id`").Where("b.`product_id` = {0}", _Id.Value).ToList();
				return _obj_attrs;
			}
		}
		private List<Product_buyruleInfo> _obj_product_buyrules;
		public List<Product_buyruleInfo> Obj_product_buyrules {
			get {
				if (_obj_product_buyrules == null) _obj_product_buyrules = Product_buyrule.SelectByProduct_id(_Id).Limit(500).ToList();
				return _obj_product_buyrules;
			}
		}
		private List<Product_commentInfo> _obj_product_comments;
		public List<Product_commentInfo> Obj_product_comments {
			get {
				if (_obj_product_comments == null) _obj_product_comments = Product_comment.SelectByProduct_id(_Id).Limit(500).ToList();
				return _obj_product_comments;
			}
		}
		private List<Product_questionInfo> _obj_product_questions;
		public List<Product_questionInfo> Obj_product_questions {
			get {
				if (_obj_product_questions == null) _obj_product_questions = Product_question.SelectByProduct_id(_Id).Limit(500).ToList();
				return _obj_product_questions;
			}
		}
		private List<ProductdescInfo> _obj_productdescs;
		public List<ProductdescInfo> Obj_productdescs {
			get {
				if (_obj_productdescs == null) _obj_productdescs = Productdesc.SelectByProduct_id(_Id).Limit(500).ToList();
				return _obj_productdescs;
			}
		}
		private List<ProductitemInfo> _obj_productitems;
		public List<ProductitemInfo> Obj_productitems {
			get {
				if (_obj_productitems == null) _obj_productitems = Productitem.SelectByProduct_id(_Id).Limit(500).ToList();
				return _obj_productitems;
			}
		}
		#endregion

		public pifa.DAL.Product.SqlUpdateBuild UpdateDiy {
			get { return Product.UpdateDiy(this, _Id.Value); }
		}
		public ProductInfo Save() {
			if (this.Id != null) {
				Product.Update(this);
				return this;
			}
			this.Create_time = DateTime.Now;
			return Product.Insert(this);
		}
		public Member_productInfo FlagMember(MemberInfo Member, DateTime? Create_time) => FlagMember(Member.Id, Create_time);
		public Member_productInfo FlagMember(uint? Member_id, DateTime? Create_time) {
			Member_productInfo item = Member_product.GetItem(Member_id.Value, this.Id.Value);
			if (item == null) item = Member_product.Insert(new Member_productInfo {
				Member_id = Member_id, 
			Product_id = this.Id, 
				Create_time = Create_time});
			else item.UpdateDiy
					.SetCreate_time(Create_time).ExecuteNonQuery();
			return item;
		}

		public int UnflagMember(MemberInfo Member) => UnflagMember(Member.Id);
		public int UnflagMember(uint? Member_id) => Member_product.Delete(Member_id.Value, this.Id.Value);
		public int UnflagMemberALL() => Member_product.DeleteByProduct_id(this.Id);

		public Product_attrInfo FlagAttr(PattrInfo Pattr, string Value) => FlagAttr(Pattr.Id, Value);
		public Product_attrInfo FlagAttr(uint? Pattr_id, string Value) {
			Product_attrInfo item = Product_attr.GetItem(Pattr_id.Value, this.Id.Value);
			if (item == null) item = Product_attr.Insert(new Product_attrInfo {
				Pattr_id = Pattr_id, 
			Product_id = this.Id, 
				Value = Value});
			else item.UpdateDiy
					.SetValue(Value).ExecuteNonQuery();
			return item;
		}

		public int UnflagAttr(PattrInfo Pattr) => UnflagAttr(Pattr.Id);
		public int UnflagAttr(uint? Pattr_id) => Product_attr.Delete(Pattr_id.Value, this.Id.Value);
		public int UnflagAttrALL() => Product_attr.DeleteByProduct_id(this.Id);

		public Product_buyruleInfo AddBuyrule(uint? Discount, uint? Ordering_end, uint? Ordering_start) => Product_buyrule.Insert(new Product_buyruleInfo {
			Product_id = this.Id, 
				Discount = Discount, 
				Ordering_end = Ordering_end, 
				Ordering_start = Ordering_start});
		public Product_buyruleInfo AddBuyrule(Product_buyruleInfo item) {
			item.Product_id = this.Id;
			return item.Save();
		}

		public Product_commentInfo AddComment(MemberInfo Member, OrderInfo Order, ProductitemInfo Productitem, string Content, DateTime? Create_time, string Nickname, byte? Star_price, byte? Star_quality, byte? Star_value, Product_commentSTATE? State, string Title, string Upload_image_url) => AddComment(Member.Id, Order.Id, Productitem.Id, Content, Create_time, Nickname, Star_price, Star_quality, Star_value, State, Title, Upload_image_url);
		public Product_commentInfo AddComment(uint? Member_id, uint? Order_id, uint? Productitem_id, string Content, DateTime? Create_time, string Nickname, byte? Star_price, byte? Star_quality, byte? Star_value, Product_commentSTATE? State, string Title, string Upload_image_url) => Product_comment.Insert(new Product_commentInfo {
				Member_id = Member_id, 
				Order_id = Order_id, 
			Product_id = this.Id, 
				Productitem_id = Productitem_id, 
				Content = Content, 
				Create_time = Create_time, 
				Nickname = Nickname, 
				Star_price = Star_price, 
				Star_quality = Star_quality, 
				Star_value = Star_value, 
				State = State, 
				Title = Title, 
				Upload_image_url = Upload_image_url});
		public Product_commentInfo AddComment(Product_commentInfo item) {
			item.Product_id = this.Id;
			return item.Save();
		}

		public Product_questionInfo AddQuestion(MemberInfo Member, Product_questionInfo Product_question, string Content, DateTime? Create_time, string Email, string Name, Product_questionSTATE? State) => AddQuestion(Member.Id, Product_question.Id, Content, Create_time, Email, Name, State);
		public Product_questionInfo AddQuestion(uint? Member_id, uint? Parent_id, string Content, DateTime? Create_time, string Email, string Name, Product_questionSTATE? State) => Product_question.Insert(new Product_questionInfo {
				Member_id = Member_id, 
				Parent_id = Parent_id, 
			Product_id = this.Id, 
				Content = Content, 
				Create_time = Create_time, 
				Email = Email, 
				Name = Name, 
				State = State});
		public Product_questionInfo AddQuestion(Product_questionInfo item) {
			item.Product_id = this.Id;
			return item.Save();
		}

		public ProductdescInfo AddProductdesc(string Content) => Productdesc.Insert(new ProductdescInfo {
			Product_id = this.Id, 
				Content = Content});
		public ProductdescInfo AddProductdesc(ProductdescInfo item) {
			item.Product_id = this.Id;
			return item.Save();
		}

		public ProductitemInfo AddProductitem(string Img_url, string Name, decimal? Original_price, decimal? Price, uint? Stock) => Productitem.Insert(new ProductitemInfo {
			Product_id = this.Id, 
				Img_url = Img_url, 
				Name = Name, 
				Original_price = Original_price, 
				Price = Price, 
				Stock = Stock});
		public ProductitemInfo AddProductitem(ProductitemInfo item) {
			item.Product_id = this.Id;
			return item.Save();
		}

	}
	[Flags]
	public enum ProductICON : long {
		/// <summary>
		/// ????
		/// </summary>
		[Description("????")]
		Unknow1 = 1, 
		/// <summary>
		/// ??
		/// </summary>
		[Description("??")]
		Unknow2 = 2, 
		/// <summary>
		/// 7??????
		/// </summary>
		[Description("7??????")]
		Unknow3 = 4, 
		/// <summary>
		/// 24?????
		/// </summary>
		[Description("24?????")]
		Unknow4 = 8
	}
}

