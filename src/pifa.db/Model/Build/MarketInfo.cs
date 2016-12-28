using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using pifa.BLL;

namespace pifa.Model {

	public partial class MarketInfo {
		#region fields
		private uint? _Id;
		private uint? _Area_id;
		private AreaInfo _obj_area;
		private DateTime? _Create_time;
		private string _Title;
		#endregion

		public MarketInfo() { }

		#region 独创的序列化，反序列化
		protected static readonly string StringifySplit = "@<Market(Info]?#>";
		public string Stringify() {
			return string.Concat(
				_Id == null ? "null" : _Id.ToString(), "|",
				_Area_id == null ? "null" : _Area_id.ToString(), "|",
				_Create_time == null ? "null" : _Create_time.Value.Ticks.ToString(), "|",
				_Title == null ? "null" : _Title.Replace("|", StringifySplit));
		}
		public MarketInfo(string stringify) {
			string[] ret = stringify.Split(new char[] { '|' }, 4, StringSplitOptions.None);
			if (ret.Length != 4) throw new Exception("格式不正确，MarketInfo：" + stringify);
			if (string.Compare("null", ret[0]) != 0) _Id = uint.Parse(ret[0]);
			if (string.Compare("null", ret[1]) != 0) _Area_id = uint.Parse(ret[1]);
			if (string.Compare("null", ret[2]) != 0) _Create_time = new DateTime(long.Parse(ret[2]));
			if (string.Compare("null", ret[3]) != 0) _Title = ret[3].Replace(StringifySplit, "|");
		}
		#endregion

