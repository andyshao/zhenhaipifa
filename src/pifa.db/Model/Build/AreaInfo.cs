using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using pifa.BLL;

namespace pifa.Model {

	public partial class AreaInfo {
		#region fields
		private uint? _Id;
		private uint? _Parent_id;
		private AreaInfo _obj_area;
		private string _Name;
		#endregion

		public AreaInfo() { }

		#region 独创的序列化，反序列化
		protected static readonly string StringifySplit = "@<Area(Info]?#>";
		public string Stringify() {
			return string.Concat(
				_Id == null ? "null" : _Id.ToString(), "|",
				_Parent_id == null ? "null" : _Parent_id.ToString(), "|",
				_Name == null ? "null" : _Name.Replace("|", StringifySplit));
		}
		public AreaInfo(string stringify) {
			string[] ret = stringify.Split(new char[] { '|' }, 3, StringSplitOptions.None);
			if (ret.Length != 3) throw new Exception("格式不正确，AreaInfo：" + stringify);
			if (string.Compare("null", ret[0]) != 0) _Id = uint.Parse(ret[0]);
			if (string.Compare("null", ret[1]) != 0) _Parent_id = uint.Parse(ret[1]);
			if (string.Compare("null", ret[2]) != 0) _Name = ret[2].Replace(StringifySplit, "|");
		}
		#endregion

		#region override
		private static Dictionary<string, bool> __jsonIgnore;
		private static object __jsonIgnore_lock = new object();
		public override string ToString() {
			this.Init__jsonIgnore();
			string json = string.Concat(
				__jsonIgnore.ContainsKey("Id") ? string.Empty : string.Format(", Id : {0}", Id == null ? "null" : Id.ToString()), 
				__jsonIgnore.ContainsKey("Parent_id") ? string.Empty : string.Format(", Parent_id : {0}", Parent_id == null ? "null" : Parent_id.ToString()), 
				__jsonIgnore.ContainsKey("Name") ? string.Empty : string.Format(", Name : {0}", Name == null ? "null" : string.Format("'{0}'", Name.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), " }");
			return string.Concat("{", json.Substring(1));
		}
		public IDictionary ToBson() {
			this.Init__jsonIgnore();
			IDictionary ht = new Hashtable();
			if (!__jsonIgnore.ContainsKey("Id")) ht["Id"] = Id;
			if (!__jsonIgnore.ContainsKey("Parent_id")) ht["Parent_id"] = Parent_id;
			if (!__jsonIgnore.ContainsKey("Name")) ht["Name"] = Name;
			return ht;
		}
		private void Init__jsonIgnore() {
			if (__jsonIgnore == null) {
				lock (__jsonIgnore_lock) {
					if (__jsonIgnore == null) {
						FieldInfo field = typeof(AreaInfo).GetField("JsonIgnore");
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
			AreaInfo item = obj as AreaInfo;
			if (item == null) return false;
			return this.ToString().Equals(item.ToString());
		}
		public override int GetHashCode() {
			return this.ToString().GetHashCode();
		}
		public static bool operator ==(AreaInfo op1, AreaInfo op2) {
			if (object.Equals(op1, null)) return object.Equals(op2, null);
			return op1.Equals(op2);
		}
		public static bool operator !=(AreaInfo op1, AreaInfo op2) {
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
		/// 父
		/// </summary>
		public uint? Parent_id {
			get { return _Parent_id; }
			set {
				if (_Parent_id != value) _obj_area = null;
				_Parent_id = value;
			}
		}
		public AreaInfo Obj_area {
			get {
				if (_obj_area == null) _obj_area = Area.GetItem(_Parent_id);
				return _obj_area;
			}
			internal set { _obj_area = value; }
		}
		/// <summary>
		/// 城市
		/// </summary>
		public string Name {
			get { return _Name; }
			set { _Name = value; }
		}
		private List<AreaInfo> _obj_areas;
		public List<AreaInfo> Obj_areas {
			get {
				if (_obj_areas == null) _obj_areas = Area.SelectByParent_id(_Id).Limit(500).ToList();
				return _obj_areas;
			}
		}
		private List<CategoryInfo> _obj_categorys;
		public List<CategoryInfo> Obj_categorys {
			get {
				if (_obj_categorys == null) _obj_categorys = Category.SelectByArea_id(_Id.Value).ToList();
				return _obj_categorys;
			}
		}
		private List<ExpressInfo> _obj_expresss;
		public List<ExpressInfo> Obj_expresss {
			get {
				if (_obj_expresss == null) _obj_expresss = Express.SelectByArea_id(_Id).Limit(500).ToList();
				return _obj_expresss;
			}
		}
		private List<FactoryInfo> _obj_factorys;
		public List<FactoryInfo> Obj_factorys {
			get {
				if (_obj_factorys == null) _obj_factorys = Factory.SelectByArea_id(_Id).Limit(500).ToList();
				return _obj_factorys;
			}
		}
		private List<MarketInfo> _obj_markets;
		public List<MarketInfo> Obj_markets {
			get {
				if (_obj_markets == null) _obj_markets = Market.SelectByArea_id(_Id).Limit(500).ToList();
				return _obj_markets;
			}
		}
		#endregion

		public pifa.DAL.Area.SqlUpdateBuild UpdateDiy {
			get { return Area.UpdateDiy(this, _Id); }
		}
		public AreaInfo AddArea(string Name) {
			return Area.Insert(new AreaInfo {
				Parent_id = this.Id, 
				Name = Name});
		}

		public Area_categoryInfo FlagCategory(CategoryInfo Category) {
			return FlagCategory(Category.Id);
		}
		public Area_categoryInfo FlagCategory(uint? Category_id) {
			Area_categoryInfo item = Area_category.GetItem(this.Id, Category_id);
			if (item == null) item = Area_category.Insert(new Area_categoryInfo {
				Area_id = this.Id, 
				Category_id = Category_id});
			return item;
		}

		public int UnflagCategory(CategoryInfo Category) {
			return UnflagCategory(Category.Id);
		}
		public int UnflagCategory(uint? Category_id) {
			return Area_category.Delete(this.Id, Category_id);
		}
		public int UnflagCategoryALL() {
			return Area_category.DeleteByArea_id(this.Id);
		}

		public ExpressInfo AddExpress(string Address, DateTime? Create_time, string Service_features, string Telphone, string Title) {
			return Express.Insert(new ExpressInfo {
				Area_id = this.Id, 
				Address = Address, 
				Create_time = Create_time, 
				Service_features = Service_features, 
				Telphone = Telphone, 
				Title = Title});
		}

		public FactoryInfo AddFactory(string Capacity, DateTime? Create_time, string Main_business, string Min_order, string Process_cost, string Sampling_period, string Sampling_price, string Telphone, string Title, string Turn_single_time) {
			return Factory.Insert(new FactoryInfo {
				Area_id = this.Id, 
				Capacity = Capacity, 
				Create_time = Create_time, 
				Main_business = Main_business, 
				Min_order = Min_order, 
				Process_cost = Process_cost, 
				Sampling_period = Sampling_period, 
				Sampling_price = Sampling_price, 
				Telphone = Telphone, 
				Title = Title, 
				Turn_single_time = Turn_single_time});
		}

		public MarketInfo AddMarket(DateTime? Create_time, string Title) {
			return Market.Insert(new MarketInfo {
				Area_id = this.Id, 
				Create_time = Create_time, 
				Title = Title});
		}

	}
}

