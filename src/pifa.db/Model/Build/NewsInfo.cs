using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Reflection;
using Newtonsoft.Json;
using pifa.BLL;

namespace pifa.Model {

	[JsonObject(MemberSerialization.OptIn)]
	public partial class NewsInfo {
		#region fields
		private uint? _Id;
		private DateTime? _Create_time;
		private string _Intro;
		private uint? _Pv;
		private string _Source;
		private NewsSTATE? _State;
		private string _Title;
		private DateTime? _Update_time;
		#endregion

		public NewsInfo() { }

		#region 序列化，反序列化
		protected static readonly string StringifySplit = "@<News(Info]?#>";
		public string Stringify() {
			return string.Concat(
				_Id == null ? "null" : _Id.ToString(), "|",
				_Create_time == null ? "null" : _Create_time.Value.Ticks.ToString(), "|",
				_Intro == null ? "null" : _Intro.Replace("|", StringifySplit), "|",
				_Pv == null ? "null" : _Pv.ToString(), "|",
				_Source == null ? "null" : _Source.Replace("|", StringifySplit), "|",
				_State == null ? "null" : _State.ToInt64().ToString(), "|",
				_Title == null ? "null" : _Title.Replace("|", StringifySplit), "|",
				_Update_time == null ? "null" : _Update_time.Value.Ticks.ToString());
		}
		public static NewsInfo Parse(string stringify) {
			string[] ret = stringify.Split(new char[] { '|' }, 8, StringSplitOptions.None);
			if (ret.Length != 8) throw new Exception("格式不正确，NewsInfo：" + stringify);
			NewsInfo item = new NewsInfo();
			if (string.Compare("null", ret[0]) != 0) item.Id = uint.Parse(ret[0]);
			if (string.Compare("null", ret[1]) != 0) item.Create_time = new DateTime(long.Parse(ret[1]));
			if (string.Compare("null", ret[2]) != 0) item.Intro = ret[2].Replace(StringifySplit, "|");
			if (string.Compare("null", ret[3]) != 0) item.Pv = uint.Parse(ret[3]);
			if (string.Compare("null", ret[4]) != 0) item.Source = ret[4].Replace(StringifySplit, "|");
			if (string.Compare("null", ret[5]) != 0) item.State = (NewsSTATE)long.Parse(ret[5]);
			if (string.Compare("null", ret[6]) != 0) item.Title = ret[6].Replace(StringifySplit, "|");
			if (string.Compare("null", ret[7]) != 0) item.Update_time = new DateTime(long.Parse(ret[7]));
			return item;
		}
		#endregion

