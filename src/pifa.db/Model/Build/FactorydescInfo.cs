using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using pifa.BLL;

namespace pifa.Model {

	public partial class FactorydescInfo {
		#region fields
		private uint? _Factory_id;
		private FactoryInfo _obj_factory;
		private string _Address;
		private string _Content;
		private string _Url;
		private string _Username;
		#endregion

		public FactorydescInfo() { }

		#region 独创的序列化，反序列化
		protected static readonly string StringifySplit = "@<Factorydesc(Info]?#>";
		public string Stringify() {
			return string.Concat(
				_Factory_id == null ? "null" : _Factory_id.ToString(), "|",
				_Address == null ? "null" : _Address.Replace("|", StringifySplit), "|",
				_Content == null ? "null" : _Content.Replace("|", StringifySplit), "|",
				_Url == null ? "null" : _Url.Replace("|", StringifySplit), "|",
				_Username == null ? "null" : _Username.Replace("|", StringifySplit));
		}
		public FactorydescInfo(string stringify) {
			string[] ret = stringify.Split(new char[] { '|' }, 5, StringSplitOptions.None);
			if (ret.Length != 5) throw new Exception("格式不正确，FactorydescInfo：" + stringify);
			if (string.Compare("null", ret[0]) != 0) _Factory_id = uint.Parse(ret[0]);
			if (string.Compare("null", ret[1]) != 0) _Address = ret[1].Replace(StringifySplit, "|");
			if (string.Compare("null", ret[2]) != 0) _Content = ret[2].Replace(StringifySplit, "|");
			if (string.Compare("null", ret[3]) != 0) _Url = ret[3].Replace(StringifySplit, "|");
			if (string.Compare("null", ret[4]) != 0) _Username = ret[4].Replace(StringifySplit, "|");
		}
		#endregion

		#region override
		private static Dictionary<string, bool> __jsonIgnore;
		private static object __jsonIgnore_lock = new object();
		public override string ToString() {
			this.Init__jsonIgnore();
			string json = string.Concat(
				__jsonIgnore.ContainsKey("Factory_id") ? string.Empty : string.Format(", Factory_id : {0}", Factory_id == null ? "null" : Factory_id.ToString()), 
				__jsonIgnore.ContainsKey("Address") ? string.Empty : string.Format(", Address : {0}", Address == null ? "null" : string.Format("'{0}'", Address.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), 
				__jsonIgnore.ContainsKey("Content") ? string.Empty : string.Format(", Content : {0}", Content == null ? "null" : string.Format("'{0}'", Content.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), 
				__jsonIgnore.ContainsKey("Url") ? string.Empty : string.Format(", Url : {0}", Url == null ? "null" : string.Format("'{0}'", Url.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), 
				__jsonIgnore.ContainsKey("Username") ? string.Empty : string.Format(", Username : {0}", Username == null ? "null" : string.Format("'{0}'", Username.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), " }");
			return string.Concat("{", json.Substring(1));
		}
		public IDictionary ToBson() {
			this.Init__jsonIgnore();
			IDictionary ht = new Hashtable();
			if (!__jsonIgnore.ContainsKey("Factory_id")) ht["Factory_id"] = Factory_id;
			if (!__jsonIgnore.ContainsKey("Address")) ht["Address"] = Address;
			if (!__jsonIgnore.ContainsKey("Content")) ht["Content"] = Content;
			if (!__jsonIgnore.ContainsKey("Url")) ht["Url"] = Url;
			if (!__jsonIgnore.ContainsKey("Username")) ht["Username"] = Username;
			return ht;
		}
		private void Init__jsonIgnore() {
			if (__jsonIgnore == null) {
				lock (__jsonIgnore_lock) {
					if (__jsonIgnore == null) {
						FieldInfo field = typeof(FactorydescInfo).GetField("JsonIgnore");
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
			FactorydescInfo item = obj as FactorydescInfo;
			if (item == null) return false;
			return this.ToString().Equals(item.ToString());
		}
		public override int GetHashCode() {
			return this.ToString().GetHashCode();
		}
		public static bool operator ==(FactorydescInfo op1, FactorydescInfo op2) {
			if (object.Equals(op1, null)) return object.Equals(op2, null);
			return op1.Equals(op2);
		}
		public static bool operator !=(FactorydescInfo op1, FactorydescInfo op2) {
			return !(op1 == op2);
		}
		public object this[string key] {
			get { return this.GetType().GetProperty(key).GetValue(this); }
			set { this.GetType().GetProperty(key).SetValue(this, value); }
		}
		#endregion

		#region properties
		public uint? Factory_id {
			get { return _Factory_id; }
			set {
				if (_Factory_id != value) _obj_factory = null;
				_Factory_id = value;
			}
		}
		public FactoryInfo Obj_factory {
			get {
				if (_obj_factory == null) _obj_factory = Factory.GetItem(_Factory_id);
				return _obj_factory;
			}
			internal set { _obj_factory = value; }
		}
		/// <summary>
		/// 联系地址
		/// </summary>
		public string Address {
			get { return _Address; }
			set { _Address = value; }
		}
		/// <summary>
		/// 公司介绍
		/// </summary>
		public string Content {
			get { return _Content; }
			set { _Content = value; }
		}
		/// <summary>
		/// 公司官网
		/// </summary>
		public string Url {
			get { return _Url; }
			set { _Url = value; }
		}
		/// <summary>
		/// 联系人
		/// </summary>
		public string Username {
			get { return _Username; }
			set { _Username = value; }
		}
		#endregion

		public pifa.DAL.Factorydesc.SqlUpdateBuild UpdateDiy {
			get { return Factorydesc.UpdateDiy(this, _Factory_id); }
		}
	}
}

