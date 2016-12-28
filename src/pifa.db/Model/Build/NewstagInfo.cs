using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using pifa.BLL;

namespace pifa.Model {

	public partial class NewstagInfo {
		#region fields
		private uint? _Id;
		private DateTime? _Create_time;
		private string _Name;
		private uint? _Total_news;
		#endregion

		public NewstagInfo() { }

		#region 独创的序列化，反序列化
		protected static readonly string StringifySplit = "@<Newstag(Info]?#>";
		public string Stringify() {
			return string.Concat(
				_Id == null ? "null" : _Id.ToString(), "|",
				_Create_time == null ? "null" : _Create_time.Value.Ticks.ToString(), "|",
				_Name == null ? "null" : _Name.Replace("|", StringifySplit), "|",
				_Total_news == null ? "null" : _Total_news.ToString());
		}
		public NewstagInfo(string stringify) {
			string[] ret = stringify.Split(new char[] { '|' }, 4, StringSplitOptions.None);
			if (ret.Length != 4) throw new Exception("格式不正确，NewstagInfo：" + stringify);
			if (string.Compare("null", ret[0]) != 0) _Id = uint.Parse(ret[0]);
			if (string.Compare("null", ret[1]) != 0) _Create_time = new DateTime(long.Parse(ret[1]));
			if (string.Compare("null", ret[2]) != 0) _Name = ret[2].Replace(StringifySplit, "|");
			if (string.Compare("null", ret[3]) != 0) _Total_news = uint.Parse(ret[3]);
		}
		#endregion

		#region override
		private static Dictionary<string, bool> __jsonIgnore;
		private static object __jsonIgnore_lock = new object();
		public override string ToString() {
			this.Init__jsonIgnore();
			string json = string.Concat(
				__jsonIgnore.ContainsKey("Id") ? string.Empty : string.Format(", Id : {0}", Id == null ? "null" : Id.ToString()), 
				__jsonIgnore.ContainsKey("Create_time") ? string.Empty : string.Format(", Create_time : {0}", Create_time == null ? "null" : Create_time.Value.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds.ToString()), 
				__jsonIgnore.ContainsKey("Name") ? string.Empty : string.Format(", Name : {0}", Name == null ? "null" : string.Format("'{0}'", Name.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), 
				__jsonIgnore.ContainsKey("Total_news") ? string.Empty : string.Format(", Total_news : {0}", Total_news == null ? "null" : Total_news.ToString()), " }");
			return string.Concat("{", json.Substring(1));
		}
		public IDictionary ToBson() {
			this.Init__jsonIgnore();
			IDictionary ht = new Hashtable();
			if (!__jsonIgnore.ContainsKey("Id")) ht["Id"] = Id;
			if (!__jsonIgnore.ContainsKey("Create_time")) ht["Create_time"] = Create_time;
			if (!__jsonIgnore.ContainsKey("Name")) ht["Name"] = Name;
			if (!__jsonIgnore.ContainsKey("Total_news")) ht["Total_news"] = Total_news;
			return ht;
		}
		private void Init__jsonIgnore() {
			if (__jsonIgnore == null) {
				lock (__jsonIgnore_lock) {
					if (__jsonIgnore == null) {
						FieldInfo field = typeof(NewstagInfo).GetField("JsonIgnore");
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
			NewstagInfo item = obj as NewstagInfo;
			if (item == null) return false;
			return this.ToString().Equals(item.ToString());
		}
		public override int GetHashCode() {
			return this.ToString().GetHashCode();
		}
		public static bool operator ==(NewstagInfo op1, NewstagInfo op2) {
			if (object.Equals(op1, null)) return object.Equals(op2, null);
			return op1.Equals(op2);
		}
		public static bool operator !=(NewstagInfo op1, NewstagInfo op2) {
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
		/// 创建时间
		/// </summary>
		public DateTime? Create_time {
			get { return _Create_time; }
			set { _Create_time = value; }
		}
		/// <summary>
		/// 标签名
		/// </summary>
		public string Name {
			get { return _Name; }
			set { _Name = value; }
		}
		/// <summary>
		/// 文章数量
		/// </summary>
		public uint? Total_news {
			get { return _Total_news; }
			set { _Total_news = value; }
		}
		private List<NewsInfo> _obj_newss;
		public List<NewsInfo> Obj_newss {
			get {
				if (_obj_newss == null) _obj_newss = News.SelectByNewstag_id(_Id.Value).ToList();
				return _obj_newss;
			}
		}
		#endregion

		public pifa.DAL.Newstag.SqlUpdateBuild UpdateDiy {
			get { return Newstag.UpdateDiy(this, _Id); }
		}
		public News_newstagInfo FlagNews(NewsInfo News) {
			return FlagNews(News.Id);
		}
		public News_newstagInfo FlagNews(uint? News_id) {
			News_newstagInfo item = News_newstag.GetItem(News_id, this.Id);
			if (item == null) item = News_newstag.Insert(new News_newstagInfo {
				News_id = News_id, 
				Newstag_id = this.Id});
			return item;
		}

		public int UnflagNews(NewsInfo News) {
			return UnflagNews(News.Id);
		}
		public int UnflagNews(uint? News_id) {
			return News_newstag.Delete(News_id, this.Id);
		}
		public int UnflagNewsALL() {
			return News_newstag.DeleteByNewstag_id(this.Id);
		}

	}
}

