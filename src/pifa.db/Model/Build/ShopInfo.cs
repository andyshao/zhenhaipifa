﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Reflection;
using Newtonsoft.Json;
using pifa.BLL;

namespace pifa.Model {

	[JsonObject(MemberSerialization.OptIn)]
	public partial class ShopInfo {
		#region fields
		private uint? _Id;
		private uint? _Markettype_id;
		private MarkettypeInfo _obj_markettype;
		private uint? _Member_id;
		private MemberInfo _obj_member;
		private string _Address;
		private decimal? _Area;
		private string _Code;
		private DateTime? _Create_time;
		private string _Fax;
		private ShopFUNC_SWITCH? _Func_switch;
		private ShopICON? _Icon;
		private string _Kefu;
		private string _Main_business;
		private string _Nickname;
		private ShopSTATE? _State;
		private string _Title;
		#endregion

		public ShopInfo() { }

		#region 序列化，反序列化
		protected static readonly string StringifySplit = "@<Shop(Info]?#>";
		public string Stringify() {
			return string.Concat(
				_Id == null ? "null" : _Id.ToString(), "|",
				_Markettype_id == null ? "null" : _Markettype_id.ToString(), "|",
				_Member_id == null ? "null" : _Member_id.ToString(), "|",
				_Address == null ? "null" : _Address.Replace("|", StringifySplit), "|",
				_Area == null ? "null" : _Area.ToString(), "|",
				_Code == null ? "null" : _Code.Replace("|", StringifySplit), "|",
				_Create_time == null ? "null" : _Create_time.Value.Ticks.ToString(), "|",
				_Fax == null ? "null" : _Fax.Replace("|", StringifySplit), "|",
				_Func_switch == null ? "null" : _Func_switch.ToInt64().ToString(), "|",
				_Icon == null ? "null" : _Icon.ToInt64().ToString(), "|",
				_Kefu == null ? "null" : _Kefu.Replace("|", StringifySplit), "|",
				_Main_business == null ? "null" : _Main_business.Replace("|", StringifySplit), "|",
				_Nickname == null ? "null" : _Nickname.Replace("|", StringifySplit), "|",
				_State == null ? "null" : _State.ToInt64().ToString(), "|",
				_Title == null ? "null" : _Title.Replace("|", StringifySplit));
		}
		public static ShopInfo Parse(string stringify) {
			string[] ret = stringify.Split(new char[] { '|' }, 15, StringSplitOptions.None);
			if (ret.Length != 15) throw new Exception("格式不正确，ShopInfo：" + stringify);
			ShopInfo item = new ShopInfo();
			if (string.Compare("null", ret[0]) != 0) item.Id = uint.Parse(ret[0]);
			if (string.Compare("null", ret[1]) != 0) item.Markettype_id = uint.Parse(ret[1]);
			if (string.Compare("null", ret[2]) != 0) item.Member_id = uint.Parse(ret[2]);
			if (string.Compare("null", ret[3]) != 0) item.Address = ret[3].Replace(StringifySplit, "|");
			if (string.Compare("null", ret[4]) != 0) item.Area = decimal.Parse(ret[4]);
			if (string.Compare("null", ret[5]) != 0) item.Code = ret[5].Replace(StringifySplit, "|");
			if (string.Compare("null", ret[6]) != 0) item.Create_time = new DateTime(long.Parse(ret[6]));
			if (string.Compare("null", ret[7]) != 0) item.Fax = ret[7].Replace(StringifySplit, "|");
			if (string.Compare("null", ret[8]) != 0) item.Func_switch = (ShopFUNC_SWITCH)long.Parse(ret[8]);
			if (string.Compare("null", ret[9]) != 0) item.Icon = (ShopICON)long.Parse(ret[9]);
			if (string.Compare("null", ret[10]) != 0) item.Kefu = ret[10].Replace(StringifySplit, "|");
			if (string.Compare("null", ret[11]) != 0) item.Main_business = ret[11].Replace(StringifySplit, "|");
			if (string.Compare("null", ret[12]) != 0) item.Nickname = ret[12].Replace(StringifySplit, "|");
			if (string.Compare("null", ret[13]) != 0) item.State = (ShopSTATE)long.Parse(ret[13]);
			if (string.Compare("null", ret[14]) != 0) item.Title = ret[14].Replace(StringifySplit, "|");
			return item;
		}
		#endregion

