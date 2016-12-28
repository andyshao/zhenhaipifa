using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Reflection;
using pifa.BLL;

namespace pifa.Model {

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

		#region 独创的序列化，反序列化
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
		public OrderInfo(string stringify) {
			string[] ret = stringify.Split(new char[] { '|' }, 13, StringSplitOptions.None);
			if (ret.Length != 13) throw new Exception("格式不正确，OrderInfo：" + stringify);
			if (string.Compare("null", ret[0]) != 0) _Id = uint.Parse(ret[0]);
			if (string.Compare("null", ret[1]) != 0) _Member_id = uint.Parse(ret[1]);
			if (string.Compare("null", ret[2]) != 0) _Code = ret[2].Replace(StringifySplit, "|");
			if (string.Compare("null", ret[3]) != 0) _Create_time = new DateTime(long.Parse(ret[3]));
			if (string.Compare("null", ret[4]) != 0) _Express_code = ret[4].Replace(StringifySplit, "|");
			if (string.Compare("null", ret[5]) != 0) _Express_name = ret[5].Replace(StringifySplit, "|");
			if (string.Compare("null", ret[6]) != 0) _Paymethod = ret[6].Replace(StringifySplit, "|");
			if (string.Compare("null", ret[7]) != 0) _Remark = ret[7].Replace(StringifySplit, "|");
			if (string.Compare("null", ret[8]) != 0) _State = (OrderSTATE)long.Parse(ret[8]);
			if (string.Compare("null", ret[9]) != 0) _Total_express_price = decimal.Parse(ret[9]);
			if (string.Compare("null", ret[10]) != 0) _Total_original_price = decimal.Parse(ret[10]);
			if (string.Compare("null", ret[11]) != 0) _Total_price = decimal.Parse(ret[11]);
			if (string.Compare("null", ret[12]) != 0) _Update_time = new DateTime(long.Parse(ret[12]));
		}
		#endregion

