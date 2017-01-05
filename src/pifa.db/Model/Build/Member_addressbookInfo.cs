using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using pifa.BLL;

namespace pifa.Model {

	[JsonObject(MemberSerialization.OptIn)]
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

		#region 序列化，反序列化
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
		public static Member_addressbookInfo Parse(string stringify) {
			string[] ret = stringify.Split(new char[] { '|' }, 9, StringSplitOptions.None);
			if (ret.Length != 9) throw new Exception("格式不正确，Member_addressbookInfo：" + stringify);
			Member_addressbookInfo item = new Member_addressbookInfo();
			if (string.Compare("null", ret[0]) != 0) item.Id = uint.Parse(ret[0]);
			if (string.Compare("null", ret[1]) != 0) item.Member_id = uint.Parse(ret[1]);
			if (string.Compare("null", ret[2]) != 0) item.Address = ret[2].Replace(StringifySplit, "|");
			if (string.Compare("null", ret[3]) != 0) item.Create_time = new DateTime(long.Parse(ret[3]));
			if (string.Compare("null", ret[4]) != 0) item.Is_default = ret[4] == "1";
			if (string.Compare("null", ret[5]) != 0) item.Name = ret[5].Replace(StringifySplit, "|");
			if (string.Compare("null", ret[6]) != 0) item.Tel = ret[6].Replace(StringifySplit, "|");
			if (string.Compare("null", ret[7]) != 0) item.Telphone = ret[7].Replace(StringifySplit, "|");
			if (string.Compare("null", ret[8]) != 0) item.Zip = ret[8].Replace(StringifySplit, "|");
			return item;
		}
		#endregion

		#region override
		private static Lazy<Dictionary<string, bool>> __jsonIgnoreLazy = new Lazy<Dictionary<string, bool>>(() => {
			FieldInfo field = typeof(Member_addressbookInfo).GetField("JsonIgnore");
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
		public IDictionary ToBson(bool allField = false) {
			IDictionary ht = new Hashtable();
			if (allField || !__jsonIgnore.ContainsKey("Id")) ht["Id"] = Id;
			if (allField || !__jsonIgnore.ContainsKey("Member_id")) ht["Member_id"] = Member_id;
			if (allField || !__jsonIgnore.ContainsKey("Address")) ht["Address"] = Address;
			if (allField || !__jsonIgnore.ContainsKey("Create_time")) ht["Create_time"] = Create_time;
			if (allField || !__jsonIgnore.ContainsKey("Is_default")) ht["Is_default"] = Is_default;
			if (allField || !__jsonIgnore.ContainsKey("Name")) ht["Name"] = Name;
			if (allField || !__jsonIgnore.ContainsKey("Tel")) ht["Tel"] = Tel;
			if (allField || !__jsonIgnore.ContainsKey("Telphone")) ht["Telphone"] = Telphone;
			if (allField || !__jsonIgnore.ContainsKey("Zip")) ht["Zip"] = Zip;
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
		/// 会员
		/// </summary>
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
		/// 收货地址
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
		/// 默认地址
		/// </summary>
		[JsonProperty] public bool? Is_default {
			get { return _Is_default; }
			set { _Is_default = value; }
		}
		/// <summary>
		/// 收件人
		/// </summary>
		[JsonProperty] public string Name {
			get { return _Name; }
			set { _Name = value; }
		}
		/// <summary>
		/// 电话
		/// </summary>
		[JsonProperty] public string Tel {
			get { return _Tel; }
			set { _Tel = value; }
		}
		/// <summary>
		/// 手机
		/// </summary>
		[JsonProperty] public string Telphone {
			get { return _Telphone; }
			set { _Telphone = value; }
		}
		/// <summary>
		/// 邮编
		/// </summary>
		[JsonProperty] public string Zip {
			get { return _Zip; }
			set { _Zip = value; }
		}
		#endregion

		public pifa.DAL.Member_addressbook.SqlUpdateBuild UpdateDiy {
			get { return Member_addressbook.UpdateDiy(this, _Id.Value); }
		}
		public Member_addressbookInfo Save() {
			if (this.Id != null) {
				Member_addressbook.Update(this);
				return this;
			}
			this.Create_time = DateTime.Now;
			return Member_addressbook.Insert(this);
		}
	}
}

