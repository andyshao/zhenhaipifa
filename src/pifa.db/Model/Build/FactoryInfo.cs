using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using pifa.BLL;

namespace pifa.Model {

	[JsonObject(MemberSerialization.OptIn)]
	public partial class FactoryInfo {
		#region fields
		private uint? _Id;
		private uint? _Area_id;
		private AreaInfo _obj_area;
		private string _Capacity;
		private DateTime? _Create_time;
		private string _Main_business;
		private string _Min_order;
		private string _Process_cost;
		private string _Sampling_period;
		private string _Sampling_price;
		private string _Telphone;
		private string _Title;
		private string _Turn_single_time;
		#endregion

		public FactoryInfo() { }

		#region 序列化，反序列化
		protected static readonly string StringifySplit = "@<Factory(Info]?#>";
		public string Stringify() {
			return string.Concat(
				_Id == null ? "null" : _Id.ToString(), "|",
				_Area_id == null ? "null" : _Area_id.ToString(), "|",
				_Capacity == null ? "null" : _Capacity.Replace("|", StringifySplit), "|",
				_Create_time == null ? "null" : _Create_time.Value.Ticks.ToString(), "|",
				_Main_business == null ? "null" : _Main_business.Replace("|", StringifySplit), "|",
				_Min_order == null ? "null" : _Min_order.Replace("|", StringifySplit), "|",
				_Process_cost == null ? "null" : _Process_cost.Replace("|", StringifySplit), "|",
				_Sampling_period == null ? "null" : _Sampling_period.Replace("|", StringifySplit), "|",
				_Sampling_price == null ? "null" : _Sampling_price.Replace("|", StringifySplit), "|",
				_Telphone == null ? "null" : _Telphone.Replace("|", StringifySplit), "|",
				_Title == null ? "null" : _Title.Replace("|", StringifySplit), "|",
				_Turn_single_time == null ? "null" : _Turn_single_time.Replace("|", StringifySplit));
		}
		public static FactoryInfo Parse(string stringify) {
			string[] ret = stringify.Split(new char[] { '|' }, 12, StringSplitOptions.None);
			if (ret.Length != 12) throw new Exception("格式不正确，FactoryInfo：" + stringify);
			FactoryInfo item = new FactoryInfo();
			if (string.Compare("null", ret[0]) != 0) item.Id = uint.Parse(ret[0]);
			if (string.Compare("null", ret[1]) != 0) item.Area_id = uint.Parse(ret[1]);
			if (string.Compare("null", ret[2]) != 0) item.Capacity = ret[2].Replace(StringifySplit, "|");
			if (string.Compare("null", ret[3]) != 0) item.Create_time = new DateTime(long.Parse(ret[3]));
			if (string.Compare("null", ret[4]) != 0) item.Main_business = ret[4].Replace(StringifySplit, "|");
			if (string.Compare("null", ret[5]) != 0) item.Min_order = ret[5].Replace(StringifySplit, "|");
			if (string.Compare("null", ret[6]) != 0) item.Process_cost = ret[6].Replace(StringifySplit, "|");
			if (string.Compare("null", ret[7]) != 0) item.Sampling_period = ret[7].Replace(StringifySplit, "|");
			if (string.Compare("null", ret[8]) != 0) item.Sampling_price = ret[8].Replace(StringifySplit, "|");
			if (string.Compare("null", ret[9]) != 0) item.Telphone = ret[9].Replace(StringifySplit, "|");
			if (string.Compare("null", ret[10]) != 0) item.Title = ret[10].Replace(StringifySplit, "|");
			if (string.Compare("null", ret[11]) != 0) item.Turn_single_time = ret[11].Replace(StringifySplit, "|");
			return item;
		}
		#endregion

