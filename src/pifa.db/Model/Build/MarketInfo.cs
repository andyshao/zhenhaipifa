using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using pifa.BLL;

namespace pifa.Model {

	[JsonObject(MemberSerialization.OptIn)]
	public partial class MarketInfo {
		#region fields
		private uint? _Id;
		private uint? _Area_id;
		private AreaInfo _obj_area;
		private DateTime? _Create_time;
		private string _Title;
		#endregion

		public MarketInfo() { }

		#region 序列化，反序列化
		protected static readonly string StringifySplit = "@<Market(Info]?#>";
		public string Stringify() {
			return string.Concat(
				_Id == null ? "null" : _Id.ToString(), "|",
				_Area_id == null ? "null" : _Area_id.ToString(), "|",
				_Create_time == null ? "null" : _Create_time.Value.Ticks.ToString(), "|",
				_Title == null ? "null" : _Title.Replace("|", StringifySplit));
		}
		public static MarketInfo Parse(string stringify) {
			string[] ret = stringify.Split(new char[] { '|' }, 4, StringSplitOptions.None);
			if (ret.Length != 4) throw new Exception("格式不正确，MarketInfo：" + stringify);
			MarketInfo item = new MarketInfo();
			if (string.Compare("null", ret[0]) != 0) item.Id = uint.Parse(ret[0]);
			if (string.Compare("null", ret[1]) != 0) item.Area_id = uint.Parse(ret[1]);
			if (string.Compare("null", ret[2]) != 0) item.Create_time = new DateTime(long.Parse(ret[2]));
			if (string.Compare("null", ret[3]) != 0) item.Title = ret[3].Replace(StringifySplit, "|");
			return item;
		}
		#endregion

		#region override
		private static Lazy<Dictionary<string, bool>> __jsonIgnoreLazy = new Lazy<Dictionary<string, bool>>(() => {
			FieldInfo field = typeof(MarketInfo).GetField("JsonIgnore");
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
				__jsonIgnore.ContainsKey("Area_id") ? string.Empty : string.Format(", Area_id : {0}", Area_id == null ? "null" : Area_id.ToString()), 
				__jsonIgnore.ContainsKey("Create_time") ? string.Empty : string.Format(", Create_time : {0}", Create_time == null ? "null" : Create_time.Value.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds.ToString()), 
				__jsonIgnore.ContainsKey("Title") ? string.Empty : string.Format(", Title : {0}", Title == null ? "null" : string.Format("'{0}'", Title.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), " }");
			return string.Concat("{", json.Substring(1));
		}
		public IDictionary ToBson(bool allField = false) {
			IDictionary ht = new Hashtable();
			if (allField || !__jsonIgnore.ContainsKey("Id")) ht["Id"] = Id;
			if (allField || !__jsonIgnore.ContainsKey("Area_id")) ht["Area_id"] = Area_id;
			if (allField || !__jsonIgnore.ContainsKey("Create_time")) ht["Create_time"] = Create_time;
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
		/// 城市
		/// </summary>
		[JsonProperty] public uint? Area_id {
			get { return _Area_id; }
			set {
				if (_Area_id != value) _obj_area = null;
				_Area_id = value;
			}
		}
		public AreaInfo Obj_area {
			get {
				if (_obj_area == null) _obj_area = Area.GetItem(_Area_id.Value);
				return _obj_area;
			}
			internal set { _obj_area = value; }
		}
		/// <summary>
		/// 创建时间
		/// </summary>
		[JsonProperty] public DateTime? Create_time {
			get { return _Create_time; }
			set { _Create_time = value; }
		}
		/// <summary>
		/// 市场名称
		/// </summary>
		[JsonProperty] public string Title {
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
		public Member_marketInfo Obj_member_market { set; get; }
		private List<MemberInfo> _obj_members;
		/// <summary>
		/// 遍历时，可通过 Obj_member_market 可获取中间表数据
		/// </summary>
		public List<MemberInfo> Obj_members {
			get {
				if (_obj_members == null) _obj_members = Member.Select.InnerJoin<Member_market>("b", "b.`member_id` = a.`id`").Where("b.`market_id` = {0}", _Id.Value).ToList();
				return _obj_members;
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
			get { return Market.UpdateDiy(this, _Id.Value); }
		}
		public MarketInfo Save() {
			if (this.Id != null) {
				Market.Update(this);
				return this;
			}
			this.Create_time = DateTime.Now;
			return Market.Insert(this);
		}
		public MarketdescInfo AddMarketdesc(string Content, string Url) => Marketdesc.Insert(new MarketdescInfo {
			Market_id = this.Id, 
				Content = Content, 
				Url = Url});
		public MarketdescInfo AddMarketdesc(MarketdescInfo item) {
			item.Market_id = this.Id;
			return item.Save();
		}

		public MarkettypeInfo AddMarkettype(MarkettypeInfo Markettype, byte? Sort, string Title) => AddMarkettype(Markettype.Id, Sort, Title);
		public MarkettypeInfo AddMarkettype(uint? Parent_id, byte? Sort, string Title) => Markettype.Insert(new MarkettypeInfo {
			Market_id = this.Id, 
				Parent_id = Parent_id, 
				Sort = Sort, 
				Title = Title});
		public MarkettypeInfo AddMarkettype(MarkettypeInfo item) {
			item.Market_id = this.Id;
			return item.Save();
		}

		public Member_marketInfo FlagMember(MemberInfo Member, DateTime? Create_time) => FlagMember(Member.Id, Create_time);
		public Member_marketInfo FlagMember(uint? Member_id, DateTime? Create_time) {
			Member_marketInfo item = Member_market.GetItem(this.Id.Value, Member_id.Value);
			if (item == null) item = Member_market.Insert(new Member_marketInfo {
			Market_id = this.Id, 
				Member_id = Member_id, 
				Create_time = Create_time});
			else item.UpdateDiy
					.SetCreate_time(Create_time).ExecuteNonQuery();
			return item;
		}

		public int UnflagMember(MemberInfo Member) => UnflagMember(Member.Id);
		public int UnflagMember(uint? Member_id) => Member_market.Delete(this.Id.Value, Member_id.Value);
		public int UnflagMemberALL() => Member_market.DeleteByMarket_id(this.Id);

		public RentsubletInfo AddRentsublet(DateTime? Create_time, decimal? Price, RentsubletTYPE? Type) => Rentsublet.Insert(new RentsubletInfo {
			Market_id = this.Id, 
				Create_time = Create_time, 
				Price = Price, 
				Type = Type});
		public RentsubletInfo AddRentsublet(RentsubletInfo item) {
			item.Market_id = this.Id;
			return item.Save();
		}

	}
}

