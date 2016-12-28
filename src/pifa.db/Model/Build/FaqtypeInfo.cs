using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using pifa.BLL;

namespace pifa.Model {

	public partial class FaqtypeInfo {
		#region fields
		private uint? _Id;
		private byte? _Sort;
		private string _Title;
		#endregion

		public FaqtypeInfo() { }

		#region 独创的序列化，反序列化
		protected static readonly string StringifySplit = "@<Faqtype(Info]?#>";
		public string Stringify() {
			return string.Concat(
				_Id == null ? "null" : _Id.ToString(), "|",
				_Sort == null ? "null" : _Sort.ToString(), "|",
				_Title == null ? "null" : _Title.Replace("|", StringifySplit));
		}
		public FaqtypeInfo(string stringify) {
			string[] ret = stringify.Split(new char[] { '|' }, 3, StringSplitOptions.None);
			if (ret.Length != 3) throw new Exception("格式不正确，FaqtypeInfo：" + stringify);
			if (string.Compare("null", ret[0]) != 0) _Id = uint.Parse(ret[0]);
			if (string.Compare("null", ret[1]) != 0) _Sort = byte.Parse(ret[1]);
			if (string.Compare("null", ret[2]) != 0) _Title = ret[2].Replace(StringifySplit, "|");
		}
		#endregion

		#region override
		private static Dictionary<string, bool> __jsonIgnore;
		private static object __jsonIgnore_lock = new object();
		public override string ToString() {
			this.Init__jsonIgnore();
			string json = string.Concat(
				__jsonIgnore.ContainsKey("Id") ? string.Empty : string.Format(", Id : {0}", Id == null ? "null" : Id.ToString()), 
				__jsonIgnore.ContainsKey("Sort") ? string.Empty : string.Format(", Sort : {0}", Sort == null ? "null" : Sort.ToString()), 
				__jsonIgnore.ContainsKey("Title") ? string.Empty : string.Format(", Title : {0}", Title == null ? "null" : string.Format("'{0}'", Title.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), " }");
			return string.Concat("{", json.Substring(1));
		}
		public IDictionary ToBson() {
			this.Init__jsonIgnore();
			IDictionary ht = new Hashtable();
			if (!__jsonIgnore.ContainsKey("Id")) ht["Id"] = Id;
			if (!__jsonIgnore.ContainsKey("Sort")) ht["Sort"] = Sort;
			if (!__jsonIgnore.ContainsKey("Title")) ht["Title"] = Title;
			return ht;
		}
		private void Init__jsonIgnore() {
			if (__jsonIgnore == null) {
				lock (__jsonIgnore_lock) {
					if (__jsonIgnore == null) {
						FieldInfo field = typeof(FaqtypeInfo).GetField("JsonIgnore");
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
			FaqtypeInfo item = obj as FaqtypeInfo;
			if (item == null) return false;
			return this.ToString().Equals(item.ToString());
		}
		public override int GetHashCode() {
			return this.ToString().GetHashCode();
		}
		public static bool operator ==(FaqtypeInfo op1, FaqtypeInfo op2) {
			if (object.Equals(op1, null)) return object.Equals(op2, null);
			return op1.Equals(op2);
		}
		public static bool operator !=(FaqtypeInfo op1, FaqtypeInfo op2) {
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
		/// 排序
		/// </summary>
		public byte? Sort {
			get { return _Sort; }
			set { _Sort = value; }
		}
		/// <summary>
		/// 类型名称
		/// </summary>
		public string Title {
			get { return _Title; }
			set { _Title = value; }
		}
		private List<FaqInfo> _obj_faqs;
		public List<FaqInfo> Obj_faqs {
			get {
				if (_obj_faqs == null) _obj_faqs = Faq.SelectByFaqtype_id(_Id).Limit(500).ToList();
				return _obj_faqs;
			}
		}
		#endregion

		public pifa.DAL.Faqtype.SqlUpdateBuild UpdateDiy {
			get { return Faqtype.UpdateDiy(this, _Id); }
		}
		public FaqInfo AddFaq(DateTime? Create_time, string Title) {
			return Faq.Insert(new FaqInfo {
				Faqtype_id = this.Id, 
				Create_time = Create_time, 
				Title = Title});
		}

	}
}

