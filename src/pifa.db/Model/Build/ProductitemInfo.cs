using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using pifa.BLL;

namespace pifa.Model {

	[JsonObject(MemberSerialization.OptIn)]
	public partial class ProductitemInfo {
		#region fields
		private uint? _Id;
		private uint? _Product_id;
		private ProductInfo _obj_product;
		private string _Img_url;
		private string _Name;
		private decimal? _Original_price;
		private decimal? _Price;
		private uint? _Stock;
		#endregion

		public ProductitemInfo() { }

		#region 序列化，反序列化
		protected static readonly string StringifySplit = "@<Productitem(Info]?#>";
		public string Stringify() {
			return string.Concat(
				_Id == null ? "null" : _Id.ToString(), "|",
				_Product_id == null ? "null" : _Product_id.ToString(), "|",
				_Img_url == null ? "null" : _Img_url.Replace("|", StringifySplit), "|",
				_Name == null ? "null" : _Name.Replace("|", StringifySplit), "|",
				_Original_price == null ? "null" : _Original_price.ToString(), "|",
				_Price == null ? "null" : _Price.ToString(), "|",
				_Stock == null ? "null" : _Stock.ToString());
		}
		public static ProductitemInfo Parse(string stringify) {
			string[] ret = stringify.Split(new char[] { '|' }, 7, StringSplitOptions.None);
			if (ret.Length != 7) throw new Exception("格式不正确，ProductitemInfo：" + stringify);
			ProductitemInfo item = new ProductitemInfo();
			if (string.Compare("null", ret[0]) != 0) item.Id = uint.Parse(ret[0]);
			if (string.Compare("null", ret[1]) != 0) item.Product_id = uint.Parse(ret[1]);
			if (string.Compare("null", ret[2]) != 0) item.Img_url = ret[2].Replace(StringifySplit, "|");
			if (string.Compare("null", ret[3]) != 0) item.Name = ret[3].Replace(StringifySplit, "|");
			if (string.Compare("null", ret[4]) != 0) item.Original_price = decimal.Parse(ret[4]);
			if (string.Compare("null", ret[5]) != 0) item.Price = decimal.Parse(ret[5]);
			if (string.Compare("null", ret[6]) != 0) item.Stock = uint.Parse(ret[6]);
			return item;
		}
		#endregion

