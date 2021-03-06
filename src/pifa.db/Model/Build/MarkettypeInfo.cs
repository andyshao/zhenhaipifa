﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using pifa.BLL;

namespace pifa.Model {

	[JsonObject(MemberSerialization.OptIn)]
	public partial class MarkettypeInfo {
		#region fields
		private uint? _Id;
		private uint? _Market_id;
		private MarketInfo _obj_market;
		private uint? _Parent_id;
		private MarkettypeInfo _obj_markettype;
		private byte? _Sort;
		private string _Title;
		#endregion

		public MarkettypeInfo() { }

		#region 序列化，反序列化
		protected static readonly string StringifySplit = "@<Markettype(Info]?#>";
		public string Stringify() {
			return string.Concat(
				_Id == null ? "null" : _Id.ToString(), "|",
				_Market_id == null ? "null" : _Market_id.ToString(), "|",
				_Parent_id == null ? "null" : _Parent_id.ToString(), "|",
				_Sort == null ? "null" : _Sort.ToString(), "|",
				_Title == null ? "null" : _Title.Replace("|", StringifySplit));
		}
		public static MarkettypeInfo Parse(string stringify) {
			string[] ret = stringify.Split(new char[] { '|' }, 5, StringSplitOptions.None);
			if (ret.Length != 5) throw new Exception("格式不正确，MarkettypeInfo：" + stringify);
			MarkettypeInfo item = new MarkettypeInfo();
			if (string.Compare("null", ret[0]) != 0) item.Id = uint.Parse(ret[0]);
			if (string.Compare("null", ret[1]) != 0) item.Market_id = uint.Parse(ret[1]);
			if (string.Compare("null", ret[2]) != 0) item.Parent_id = uint.Parse(ret[2]);
			if (string.Compare("null", ret[3]) != 0) item.Sort = byte.Parse(ret[3]);
			if (string.Compare("null", ret[4]) != 0) item.Title = ret[4].Replace(StringifySplit, "|");
			return item;
		}
		#endregion

