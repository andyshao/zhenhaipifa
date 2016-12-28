using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using pifa.BLL;

namespace pifa.Model {

	public partial class Member_securityInfo {
		#region fields
		private uint? _Member_id;
		private MemberInfo _obj_member;
		private string _Password;
		#endregion

		public Member_securityInfo() { }

		#region 独创的序列化，反序列化
		protected static readonly string StringifySplit = "@<Member_security(Info]?#>";
		public string Stringify() {
			return string.Concat(
				_Member_id == null ? "null" : _Member_id.ToString(), "|",
				_Password == null ? "null" : _Password.Replace("|", StringifySplit));
		}
		public Member_securityInfo(string stringify) {
			string[] ret = stringify.Split(new char[] { '|' }, 2, StringSplitOptions.None);
			if (ret.Length != 2) throw new Exception("格式不正确，Member_securityInfo：" + stringify);
			if (string.Compare("null", ret[0]) != 0) _Member_id = uint.Parse(ret[0]);
			if (string.Compare("null", ret[1]) != 0) _Password = ret[1].Replace(StringifySplit, "|");
		}
		#endregion

		#region override
		private static Dictionary<string, bool> __jsonIgnore;
		private static object __jsonIgnore_lock = new object();
		public override string ToString() {
			this.Init__jsonIgnore();
			string json = string.Concat(
				__jsonIgnore.ContainsKey("Member_id") ? string.Empty : string.Format(", Member_id : {0}", Member_id == null ? "null" : Member_id.ToString()), 
				__jsonIgnore.ContainsKey("Password") ? string.Empty : string.Format(", Password : {0}", Password == null ? "null" : string.Format("'{0}'", Password.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), " }");
			return string.Concat("{", json.Substring(1));
		}
		public IDictionary ToBson() {
			this.Init__jsonIgnore();
			IDictionary ht = new Hashtable();
			if (!__jsonIgnore.ContainsKey("Member_id")) ht["Member_id"] = Member_id;
			if (!__jsonIgnore.ContainsKey("Password")) ht["Password"] = Password;
			return ht;
		}
		private void Init__jsonIgnore() {
			if (__jsonIgnore == null) {
				lock (__jsonIgnore_lock) {
					if (__jsonIgnore == null) {
						FieldInfo field = typeof(Member_securityInfo).GetField("JsonIgnore");
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
			Member_securityInfo item = obj as Member_securityInfo;
			if (item == null) return false;
			return this.ToString().Equals(item.ToString());
		}
		public override int GetHashCode() {
			return this.ToString().GetHashCode();
		}
		public static bool operator ==(Member_securityInfo op1, Member_securityInfo op2) {
			if (object.Equals(op1, null)) return object.Equals(op2, null);
			return op1.Equals(op2);
		}
		public static bool operator !=(Member_securityInfo op1, Member_securityInfo op2) {
			return !(op1 == op2);
		}
		public object this[string key] {
			get { return this.GetType().GetProperty(key).GetValue(this); }
			set { this.GetType().GetProperty(key).SetValue(this, value); }
		}
		#endregion

		#region properties
		public uint? Member_id {
			get { return _Member_id; }
			set {
				if (_Member_id != value) _obj_member = null;
				_Member_id = value;
			}
		}
		public MemberInfo Obj_member {
			get {
				if (_obj_member == null) _obj_member = Member.GetItem(_Member_id);
				return _obj_member;
			}
			internal set { _obj_member = value; }
		}
		/// <summary>
		/// 密码
		/// </summary>
		public string Password {
			get { return _Password; }
			set { _Password = value; }
		}
		#endregion

		public pifa.DAL.Member_security.SqlUpdateBuild UpdateDiy {
			get { return Member_security.UpdateDiy(this, _Member_id); }
		}
	}
}