		#region override
		private static Lazy<Dictionary<string, bool>> __jsonIgnoreLazy = new Lazy<Dictionary<string, bool>>(() => {
			FieldInfo field = typeof(ProductitemInfo).GetField("JsonIgnore");
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
				__jsonIgnore.ContainsKey("Img_url") ? string.Empty : string.Format(", Img_url : {0}", Img_url == null ? "null" : string.Format("'{0}'", Img_url.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), 
				__jsonIgnore.ContainsKey("Name") ? string.Empty : string.Format(", Name : {0}", Name == null ? "null" : string.Format("'{0}'", Name.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), 
				__jsonIgnore.ContainsKey("Original_price") ? string.Empty : string.Format(", Original_price : {0}", Original_price == null ? "null" : Original_price.ToString()), 
				__jsonIgnore.ContainsKey("Price") ? string.Empty : string.Format(", Price : {0}", Price == null ? "null" : Price.ToString()), 
				__jsonIgnore.ContainsKey("Stock") ? string.Empty : string.Format(", Stock : {0}", Stock == null ? "null" : Stock.ToString()), " }");
			return string.Concat("{", json.Substring(1));
		}
		public IDictionary ToBson(bool allField = false) {
			IDictionary ht = new Hashtable();
			if (allField || !__jsonIgnore.ContainsKey("Id")) ht["Id"] = Id;
			if (allField || !__jsonIgnore.ContainsKey("Product_id")) ht["Product_id"] = Product_id;
			if (allField || !__jsonIgnore.ContainsKey("Img_url")) ht["Img_url"] = Img_url;
			if (allField || !__jsonIgnore.ContainsKey("Name")) ht["Name"] = Name;
			if (allField || !__jsonIgnore.ContainsKey("Original_price")) ht["Original_price"] = Original_price;
			if (allField || !__jsonIgnore.ContainsKey("Price")) ht["Price"] = Price;
			if (allField || !__jsonIgnore.ContainsKey("Stock")) ht["Stock"] = Stock;
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
		/// 图片
		/// </summary>
		[JsonProperty] public string Img_url {
			get { return _Img_url; }
			set { _Img_url = value; }
		}
		/// <summary>
		/// 产品项
		/// </summary>
		[JsonProperty] public string Name {
			get { return _Name; }
			set { _Name = value; }
		}
		/// <summary>
		/// 原价
		/// </summary>
		[JsonProperty] public decimal? Original_price {
			get { return _Original_price; }
			set { _Original_price = value; }
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
		public Order_productitemInfo Obj_order_productitem { set; get; }
		private List<OrderInfo> _obj_orders;
		/// <summary>
		/// 遍历时，可通过 Obj_order_productitem 可获取中间表数据
		/// </summary>
		public List<OrderInfo> Obj_orders {
			get {
				if (_obj_orders == null) _obj_orders = Order.Select.InnerJoin<Order_productitem>("b", "b.`order_id` = a.`id`").Where("b.`productitem_id` = {0}", _Id.Value).ToList();
				return _obj_orders;
			}
		}
		private List<Order_refundInfo> _obj_order_refunds;
		public List<Order_refundInfo> Obj_order_refunds {
			get {
				if (_obj_order_refunds == null) _obj_order_refunds = Order_refund.SelectByProductitem_id(_Id).Limit(500).ToList();
				return _obj_order_refunds;
			}
		}
		private List<Product_commentInfo> _obj_product_comments;
		public List<Product_commentInfo> Obj_product_comments {
			get {
				if (_obj_product_comments == null) _obj_product_comments = Product_comment.SelectByProductitem_id(_Id).Limit(500).ToList();
				return _obj_product_comments;
			}
		}
		#endregion

		public pifa.DAL.Productitem.SqlUpdateBuild UpdateDiy {
			get { return Productitem.UpdateDiy(this, _Id.Value); }
		}
		public ProductitemInfo Save() {
			if (this.Id != null) {
				Productitem.Update(this);
				return this;
			}
			return Productitem.Insert(this);
		}
		public Order_productitemInfo FlagOrder(OrderInfo Order, uint? Number, decimal? Price, Order_productitemSTATE? State, string Title) => FlagOrder(Order.Id, Number, Price, State, Title);
		public Order_productitemInfo FlagOrder(uint? Order_id, uint? Number, decimal? Price, Order_productitemSTATE? State, string Title) {
			Order_productitemInfo item = Order_productitem.GetItem(Order_id.Value, this.Id.Value);
			if (item == null) item = Order_productitem.Insert(new Order_productitemInfo {
				Order_id = Order_id, 
			Productitem_id = this.Id, 
				Number = Number, 
				Price = Price, 
				State = State, 
				Title = Title});
			else item.UpdateDiy
					.SetNumber(Number)
					.SetPrice(Price)
					.SetState(State)
					.SetTitle(Title).ExecuteNonQuery();
			return item;
		}

		public int UnflagOrder(OrderInfo Order) => UnflagOrder(Order.Id);
		public int UnflagOrder(uint? Order_id) => Order_productitem.Delete(Order_id.Value, this.Id.Value);
		public int UnflagOrderALL() => Order_productitem.DeleteByProductitem_id(this.Id);

		public Order_refundInfo AddOrder_refund(OrderInfo Order, DateTime? Create_time, string Descript, string Email, string Img_url, Order_refundSTATE? State, string Tel, string Telphone, decimal? Wealth) => AddOrder_refund(Order.Id, Create_time, Descript, Email, Img_url, State, Tel, Telphone, Wealth);
		public Order_refundInfo AddOrder_refund(uint? Order_id, DateTime? Create_time, string Descript, string Email, string Img_url, Order_refundSTATE? State, string Tel, string Telphone, decimal? Wealth) => Order_refund.Insert(new Order_refundInfo {
				Order_id = Order_id, 
			Productitem_id = this.Id, 
				Create_time = Create_time, 
				Descript = Descript, 
				Email = Email, 
				Img_url = Img_url, 
				State = State, 
				Tel = Tel, 
				Telphone = Telphone, 
				Wealth = Wealth});
		public Order_refundInfo AddOrder_refund(Order_refundInfo item) {
			item.Productitem_id = this.Id;
			return item.Save();
		}

		public Product_commentInfo AddProduct_comment(MemberInfo Member, OrderInfo Order, ProductInfo Product, string Content, DateTime? Create_time, string Nickname, byte? Star_price, byte? Star_quality, byte? Star_value, Product_commentSTATE? State, string Title, string Upload_image_url) => AddProduct_comment(Member.Id, Order.Id, Product.Id, Content, Create_time, Nickname, Star_price, Star_quality, Star_value, State, Title, Upload_image_url);
		public Product_commentInfo AddProduct_comment(uint? Member_id, uint? Order_id, uint? Product_id, string Content, DateTime? Create_time, string Nickname, byte? Star_price, byte? Star_quality, byte? Star_value, Product_commentSTATE? State, string Title, string Upload_image_url) => Product_comment.Insert(new Product_commentInfo {
				Member_id = Member_id, 
				Order_id = Order_id, 
				Product_id = Product_id, 
			Productitem_id = this.Id, 
				Content = Content, 
				Create_time = Create_time, 
				Nickname = Nickname, 
				Star_price = Star_price, 
				Star_quality = Star_quality, 
				Star_value = Star_value, 
				State = State, 
				Title = Title, 
				Upload_image_url = Upload_image_url});
		public Product_commentInfo AddProduct_comment(Product_commentInfo item) {
			item.Productitem_id = this.Id;
			return item.Save();
		}

	}
}

