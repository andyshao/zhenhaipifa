using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using pifa.BLL;

namespace pifa.Model {

	[JsonObject(MemberSerialization.OptIn)]
	public partial class ExpressdescInfo {
		#region fields
		private uint? _Express_id;
		private ExpressInfo _obj_express;
		private string _Content;
		#endregion

		public ExpressdescInfo() { }

		#region 序列化，反序列化
		protected static readonly string StringifySplit = "@<Expressdesc(Info]?#>";
		public string Stringify() {
			return string.Concat(
				_Express_id == null ? "null" : _Express_id.ToString(), "|",
				_Content == null ? "null" : _Content.Replace("|", StringifySplit));
		}
		public static ExpressdescInfo Parse(string stringify) {
			string[] ret = stringify.Split(new char[] { '|' }, 2, StringSplitOptions.None);
			if (ret.Length != 2) throw new Exception("格式不正确，ExpressdescInfo：" + stringify);
			ExpressdescInfo item = new ExpressdescInfo();
			if (string.Compare("null", ret[0]) != 0) item.Express_id = uint.Parse(ret[0]);
			if (string.Compare("null", ret[1]) != 0) item.Content = ret[1].Replace(StringifySplit, "|");
			return item;
		}
		#endregion

		#region override
		private static Lazy<Dictionary<string, bool>> __jsonIgnoreLazy = new Lazy<Dictionary<string, bool>>(() => {
			FieldInfo field = typeof(ExpressdescInfo).GetField("JsonIgnore");
			Dictionary<string, bool> ret = new Dictionary<string, bool>();
			if (field != null) string.Concat(field.GetValue(null)).Split(',').ToList().ForEach(f => {
				if (!string.IsNullOrEmpty(f)) ret[f] = true;
			});
			return ret;
		});
		private static Dictionary<string, bool> __jsonIgnore => __jsonIgnoreLazy.Value;
		public override string ToString() {
			string json = string.Concat(
				__jsonIgnore.ContainsKey("Express_id") ? string.Empty : string.Format(", Express_id : {0}", Express_id == null ? "null" : Express_id.ToString()), 
				__jsonIgnore.ContainsKey("Content") ? string.Empty : string.Format(", Content : {0}", Content == null ? "null" : string.Format("'{0}'", Content.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), " }");
			return string.Concat("{", json.Substring(1));
		}
		public IDictionary ToBson(bool allField = false) {
			IDictionary ht = new Hashtable();
			if (allField || !__jsonIgnore.ContainsKey("Express_id")) ht["Express_id"] = Express_id;
			if (allField || !__jsonIgnore.ContainsKey("Content")) ht["Content"] = Content;
			return ht;
		}
		public object this[string key] {
			get { return this.GetType().GetProperty(key).GetValue(this); }
			set { this.GetType().GetProperty(key).SetValue(this, value); }
		}
		#endregion

		#region properties
		[JsonProperty] public uint? Express_id {
			get { return _Express_id; }
			set {
				if (_Express_id != value) _obj_express = null;
				_Express_id = value;
			}
		}
		public ExpressInfo Obj_express {
			get {
				if (_obj_express == null) _obj_express = Express.GetItem(_Express_id.Value);
				return _obj_express;
			}
			internal set { _obj_express = value; }
		}
		/// <summary>
		/// 物流介绍
		/// </summary>
		[JsonProperty] public string Content {
			get { return _Content; }
			set { _Content = value; }
		}
		#endregion

		public pifa.DAL.Expressdesc.SqlUpdateBuild UpdateDiy {
			get { return Expressdesc.UpdateDiy(this, _Express_id.Value); }
		}
		public ExpressdescInfo Save() {
			if (this.Express_id != null) {
				Expressdesc.Update(this);
				return this;
			}
			return Expressdesc.Insert(this);
		}
	}
}