		#region override
		private static Lazy<Dictionary<string, bool>> __jsonIgnoreLazy = new Lazy<Dictionary<string, bool>>(() => {
			FieldInfo field = typeof(ShopInfo).GetField("JsonIgnore");
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
				__jsonIgnore.ContainsKey("Markettype_id") ? string.Empty : string.Format(", Markettype_id : {0}", Markettype_id == null ? "null" : Markettype_id.ToString()), 
				__jsonIgnore.ContainsKey("Member_id") ? string.Empty : string.Format(", Member_id : {0}", Member_id == null ? "null" : Member_id.ToString()), 
				__jsonIgnore.ContainsKey("Address") ? string.Empty : string.Format(", Address : {0}", Address == null ? "null" : string.Format("'{0}'", Address.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), 
				__jsonIgnore.ContainsKey("Area") ? string.Empty : string.Format(", Area : {0}", Area == null ? "null" : Area.ToString()), 
				__jsonIgnore.ContainsKey("Code") ? string.Empty : string.Format(", Code : {0}", Code == null ? "null" : string.Format("'{0}'", Code.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), 
				__jsonIgnore.ContainsKey("Create_time") ? string.Empty : string.Format(", Create_time : {0}", Create_time == null ? "null" : Create_time.Value.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds.ToString()), 
				__jsonIgnore.ContainsKey("Fax") ? string.Empty : string.Format(", Fax : {0}", Fax == null ? "null" : string.Format("'{0}'", Fax.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), 
				__jsonIgnore.ContainsKey("Func_switch") ? string.Empty : string.Format(", Func_switch : {0}", Func_switch == null ? "null" : string.Format("[ '{0}' ]", string.Join("', '", Func_switch.ToInt64().ToSet<ShopFUNC_SWITCH>().Select<ShopFUNC_SWITCH, string>(a => a.ToDescriptionOrString().Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))))), 
				__jsonIgnore.ContainsKey("Icon") ? string.Empty : string.Format(", Icon : {0}", Icon == null ? "null" : string.Format("[ '{0}' ]", string.Join("', '", Icon.ToInt64().ToSet<ShopICON>().Select<ShopICON, string>(a => a.ToDescriptionOrString().Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))))), 
				__jsonIgnore.ContainsKey("Kefu") ? string.Empty : string.Format(", Kefu : {0}", Kefu == null ? "null" : string.Format("'{0}'", Kefu.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), 
				__jsonIgnore.ContainsKey("Main_business") ? string.Empty : string.Format(", Main_business : {0}", Main_business == null ? "null" : string.Format("'{0}'", Main_business.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), 
				__jsonIgnore.ContainsKey("Nickname") ? string.Empty : string.Format(", Nickname : {0}", Nickname == null ? "null" : string.Format("'{0}'", Nickname.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), 
				__jsonIgnore.ContainsKey("State") ? string.Empty : string.Format(", State : {0}", State == null ? "null" : string.Format("'{0}'", State.ToDescriptionOrString().Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), 
				__jsonIgnore.ContainsKey("Title") ? string.Empty : string.Format(", Title : {0}", Title == null ? "null" : string.Format("'{0}'", Title.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), " }");
			return string.Concat("{", json.Substring(1));
		}
		public IDictionary ToBson(bool allField = false) {
			IDictionary ht = new Hashtable();
			if (allField || !__jsonIgnore.ContainsKey("Id")) ht["Id"] = Id;
			if (allField || !__jsonIgnore.ContainsKey("Markettype_id")) ht["Markettype_id"] = Markettype_id;
			if (allField || !__jsonIgnore.ContainsKey("Member_id")) ht["Member_id"] = Member_id;
			if (allField || !__jsonIgnore.ContainsKey("Address")) ht["Address"] = Address;
			if (allField || !__jsonIgnore.ContainsKey("Area")) ht["Area"] = Area;
			if (allField || !__jsonIgnore.ContainsKey("Code")) ht["Code"] = Code;
			if (allField || !__jsonIgnore.ContainsKey("Create_time")) ht["Create_time"] = Create_time;
			if (allField || !__jsonIgnore.ContainsKey("Fax")) ht["Fax"] = Fax;
			if (allField || !__jsonIgnore.ContainsKey("Func_switch")) ht["Func_switch"] = Func_switch?.ToInt64().ToSet<ShopFUNC_SWITCH>().Select(a => a.ToDescriptionOrString());
			if (allField || !__jsonIgnore.ContainsKey("Icon")) ht["Icon"] = Icon?.ToInt64().ToSet<ShopICON>().Select(a => a.ToDescriptionOrString());
			if (allField || !__jsonIgnore.ContainsKey("Kefu")) ht["Kefu"] = Kefu;
			if (allField || !__jsonIgnore.ContainsKey("Main_business")) ht["Main_business"] = Main_business;
			if (allField || !__jsonIgnore.ContainsKey("Nickname")) ht["Nickname"] = Nickname;
			if (allField || !__jsonIgnore.ContainsKey("State")) ht["State"] = State?.ToDescriptionOrString();
			if (allField || !__jsonIgnore.ContainsKey("Title")) ht["Title"] = Title;
			return ht;
		}
		public object this[string key] {
			get { return this.GetType().GetProperty(key).GetValue(this); }
			set { this.GetType().GetProperty(key).SetValue(this, value); }
		}
		#endregion

		#region properties
		/// <summary>
		/// logo,主图以id命名
		/// </summary>
		[JsonProperty] public uint? Id {
			get { return _Id; }
			set { _Id = value; }
		}
		/// <summary>
		/// 市场楼层
		/// </summary>
		[JsonProperty] public uint? Markettype_id {
			get { return _Markettype_id; }
			set {
				if (_Markettype_id != value) _obj_markettype = null;
				_Markettype_id = value;
			}
		}
		public MarkettypeInfo Obj_markettype {
			get {
				if (_obj_markettype == null) _obj_markettype = Markettype.GetItem(_Markettype_id.Value);
				return _obj_markettype;
			}
			internal set { _obj_markettype = value; }
		}
		/// <summary>
		/// 所属人
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
		/// 地址
		/// </summary>
		[JsonProperty] public string Address {
			get { return _Address; }
			set { _Address = value; }
		}
		/// <summary>
		/// 面积
		/// </summary>
		[JsonProperty] public decimal? Area {
			get { return _Area; }
			set { _Area = value; }
		}
		/// <summary>
		/// 商位号
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
		/// <summary>
		/// 传真
		/// </summary>
		[JsonProperty] public string Fax {
			get { return _Fax; }
			set { _Fax = value; }
		}
		/// <summary>
		/// 功能开关
		/// </summary>
		[JsonProperty] public ShopFUNC_SWITCH? Func_switch {
			get { return _Func_switch; }
			set { _Func_switch = value; }
		}
		/// <summary>
		/// 点亮图标
		/// </summary>
		[JsonProperty] public ShopICON? Icon {
			get { return _Icon; }
			set { _Icon = value; }
		}
		/// <summary>
		/// 客服
		/// </summary>
		[JsonProperty] public string Kefu {
			get { return _Kefu; }
			set { _Kefu = value; }
		}
		/// <summary>
		/// 店铺主营
		/// </summary>
		[JsonProperty] public string Main_business {
			get { return _Main_business; }
			set { _Main_business = value; }
		}
		/// <summary>
		/// 姓名
		/// </summary>
		[JsonProperty] public string Nickname {
			get { return _Nickname; }
			set { _Nickname = value; }
		}
		/// <summary>
		/// 状态
		/// </summary>
		[JsonProperty] public ShopSTATE? State {
			get { return _State; }
			set { _State = value; }
		}
		/// <summary>
		/// 店铺名称
		/// </summary>
		[JsonProperty] public string Title {
			get { return _Title; }
			set { _Title = value; }
		}
		public Member_shopInfo Obj_member_shop { set; get; }
		private List<MemberInfo> _obj_members;
		/// <summary>
		/// 遍历时，可通过 Obj_member_shop 可获取中间表数据
		/// </summary>
		public List<MemberInfo> Obj_members {
			get {
				if (_obj_members == null) _obj_members = Member.Select.InnerJoin<Member_shop>("b", "b.`member_id` = a.`id`").Where("b.`shop_id` = {0}", _Id.Value).ToList();
				return _obj_members;
			}
		}
		private List<ProductInfo> _obj_products;
		public List<ProductInfo> Obj_products {
			get {
				if (_obj_products == null) _obj_products = Product.SelectByShop_id(_Id).Limit(500).ToList();
				return _obj_products;
			}
		}
		private List<FranchisingInfo> _obj_franchisings;
		public List<FranchisingInfo> Obj_franchisings {
			get {
				if (_obj_franchisings == null) _obj_franchisings = Franchising.SelectByShop_id(_Id.Value).ToList();
				return _obj_franchisings;
			}
		}
		private List<Shop_friendly_linksInfo> _obj_shop_friendly_linkss;
		public List<Shop_friendly_linksInfo> Obj_shop_friendly_linkss {
			get {
				if (_obj_shop_friendly_linkss == null) _obj_shop_friendly_linkss = Shop_friendly_links.SelectByShop_id(_Id).Limit(500).ToList();
				return _obj_shop_friendly_linkss;
			}
		}
		private List<ShopsecurityInfo> _obj_shopsecuritys;
		public List<ShopsecurityInfo> Obj_shopsecuritys {
			get {
				if (_obj_shopsecuritys == null) _obj_shopsecuritys = Shopsecurity.SelectByShop_id(_Id).Limit(500).ToList();
				return _obj_shopsecuritys;
			}
		}
		private List<ShopstatInfo> _obj_shopstats;
		public List<ShopstatInfo> Obj_shopstats {
			get {
				if (_obj_shopstats == null) _obj_shopstats = Shopstat.SelectByShop_id(_Id).Limit(500).ToList();
				return _obj_shopstats;
			}
		}
		#endregion

		public pifa.DAL.Shop.SqlUpdateBuild UpdateDiy {
			get { return Shop.UpdateDiy(this, _Id.Value); }
		}
		public ShopInfo Save() {
			if (this.Id != null) {
				Shop.Update(this);
				return this;
			}
			this.Create_time = DateTime.Now;
			return Shop.Insert(this);
		}
		public Member_shopInfo FlagMember(MemberInfo Member, DateTime? Create_time) => FlagMember(Member.Id, Create_time);
		public Member_shopInfo FlagMember(uint? Member_id, DateTime? Create_time) {
			Member_shopInfo item = Member_shop.GetItem(Member_id.Value, this.Id.Value);
			if (item == null) item = Member_shop.Insert(new Member_shopInfo {
				Member_id = Member_id, 
			Shop_id = this.Id, 
				Create_time = Create_time});
			else item.UpdateDiy
					.SetCreate_time(Create_time).ExecuteNonQuery();
			return item;
		}

		public int UnflagMember(MemberInfo Member) => UnflagMember(Member.Id);
		public int UnflagMember(uint? Member_id) => Member_shop.Delete(Member_id.Value, this.Id.Value);
		public int UnflagMemberALL() => Member_shop.DeleteByShop_id(this.Id);

		public ProductInfo AddProduct(CategoryInfo Category, DateTime? Create_time, ProductICON? Icon, decimal? Price, uint? Stock, string Title, string Unit) => AddProduct(Category.Id, Create_time, Icon, Price, Stock, Title, Unit);
		public ProductInfo AddProduct(uint? Category_id, DateTime? Create_time, ProductICON? Icon, decimal? Price, uint? Stock, string Title, string Unit) => Product.Insert(new ProductInfo {
				Category_id = Category_id, 
			Shop_id = this.Id, 
				Create_time = Create_time, 
				Icon = Icon, 
				Price = Price, 
				Stock = Stock, 
				Title = Title, 
				Unit = Unit});
		public ProductInfo AddProduct(ProductInfo item) {
			item.Shop_id = this.Id;
			return item.Save();
		}

		public Shop_franchisingInfo FlagFranchising(FranchisingInfo Franchising) => FlagFranchising(Franchising.Id);
		public Shop_franchisingInfo FlagFranchising(uint? Franchising_id) {
			Shop_franchisingInfo item = Shop_franchising.GetItem(Franchising_id.Value, this.Id.Value);
			if (item == null) item = Shop_franchising.Insert(new Shop_franchisingInfo {
				Franchising_id = Franchising_id, 
			Shop_id = this.Id});
			return item;
		}

		public int UnflagFranchising(FranchisingInfo Franchising) => UnflagFranchising(Franchising.Id);
		public int UnflagFranchising(uint? Franchising_id) => Shop_franchising.Delete(Franchising_id.Value, this.Id.Value);
		public int UnflagFranchisingALL() => Shop_franchising.DeleteByShop_id(this.Id);

		public Shop_friendly_linksInfo AddFriendly_links(DateTime? Create_time, byte? Sort, string Title, string Url) => Shop_friendly_links.Insert(new Shop_friendly_linksInfo {
			Shop_id = this.Id, 
				Create_time = Create_time, 
				Sort = Sort, 
				Title = Title, 
				Url = Url});
		public Shop_friendly_linksInfo AddFriendly_links(Shop_friendly_linksInfo item) {
			item.Shop_id = this.Id;
			return item.Save();
		}

		public ShopsecurityInfo AddShopsecurity(string Idcard, string Idcard_img1, string Idcard_img2, string License_img) => Shopsecurity.Insert(new ShopsecurityInfo {
			Shop_id = this.Id, 
				Idcard = Idcard, 
				Idcard_img1 = Idcard_img1, 
				Idcard_img2 = Idcard_img2, 
				License_img = License_img});
		public ShopsecurityInfo AddShopsecurity(ShopsecurityInfo item) {
			item.Shop_id = this.Id;
			return item.Save();
		}

		public ShopstatInfo AddShopstat(uint? Today_fav, uint? Today_session, uint? Today_share, uint? Total_fav, uint? Total_session, uint? Total_share) => Shopstat.Insert(new ShopstatInfo {
			Shop_id = this.Id, 
				Today_fav = Today_fav, 
				Today_session = Today_session, 
				Today_share = Today_share, 
				Total_fav = Total_fav, 
				Total_session = Total_session, 
				Total_share = Total_share});
		public ShopstatInfo AddShopstat(ShopstatInfo item) {
			item.Shop_id = this.Id;
			return item.Save();
		}

	}
	[Flags]
	public enum ShopFUNC_SWITCH : long {
		/// <summary>
		/// ??
		/// </summary>
		[Description("??")]
		Unknow1 = 1, 
		/// <summary>
		/// ????
		/// </summary>
		[Description("????")]
		Unknow2 = 2, 
		/// <summary>
		/// ?????
		/// </summary>
		[Description("?????")]
		Unknow3 = 4
	}
	[Flags]
	public enum ShopICON : long {
		/// <summary>
		/// ????
		/// </summary>
		[Description("????")]
		Unknow1 = 1, 
		/// <summary>
		/// ????
		/// </summary>
		[Description("????")]
		Unknow2 = 2
	}
	public enum ShopSTATE {
		/// <summary>
		/// ??
		/// </summary>
		[Description("??")]
		Unknow1 = 1, 
		/// <summary>
		/// ??
		/// </summary>
		[Description("??")]
		Unknow2
	}
}

