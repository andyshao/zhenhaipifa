using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using pifa.BLL;

namespace pifa.Model {

	[JsonObject(MemberSerialization.OptIn)]
	public partial class ExpressInfo {
		#region fields
		private uint? _Id;
		private uint? _Area_id;
		private AreaInfo _obj_area;
		private string _Address;
		private DateTime? _Create_time;
		private string _Service_features;
		private string _Telphone;
		private string _Title;
		#endregion

		public ExpressInfo() { }

		#region 序列化，反序列化
		protected static readonly string StringifySplit = "@<Express(Info]?#>";
		public string Stringify() {
			return string.Concat(
				_Id == null ? "null" : _Id.ToString(), "|",
				_Area_id == null ? "null" : _Area_id.ToString(), "|",
				_Address == null ? "null" : _Address.Replace("|", StringifySplit), "|",
				_Create_time == null ? "null" : _Create_time.Value.Ticks.ToString(), "|",
				_Service_features == null ? "null" : _Service_features.Replace("|", StringifySplit), "|",
				_Telphone == null ? "null" : _Telphone.Replace("|", StringifySplit), "|",
				_Title == null ? "null" : _Title.Replace("|", StringifySplit));
		}
		public static ExpressInfo Parse(string stringify) {
			string[] ret = stringify.Split(new char[] { '|' }, 7, StringSplitOptions.None);
			if (ret.Length != 7) throw new Exception("格式不正确，ExpressInfo：" + stringify);
			ExpressInfo item = new ExpressInfo();
			if (string.Compare("null", ret[0]) != 0) item.Id = uint.Parse(ret[0]);
			if (string.Compare("null", ret[1]) != 0) item.Area_id = uint.Parse(ret[1]);
			if (string.Compare("null", ret[2]) != 0) item.Address = ret[2].Replace(StringifySplit, "|");
			if (string.Compare("null", ret[3]) != 0) item.Create_time = new DateTime(long.Parse(ret[3]));
			if (string.Compare("null", ret[4]) != 0) item.Service_features = ret[4].Replace(StringifySplit, "|");
			if (string.Compare("null", ret[5]) != 0) item.Telphone = ret[5].Replace(StringifySplit, "|");
			if (string.Compare("null", ret[6]) != 0) item.Title = ret[6].Replace(StringifySplit, "|");
			return item;
		}
		#endregion

		#region override
		private static Lazy<Dictionary<string, bool>> __jsonIgnoreLazy = new Lazy<Dictionary<string, bool>>(() => {
			FieldInfo field = typeof(ExpressInfo).GetField("JsonIgnore");
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
				__jsonIgnore.ContainsKey("Address") ? string.Empty : string.Format(", Address : {0}", Address == null ? "null" : string.Format("'{0}'", Address.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), 
				__jsonIgnore.ContainsKey("Create_time") ? string.Empty : string.Format(", Create_time : {0}", Create_time == null ? "null" : Create_time.Value.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds.ToString()), 
				__jsonIgnore.ContainsKey("Service_features") ? string.Empty : string.Format(", Service_features : {0}", Service_features == null ? "null" : string.Format("'{0}'", Service_features.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), 
				__jsonIgnore.ContainsKey("Telphone") ? string.Empty : string.Format(", Telphone : {0}", Telphone == null ? "null" : string.Format("'{0}'", Telphone.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), 
				__jsonIgnore.ContainsKey("Title") ? string.Empty : string.Format(", Title : {0}", Title == null ? "null" : string.Format("'{0}'", Title.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), " }");
			return string.Concat("{", json.Substring(1));
		}
		public IDictionary ToBson(bool allField = false) {
			IDictionary ht = new Hashtable();
			if (allField || !__jsonIgnore.ContainsKey("Id")) ht["Id"] = Id;
			if (allField || !__jsonIgnore.ContainsKey("Area_id")) ht["Area_id"] = Area_id;
			if (allField || !__jsonIgnore.ContainsKey("Address")) ht["Address"] = Address;
			if (allField || !__jsonIgnore.ContainsKey("Create_time")) ht["Create_time"] = Create_time;
			if (allField || !__jsonIgnore.ContainsKey("Service_features")) ht["Service_features"] = Service_features;
			if (allField || !__jsonIgnore.ContainsKey("Telphone")) ht["Telphone"] = Telphone;
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
		/// 地址
		/// </summary>
		[JsonProperty] public string Address {
			get { return _Address; }
			set { _Address = value; }
		}
		/// <summary>
		/// 创建时间
		/// </summary>
		[JsonProperty] public DateTime? Create_time {
			get { return _Create_time; }
			set { _Create_time = value; }
		}
		/// <summary>
		/// 服务特色
		/// </summary>
		[JsonProperty] public string Service_features {
			get { return _Service_features; }
			set { _Service_features = value; }
		}
		/// <summary>
		/// 联系电话
		/// </summary>
		[JsonProperty] public string Telphone {
			get { return _Telphone; }
			set { _Telphone = value; }
		}
		/// <summary>
		/// 物流名称
		/// </summary>
		[JsonProperty] public string Title {
			get { return _Title; }
			set { _Title = value; }
		}
		private List<ExpressdescInfo> _obj_expressdescs;
		public List<ExpressdescInfo> Obj_expressdescs {
			get {
				if (_obj_expressdescs == null) _obj_expressdescs = Expressdesc.SelectByExpress_id(_Id).Limit(500).ToList();
				return _obj_expressdescs;
			}
		}
		#endregion

		public pifa.DAL.Express.SqlUpdateBuild UpdateDiy {
			get { return Express.UpdateDiy(this, _Id.Value); }
		}
		public ExpressInfo Save() {
			if (this.Id != null) {
				Express.Update(this);
				return this;
			}
			this.Create_time = DateTime.Now;
			return Express.Insert(this);
		}
		public ExpressdescInfo AddExpressdesc(string Content) => Expressdesc.Insert(new ExpressdescInfo {
			Express_id = this.Id, 
				Content = Content});
		public ExpressdescInfo AddExpressdesc(ExpressdescInfo item) {
			item.Express_id = this.Id;
			return item.Save();
		}

	}
}

