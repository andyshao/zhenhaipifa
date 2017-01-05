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
	public partial class OrderInfo {
		#region fields
		private uint? _Id;
		private uint? _Member_id;
		private MemberInfo _obj_member;
		private string _Code;
		private DateTime? _Create_time;
		private string _Express_code;
		private string _Express_name;
		private string _Paymethod;
		private string _Remark;
		private OrderSTATE? _State;
		private decimal? _Total_express_price;
		private decimal? _Total_original_price;
		private decimal? _Total_price;
		private DateTime? _Update_time;
		#endregion

		public OrderInfo() { }

		#region 序列化，反序列化
		protected static readonly string StringifySplit = "@<Order(Info]?#>";
		public string Stringify() {
			return string.Concat(
				_Id == null ? "null" : _Id.ToString(), "|",
				_Member_id == null ? "null" : _Member_id.ToString(), "|",
				_Code == null ? "null" : _Code.Replace("|", StringifySplit), "|",
				_Create_time == null ? "null" : _Create_time.Value.Ticks.ToString(), "|",
				_Express_code == null ? "null" : _Express_code.Replace("|", StringifySplit), "|",
				_Express_name == null ? "null" : _Express_name.Replace("|", StringifySplit), "|",
				_Paymethod == null ? "null" : _Paymethod.Replace("|", StringifySplit), "|",
				_Remark == null ? "null" : _Remark.Replace("|", StringifySplit), "|",
				_State == null ? "null" : _State.ToInt64().ToString(), "|",
				_Total_express_price == null ? "null" : _Total_express_price.ToString(), "|",
				_Total_original_price == null ? "null" : _Total_original_price.ToString(), "|",
				_Total_price == null ? "null" : _Total_price.ToString(), "|",
				_Update_time == null ? "null" : _Update_time.Value.Ticks.ToString());
		}
		public static OrderInfo Parse(string stringify) {
			string[] ret = stringify.Split(new char[] { '|' }, 13, StringSplitOptions.None);
			if (ret.Length != 13) throw new Exception("格式不正确，OrderInfo：" + stringify);
			OrderInfo item = new OrderInfo();
			if (string.Compare("null", ret[0]) != 0) item.Id = uint.Parse(ret[0]);
			if (string.Compare("null", ret[1]) != 0) item.Member_id = uint.Parse(ret[1]);
			if (string.Compare("null", ret[2]) != 0) item.Code = ret[2].Replace(StringifySplit, "|");
			if (string.Compare("null", ret[3]) != 0) item.Create_time = new DateTime(long.Parse(ret[3]));
			if (string.Compare("null", ret[4]) != 0) item.Express_code = ret[4].Replace(StringifySplit, "|");
			if (string.Compare("null", ret[5]) != 0) item.Express_name = ret[5].Replace(StringifySplit, "|");
			if (string.Compare("null", ret[6]) != 0) item.Paymethod = ret[6].Replace(StringifySplit, "|");
			if (string.Compare("null", ret[7]) != 0) item.Remark = ret[7].Replace(StringifySplit, "|");
			if (string.Compare("null", ret[8]) != 0) item.State = (OrderSTATE)long.Parse(ret[8]);
			if (string.Compare("null", ret[9]) != 0) item.Total_express_price = decimal.Parse(ret[9]);
			if (string.Compare("null", ret[10]) != 0) item.Total_original_price = decimal.Parse(ret[10]);
			if (string.Compare("null", ret[11]) != 0) item.Total_price = decimal.Parse(ret[11]);
			if (string.Compare("null", ret[12]) != 0) item.Update_time = new DateTime(long.Parse(ret[12]));
			return item;
		}
		#endregion

		#region override
		private static Lazy<Dictionary<string, bool>> __jsonIgnoreLazy = new Lazy<Dictionary<string, bool>>(() => {
			FieldInfo field = typeof(OrderInfo).GetField("JsonIgnore");
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
				__jsonIgnore.ContainsKey("Member_id") ? string.Empty : string.Format(", Member_id : {0}", Member_id == null ? "null" : Member_id.ToString()), 
				__jsonIgnore.ContainsKey("Code") ? string.Empty : string.Format(", Code : {0}", Code == null ? "null" : string.Format("'{0}'", Code.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), 
				__jsonIgnore.ContainsKey("Create_time") ? string.Empty : string.Format(", Create_time : {0}", Create_time == null ? "null" : Create_time.Value.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds.ToString()), 
				__jsonIgnore.ContainsKey("Express_code") ? string.Empty : string.Format(", Express_code : {0}", Express_code == null ? "null" : string.Format("'{0}'", Express_code.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), 
				__jsonIgnore.ContainsKey("Express_name") ? string.Empty : string.Format(", Express_name : {0}", Express_name == null ? "null" : string.Format("'{0}'", Express_name.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), 
				__jsonIgnore.ContainsKey("Paymethod") ? string.Empty : string.Format(", Paymethod : {0}", Paymethod == null ? "null" : string.Format("'{0}'", Paymethod.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), 
				__jsonIgnore.ContainsKey("Remark") ? string.Empty : string.Format(", Remark : {0}", Remark == null ? "null" : string.Format("'{0}'", Remark.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), 
				__jsonIgnore.ContainsKey("State") ? string.Empty : string.Format(", State : {0}", State == null ? "null" : string.Format("'{0}'", State.ToDescriptionOrString().Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), 
				__jsonIgnore.ContainsKey("Total_express_price") ? string.Empty : string.Format(", Total_express_price : {0}", Total_express_price == null ? "null" : Total_express_price.ToString()), 
				__jsonIgnore.ContainsKey("Total_original_price") ? string.Empty : string.Format(", Total_original_price : {0}", Total_original_price == null ? "null" : Total_original_price.ToString()), 
				__jsonIgnore.ContainsKey("Total_price") ? string.Empty : string.Format(", Total_price : {0}", Total_price == null ? "null" : Total_price.ToString()), 
				__jsonIgnore.ContainsKey("Update_time") ? string.Empty : string.Format(", Update_time : {0}", Update_time == null ? "null" : Update_time.Value.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds.ToString()), " }");
			return string.Concat("{", json.Substring(1));
		}
		public IDictionary ToBson(bool allField = false) {
			IDictionary ht = new Hashtable();
			if (allField || !__jsonIgnore.ContainsKey("Id")) ht["Id"] = Id;
			if (allField || !__jsonIgnore.ContainsKey("Member_id")) ht["Member_id"] = Member_id;
			if (allField || !__jsonIgnore.ContainsKey("Code")) ht["Code"] = Code;
			if (allField || !__jsonIgnore.ContainsKey("Create_time")) ht["Create_time"] = Create_time;
			if (allField || !__jsonIgnore.ContainsKey("Express_code")) ht["Express_code"] = Express_code;
			if (allField || !__jsonIgnore.ContainsKey("Express_name")) ht["Express_name"] = Express_name;
			if (allField || !__jsonIgnore.ContainsKey("Paymethod")) ht["Paymethod"] = Paymethod;
			if (allField || !__jsonIgnore.ContainsKey("Remark")) ht["Remark"] = Remark;
			if (allField || !__jsonIgnore.ContainsKey("State")) ht["State"] = State?.ToDescriptionOrString();
			if (allField || !__jsonIgnore.ContainsKey("Total_express_price")) ht["Total_express_price"] = Total_express_price;
			if (allField || !__jsonIgnore.ContainsKey("Total_original_price")) ht["Total_original_price"] = Total_original_price;
			if (allField || !__jsonIgnore.ContainsKey("Total_price")) ht["Total_price"] = Total_price;
			if (allField || !__jsonIgnore.ContainsKey("Update_time")) ht["Update_time"] = Update_time;
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
		/// 会员
		/// </summary>
		[JsonProperty] public uint? Member_id {
			get { return _Member_id; }
			set {
				if (_Member_id != value) _obj_member = null;
				_Member_id = value;
			}
		}
		public MemberInfo Obj_member {
			get {
				if (_obj_member == null) _obj_member = Member.GetItem(_Member_id.Value);
				return _obj_member;
			}
			internal set { _obj_member = value; }
		}
		/// <summary>
		/// 订单号
		/// </summary>
		[JsonProperty] public string Code {
			get { return _Code; }
			set { _Code = value; }
		}
		/// <summary>
		/// 创建时间
		/// </summary>
		[JsonProperty] public DateTime? Create_time {
			get { return _Create_time; }
			set { _Create_time = value; }
		}
		[JsonProperty] public string Express_code {
			get { return _Express_code; }
			set { _Express_code = value; }
		}
		[JsonProperty] public string Express_name {
			get { return _Express_name; }
			set { _Express_name = value; }
		}
		[JsonProperty] public string Paymethod {
			get { return _Paymethod; }
			set { _Paymethod = value; }
		}
		/// <summary>
		/// 备注
		/// </summary>
		[JsonProperty] public string Remark {
			get { return _Remark; }
			set { _Remark = value; }
		}
		[JsonProperty] public OrderSTATE? State {
			get { return _State; }
			set { _State = value; }
		}
		/// <summary>
		/// 总运费
		/// </summary>
		[JsonProperty] public decimal? Total_express_price {
			get { return _Total_express_price; }
			set { _Total_express_price = value; }
		}
		/// <summary>
		/// 原总金额
		/// </summary>
		[JsonProperty] public decimal? Total_original_price {
			get { return _Total_original_price; }
			set { _Total_original_price = value; }
		}
		/// <summary>
		/// 总金额
		/// </summary>
		[JsonProperty] public decimal? Total_price {
			get { return _Total_price; }
			set { _Total_price = value; }
		}
		/// <summary>
		/// 修改时间
		/// </summary>
		[JsonProperty] public DateTime? Update_time {
			get { return _Update_time; }
			set { _Update_time = value; }
		}
		private List<Order_addressInfo> _obj_order_addresss;
		public List<Order_addressInfo> Obj_order_addresss {
			get {
				if (_obj_order_addresss == null) _obj_order_addresss = Order_address.SelectByOrder_id(_Id).Limit(500).ToList();
				return _obj_order_addresss;
			}
		}
		public Order_productitemInfo Obj_order_productitem { set; get; }
		private List<ProductitemInfo> _obj_productitems;
		/// <summary>
		/// 遍历时，可通过 Obj_order_productitem 可获取中间表数据
		/// </summary>
		public List<ProductitemInfo> Obj_productitems {
			get {
				if (_obj_productitems == null) _obj_productitems = Productitem.Select.InnerJoin<Order_productitem>("b", "b.`productitem_id` = a.`id`").Where("b.`order_id` = {0}", _Id.Value).ToList();
				return _obj_productitems;
			}
		}
		private List<Order_refundInfo> _obj_order_refunds;
		public List<Order_refundInfo> Obj_order_refunds {
			get {
				if (_obj_order_refunds == null) _obj_order_refunds = Order_refund.SelectByOrder_id(_Id).Limit(500).ToList();
				return _obj_order_refunds;
			}
		}
		private List<Product_commentInfo> _obj_product_comments;
		public List<Product_commentInfo> Obj_product_comments {
			get {
				if (_obj_product_comments == null) _obj_product_comments = Product_comment.SelectByOrder_id(_Id).Limit(500).ToList();
				return _obj_product_comments;
			}
		}
		#endregion

		public pifa.DAL.Order.SqlUpdateBuild UpdateDiy {
			get { return Order.UpdateDiy(this, _Id.Value); }
		}
		public OrderInfo Save() {
			this.Update_time = DateTime.Now;
			if (this.Id != null) {
				Order.Update(this);
				return this;
			}
			this.Create_time = DateTime.Now;
			return Order.Insert(this);
		}
		public Order_addressInfo AddAddress(string Address, string Name, string Tel, string Telphone, string Zip) => Order_address.Insert(new Order_addressInfo {
			Order_id = this.Id, 
				Address = Address, 
				Name = Name, 
				Tel = Tel, 
				Telphone = Telphone, 
				Zip = Zip});
		public Order_addressInfo AddAddress(Order_addressInfo item) {
			item.Order_id = this.Id;
			return item.Save();
		}

		public Order_productitemInfo FlagProductitem(ProductitemInfo Productitem, uint? Number, decimal? Price, Order_productitemSTATE? State, string Title) => FlagProductitem(Productitem.Id, Number, Price, State, Title);
		public Order_productitemInfo FlagProductitem(uint? Productitem_id, uint? Number, decimal? Price, Order_productitemSTATE? State, string Title) {
			Order_productitemInfo item = Order_productitem.GetItem(this.Id.Value, Productitem_id.Value);
			if (item == null) item = Order_productitem.Insert(new Order_productitemInfo {
			Order_id = this.Id, 
				Productitem_id = Productitem_id, 
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

		public int UnflagProductitem(ProductitemInfo Productitem) => UnflagProductitem(Productitem.Id);
		public int UnflagProductitem(uint? Productitem_id) => Order_productitem.Delete(this.Id.Value, Productitem_id.Value);
		public int UnflagProductitemALL() => Order_productitem.DeleteByOrder_id(this.Id);

		public Order_refundInfo AddRefund(ProductitemInfo Productitem, DateTime? Create_time, string Descript, string Email, string Img_url, Order_refundSTATE? State, string Tel, string Telphone, decimal? Wealth) => AddRefund(Productitem.Id, Create_time, Descript, Email, Img_url, State, Tel, Telphone, Wealth);
		public Order_refundInfo AddRefund(uint? Productitem_id, DateTime? Create_time, string Descript, string Email, string Img_url, Order_refundSTATE? State, string Tel, string Telphone, decimal? Wealth) => Order_refund.Insert(new Order_refundInfo {
			Order_id = this.Id, 
				Productitem_id = Productitem_id, 
				Create_time = Create_time, 
				Descript = Descript, 
				Email = Email, 
				Img_url = Img_url, 
				State = State, 
				Tel = Tel, 
				Telphone = Telphone, 
				Wealth = Wealth});
		public Order_refundInfo AddRefund(Order_refundInfo item) {
			item.Order_id = this.Id;
			return item.Save();
		}

		public Product_commentInfo AddProduct_comment(MemberInfo Member, ProductInfo Product, ProductitemInfo Productitem, string Content, DateTime? Create_time, string Nickname, byte? Star_price, byte? Star_quality, byte? Star_value, Product_commentSTATE? State, string Title, string Upload_image_url) => AddProduct_comment(Member.Id, Product.Id, Productitem.Id, Content, Create_time, Nickname, Star_price, Star_quality, Star_value, State, Title, Upload_image_url);
		public Product_commentInfo AddProduct_comment(uint? Member_id, uint? Product_id, uint? Productitem_id, string Content, DateTime? Create_time, string Nickname, byte? Star_price, byte? Star_quality, byte? Star_value, Product_commentSTATE? State, string Title, string Upload_image_url) => Product_comment.Insert(new Product_commentInfo {
				Member_id = Member_id, 
			Order_id = this.Id, 
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
			item.Order_id = this.Id;
			return item.Save();
		}

	}
	public enum OrderSTATE {
		/// <summary>
		/// ????
		/// </summary>
		[Description("????")]
		Unknow1 = 1, 
		/// <summary>
		/// ??????
		/// </summary>
		[Description("??????")]
		Unknow2, 
		/// <summary>
		/// ????
		/// </summary>
		[Description("????")]
		Unknow3, 
		/// <summary>
		/// ??????
		/// </summary>
		[Description("??????")]
		Unknow4, 
		/// <summary>
		/// ??
		/// </summary>
		[Description("??")]
		Unknow5, 
		/// <summary>
		/// ??
		/// </summary>
		[Description("??")]
		Unknow6, 
		/// <summary>
		/// ??
		/// </summary>
		[Description("??")]
		Unknow7
	}
}