		#region override
		private static Lazy<Dictionary<string, bool>> __jsonIgnoreLazy = new Lazy<Dictionary<string, bool>>(() => {
			FieldInfo field = typeof(NewsInfo).GetField("JsonIgnore");
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
				__jsonIgnore.ContainsKey("Create_time") ? string.Empty : string.Format(", Create_time : {0}", Create_time == null ? "null" : Create_time.Value.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds.ToString()), 
				__jsonIgnore.ContainsKey("Intro") ? string.Empty : string.Format(", Intro : {0}", Intro == null ? "null" : string.Format("'{0}'", Intro.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), 
				__jsonIgnore.ContainsKey("Pv") ? string.Empty : string.Format(", Pv : {0}", Pv == null ? "null" : Pv.ToString()), 
				__jsonIgnore.ContainsKey("Source") ? string.Empty : string.Format(", Source : {0}", Source == null ? "null" : string.Format("'{0}'", Source.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), 
				__jsonIgnore.ContainsKey("State") ? string.Empty : string.Format(", State : {0}", State == null ? "null" : string.Format("'{0}'", State.ToDescriptionOrString().Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), 
				__jsonIgnore.ContainsKey("Title") ? string.Empty : string.Format(", Title : {0}", Title == null ? "null" : string.Format("'{0}'", Title.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), 
				__jsonIgnore.ContainsKey("Update_time") ? string.Empty : string.Format(", Update_time : {0}", Update_time == null ? "null" : Update_time.Value.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds.ToString()), " }");
			return string.Concat("{", json.Substring(1));
		}
		public IDictionary ToBson(bool allField = false) {
			IDictionary ht = new Hashtable();
			if (allField || !__jsonIgnore.ContainsKey("Id")) ht["Id"] = Id;
			if (allField || !__jsonIgnore.ContainsKey("Create_time")) ht["Create_time"] = Create_time;
			if (allField || !__jsonIgnore.ContainsKey("Intro")) ht["Intro"] = Intro;
			if (allField || !__jsonIgnore.ContainsKey("Pv")) ht["Pv"] = Pv;
			if (allField || !__jsonIgnore.ContainsKey("Source")) ht["Source"] = Source;
			if (allField || !__jsonIgnore.ContainsKey("State")) ht["State"] = State?.ToDescriptionOrString();
			if (allField || !__jsonIgnore.ContainsKey("Title")) ht["Title"] = Title;
			if (allField || !__jsonIgnore.ContainsKey("Update_time")) ht["Update_time"] = Update_time;
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
		/// 创建时间
		/// </summary>
		[JsonProperty] public DateTime? Create_time {
			get { return _Create_time; }
			set { _Create_time = value; }
		}
		/// <summary>
		/// 导读
		/// </summary>
		[JsonProperty] public string Intro {
			get { return _Intro; }
			set { _Intro = value; }
		}
		/// <summary>
		/// 阅读次数
		/// </summary>
		[JsonProperty] public uint? Pv {
			get { return _Pv; }
			set { _Pv = value; }
		}
		/// <summary>
		/// 来源
		/// </summary>
		[JsonProperty] public string Source {
			get { return _Source; }
			set { _Source = value; }
		}
		/// <summary>
		/// 状态
		/// </summary>
		[JsonProperty] public NewsSTATE? State {
			get { return _State; }
			set { _State = value; }
		}
		/// <summary>
		/// 标题
		/// </summary>
		[JsonProperty] public string Title {
			get { return _Title; }
			set { _Title = value; }
		}
		/// <summary>
		/// 修改时间
		/// </summary>
		[JsonProperty] public DateTime? Update_time {
			get { return _Update_time; }
			set { _Update_time = value; }
		}
		private List<NewstagInfo> _obj_newstags;
		public List<NewstagInfo> Obj_newstags {
			get {
				if (_obj_newstags == null) _obj_newstags = Newstag.SelectByNews_id(_Id.Value).ToList();
				return _obj_newstags;
			}
		}
		private List<NewsdescInfo> _obj_newsdescs;
		public List<NewsdescInfo> Obj_newsdescs {
			get {
				if (_obj_newsdescs == null) _obj_newsdescs = Newsdesc.SelectByNews_id(_Id).Limit(500).ToList();
				return _obj_newsdescs;
			}
		}
		#endregion

		public pifa.DAL.News.SqlUpdateBuild UpdateDiy {
			get { return News.UpdateDiy(this, _Id.Value); }
		}
		public NewsInfo Save() {
			this.Update_time = DateTime.Now;
			if (this.Id != null) {
				News.Update(this);
				return this;
			}
			this.Create_time = DateTime.Now;
			return News.Insert(this);
		}
		public News_newstagInfo FlagNewstag(NewstagInfo Newstag) => FlagNewstag(Newstag.Id);
		public News_newstagInfo FlagNewstag(uint? Newstag_id) {
			News_newstagInfo item = News_newstag.GetItem(this.Id.Value, Newstag_id.Value);
			if (item == null) item = News_newstag.Insert(new News_newstagInfo {
			News_id = this.Id, 
				Newstag_id = Newstag_id});
			return item;
		}

		public int UnflagNewstag(NewstagInfo Newstag) => UnflagNewstag(Newstag.Id);
		public int UnflagNewstag(uint? Newstag_id) => News_newstag.Delete(this.Id.Value, Newstag_id.Value);
		public int UnflagNewstagALL() => News_newstag.DeleteByNews_id(this.Id);

		public NewsdescInfo AddNewsdesc(string Content) => Newsdesc.Insert(new NewsdescInfo {
			News_id = this.Id, 
				Content = Content});
		public NewsdescInfo AddNewsdesc(NewsdescInfo item) {
			item.News_id = this.Id;
			return item.Save();
		}

	}
	public enum NewsSTATE {
		/// <summary>
		/// ??
		/// </summary>
		[Description("??")]
		Unknow1 = 1
	}
}

