using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using pifa.BLL;

namespace pifa.Model {

	public partial class Member_addressbookInfo {
		#region fields
		private uint? _Id;
		private uint? _Member_id;
		private MemberInfo _obj_member;
		private string _Address;
		private DateTime? _Create_time;
		private bool? _Is_default;
		private string _Name;
		private string _Tel;
		private string _Telphone;
		private string _Zip;
		#endregion

		public Member_addressbookInfo() { }

		#region 独创的序列化，反序列化
		protected static readonly string StringifySplit = "@<Member_addressbook(Info]?#>";
		public string Stringify() {
			return string.Concat(
				_Id == null ? "null" : _Id.ToString(), "|",
				_Member_id == null ? "null" : _Member_id.ToString(), "|",
				_Address == null ? "null" : _Address.Replace("|", StringifySplit), "|",
				_Create_time == null ? "null" : _Create_time.Value.Ticks.ToString(), "|",
				_Is_default == null ? "null" : (_Is_default == true ? "1" : "0"), "|",
				_Name == null ? "null" : _Name.Replace("|", StringifySplit), "|",
				_Tel == null ? "null" : _Tel.Replace("|", StringifySplit), "|",
				_Telphone == null ? "null" : _Telphone.Replace("|", StringifySplit), "|",
				_Zip == null ? "null" : _Zip.Replace("|", StringifySplit));
		}
		public Member_addressbookInfo(string stringify) {
			string[] ret = stringify.Split(new char[] { '|' }, 9, StringSplitOptions.None);
			if (ret.Length != 9) throw new Exception("格式不正确，Member_addressbookInfo：" + stringify);
			if (string.Compare("null", ret[0]) != 0) _Id = uint.Parse(ret[0]);
			if (string.Compare("null", ret[1]) != 0) _Member_id = uint.Parse(ret[1]);
			if (string.Compare("null", ret[2]) != 0) _Address = ret[2].Replace(StringifySplit, "|");
			if (string.Compare("null", ret[3]) != 0) _Create_time = new DateTime(long.Parse(ret[3]));
			if (string.Compare("null", ret[4]) != 0) _Is_default = ret[4] == "1";
			if (string.Compare("null", ret[5]) != 0) _Name = ret[5].Replace(StringifySplit, "|");
			if (string.Compare("null", ret[6]) != 0) _Tel = ret[6].Replace(StringifySplit, "|");
			if (string.Compare("null", ret[7]) != 0) _Telphone = ret[7].Replace(StringifySplit, "|");
			if (string.Compare("null", ret[8]) != 0) _Zip = ret[8].Replace(StringifySplit, "|");
		}
		#endregion

		#region override
		private static Dictionary<string, bool> __jsonIgnore;
		private static object __jsonIgnore_lock = new object();
		public override string ToString() {
			this.Init__jsonIgnore();
			string json = string.Concat(
				__jsonIgnore.ContainsKey("Id") ? string.Empty : string.Format(", Id : {0}", Id == null ? "null" : Id.ToString()), 
				__jsonIgnore.ContainsKey("Member_id") ? string.Empty : string.Format(", Member_id : {0}", Member_id == null ? "null" : Member_id.ToString()), 
				__jsonIgnore.ContainsKey("Address") ? string.Empty : string.Format(", Address : {0}", Address == null ? "null" : string.Format("'{0}'", Address.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), 
				__jsonIgnore.ContainsKey("Create_time") ? string.Empty : string.Format(", Create_time : {0}", Create_time == null ? "null" : Create_time.Value.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds.ToString()), 
				__jsonIgnore.ContainsKey("Is_default") ? string.Empty : string.Format(", Is_default : {0}", Is_default == null ? "null" : (Is_default == true ? "true" : "false")), 
				__jsonIgnore.ContainsKey("Name") ? string.Empty : string.Format(", Name : {0}", Name == null ? "null" : string.Format("'{0}'", Name.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), 
				__jsonIgnore.ContainsKey("Tel") ? string.Empty : string.Format(", Tel : {0}", Tel == null ? "null" : string.Format("'{0}'", Tel.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), 
				__jsonIgnore.ContainsKey("Telphone") ? string.Empty : string.Format(", Telphone : {0}", Telphone == null ? "null" : string.Format("'{0}'", Telphone.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), 
				__jsonIgnore.ContainsKey("Zip") ? string.Empty : string.Format(", Zip : {0}", Zip == null ? "null" : string.Format("'{0}'", Zip.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), " }");
			return string.Concat("{", json.Substring(1));
		}
		public IDictionary ToBson() {
			this.Init__jsonIgnore();
			IDictionary ht = new Hashtable();
			if (!__jsonIgnore.ContainsKey("Id")) ht["Id"] = Id;
			if (!__jsonIgnore.ContainsKey("Member_id")) ht["Member_id"] = Member_id;
			if (!__jsonIgnore.ContainsKey("Address")) ht["Address"] = Address;
			if (!__jsonIgnore.ContainsKey("Create_time")) ht["Create_time"] = Create_time;
			if (!__jsonIgnore.ContainsKey("Is_default")) ht["Is_default"] = Is_default;
			if (!__jsonIgnore.ContainsKey("Name")) ht["Name"] = Name;
			if (!__jsonIgnore.ContainsKey("Tel")) ht["Tel"] = Tel;
			if (!__jsonIgnore.ContainsKey("Telphone")) ht["Telphone"] = Telphone;
			if (!__jsonIgnore.ContainsKey("Zip")) ht["Zip"] = Zip;
			return ht;
		}
		private void Init__jsonIgnore() {
			if (__jsonIgnore == null) {
				lock (__jsonIgnore_lock) {
					if (__jsonIgnore == null) {
						FieldInfo field = typeof(Member_addressbookInfo).GetField("JsonIgnore");
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
			Member_addressbookInfo item = obj as Member_addressbookInfo;
			if (item == null) return false;
			return this.ToString().Equals(item.ToString());
		}
		public override int GetHashCode() {
			return this.ToString().GetHashCode();
		}
		public static bool operator ==(Member_addressbookInfo op1, Member_addressbookInfo op2) {
			if (object.Equals(op1, null)) return object.Equals(op2, null);
			return op1.Equals(op2);
		}
		public static bool operator !=(Member_addressbookInfo op1, Member_addressbookInfo op2) {
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
		/// 会员
		/// </summary>
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
		/// 收货地址
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
		/// 默认地址
		/// </summary>
		public bool? Is_default {
			get { return _Is_default; }
			set { _Is_default = value; }
		}
		/// <summary>
		/// 收件人
		/// </summary>
		public string Name {
			get { return _Name; }
			set { _Name = value; }
		}
		/// <summary>
		/// 电话
		/// </summary>
		public string Tel {
			get { return _Tel; }
			set { _Tel = value; }
		}
		/// <summary>
		/// 手机
		/// </summary>
		public string Telphone {
			get { return _Telphone; }
			set { _Telphone = value; }
		}
		/// <summary>
		/// 邮编
		/// </summary>
		public string Zip {
			get { return _Zip; }
			set { _Zip = value; }
		}
		#endregion

		public pifa.DAL.Member_addressbook.SqlUpdateBuild UpdateDiy {
			get { return Member_addressbook.UpdateDiy(this, _Id); }
		}
	}
}

