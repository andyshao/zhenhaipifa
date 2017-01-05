using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using pifa.BLL;

namespace pifa.Model {

	[JsonObject(MemberSerialization.OptIn)]
	public partial class Member_securityInfo {
		#region fields
		private uint? _Member_id;
		private MemberInfo _obj_member;
		private string _Password;
		#endregion

		public Member_securityInfo() { }

		#region 序列化，反序列化
		protected static readonly string StringifySplit = "@<Member_security(Info]?#>";
		public string Stringify() {
			return string.Concat(
				_Member_id == null ? "null" : _Member_id.ToString(), "|",
				_Password == null ? "null" : _Password.Replace("|", StringifySplit));
		}
		public static Member_securityInfo Parse(string stringify) {
			string[] ret = stringify.Split(new char[] { '|' }, 2, StringSplitOptions.None);
			if (ret.Length != 2) throw new Exception("格式不正确，Member_securityInfo：" + stringify);
			Member_securityInfo item = new Member_securityInfo();
			if (string.Compare("null", ret[0]) != 0) item.Member_id = uint.Parse(ret[0]);
			if (string.Compare("null", ret[1]) != 0) item.Password = ret[1].Replace(StringifySplit, "|");
			return item;
		}
		#endregion

		#region override
		private static Lazy<Dictionary<string, bool>> __jsonIgnoreLazy = new Lazy<Dictionary<string, bool>>(() => {
			FieldInfo field = typeof(Member_securityInfo).GetField("JsonIgnore");
			Dictionary<string, bool> ret = new Dictionary<string, bool>();
			if (field != null) string.Concat(field.GetValue(null)).Split(',').ToList().ForEach(f => {
				if (!string.IsNullOrEmpty(f)) ret[f] = true;
			});
			return ret;
		});
		private static Dictionary<string, bool> __jsonIgnore => __jsonIgnoreLazy.Value;
		public override string ToString() {
			string json = string.Concat(
				__jsonIgnore.ContainsKey("Member_id") ? string.Empty : string.Format(", Member_id : {0}", Member_id == null ? "null" : Member_id.ToString()), 
				__jsonIgnore.ContainsKey("Password") ? string.Empty : string.Format(", Password : {0}", Password == null ? "null" : string.Format("'{0}'", Password.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), " }");
			return string.Concat("{", json.Substring(1));
		}
		public IDictionary ToBson(bool allField = false) {
			IDictionary ht = new Hashtable();
			if (allField || !__jsonIgnore.ContainsKey("Member_id")) ht["Member_id"] = Member_id;
			if (allField || !__jsonIgnore.ContainsKey("Password")) ht["Password"] = Password;
			return ht;
		}
		public object this[string key] {
			get { return this.GetType().GetProperty(key).GetValue(this); }
			set { this.GetType().GetProperty(key).SetValue(this, value); }
		}
		#endregion

		#region properties
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
		/// 密码
		/// </summary>
		[JsonProperty] public string Password {
			get { return _Password; }
			set { _Password = value; }
		}
		#endregion

		public pifa.DAL.Member_security.SqlUpdateBuild UpdateDiy {
			get { return Member_security.UpdateDiy(this, _Member_id.Value); }
		}
		public Member_securityInfo Save() {
			if (this.Member_id != null) {
				Member_security.Update(this);
				return this;
			}
			return Member_security.Insert(this);
		}
	}
}

