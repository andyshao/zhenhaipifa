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
	public partial class RentsubletInfo {
		#region fields
		private uint? _Id;
		private uint? _Market_id;
		private MarketInfo _obj_market;
		private DateTime? _Create_time;
		private decimal? _Price;
		private RentsubletTYPE? _Type;
		#endregion

		public RentsubletInfo() { }

		#region 序列化，反序列化
		protected static readonly string StringifySplit = "@<Rentsublet(Info]?#>";
		public string Stringify() {
			return string.Concat(
				_Id == null ? "null" : _Id.ToString(), "|",
				_Market_id == null ? "null" : _Market_id.ToString(), "|",
				_Create_time == null ? "null" : _Create_time.Value.Ticks.ToString(), "|",
				_Price == null ? "null" : _Price.ToString(), "|",
				_Type == null ? "null" : _Type.ToInt64().ToString());
		}
		public static RentsubletInfo Parse(string stringify) {
			string[] ret = stringify.Split(new char[] { '|' }, 5, StringSplitOptions.None);
			if (ret.Length != 5) throw new Exception("格式不正确，RentsubletInfo：" + stringify);
			RentsubletInfo item = new RentsubletInfo();
			if (string.Compare("null", ret[0]) != 0) item.Id = uint.Parse(ret[0]);
			if (string.Compare("null", ret[1]) != 0) item.Market_id = uint.Parse(ret[1]);
			if (string.Compare("null", ret[2]) != 0) item.Create_time = new DateTime(long.Parse(ret[2]));
			if (string.Compare("null", ret[3]) != 0) item.Price = decimal.Parse(ret[3]);
			if (string.Compare("null", ret[4]) != 0) item.Type = (RentsubletTYPE)long.Parse(ret[4]);
			return item;
		}
		#endregion

		#region override
		private static Lazy<Dictionary<string, bool>> __jsonIgnoreLazy = new Lazy<Dictionary<string, bool>>(() => {
			FieldInfo field = typeof(RentsubletInfo).GetField("JsonIgnore");
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
				__jsonIgnore.ContainsKey("Create_time") ? string.Empty : string.Format(", Create_time : {0}", Create_time == null ? "null" : Create_time.Value.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds.ToString()), 
				__jsonIgnore.ContainsKey("Price") ? string.Empty : string.Format(", Price : {0}", Price == null ? "null" : Price.ToString()), 
				__jsonIgnore.ContainsKey("Type") ? string.Empty : string.Format(", Type : {0}", Type == null ? "null" : string.Format("'{0}'", Type.ToDescriptionOrString().Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), " }");
			return string.Concat("{", json.Substring(1));
		}
		public IDictionary ToBson(bool allField = false) {
			IDictionary ht = new Hashtable();
			if (allField || !__jsonIgnore.ContainsKey("Id")) ht["Id"] = Id;
			if (allField || !__jsonIgnore.ContainsKey("Market_id")) ht["Market_id"] = Market_id;
			if (allField || !__jsonIgnore.ContainsKey("Create_time")) ht["Create_time"] = Create_time;
			if (allField || !__jsonIgnore.ContainsKey("Price")) ht["Price"] = Price;
			if (allField || !__jsonIgnore.ContainsKey("Type")) ht["Type"] = Type?.ToDescriptionOrString();
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
		/// 创建时间
		/// </summary>
		[JsonProperty] public DateTime? Create_time {
			get { return _Create_time; }
			set { _Create_time = value; }
		}
		/// <summary>
		/// 费用
		/// </summary>
		[JsonProperty] public decimal? Price {
			get { return _Price; }
			set { _Price = value; }
		}
		/// <summary>
		/// 类型
		/// </summary>
		[JsonProperty] public RentsubletTYPE? Type {
			get { return _Type; }
			set { _Type = value; }
		}
		private List<FranchisingInfo> _obj_franchisings;
		public List<FranchisingInfo> Obj_franchisings {
			get {
				if (_obj_franchisings == null) _obj_franchisings = Franchising.SelectByRentsublet_id(_Id.Value).ToList();
				return _obj_franchisings;
			}
		}
		#endregion

		public pifa.DAL.Rentsublet.SqlUpdateBuild UpdateDiy {
			get { return Rentsublet.UpdateDiy(this, _Id.Value); }
		}
		public RentsubletInfo Save() {
			if (this.Id != null) {
				Rentsublet.Update(this);
				return this;
			}
			this.Create_time = DateTime.Now;
			return Rentsublet.Insert(this);
		}
		public Rentsublet_franchisingInfo FlagFranchising(FranchisingInfo Franchising) => FlagFranchising(Franchising.Id);
		public Rentsublet_franchisingInfo FlagFranchising(uint? Franchising_id) {
			Rentsublet_franchisingInfo item = Rentsublet_franchising.GetItem(Franchising_id.Value, this.Id.Value);
			if (item == null) item = Rentsublet_franchising.Insert(new Rentsublet_franchisingInfo {
				Franchising_id = Franchising_id, 
			Rentsublet_id = this.Id});
			return item;
		}

		public int UnflagFranchising(FranchisingInfo Franchising) => UnflagFranchising(Franchising.Id);
		public int UnflagFranchising(uint? Franchising_id) => Rentsublet_franchising.Delete(Franchising_id.Value, this.Id.Value);
		public int UnflagFranchisingALL() => Rentsublet_franchising.DeleteByRentsublet_id(this.Id);

	}
	public enum RentsubletTYPE {
		/// <summary>
		/// ??
		/// </summary>
		[Description("??")]
		Unknow1 = 1, 
		/// <summary>
		/// ??
		/// </summary>
		[Description("??")]
		Unknow2, 
		/// <summary>
		/// ??
		/// </summary>
		[Description("??")]
		Unknow3
	}
}

