using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using pifa.BLL;

namespace pifa.Model {

	[JsonObject(MemberSerialization.OptIn)]
	public partial class FaqtypeInfo {
		#region fields
		private uint? _Id;
		private byte? _Sort;
		private string _Title;
		#endregion

		public FaqtypeInfo() { }

		#region 序列化，反序列化
		protected static readonly string StringifySplit = "@<Faqtype(Info]?#>";
		public string Stringify() {
			return string.Concat(
				_Id == null ? "null" : _Id.ToString(), "|",
				_Sort == null ? "null" : _Sort.ToString(), "|",
				_Title == null ? "null" : _Title.Replace("|", StringifySplit));
		}
		public static FaqtypeInfo Parse(string stringify) {
			string[] ret = stringify.Split(new char[] { '|' }, 3, StringSplitOptions.None);
			if (ret.Length != 3) throw new Exception("格式不正确，FaqtypeInfo：" + stringify);
			FaqtypeInfo item = new FaqtypeInfo();
			if (string.Compare("null", ret[0]) != 0) item.Id = uint.Parse(ret[0]);
			if (string.Compare("null", ret[1]) != 0) item.Sort = byte.Parse(ret[1]);
			if (string.Compare("null", ret[2]) != 0) item.Title = ret[2].Replace(StringifySplit, "|");
			return item;
		}
		#endregion

		#region override
		private static Lazy<Dictionary<string, bool>> __jsonIgnoreLazy = new Lazy<Dictionary<string, bool>>(() => {
			FieldInfo field = typeof(FaqtypeInfo).GetField("JsonIgnore");
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
				__jsonIgnore.ContainsKey("Sort") ? string.Empty : string.Format(", Sort : {0}", Sort == null ? "null" : Sort.ToString()), 
				__jsonIgnore.ContainsKey("Title") ? string.Empty : string.Format(", Title : {0}", Title == null ? "null" : string.Format("'{0}'", Title.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), " }");
			return string.Concat("{", json.Substring(1));
		}
		public IDictionary ToBson(bool allField = false) {
			IDictionary ht = new Hashtable();
			if (allField || !__jsonIgnore.ContainsKey("Id")) ht["Id"] = Id;
			if (allField || !__jsonIgnore.ContainsKey("Sort")) ht["Sort"] = Sort;
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
		/// 排序
		/// </summary>
		[JsonProperty] public byte? Sort {
			get { return _Sort; }
			set { _Sort = value; }
		}
		/// <summary>
		/// 类型名称
		/// </summary>
		[JsonProperty] public string Title {
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
			get { return Faqtype.UpdateDiy(this, _Id.Value); }
		}
		public FaqtypeInfo Save() {
			if (this.Id != null) {
				Faqtype.Update(this);
				return this;
			}
			return Faqtype.Insert(this);
		}
		public FaqInfo AddFaq(DateTime? Create_time, string Title) => Faq.Insert(new FaqInfo {
			Faqtype_id = this.Id, 
				Create_time = Create_time, 
				Title = Title});
		public FaqInfo AddFaq(FaqInfo item) {
			item.Faqtype_id = this.Id;
			return item.Save();
		}

	}
}

