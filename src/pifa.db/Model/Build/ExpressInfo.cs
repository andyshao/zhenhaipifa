using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using pifa.BLL;

namespace pifa.Model {

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

		#region 独创的序列化，反序列化
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
		public ExpressInfo(string stringify) {
			string[] ret = stringify.Split(new char[] { '|' }, 7, StringSplitOptions.None);
			if (ret.Length != 7) throw new Exception("格式不正确，ExpressInfo：" + stringify);
			if (string.Compare("null", ret[0]) != 0) _Id = uint.Parse(ret[0]);
			if (string.Compare("null", ret[1]) != 0) _Area_id = uint.Parse(ret[1]);
			if (string.Compare("null", ret[2]) != 0) _Address = ret[2].Replace(StringifySplit, "|");
			if (string.Compare("null", ret[3]) != 0) _Create_time = new DateTime(long.Parse(ret[3]));
			if (string.Compare("null", ret[4]) != 0) _Service_features = ret[4].Replace(StringifySplit, "|");
			if (string.Compare("null", ret[5]) != 0) _Telphone = ret[5].Replace(StringifySplit, "|");
			if (string.Compare("null", ret[6]) != 0) _Title = ret[6].Replace(StringifySplit, "|");
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
				__jsonIgnore.ContainsKey("Address") ? string.Empty : string.Format(", Address : {0}", Address == null ? "null" : string.Format("'{0}'", Address.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), 
				__jsonIgnore.ContainsKey("Create_time") ? string.Empty : string.Format(", Create_time : {0}", Create_time == null ? "null" : Create_time.Value.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds.ToString()), 
				__jsonIgnore.ContainsKey("Service_features") ? string.Empty : string.Format(", Service_features : {0}", Service_features == null ? "null" : string.Format("'{0}'", Service_features.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), 
				__jsonIgnore.ContainsKey("Telphone") ? string.Empty : string.Format(", Telphone : {0}", Telphone == null ? "null" : string.Format("'{0}'", Telphone.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), 
				__jsonIgnore.ContainsKey("Title") ? string.Empty : string.Format(", Title : {0}", Title == null ? "null" : string.Format("'{0}'", Title.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), " }");
			return string.Concat("{", json.Substring(1));
		}
		public IDictionary ToBson() {
			this.Init__jsonIgnore();
			IDictionary ht = new Hashtable();
			if (!__jsonIgnore.ContainsKey("Id")) ht["Id"] = Id;
			if (!__jsonIgnore.ContainsKey("Area_id")) ht["Area_id"] = Area_id;
			if (!__jsonIgnore.ContainsKey("Address")) ht["Address"] = Address;
			if (!__jsonIgnore.ContainsKey("Create_time")) ht["Create_time"] = Create_time;
			if (!__jsonIgnore.ContainsKey("Service_features")) ht["Service_features"] = Service_features;
			if (!__jsonIgnore.ContainsKey("Telphone")) ht["Telphone"] = Telphone;
			if (!__jsonIgnore.ContainsKey("Title")) ht["Title"] = Title;
			return ht;
		}
		private void Init__jsonIgnore() {
			if (__jsonIgnore == null) {
				lock (__jsonIgnore_lock) {
					if (__jsonIgnore == null) {
						FieldInfo field = typeof(ExpressInfo).GetField("JsonIgnore");
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
			ExpressInfo item = obj as ExpressInfo;
			if (item == null) return false;
			return this.ToString().Equals(item.ToString());
		}
		public override int GetHashCode() {
			return this.ToString().GetHashCode();
		}
		public static bool operator ==(ExpressInfo op1, ExpressInfo op2) {
			if (object.Equals(op1, null)) return object.Equals(op2, null);
			return op1.Equals(op2);
		}
		public static bool operator !=(ExpressInfo op1, ExpressInfo op2) {
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
		/// 区域
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
		/// 地址
		/// </summary>
		public string Address {
			get { return _Address; }
			set { _Address = value; }
		}
		/// <summary>
		/// 创建时间
		/// </summary>
		public DateTime? Create_time {
			get { return _Create_time; }
			set { _Create_time = value; }
		}
		/// <summary>
		/// 服务特色
		/// </summary>
		public string Service_features {
			get { return _Service_features; }
			set { _Service_features = value; }
		}
		/// <summary>
		/// 联系电话
		/// </summary>
		public string Telphone {
			get { return _Telphone; }
			set { _Telphone = value; }
		}
		/// <summary>
		/// 物流名称
		/// </summary>
		public string Title {
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
			get { return Express.UpdateDiy(this, _Id); }
		}
		public ExpressdescInfo AddExpressdesc(string Content) {
			return Expressdesc.Insert(new ExpressdescInfo {
				Express_id = this.Id, 
				Content = Content});
		}

	}
}

