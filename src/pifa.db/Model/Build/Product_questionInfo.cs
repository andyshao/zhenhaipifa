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
	public partial class Product_questionInfo {
		#region fields
		private uint? _Id;
		private uint? _Member_id;
		private MemberInfo _obj_member;
		private uint? _Parent_id;
		private Product_questionInfo _obj_product_question;
		private uint? _Product_id;
		private ProductInfo _obj_product;
		private string _Content;
		private DateTime? _Create_time;
		private string _Email;
		private string _Name;
		private Product_questionSTATE? _State;
		#endregion

		public Product_questionInfo() { }

		#region 序列化，反序列化
		protected static readonly string StringifySplit = "@<Product_question(Info]?#>";
		public string Stringify() {
			return string.Concat(
				_Id == null ? "null" : _Id.ToString(), "|",
				_Member_id == null ? "null" : _Member_id.ToString(), "|",
				_Parent_id == null ? "null" : _Parent_id.ToString(), "|",
				_Product_id == null ? "null" : _Product_id.ToString(), "|",
				_Content == null ? "null" : _Content.Replace("|", StringifySplit), "|",
				_Create_time == null ? "null" : _Create_time.Value.Ticks.ToString(), "|",
				_Email == null ? "null" : _Email.Replace("|", StringifySplit), "|",
				_Name == null ? "null" : _Name.Replace("|", StringifySplit), "|",
				_State == null ? "null" : _State.ToInt64().ToString());
		}
		public static Product_questionInfo Parse(string stringify) {
			string[] ret = stringify.Split(new char[] { '|' }, 9, StringSplitOptions.None);
			if (ret.Length != 9) throw new Exception("格式不正确，Product_questionInfo：" + stringify);
			Product_questionInfo item = new Product_questionInfo();
			if (string.Compare("null", ret[0]) != 0) item.Id = uint.Parse(ret[0]);
			if (string.Compare("null", ret[1]) != 0) item.Member_id = uint.Parse(ret[1]);
			if (string.Compare("null", ret[2]) != 0) item.Parent_id = uint.Parse(ret[2]);
			if (string.Compare("null", ret[3]) != 0) item.Product_id = uint.Parse(ret[3]);
			if (string.Compare("null", ret[4]) != 0) item.Content = ret[4].Replace(StringifySplit, "|");
			if (string.Compare("null", ret[5]) != 0) item.Create_time = new DateTime(long.Parse(ret[5]));
			if (string.Compare("null", ret[6]) != 0) item.Email = ret[6].Replace(StringifySplit, "|");
			if (string.Compare("null", ret[7]) != 0) item.Name = ret[7].Replace(StringifySplit, "|");
			if (string.Compare("null", ret[8]) != 0) item.State = (Product_questionSTATE)long.Parse(ret[8]);
			return item;
		}
		#endregion

		#region override
		private static Lazy<Dictionary<string, bool>> __jsonIgnoreLazy = new Lazy<Dictionary<string, bool>>(() => {
			FieldInfo field = typeof(Product_questionInfo).GetField("JsonIgnore");
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
				__jsonIgnore.ContainsKey("Parent_id") ? string.Empty : string.Format(", Parent_id : {0}", Parent_id == null ? "null" : Parent_id.ToString()), 
				__jsonIgnore.ContainsKey("Product_id") ? string.Empty : string.Format(", Product_id : {0}", Product_id == null ? "null" : Product_id.ToString()), 
				__jsonIgnore.ContainsKey("Content") ? string.Empty : string.Format(", Content : {0}", Content == null ? "null" : string.Format("'{0}'", Content.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), 
				__jsonIgnore.ContainsKey("Create_time") ? string.Empty : string.Format(", Create_time : {0}", Create_time == null ? "null" : Create_time.Value.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds.ToString()), 
				__jsonIgnore.ContainsKey("Email") ? string.Empty : string.Format(", Email : {0}", Email == null ? "null" : string.Format("'{0}'", Email.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), 
				__jsonIgnore.ContainsKey("Name") ? string.Empty : string.Format(", Name : {0}", Name == null ? "null" : string.Format("'{0}'", Name.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), 
				__jsonIgnore.ContainsKey("State") ? string.Empty : string.Format(", State : {0}", State == null ? "null" : string.Format("'{0}'", State.ToDescriptionOrString().Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), " }");
			return string.Concat("{", json.Substring(1));
		}
		public IDictionary ToBson(bool allField = false) {
			IDictionary ht = new Hashtable();
			if (allField || !__jsonIgnore.ContainsKey("Id")) ht["Id"] = Id;
			if (allField || !__jsonIgnore.ContainsKey("Member_id")) ht["Member_id"] = Member_id;
			if (allField || !__jsonIgnore.ContainsKey("Parent_id")) ht["Parent_id"] = Parent_id;
			if (allField || !__jsonIgnore.ContainsKey("Product_id")) ht["Product_id"] = Product_id;
			if (allField || !__jsonIgnore.ContainsKey("Content")) ht["Content"] = Content;
			if (allField || !__jsonIgnore.ContainsKey("Create_time")) ht["Create_time"] = Create_time;
			if (allField || !__jsonIgnore.ContainsKey("Email")) ht["Email"] = Email;
			if (allField || !__jsonIgnore.ContainsKey("Name")) ht["Name"] = Name;
			if (allField || !__jsonIgnore.ContainsKey("State")) ht["State"] = State?.ToDescriptionOrString();
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
		/// 父
		/// </summary>
		[JsonProperty] public uint? Parent_id {
			get { return _Parent_id; }
			set {
				if (_Parent_id != value) _obj_product_question = null;
				_Parent_id = value;
			}
		}
		public Product_questionInfo Obj_product_question {
			get {
				if (_obj_product_question == null) _obj_product_question = Product_question.GetItem(_Parent_id.Value);
				return _obj_product_question;
			}
			internal set { _obj_product_question = value; }
		}
		/// <summary>
		/// 产品
		/// </summary>
		[JsonProperty] public uint? Product_id {
			get { return _Product_id; }
			set {
				if (_Product_id != value) _obj_product = null;
				_Product_id = value;
			}
		}
		public ProductInfo Obj_product {
			get {
				if (_obj_product == null) _obj_product = Product.GetItem(_Product_id.Value);
				return _obj_product;
			}
			internal set { _obj_product = value; }
		}
		/// <summary>
		/// 内容
		/// </summary>
		[JsonProperty] public string Content {
			get { return _Content; }
			set { _Content = value; }
		}
		/// <summary>
		/// 创建时间
		/// </summary>
		[JsonProperty] public DateTime? Create_time {
			get { return _Create_time; }
			set { _Create_time = value; }
		}
		/// <summary>
		/// 邮箱
		/// </summary>
		[JsonProperty] public string Email {
			get { return _Email; }
			set { _Email = value; }
		}
		/// <summary>
		/// 昵称
		/// </summary>
		[JsonProperty] public string Name {
			get { return _Name; }
			set { _Name = value; }
		}
		/// <summary>
		/// 状态
		/// </summary>
		[JsonProperty] public Product_questionSTATE? State {
			get { return _State; }
			set { _State = value; }
		}
		private List<Product_questionInfo> _obj_product_questions;
		public List<Product_questionInfo> Obj_product_questions {
			get {
				if (_obj_product_questions == null) _obj_product_questions = Product_question.SelectByParent_id(_Id).Limit(500).ToList();
				return _obj_product_questions;
			}
		}
		#endregion

		public pifa.DAL.Product_question.SqlUpdateBuild UpdateDiy {
			get { return Product_question.UpdateDiy(this, _Id.Value); }
		}
		public Product_questionInfo Save() {
			if (this.Id != null) {
				Product_question.Update(this);
				return this;
			}
			this.Create_time = DateTime.Now;
			return Product_question.Insert(this);
		}
		public Product_questionInfo AddProduct_question(MemberInfo Member, ProductInfo Product, string Content, DateTime? Create_time, string Email, string Name, Product_questionSTATE? State) => AddProduct_question(Member.Id, Product.Id, Content, Create_time, Email, Name, State);
		public Product_questionInfo AddProduct_question(uint? Member_id, uint? Product_id, string Content, DateTime? Create_time, string Email, string Name, Product_questionSTATE? State) => Product_question.Insert(new Product_questionInfo {
				Member_id = Member_id, 
			Parent_id = this.Id, 
				Product_id = Product_id, 
				Content = Content, 
				Create_time = Create_time, 
				Email = Email, 
				Name = Name, 
				State = State});
		public Product_questionInfo AddProduct_question(Product_questionInfo item) {
			item.Parent_id = this.Id;
			return item.Save();
		}

	}
	public enum Product_questionSTATE {
		/// <summary>
		/// ??
		/// </summary>
		[Description("??")]
		Unknow1 = 1, 
		/// <summary>
		/// ??
		/// </summary>
		[Description("??")]
		Unknow2
	}
}

