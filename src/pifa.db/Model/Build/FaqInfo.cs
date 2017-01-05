using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using pifa.BLL;

namespace pifa.Model {

	[JsonObject(MemberSerialization.OptIn)]
	public partial class FaqInfo {
		#region fields
		private uint? _Id;
		private uint? _Faqtype_id;
		private FaqtypeInfo _obj_faqtype;
		private DateTime? _Create_time;
		private string _Title;
		#endregion

		public FaqInfo() { }

		#region 序列化，反序列化
		protected static readonly string StringifySplit = "@<Faq(Info]?#>";
		public string Stringify() {
			return string.Concat(
				_Id == null ? "null" : _Id.ToString(), "|",
				_Faqtype_id == null ? "null" : _Faqtype_id.ToString(), "|",
				_Create_time == null ? "null" : _Create_time.Value.Ticks.ToString(), "|",
				_Title == null ? "null" : _Title.Replace("|", StringifySplit));
		}
		public static FaqInfo Parse(string stringify) {
			string[] ret = stringify.Split(new char[] { '|' }, 4, StringSplitOptions.None);
			if (ret.Length != 4) throw new Exception("格式不正确，FaqInfo：" + stringify);
			FaqInfo item = new FaqInfo();
			if (string.Compare("null", ret[0]) != 0) item.Id = uint.Parse(ret[0]);
			if (string.Compare("null", ret[1]) != 0) item.Faqtype_id = uint.Parse(ret[1]);
			if (string.Compare("null", ret[2]) != 0) item.Create_time = new DateTime(long.Parse(ret[2]));
			if (string.Compare("null", ret[3]) != 0) item.Title = ret[3].Replace(StringifySplit, "|");
			return item;
		}
		#endregion

		#region override
		private static Lazy<Dictionary<string, bool>> __jsonIgnoreLazy = new Lazy<Dictionary<string, bool>>(() => {
			FieldInfo field = typeof(FaqInfo).GetField("JsonIgnore");
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
				__jsonIgnore.ContainsKey("Faqtype_id") ? string.Empty : string.Format(", Faqtype_id : {0}", Faqtype_id == null ? "null" : Faqtype_id.ToString()), 
				__jsonIgnore.ContainsKey("Create_time") ? string.Empty : string.Format(", Create_time : {0}", Create_time == null ? "null" : Create_time.Value.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds.ToString()), 
				__jsonIgnore.ContainsKey("Title") ? string.Empty : string.Format(", Title : {0}", Title == null ? "null" : string.Format("'{0}'", Title.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), " }");
			return string.Concat("{", json.Substring(1));
		}
		public IDictionary ToBson(bool allField = false) {
			IDictionary ht = new Hashtable();
			if (allField || !__jsonIgnore.ContainsKey("Id")) ht["Id"] = Id;
			if (allField || !__jsonIgnore.ContainsKey("Faqtype_id")) ht["Faqtype_id"] = Faqtype_id;
			if (allField || !__jsonIgnore.ContainsKey("Create_time")) ht["Create_time"] = Create_time;
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
		/// 类型
		/// </summary>
		[JsonProperty] public uint? Faqtype_id {
			get { return _Faqtype_id; }
			set {
				if (_Faqtype_id != value) _obj_faqtype = null;
				_Faqtype_id = value;
			}
		}
		public FaqtypeInfo Obj_faqtype {
			get {
				if (_obj_faqtype == null) _obj_faqtype = Faqtype.GetItem(_Faqtype_id.Value);
				return _obj_faqtype;
			}
			internal set { _obj_faqtype = value; }
		}
		/// <summary>
		/// 创建时间
		/// </summary>
		[JsonProperty] public DateTime? Create_time {
			get { return _Create_time; }
			set { _Create_time = value; }
		}
		/// <summary>
		/// 标题
		/// </summary>
		[JsonProperty] public string Title {
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
			get { return Faq.UpdateDiy(this, _Id.Value); }
		}
		public FaqInfo Save() {
			if (this.Id != null) {
				Faq.Update(this);
				return this;
			}
			this.Create_time = DateTime.Now;
			return Faq.Insert(this);
		}
		public FaqdescInfo AddFaqdesc(string Content) => Faqdesc.Insert(new FaqdescInfo {
			Faq_id = this.Id, 
				Content = Content});
		public FaqdescInfo AddFaqdesc(FaqdescInfo item) {
			item.Faq_id = this.Id;
			return item.Save();
		}

	}
}