		#region override
		private static Lazy<Dictionary<string, bool>> __jsonIgnoreLazy = new Lazy<Dictionary<string, bool>>(() => {
			FieldInfo field = typeof(FactoryInfo).GetField("JsonIgnore");
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
				__jsonIgnore.ContainsKey("Capacity") ? string.Empty : string.Format(", Capacity : {0}", Capacity == null ? "null" : string.Format("'{0}'", Capacity.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), 
				__jsonIgnore.ContainsKey("Create_time") ? string.Empty : string.Format(", Create_time : {0}", Create_time == null ? "null" : Create_time.Value.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds.ToString()), 
				__jsonIgnore.ContainsKey("Main_business") ? string.Empty : string.Format(", Main_business : {0}", Main_business == null ? "null" : string.Format("'{0}'", Main_business.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), 
				__jsonIgnore.ContainsKey("Min_order") ? string.Empty : string.Format(", Min_order : {0}", Min_order == null ? "null" : string.Format("'{0}'", Min_order.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), 
				__jsonIgnore.ContainsKey("Process_cost") ? string.Empty : string.Format(", Process_cost : {0}", Process_cost == null ? "null" : string.Format("'{0}'", Process_cost.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), 
				__jsonIgnore.ContainsKey("Sampling_period") ? string.Empty : string.Format(", Sampling_period : {0}", Sampling_period == null ? "null" : string.Format("'{0}'", Sampling_period.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), 
				__jsonIgnore.ContainsKey("Sampling_price") ? string.Empty : string.Format(", Sampling_price : {0}", Sampling_price == null ? "null" : string.Format("'{0}'", Sampling_price.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), 
				__jsonIgnore.ContainsKey("Telphone") ? string.Empty : string.Format(", Telphone : {0}", Telphone == null ? "null" : string.Format("'{0}'", Telphone.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), 
				__jsonIgnore.ContainsKey("Title") ? string.Empty : string.Format(", Title : {0}", Title == null ? "null" : string.Format("'{0}'", Title.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), 
				__jsonIgnore.ContainsKey("Turn_single_time") ? string.Empty : string.Format(", Turn_single_time : {0}", Turn_single_time == null ? "null" : string.Format("'{0}'", Turn_single_time.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), " }");
			return string.Concat("{", json.Substring(1));
		}
		public IDictionary ToBson(bool allField = false) {
			IDictionary ht = new Hashtable();
			if (allField || !__jsonIgnore.ContainsKey("Id")) ht["Id"] = Id;
			if (allField || !__jsonIgnore.ContainsKey("Area_id")) ht["Area_id"] = Area_id;
			if (allField || !__jsonIgnore.ContainsKey("Capacity")) ht["Capacity"] = Capacity;
			if (allField || !__jsonIgnore.ContainsKey("Create_time")) ht["Create_time"] = Create_time;
			if (allField || !__jsonIgnore.ContainsKey("Main_business")) ht["Main_business"] = Main_business;
			if (allField || !__jsonIgnore.ContainsKey("Min_order")) ht["Min_order"] = Min_order;
			if (allField || !__jsonIgnore.ContainsKey("Process_cost")) ht["Process_cost"] = Process_cost;
			if (allField || !__jsonIgnore.ContainsKey("Sampling_period")) ht["Sampling_period"] = Sampling_period;
			if (allField || !__jsonIgnore.ContainsKey("Sampling_price")) ht["Sampling_price"] = Sampling_price;
			if (allField || !__jsonIgnore.ContainsKey("Telphone")) ht["Telphone"] = Telphone;
			if (allField || !__jsonIgnore.ContainsKey("Title")) ht["Title"] = Title;
			if (allField || !__jsonIgnore.ContainsKey("Turn_single_time")) ht["Turn_single_time"] = Turn_single_time;
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
		/// 区域
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
		/// 生产产能
		/// </summary>
		[JsonProperty] public string Capacity {
			get { return _Capacity; }
			set { _Capacity = value; }
		}
		/// <summary>
		/// 创建时间
		/// </summary>
		[JsonProperty] public DateTime? Create_time {
			get { return _Create_time; }
			set { _Create_time = value; }
		}
		/// <summary>
		/// 加工品类
		/// </summary>
		[JsonProperty] public string Main_business {
			get { return _Main_business; }
			set { _Main_business = value; }
		}
		/// <summary>
		/// 起订量
		/// </summary>
		[JsonProperty] public string Min_order {
			get { return _Min_order; }
			set { _Min_order = value; }
		}
		/// <summary>
		/// 加工费
		/// </summary>
		[JsonProperty] public string Process_cost {
			get { return _Process_cost; }
			set { _Process_cost = value; }
		}
		/// <summary>
		/// 打样周期
		/// </summary>
		[JsonProperty] public string Sampling_period {
			get { return _Sampling_period; }
			set { _Sampling_period = value; }
		}
		/// <summary>
		/// 打样费用
		/// </summary>
		[JsonProperty] public string Sampling_price {
			get { return _Sampling_price; }
			set { _Sampling_price = value; }
		}
		/// <summary>
		/// 联系电话
		/// </summary>
		[JsonProperty] public string Telphone {
			get { return _Telphone; }
			set { _Telphone = value; }
		}
		/// <summary>
		/// 工厂名称
		/// </summary>
		[JsonProperty] public string Title {
			get { return _Title; }
			set { _Title = value; }
		}
		/// <summary>
		/// 翻单时间
		/// </summary>
		[JsonProperty] public string Turn_single_time {
			get { return _Turn_single_time; }
			set { _Turn_single_time = value; }
		}
		private List<FranchisingInfo> _obj_franchisings;
		public List<FranchisingInfo> Obj_franchisings {
			get {
				if (_obj_franchisings == null) _obj_franchisings = Franchising.SelectByFactory_id(_Id.Value).ToList();
				return _obj_franchisings;
			}
		}
		private List<FactorydescInfo> _obj_factorydescs;
		public List<FactorydescInfo> Obj_factorydescs {
			get {
				if (_obj_factorydescs == null) _obj_factorydescs = Factorydesc.SelectByFactory_id(_Id).Limit(500).ToList();
				return _obj_factorydescs;
			}
		}
		#endregion

		public pifa.DAL.Factory.SqlUpdateBuild UpdateDiy {
			get { return Factory.UpdateDiy(this, _Id.Value); }
		}
		public FactoryInfo Save() {
			if (this.Id != null) {
				Factory.Update(this);
				return this;
			}
			this.Create_time = DateTime.Now;
			return Factory.Insert(this);
		}
		public Factory_franchisingInfo FlagFranchising(FranchisingInfo Franchising) => FlagFranchising(Franchising.Id);
		public Factory_franchisingInfo FlagFranchising(uint? Franchising_id) {
			Factory_franchisingInfo item = Factory_franchising.GetItem(this.Id.Value, Franchising_id.Value);
			if (item == null) item = Factory_franchising.Insert(new Factory_franchisingInfo {
			Factory_id = this.Id, 
				Franchising_id = Franchising_id});
			return item;
		}

		public int UnflagFranchising(FranchisingInfo Franchising) => UnflagFranchising(Franchising.Id);
		public int UnflagFranchising(uint? Franchising_id) => Factory_franchising.Delete(this.Id.Value, Franchising_id.Value);
		public int UnflagFranchisingALL() => Factory_franchising.DeleteByFactory_id(this.Id);

		public FactorydescInfo AddFactorydesc(string Address, string Content, string Url, string Username) => Factorydesc.Insert(new FactorydescInfo {
			Factory_id = this.Id, 
				Address = Address, 
				Content = Content, 
				Url = Url, 
				Username = Username});
		public FactorydescInfo AddFactorydesc(FactorydescInfo item) {
			item.Factory_id = this.Id;
			return item.Save();
		}

	}
}