		#region override
		private static Dictionary<string, bool> __jsonIgnore;
		private static object __jsonIgnore_lock = new object();
		public override string ToString() {
			this.Init__jsonIgnore();
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
		public IDictionary ToBson() {
			this.Init__jsonIgnore();
			IDictionary ht = new Hashtable();
			if (!__jsonIgnore.ContainsKey("Id")) ht["Id"] = Id;
			if (!__jsonIgnore.ContainsKey("Member_id")) ht["Member_id"] = Member_id;
			if (!__jsonIgnore.ContainsKey("Code")) ht["Code"] = Code;
			if (!__jsonIgnore.ContainsKey("Create_time")) ht["Create_time"] = Create_time;
			if (!__jsonIgnore.ContainsKey("Express_code")) ht["Express_code"] = Express_code;
			if (!__jsonIgnore.ContainsKey("Express_name")) ht["Express_name"] = Express_name;
			if (!__jsonIgnore.ContainsKey("Paymethod")) ht["Paymethod"] = Paymethod;
			if (!__jsonIgnore.ContainsKey("Remark")) ht["Remark"] = Remark;
			if (!__jsonIgnore.ContainsKey("State")) ht["State"] = State?.ToDescriptionOrString();
			if (!__jsonIgnore.ContainsKey("Total_express_price")) ht["Total_express_price"] = Total_express_price;
			if (!__jsonIgnore.ContainsKey("Total_original_price")) ht["Total_original_price"] = Total_original_price;
			if (!__jsonIgnore.ContainsKey("Total_price")) ht["Total_price"] = Total_price;
			if (!__jsonIgnore.ContainsKey("Update_time")) ht["Update_time"] = Update_time;
			return ht;
		}
		private void Init__jsonIgnore() {
			if (__jsonIgnore == null) {
				lock (__jsonIgnore_lock) {
					if (__jsonIgnore == null) {
						FieldInfo field = typeof(OrderInfo).GetField("JsonIgnore");
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
			OrderInfo item = obj as OrderInfo;
			if (item == null) return false;
			return this.ToString().Equals(item.ToString());
		}
		public override int GetHashCode() {
			return this.ToString().GetHashCode();
		}
		public static bool operator ==(OrderInfo op1, OrderInfo op2) {
			if (object.Equals(op1, null)) return object.Equals(op2, null);
			return op1.Equals(op2);
		}
		public static bool operator !=(OrderInfo op1, OrderInfo op2) {
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
		/// 会员
		/// </summary>
		public uint? Member_id {
			get { return _Member_id; }
			set {
				if (_Member_id != value) _obj_member = null;
				_Member_id = value;
			}
		}
		public MemberInfo Obj_member {
			get {
				if (_obj_member == null) _obj_member = Member.GetItem(_Member_id);
				return _obj_member;
			}
			internal set { _obj_member = value; }
		}
		/// <summary>
		/// 订单号
		/// </summary>
		public string Code {
			get { return _Code; }
			set { _Code = value; }
		}
		/// <summary>
		/// 创建时间
		/// </summary>
		public DateTime? Create_time {
			get { return _Create_time; }
			set { _Create_time = value; }
		}
		public string Express_code {
			get { return _Express_code; }
			set { _Express_code = value; }
		}
		public string Express_name {
			get { return _Express_name; }
			set { _Express_name = value; }
		}
		public string Paymethod {
			get { return _Paymethod; }
			set { _Paymethod = value; }
		}
		/// <summary>
		/// 备注
		/// </summary>
		public string Remark {
			get { return _Remark; }
			set { _Remark = value; }
		}
		public OrderSTATE? State {
			get { return _State; }
			set { _State = value; }
		}
		/// <summary>
		/// 总运费
		/// </summary>
		public decimal? Total_express_price {
			get { return _Total_express_price; }
			set { _Total_express_price = value; }
		}
		/// <summary>
		/// 原总金额
		/// </summary>
		public decimal? Total_original_price {
			get { return _Total_original_price; }
			set { _Total_original_price = value; }
		}
		/// <summary>
		/// 总金额
		/// </summary>
		public decimal? Total_price {
			get { return _Total_price; }
			set { _Total_price = value; }
		}
		/// <summary>
		/// 修改时间
		/// </summary>
		public DateTime? Update_time {
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
			get { return Order.UpdateDiy(this, _Id); }
		}
		public Order_addressInfo AddAddress(string Address, string Name, string Tel, string Telphone, string Zip) {
			return Order_address.Insert(new Order_addressInfo {
				Order_id = this.Id, 
				Address = Address, 
				Name = Name, 
				Tel = Tel, 
				Telphone = Telphone, 
				Zip = Zip});
		}

		public Order_productitemInfo FlagProductitem(ProductitemInfo Productitem, uint? Number, decimal? Price, Order_productitemSTATE? State, string Title) {
			return FlagProductitem(Productitem.Id, Number, Price, State, Title);
		}
		public Order_productitemInfo FlagProductitem(uint? Productitem_id, uint? Number, decimal? Price, Order_productitemSTATE? State, string Title) {
			Order_productitemInfo item = Order_productitem.GetItem(this.Id, Productitem_id);
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

		public int UnflagProductitem(ProductitemInfo Productitem) {
			return UnflagProductitem(Productitem.Id);
		}
		public int UnflagProductitem(uint? Productitem_id) {
			return Order_productitem.Delete(this.Id, Productitem_id);
		}
		public int UnflagProductitemALL() {
			return Order_productitem.DeleteByOrder_id(this.Id);
		}

		public Order_refundInfo AddRefund(ProductitemInfo Productitem, DateTime? Create_time, string Descript, string Email, string Img_url, Order_refundSTATE? State, string Tel, string Telphone, decimal? Wealth) {
			return AddRefund(Productitem.Id, Create_time, Descript, Email, Img_url, State, Tel, Telphone, Wealth);
		}
		public Order_refundInfo AddRefund(uint? Productitem_id, DateTime? Create_time, string Descript, string Email, string Img_url, Order_refundSTATE? State, string Tel, string Telphone, decimal? Wealth) {
			return Order_refund.Insert(new Order_refundInfo {
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
		}

		public Product_commentInfo AddProduct_comment(MemberInfo Member, ProductInfo Product, ProductitemInfo Productitem, string Content, DateTime? Create_time, string Nickname, byte? Star_price, byte? Star_quality, byte? Star_value, Product_commentSTATE? State, string Title, string Upload_image_url) {
			return AddProduct_comment(Member.Id, Product.Id, Productitem.Id, Content, Create_time, Nickname, Star_price, Star_quality, Star_value, State, Title, Upload_image_url);
		}
		public Product_commentInfo AddProduct_comment(uint? Member_id, uint? Product_id, uint? Productitem_id, string Content, DateTime? Create_time, string Nickname, byte? Star_price, byte? Star_quality, byte? Star_value, Product_commentSTATE? State, string Title, string Upload_image_url) {
			return Product_comment.Insert(new Product_commentInfo {
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

