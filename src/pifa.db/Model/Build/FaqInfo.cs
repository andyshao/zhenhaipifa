using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using pifa.BLL;

namespace pifa.Model {

	public partial class FaqInfo {
		#region fields
		private uint? _Id;
		private uint? _Faqtype_id;
		private FaqtypeInfo _obj_faqtype;
		private DateTime? _Create_time;
		private string _Title;
		#endregion

		public FaqInfo() { }

		#region 独创的序列化，反序列化
		protected static readonly string StringifySplit = "@<Faq(Info]?#>";
		public string Stringify() {
			return string.Concat(
				_Id == null ? "null" : _Id.ToString(), "|",
				_Faqtype_id == null ? "null" : _Faqtype_id.ToString(), "|",
				_Create_time == null ? "null" : _Create_time.Value.Ticks.ToString(), "|",
				_Title == null ? "null" : _Title.Replace("|", StringifySplit));
		}
		public FaqInfo(string stringify) {
			string[] ret = stringify.Split(new char[] { '|' }, 4, StringSplitOptions.None);
			if (ret.Length != 4) throw new Exception("格式不正确，FaqInfo：" + stringify);
			if (string.Compare("null", ret[0]) != 0) _Id = uint.Parse(ret[0]);
			if (string.Compare("null", ret[1]) != 0) _Faqtype_id = uint.Parse(ret[1]);
			if (string.Compare("null", ret[2]) != 0) _Create_time = new DateTime(long.Parse(ret[2]));
			if (string.Compare("null", ret[3]) != 0) _Title = ret[3].Replace(StringifySplit, "|");
		}
		#endregion

		#region override
		private static Dictionary<string, bool> __jsonIgnore;
		private static object __jsonIgnore_lock = new object();
		public override string ToString() {
			this.Init__jsonIgnore();
			string json = string.Concat(
				__jsonIgnore.ContainsKey("Id") ? string.Empty : string.Format(", Id : {0}", Id == null ? "null" : Id.ToString()), 
				__jsonIgnore.ContainsKey("Faqtype_id") ? string.Empty : string.Format(", Faqtype_id : {0}", Faqtype_id == null ? "null" : Faqtype_id.ToString()), 
				__jsonIgnore.ContainsKey("Create_time") ? string.Empty : string.Format(", Create_time : {0}", Create_time == null ? "null" : Create_time.Value.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds.ToString()), 
				__jsonIgnore.ContainsKey("Title") ? string.Empty : string.Format(", Title : {0}", Title == null ? "null" : string.Format("'{0}'", Title.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), " }");
			return string.Concat("{", json.Substring(1));
		}
		public IDictionary ToBson() {
			this.Init__jsonIgnore();
			IDictionary ht = new Hashtable();
			if (!__jsonIgnore.ContainsKey("Id")) ht["Id"] = Id;
			if (!__jsonIgnore.ContainsKey("Faqtype_id")) ht["Faqtype_id"] = Faqtype_id;
			if (!__jsonIgnore.ContainsKey("Create_time")) ht["Create_time"] = Create_time;
			if (!__jsonIgnore.ContainsKey("Title")) ht["Title"] = Title;
			return ht;
		}
		private void Init__jsonIgnore() {
			if (__jsonIgnore == null) {
				lock (__jsonIgnore_lock) {
					if (__jsonIgnore == null) {
						FieldInfo field = typeof(FaqInfo).GetField("JsonIgnore");
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
			FaqInfo item = obj as FaqInfo;
			if (item == null) return false;
			return this.ToString().Equals(item.ToString());
		}
		public override int GetHashCode() {
			return this.ToString().GetHashCode();
		}
		public static bool operator ==(FaqInfo op1, FaqInfo op2) {
			if (object.Equals(op1, null)) return object.Equals(op2, null);
			return op1.Equals(op2);
		}
		public static bool operator !=(FaqInfo op1, FaqInfo op2) {
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
		/// 类型
		/// </summary>
		public uint? Faqtype_id {
			get { return _Faqtype_id; }
			set {
				if (_Faqtype_id != value) _obj_faqtype = null;
				_Faqtype_id = value;
			}
		}
		public FaqtypeInfo Obj_faqtype {
			get {
				if (_obj_faqtype == null) _obj_faqtype = Faqtype.GetItem(_Faqtype_id);
				return _obj_faqtype;
			}
			internal set { _obj_faqtype = value; }
		}
		/// <summary>
		/// 创建时间
		/// </summary>
		public DateTime? Create_time {
			get { return _Create_time; }
			set { _Create_time = value; }
		}
		/// <summary>
		/// 标题
		/// </summary>
		public string Title {
			get { return _Title; }
			set { _Title = value; }
		}
		private List<FaqdescInfo> _obj_faqdescs;
		public List<FaqdescInfo> Obj_faqdescs {
			get {
				if (_obj_faqdescs == null) _obj_faqdescs = Faqdesc.SelectByFaq_id(_Id).Limit(500).ToList();
				return _obj_faqdescs;
			}
		}
		#endregion

		public pifa.DAL.Faq.SqlUpdateBuild UpdateDiy {
			get { return Faq.UpdateDiy(this, _Id); }
		}
		public FaqdescInfo AddFaqdesc(string Content) {
			return Faqdesc.Insert(new FaqdescInfo {
				Faq_id = this.Id, 
				Content = Content});
		}

	}
}