		#region override
		private static Dictionary<string, bool> __jsonIgnore;
		private static object __jsonIgnore_lock = new object();
		public override string ToString() {
			this.Init__jsonIgnore();
			string json = string.Concat(
				__jsonIgnore.ContainsKey("Id") ? string.Empty : string.Format(", Id : {0}", Id == null ? "null" : Id.ToString()), 
				__jsonIgnore.ContainsKey("Area_id") ? string.Empty : string.Format(", Area_id : {0}", Area_id == null ? "null" : Area_id.ToString()), 
				__jsonIgnore.ContainsKey("Create_time") ? string.Empty : string.Format(", Create_time : {0}", Create_time == null ? "null" : Create_time.Value.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds.ToString()), 
				__jsonIgnore.ContainsKey("Title") ? string.Empty : string.Format(", Title : {0}", Title == null ? "null" : string.Format("'{0}'", Title.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), " }");
			return string.Concat("{", json.Substring(1));
		}
		public IDictionary ToBson() {
			this.Init__jsonIgnore();
			IDictionary ht = new Hashtable();
			if (!__jsonIgnore.ContainsKey("Id")) ht["Id"] = Id;
			if (!__jsonIgnore.ContainsKey("Area_id")) ht["Area_id"] = Area_id;
			if (!__jsonIgnore.ContainsKey("Create_time")) ht["Create_time"] = Create_time;
			if (!__jsonIgnore.ContainsKey("Title")) ht["Title"] = Title;
			return ht;
		}
		private void Init__jsonIgnore() {
			if (__jsonIgnore == null) {
				lock (__jsonIgnore_lock) {
					if (__jsonIgnore == null) {
						FieldInfo field = typeof(MarketInfo).GetField("JsonIgnore");
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
			MarketInfo item = obj as MarketInfo;
			if (item == null) return false;
			return this.ToString().Equals(item.ToString());
		}
		public override int GetHashCode() {
			return this.ToString().GetHashCode();
		}
		public static bool operator ==(MarketInfo op1, MarketInfo op2) {
			if (object.Equals(op1, null)) return object.Equals(op2, null);
			return op1.Equals(op2);
		}
		public static bool operator !=(MarketInfo op1, MarketInfo op2) {
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
		/// 城市
		/// </summary>
		public uint? Area_id {
			get { return _Area_id; }
			set {
				if (_Area_id != value) _obj_area = null;
				_Area_id = value;
			}
		}
		public AreaInfo Obj_area {
			get {
				if (_obj_area == null) _obj_area = Area.GetItem(_Area_id);
				return _obj_area;
			}
			internal set { _obj_area = value; }
		}
		/// <summary>
		/// 创建时间
		/// </summary>
		public DateTime? Create_time {
			get { return _Create_time; }
			set { _Create_time = value; }
		}
		/// <summary>
		/// 市场名称
		/// </summary>
		public string Title {
			get { return _Title; }
			set { _Title = value; }
		}
		private List<MarketdescInfo> _obj_marketdescs;
		public List<MarketdescInfo> Obj_marketdescs {
			get {
				if (_obj_marketdescs == null) _obj_marketdescs = Marketdesc.SelectByMarket_id(_Id).Limit(500).ToList();
				return _obj_marketdescs;
			}
		}
		private List<MarkettypeInfo> _obj_markettypes;
		public List<MarkettypeInfo> Obj_markettypes {
			get {
				if (_obj_markettypes == null) _obj_markettypes = Markettype.SelectByMarket_id(_Id).Limit(500).ToList();
				return _obj_markettypes;
			}
		}
		public Member_fav_marketInfo Obj_member_fav_market { set; get; }
		private List<MemberInfo> _obj_member_favs;
		/// <summary>
		/// 遍历时，可通过 Obj_member_fav_market 可获取中间表数据
		/// </summary>
		public List<MemberInfo> Obj_member_favs {
			get {
				if (_obj_member_favs == null) _obj_member_favs = Member.Select.InnerJoin<Member_fav_market>("b", "b.`member_id` = a.`id`").Where("b.`market_id` = {0}", _Id.Value).ToList();
				return _obj_member_favs;
			}
		}
		private List<RentsubletInfo> _obj_rentsublets;
		public List<RentsubletInfo> Obj_rentsublets {
			get {
				if (_obj_rentsublets == null) _obj_rentsublets = Rentsublet.SelectByMarket_id(_Id).Limit(500).ToList();
				return _obj_rentsublets;
			}
		}
		#endregion

		public pifa.DAL.Market.SqlUpdateBuild UpdateDiy {
			get { return Market.UpdateDiy(this, _Id); }
		}
		public MarketdescInfo AddMarketdesc(string Content, string Url) {
			return Marketdesc.Insert(new MarketdescInfo {
				Market_id = this.Id, 
				Content = Content, 
				Url = Url});
		}

		public MarkettypeInfo AddMarkettype(MarkettypeInfo Markettype, byte? Sort, string Title) {
			return AddMarkettype(Markettype.Id, Sort, Title);
		}
		public MarkettypeInfo AddMarkettype(uint? Parent_id, byte? Sort, string Title) {
			return Markettype.Insert(new MarkettypeInfo {
				Market_id = this.Id, 
				Parent_id = Parent_id, 
				Sort = Sort, 
				Title = Title});
		}

		public Member_fav_marketInfo FlagMember_fav(MemberInfo Member, DateTime? Create_time) {
			return FlagMember_fav(Member.Id, Create_time);
		}
		public Member_fav_marketInfo FlagMember_fav(uint? Member_id, DateTime? Create_time) {
			Member_fav_marketInfo item = Member_fav_market.GetItem(this.Id, Member_id);
			if (item == null) item = Member_fav_market.Insert(new Member_fav_marketInfo {
				Market_id = this.Id, 
				Member_id = Member_id, 
				Create_time = Create_time});
			else item.UpdateDiy
					.SetCreate_time(Create_time).ExecuteNonQuery();
			return item;
		}

		public int UnflagMember_fav(MemberInfo Member) {
			return UnflagMember_fav(Member.Id);
		}
		public int UnflagMember_fav(uint? Member_id) {
			return Member_fav_market.Delete(this.Id, Member_id);
		}
		public int UnflagMember_favALL() {
			return Member_fav_market.DeleteByMarket_id(this.Id);
		}

		public RentsubletInfo AddRentsublet(DateTime? Create_time, decimal? Price, RentsubletTYPE? Type) {
			return Rentsublet.Insert(new RentsubletInfo {
				Market_id = this.Id, 
				Create_time = Create_time, 
				Price = Price, 
				Type = Type});
		}

	}
}

