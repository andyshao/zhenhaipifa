using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using pifa.BLL;

namespace pifa.Model {

	[JsonObject(MemberSerialization.OptIn)]
	public partial class MemberInfo {
		#region fields
		private uint? _Id;
		private DateTime? _Create_time;
		private string _Email;
		private DateTime? _Lastlogin_time;
		private string _Telphone;
		private string _Username;
		#endregion

		public MemberInfo() { }

		#region 序列化，反序列化
		protected static readonly string StringifySplit = "@<Member(Info]?#>";
		public string Stringify() {
			return string.Concat(
				_Id == null ? "null" : _Id.ToString(), "|",
				_Create_time == null ? "null" : _Create_time.Value.Ticks.ToString(), "|",
				_Email == null ? "null" : _Email.Replace("|", StringifySplit), "|",
				_Lastlogin_time == null ? "null" : _Lastlogin_time.Value.Ticks.ToString(), "|",
				_Telphone == null ? "null" : _Telphone.Replace("|", StringifySplit), "|",
				_Username == null ? "null" : _Username.Replace("|", StringifySplit));
		}
		public static MemberInfo Parse(string stringify) {
			string[] ret = stringify.Split(new char[] { '|' }, 6, StringSplitOptions.None);
			if (ret.Length != 6) throw new Exception("格式不正确，MemberInfo：" + stringify);
			MemberInfo item = new MemberInfo();
			if (string.Compare("null", ret[0]) != 0) item.Id = uint.Parse(ret[0]);
			if (string.Compare("null", ret[1]) != 0) item.Create_time = new DateTime(long.Parse(ret[1]));
			if (string.Compare("null", ret[2]) != 0) item.Email = ret[2].Replace(StringifySplit, "|");
			if (string.Compare("null", ret[3]) != 0) item.Lastlogin_time = new DateTime(long.Parse(ret[3]));
			if (string.Compare("null", ret[4]) != 0) item.Telphone = ret[4].Replace(StringifySplit, "|");
			if (string.Compare("null", ret[5]) != 0) item.Username = ret[5].Replace(StringifySplit, "|");
			return item;
		}
		#endregion

