using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using pifa.BLL;

namespace pifa.Model {

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

		#region 独创的序列化，反序列化
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
		public ProductitemInfo(string stringify) {
			string[] ret = stringify.Split(new char[] { '|' }, 7, StringSplitOptions.None);
			if (ret.Length != 7) throw new Exception("格式不正确，ProductitemInfo：" + stringify);
			if (string.Compare("null", ret[0]) != 0) _Id = uint.Parse(ret[0]);
			if (string.Compare("null", ret[1]) != 0) _Product_id = uint.Parse(ret[1]);
			if (string.Compare("null", ret[2]) != 0) _Img_url = ret[2].Replace(StringifySplit, "|");
			if (string.Compare("null", ret[3]) != 0) _Name = ret[3].Replace(StringifySplit, "|");
			if (string.Compare("null", ret[4]) != 0) _Original_price = decimal.Parse(ret[4]);
			if (string.Compare("null", ret[5]) != 0) _Price = decimal.Parse(ret[5]);
			if (string.Compare("null", ret[6]) != 0) _Stock = uint.Parse(ret[6]);
		}
		#endregion

		#region override
		private static Dictionary<string, bool> __jsonIgnore;
		private static object __jsonIgnore_lock = new object();
		public override string ToString() {
			this.Init__jsonIgnore();
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
		public IDictionary ToBson() {
			this.Init__jsonIgnore();
			IDictionary ht = new Hashtable();
			if (!__jsonIgnore.ContainsKey("Id")) ht["Id"] = Id;
			if (!__jsonIgnore.ContainsKey("Product_id")) ht["Product_id"] = Product_id;
			if (!__jsonIgnore.ContainsKey("Img_url")) ht["Img_url"] = Img_url;
			if (!__jsonIgnore.ContainsKey("Name")) ht["Name"] = Name;
			if (!__jsonIgnore.ContainsKey("Original_price")) ht["Original_price"] = Original_price;
			if (!__jsonIgnore.ContainsKey("Price")) ht["Price"] = Price;
			if (!__jsonIgnore.ContainsKey("Stock")) ht["Stock"] = Stock;
			return ht;
		}
		private void Init__jsonIgnore() {
			if (__jsonIgnore == null) {
				lock (__jsonIgnore_lock) {
					if (__jsonIgnore == null) {
						FieldInfo field = typeof(ProductitemInfo).GetField("JsonIgnore");
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
			ProductitemInfo item = obj as ProductitemInfo;
			if (item == null) return false;
			return this.ToString().Equals(item.ToString());
		}
		public override int GetHashCode() {
			return this.ToString().GetHashCode();
		}
		public static bool operator ==(ProductitemInfo op1, ProductitemInfo op2) {
			if (object.Equals(op1, null)) return object.Equals(op2, null);
			return op1.Equals(op2);
		}
		public static bool operator !=(ProductitemInfo op1, ProductitemInfo op2) {
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
		/// 产品
		/// </summary>
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
		/// 图片
		/// </summary>
		public string Img_url {
			get { return _Img_url; }
			set { _Img_url = value; }
		}
		/// <summary>
		/// 产品项
		/// </summary>
		public string Name {
			get { return _Name; }
			set { _Name = value; }
		}
		/// <summary>
		/// 原价
		/// </summary>
		public decimal? Original_price {
			get { return _Original_price; }
			set { _Original_price = value; }
		}
		/// <summary>
		/// 价格
		/// </summary>
		public decimal? Price {
			get { return _Price; }
			set { _Price = value; }
		}
		/// <summary>
		/// 库存
		/// </summary>
		public uint? Stock {
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
			get { return Productitem.UpdateDiy(this, _Id); }
		}
		public Order_productitemInfo FlagOrder(OrderInfo Order, uint? Number, decimal? Price, Order_productitemSTATE? State, string Title) {
			return FlagOrder(Order.Id, Number, Price, State, Title);
		}
		public Order_productitemInfo FlagOrder(uint? Order_id, uint? Number, decimal? Price, Order_productitemSTATE? State, string Title) {
			Order_productitemInfo item = Order_productitem.GetItem(Order_id, this.Id);
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

		public int UnflagOrder(OrderInfo Order) {
			return UnflagOrder(Order.Id);
		}
		public int UnflagOrder(uint? Order_id) {
			return Order_productitem.Delete(Order_id, this.Id);
		}
		public int UnflagOrderALL() {
			return Order_productitem.DeleteByProductitem_id(this.Id);
		}

		public Order_refundInfo AddOrder_refund(OrderInfo Order, DateTime? Create_time, string Descript, string Email, string Img_url, Order_refundSTATE? State, string Tel, string Telphone, decimal? Wealth) {
			return AddOrder_refund(Order.Id, Create_time, Descript, Email, Img_url, State, Tel, Telphone, Wealth);
		}
		public Order_refundInfo AddOrder_refund(uint? Order_id, DateTime? Create_time, string Descript, string Email, string Img_url, Order_refundSTATE? State, string Tel, string Telphone, decimal? Wealth) {
			return Order_refund.Insert(new Order_refundInfo {
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
		}

		public Product_commentInfo AddProduct_comment(MemberInfo Member, OrderInfo Order, ProductInfo Product, string Content, DateTime? Create_time, string Nickname, byte? Star_price, byte? Star_quality, byte? Star_value, Product_commentSTATE? State, string Title, string Upload_image_url) {
			return AddProduct_comment(Member.Id, Order.Id, Product.Id, Content, Create_time, Nickname, Star_price, Star_quality, Star_value, State, Title, Upload_image_url);
		}
		public Product_commentInfo AddProduct_comment(uint? Member_id, uint? Order_id, uint? Product_id, string Content, DateTime? Create_time, string Nickname, byte? Star_price, byte? Star_quality, byte? Star_value, Product_commentSTATE? State, string Title, string Upload_image_url) {
			return Product_comment.Insert(new Product_commentInfo {
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
		}

	}
}