		#region override
		private static Lazy<Dictionary<string, bool>> __jsonIgnoreLazy = new Lazy<Dictionary<string, bool>>(() => {
			FieldInfo field = typeof(MarkettypeInfo).GetField("JsonIgnore");
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
				__jsonIgnore.ContainsKey("Market_id") ? string.Empty : string.Format(", Market_id : {0}", Market_id == null ? "null" : Market_id.ToString()), 
				__jsonIgnore.ContainsKey("Parent_id") ? string.Empty : string.Format(", Parent_id : {0}", Parent_id == null ? "null" : Parent_id.ToString()), 
				__jsonIgnore.ContainsKey("Sort") ? string.Empty : string.Format(", Sort : {0}", Sort == null ? "null" : Sort.ToString()), 
				__jsonIgnore.ContainsKey("Title") ? string.Empty : string.Format(", Title : {0}", Title == null ? "null" : string.Format("'{0}'", Title.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), " }");
			return string.Concat("{", json.Substring(1));
		}
		public IDictionary ToBson(bool allField = false) {
			IDictionary ht = new Hashtable();
			if (allField || !__jsonIgnore.ContainsKey("Id")) ht["Id"] = Id;
			if (allField || !__jsonIgnore.ContainsKey("Market_id")) ht["Market_id"] = Market_id;
			if (allField || !__jsonIgnore.ContainsKey("Parent_id")) ht["Parent_id"] = Parent_id;
			if (allField || !__jsonIgnore.ContainsKey("Sort")) ht["Sort"] = Sort;
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
		/// 市场
		/// </summary>
		[JsonProperty] public uint? Market_id {
			get { return _Market_id; }
			set {
				if (_Market_id != value) _obj_market = null;
				_Market_id = value;
			}
		}
		public MarketInfo Obj_market {
			get {
				if (_obj_market == null) _obj_market = Market.GetItem(_Market_id.Value);
				return _obj_market;
			}
			internal set { _obj_market = value; }
		}
		/// <summary>
		/// 父
		/// </summary>
		[JsonProperty] public uint? Parent_id {
			get { return _Parent_id; }
			set {
				if (_Parent_id != value) _obj_markettype = null;
				_Parent_id = value;
			}
		}
		public MarkettypeInfo Obj_markettype {
			get {
				if (_obj_markettype == null) _obj_markettype = Markettype.GetItem(_Parent_id.Value);
				return _obj_markettype;
			}
			internal set { _obj_markettype = value; }
		}
		/// <summary>
		/// 排序
		/// </summary>
		[JsonProperty] public byte? Sort {
			get { return _Sort; }
			set { _Sort = value; }
		}
		/// <summary>
		/// 结构名称
		/// </summary>
		[JsonProperty] public string Title {
			get { return _Title; }
			set { _Title = value; }
		}
		private List<MarkettypeInfo> _obj_markettypes;
		public List<MarkettypeInfo> Obj_markettypes {
			get {
				if (_obj_markettypes == null) _obj_markettypes = Markettype.SelectByParent_id(_Id).Limit(500).ToList();
				return _obj_markettypes;
			}
		}
		private List<CategoryInfo> _obj_categorys;
		public List<CategoryInfo> Obj_categorys {
			get {
				if (_obj_categorys == null) _obj_categorys = Category.SelectByMarkettype_id(_Id.Value).ToList();
				return _obj_categorys;
			}
		}
		private List<ShopInfo> _obj_shops;
		public List<ShopInfo> Obj_shops {
			get {
				if (_obj_shops == null) _obj_shops = Shop.SelectByMarkettype_id(_Id).Limit(500).ToList();
				return _obj_shops;
			}
		}
		#endregion

		public pifa.DAL.Markettype.SqlUpdateBuild UpdateDiy {
			get { return Markettype.UpdateDiy(this, _Id.Value); }
		}
		public MarkettypeInfo Save() {
			if (this.Id != null) {
				Markettype.Update(this);
				return this;
			}
			return Markettype.Insert(this);
		}
		public MarkettypeInfo AddMarkettype(MarketInfo Market, byte? Sort, string Title) => AddMarkettype(Market.Id, Sort, Title);
		public MarkettypeInfo AddMarkettype(uint? Market_id, byte? Sort, string Title) => Markettype.Insert(new MarkettypeInfo {
				Market_id = Market_id, 
			Parent_id = this.Id, 
				Sort = Sort, 
				Title = Title});
		public MarkettypeInfo AddMarkettype(MarkettypeInfo item) {
			item.Parent_id = this.Id;
			return item.Save();
		}

		public Markettype_categoryInfo FlagCategory(CategoryInfo Category) => FlagCategory(Category.Id);
		public Markettype_categoryInfo FlagCategory(uint? Category_id) {
			Markettype_categoryInfo item = Markettype_category.GetItem(Category_id.Value, this.Id.Value);
			if (item == null) item = Markettype_category.Insert(new Markettype_categoryInfo {
				Category_id = Category_id, 
			Markettype_id = this.Id});
			return item;
		}

		public int UnflagCategory(CategoryInfo Category) => UnflagCategory(Category.Id);
		public int UnflagCategory(uint? Category_id) => Markettype_category.Delete(Category_id.Value, this.Id.Value);
		public int UnflagCategoryALL() => Markettype_category.DeleteByMarkettype_id(this.Id);

		public ShopInfo AddShop(MemberInfo Member, string Address, decimal? Area, string Code, DateTime? Create_time, string Fax, ShopFUNC_SWITCH? Func_switch, ShopICON? Icon, string Kefu, string Main_business, string Nickname, ShopSTATE? State, string Title) => AddShop(Member.Id, Address, Area, Code, Create_time, Fax, Func_switch, Icon, Kefu, Main_business, Nickname, State, Title);
		public ShopInfo AddShop(uint? Member_id, string Address, decimal? Area, string Code, DateTime? Create_time, string Fax, ShopFUNC_SWITCH? Func_switch, ShopICON? Icon, string Kefu, string Main_business, string Nickname, ShopSTATE? State, string Title) => Shop.Insert(new ShopInfo {
			Markettype_id = this.Id, 
				Member_id = Member_id, 
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
			item.Markettype_id = this.Id;
			return item.Save();
		}

	}
}