		#region override
		private static Lazy<Dictionary<string, bool>> __jsonIgnoreLazy = new Lazy<Dictionary<string, bool>>(() => {
			FieldInfo field = typeof(MemberInfo).GetField("JsonIgnore");
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
				__jsonIgnore.ContainsKey("Create_time") ? string.Empty : string.Format(", Create_time : {0}", Create_time == null ? "null" : Create_time.Value.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds.ToString()), 
				__jsonIgnore.ContainsKey("Email") ? string.Empty : string.Format(", Email : {0}", Email == null ? "null" : string.Format("'{0}'", Email.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), 
				__jsonIgnore.ContainsKey("Lastlogin_time") ? string.Empty : string.Format(", Lastlogin_time : {0}", Lastlogin_time == null ? "null" : Lastlogin_time.Value.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds.ToString()), 
				__jsonIgnore.ContainsKey("Telphone") ? string.Empty : string.Format(", Telphone : {0}", Telphone == null ? "null" : string.Format("'{0}'", Telphone.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), 
				__jsonIgnore.ContainsKey("Username") ? string.Empty : string.Format(", Username : {0}", Username == null ? "null" : string.Format("'{0}'", Username.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), " }");
			return string.Concat("{", json.Substring(1));
		}
		public IDictionary ToBson(bool allField = false) {
			IDictionary ht = new Hashtable();
			if (allField || !__jsonIgnore.ContainsKey("Id")) ht["Id"] = Id;
			if (allField || !__jsonIgnore.ContainsKey("Create_time")) ht["Create_time"] = Create_time;
			if (allField || !__jsonIgnore.ContainsKey("Email")) ht["Email"] = Email;
			if (allField || !__jsonIgnore.ContainsKey("Lastlogin_time")) ht["Lastlogin_time"] = Lastlogin_time;
			if (allField || !__jsonIgnore.ContainsKey("Telphone")) ht["Telphone"] = Telphone;
			if (allField || !__jsonIgnore.ContainsKey("Username")) ht["Username"] = Username;
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
		/// 创建时间
		/// </summary>
		[JsonProperty] public DateTime? Create_time {
			get { return _Create_time; }
			set { _Create_time = value; }
		}
		/// <summary>
		/// 邮箱
		/// </summary>
		[JsonProperty] public string Email {
			get { return _Email; }
			set { _Email = value; }
		}
		/// <summary>
		/// 最后登陆时间
		/// </summary>
		[JsonProperty] public DateTime? Lastlogin_time {
			get { return _Lastlogin_time; }
			set { _Lastlogin_time = value; }
		}
		/// <summary>
		/// 手机
		/// </summary>
		[JsonProperty] public string Telphone {
			get { return _Telphone; }
			set { _Telphone = value; }
		}
		/// <summary>
		/// 用户名
		/// </summary>
		[JsonProperty] public string Username {
			get { return _Username; }
			set { _Username = value; }
		}
		private List<Member_addressbookInfo> _obj_member_addressbooks;
		public List<Member_addressbookInfo> Obj_member_addressbooks {
			get {
				if (_obj_member_addressbooks == null) _obj_member_addressbooks = Member_addressbook.SelectByMember_id(_Id).Limit(500).ToList();
				return _obj_member_addressbooks;
			}
		}
		public Member_marketInfo Obj_member_market { set; get; }
		private List<MarketInfo> _obj_markets;
		/// <summary>
		/// 遍历时，可通过 Obj_member_market 可获取中间表数据
		/// </summary>
		public List<MarketInfo> Obj_markets {
			get {
				if (_obj_markets == null) _obj_markets = Market.Select.InnerJoin<Member_market>("b", "b.`market_id` = a.`id`").Where("b.`member_id` = {0}", _Id.Value).ToList();
				return _obj_markets;
			}
		}
		public Member_productInfo Obj_member_product { set; get; }
		private List<ProductInfo> _obj_products;
		/// <summary>
		/// 遍历时，可通过 Obj_member_product 可获取中间表数据
		/// </summary>
		public List<ProductInfo> Obj_products {
			get {
				if (_obj_products == null) _obj_products = Product.Select.InnerJoin<Member_product>("b", "b.`product_id` = a.`id`").Where("b.`member_id` = {0}", _Id.Value).ToList();
				return _obj_products;
			}
		}
		private List<Member_securityInfo> _obj_member_securitys;
		public List<Member_securityInfo> Obj_member_securitys {
			get {
				if (_obj_member_securitys == null) _obj_member_securitys = Member_security.SelectByMember_id(_Id).Limit(500).ToList();
				return _obj_member_securitys;
			}
		}
		public Member_shopInfo Obj_member_shop { set; get; }
		private List<ShopInfo> _obj_shops;
		/// <summary>
		/// 遍历时，可通过 Obj_member_shop 可获取中间表数据
		/// </summary>
		public List<ShopInfo> Obj_shops {
			get {
				if (_obj_shops == null) _obj_shops = Shop.Select.InnerJoin<Member_shop>("b", "b.`shop_id` = a.`id`").Where("b.`member_id` = {0}", _Id.Value).ToList();
				return _obj_shops;
			}
		}
		private List<OrderInfo> _obj_orders;
		public List<OrderInfo> Obj_orders {
			get {
				if (_obj_orders == null) _obj_orders = Order.SelectByMember_id(_Id).Limit(500).ToList();
				return _obj_orders;
			}
		}
		private List<Product_commentInfo> _obj_product_comments;
		public List<Product_commentInfo> Obj_product_comments {
			get {
				if (_obj_product_comments == null) _obj_product_comments = Product_comment.SelectByMember_id(_Id).Limit(500).ToList();
				return _obj_product_comments;
			}
		}
		private List<Product_questionInfo> _obj_product_questions;
		public List<Product_questionInfo> Obj_product_questions {
			get {
				if (_obj_product_questions == null) _obj_product_questions = Product_question.SelectByMember_id(_Id).Limit(500).ToList();
				return _obj_product_questions;
			}
		}
		#endregion

		public pifa.DAL.Member.SqlUpdateBuild UpdateDiy {
			get { return Member.UpdateDiy(this, _Id.Value); }
		}
		public MemberInfo Save() {
			if (this.Id != null) {
				Member.Update(this);
				return this;
			}
			this.Create_time = DateTime.Now;
			return Member.Insert(this);
		}
		public Member_addressbookInfo AddAddressbook(string Address, DateTime? Create_time, bool? Is_default, string Name, string Tel, string Telphone, string Zip) => Member_addressbook.Insert(new Member_addressbookInfo {
			Member_id = this.Id, 
				Address = Address, 
				Create_time = Create_time, 
				Is_default = Is_default, 
				Name = Name, 
				Tel = Tel, 
				Telphone = Telphone, 
				Zip = Zip});
		public Member_addressbookInfo AddAddressbook(Member_addressbookInfo item) {
			item.Member_id = this.Id;
			return item.Save();
		}

		public Member_marketInfo FlagMarket(MarketInfo Market, DateTime? Create_time) => FlagMarket(Market.Id, Create_time);
		public Member_marketInfo FlagMarket(uint? Market_id, DateTime? Create_time) {
			Member_marketInfo item = Member_market.GetItem(Market_id.Value, this.Id.Value);
			if (item == null) item = Member_market.Insert(new Member_marketInfo {
				Market_id = Market_id, 
			Member_id = this.Id, 
				Create_time = Create_time});
			else item.UpdateDiy
					.SetCreate_time(Create_time).ExecuteNonQuery();
			return item;
		}

		public int UnflagMarket(MarketInfo Market) => UnflagMarket(Market.Id);
		public int UnflagMarket(uint? Market_id) => Member_market.Delete(Market_id.Value, this.Id.Value);
		public int UnflagMarketALL() => Member_market.DeleteByMember_id(this.Id);

		public Member_productInfo FlagProduct(ProductInfo Product, DateTime? Create_time) => FlagProduct(Product.Id, Create_time);
		public Member_productInfo FlagProduct(uint? Product_id, DateTime? Create_time) {
			Member_productInfo item = Member_product.GetItem(this.Id.Value, Product_id.Value);
			if (item == null) item = Member_product.Insert(new Member_productInfo {
			Member_id = this.Id, 
				Product_id = Product_id, 
				Create_time = Create_time});
			else item.UpdateDiy
					.SetCreate_time(Create_time).ExecuteNonQuery();
			return item;
		}

		public int UnflagProduct(ProductInfo Product) => UnflagProduct(Product.Id);
		public int UnflagProduct(uint? Product_id) => Member_product.Delete(this.Id.Value, Product_id.Value);
		public int UnflagProductALL() => Member_product.DeleteByMember_id(this.Id);

		public Member_securityInfo AddSecurity(string Password) => Member_security.Insert(new Member_securityInfo {
			Member_id = this.Id, 
				Password = Password});
		public Member_securityInfo AddSecurity(Member_securityInfo item) {
			item.Member_id = this.Id;
			return item.Save();
		}

		public Member_shopInfo FlagShop(ShopInfo Shop, DateTime? Create_time) => FlagShop(Shop.Id, Create_time);
		public Member_shopInfo FlagShop(uint? Shop_id, DateTime? Create_time) {
			Member_shopInfo item = Member_shop.GetItem(this.Id.Value, Shop_id.Value);
			if (item == null) item = Member_shop.Insert(new Member_shopInfo {
			Member_id = this.Id, 
				Shop_id = Shop_id, 
				Create_time = Create_time});
			else item.UpdateDiy
					.SetCreate_time(Create_time).ExecuteNonQuery();
			return item;
		}

		public int UnflagShop(ShopInfo Shop) => UnflagShop(Shop.Id);
		public int UnflagShop(uint? Shop_id) => Member_shop.Delete(this.Id.Value, Shop_id.Value);
		public int UnflagShopALL() => Member_shop.DeleteByMember_id(this.Id);

		public OrderInfo AddOrder(string Code, DateTime? Create_time, string Express_code, string Express_name, string Paymethod, string Remark, OrderSTATE? State, decimal? Total_express_price, decimal? Total_original_price, decimal? Total_price, DateTime? Update_time) => Order.Insert(new OrderInfo {
			Member_id = this.Id, 
				Code = Code, 
				Create_time = Create_time, 
				Express_code = Express_code, 
				Express_name = Express_name, 
				Paymethod = Paymethod, 
				Remark = Remark, 
				State = State, 
				Total_express_price = Total_express_price, 
				Total_original_price = Total_original_price, 
				Total_price = Total_price, 
				Update_time = Update_time});
		public OrderInfo AddOrder(OrderInfo item) {
			item.Member_id = this.Id;
			return item.Save();
		}

		public Product_commentInfo AddProduct_comment(OrderInfo Order, ProductInfo Product, ProductitemInfo Productitem, string Content, DateTime? Create_time, string Nickname, byte? Star_price, byte? Star_quality, byte? Star_value, Product_commentSTATE? State, string Title, string Upload_image_url) => AddProduct_comment(Order.Id, Product.Id, Productitem.Id, Content, Create_time, Nickname, Star_price, Star_quality, Star_value, State, Title, Upload_image_url);
		public Product_commentInfo AddProduct_comment(uint? Order_id, uint? Product_id, uint? Productitem_id, string Content, DateTime? Create_time, string Nickname, byte? Star_price, byte? Star_quality, byte? Star_value, Product_commentSTATE? State, string Title, string Upload_image_url) => Product_comment.Insert(new Product_commentInfo {
			Member_id = this.Id, 
				Order_id = Order_id, 
				Product_id = Product_id, 
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
		public Product_commentInfo AddProduct_comment(Product_commentInfo item) {
			item.Member_id = this.Id;
			return item.Save();
		}

		public Product_questionInfo AddProduct_question(Product_questionInfo Product_question, ProductInfo Product, string Content, DateTime? Create_time, string Email, string Name, Product_questionSTATE? State) => AddProduct_question(Product_question.Id, Product.Id, Content, Create_time, Email, Name, State);
		public Product_questionInfo AddProduct_question(uint? Parent_id, uint? Product_id, string Content, DateTime? Create_time, string Email, string Name, Product_questionSTATE? State) => Product_question.Insert(new Product_questionInfo {
			Member_id = this.Id, 
				Parent_id = Parent_id, 
				Product_id = Product_id, 
				Content = Content, 
				Create_time = Create_time, 
				Email = Email, 
				Name = Name, 
				State = State});
		public Product_questionInfo AddProduct_question(Product_questionInfo item) {
			item.Member_id = this.Id;
			return item.Save();
		}

		public ShopInfo AddShop(MarkettypeInfo Markettype, string Address, decimal? Area, string Code, DateTime? Create_time, string Fax, ShopFUNC_SWITCH? Func_switch, ShopICON? Icon, string Kefu, string Main_business, string Nickname, ShopSTATE? State, string Title) => AddShop(Markettype.Id, Address, Area, Code, Create_time, Fax, Func_switch, Icon, Kefu, Main_business, Nickname, State, Title);
		public ShopInfo AddShop(uint? Markettype_id, string Address, decimal? Area, string Code, DateTime? Create_time, string Fax, ShopFUNC_SWITCH? Func_switch, ShopICON? Icon, string Kefu, string Main_business, string Nickname, ShopSTATE? State, string Title) => Shop.Insert(new ShopInfo {
				Markettype_id = Markettype_id, 
			Member_id = this.Id, 
				Address = Address, 
				Area = Area, 
				Code = Code, 
				Create_time = Create_time, 
				Fax = Fax, 
				Func_switch = Func_switch, 
				Icon = Icon, 
				Kefu = Kefu, 
				Main_business = Main_business, 
				Nickname = Nickname, 
				State = State, 
				Title = Title});
		public ShopInfo AddShop(ShopInfo item) {
			item.Member_id = this.Id;
			return item.Save();
		}

	}
}

