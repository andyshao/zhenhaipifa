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
	public partial class Product_commentInfo {
		#region fields
		private uint? _Id;
		private uint? _Member_id;
		private MemberInfo _obj_member;
		private uint? _Order_id;
		private OrderInfo _obj_order;
		private uint? _Product_id;
		private ProductInfo _obj_product;
		private uint? _Productitem_id;
		private ProductitemInfo _obj_productitem;
		private string _Content;
		private DateTime? _Create_time;
		private string _Nickname;
		private byte? _Star_price;
		private byte? _Star_quality;
		private byte? _Star_value;
		private Product_commentSTATE? _State;
		private string _Title;
		private string _Upload_image_url;
		#endregion

		public Product_commentInfo() { }

		#region 序列化，反序列化
		protected static readonly string StringifySplit = "@<Product_comment(Info]?#>";
		public string Stringify() {
			return string.Concat(
				_Id == null ? "null" : _Id.ToString(), "|",
				_Member_id == null ? "null" : _Member_id.ToString(), "|",
				_Order_id == null ? "null" : _Order_id.ToString(), "|",
				_Product_id == null ? "null" : _Product_id.ToString(), "|",
				_Productitem_id == null ? "null" : _Productitem_id.ToString(), "|",
				_Content == null ? "null" : _Content.Replace("|", StringifySplit), "|",
				_Create_time == null ? "null" : _Create_time.Value.Ticks.ToString(), "|",
				_Nickname == null ? "null" : _Nickname.Replace("|", StringifySplit), "|",
				_Star_price == null ? "null" : _Star_price.ToString(), "|",
				_Star_quality == null ? "null" : _Star_quality.ToString(), "|",
				_Star_value == null ? "null" : _Star_value.ToString(), "|",
				_State == null ? "null" : _State.ToInt64().ToString(), "|",
				_Title == null ? "null" : _Title.Replace("|", StringifySplit), "|",
				_Upload_image_url == null ? "null" : _Upload_image_url.Replace("|", StringifySplit));
		}
		public static Product_commentInfo Parse(string stringify) {
			string[] ret = stringify.Split(new char[] { '|' }, 14, StringSplitOptions.None);
			if (ret.Length != 14) throw new Exception("格式不正确，Product_commentInfo：" + stringify);
			Product_commentInfo item = new Product_commentInfo();
			if (string.Compare("null", ret[0]) != 0) item.Id = uint.Parse(ret[0]);
			if (string.Compare("null", ret[1]) != 0) item.Member_id = uint.Parse(ret[1]);
			if (string.Compare("null", ret[2]) != 0) item.Order_id = uint.Parse(ret[2]);
			if (string.Compare("null", ret[3]) != 0) item.Product_id = uint.Parse(ret[3]);
			if (string.Compare("null", ret[4]) != 0) item.Productitem_id = uint.Parse(ret[4]);
			if (string.Compare("null", ret[5]) != 0) item.Content = ret[5].Replace(StringifySplit, "|");
			if (string.Compare("null", ret[6]) != 0) item.Create_time = new DateTime(long.Parse(ret[6]));
			if (string.Compare("null", ret[7]) != 0) item.Nickname = ret[7].Replace(StringifySplit, "|");
			if (string.Compare("null", ret[8]) != 0) item.Star_price = byte.Parse(ret[8]);
			if (string.Compare("null", ret[9]) != 0) item.Star_quality = byte.Parse(ret[9]);
			if (string.Compare("null", ret[10]) != 0) item.Star_value = byte.Parse(ret[10]);
			if (string.Compare("null", ret[11]) != 0) item.State = (Product_commentSTATE)long.Parse(ret[11]);
			if (string.Compare("null", ret[12]) != 0) item.Title = ret[12].Replace(StringifySplit, "|");
			if (string.Compare("null", ret[13]) != 0) item.Upload_image_url = ret[13].Replace(StringifySplit, "|");
			return item;
		}
		#endregion

		#region override
		private static Lazy<Dictionary<string, bool>> __jsonIgnoreLazy = new Lazy<Dictionary<string, bool>>(() => {
			FieldInfo field = typeof(Product_commentInfo).GetField("JsonIgnore");
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
				__jsonIgnore.ContainsKey("Order_id") ? string.Empty : string.Format(", Order_id : {0}", Order_id == null ? "null" : Order_id.ToString()), 
				__jsonIgnore.ContainsKey("Product_id") ? string.Empty : string.Format(", Product_id : {0}", Product_id == null ? "null" : Product_id.ToString()), 
				__jsonIgnore.ContainsKey("Productitem_id") ? string.Empty : string.Format(", Productitem_id : {0}", Productitem_id == null ? "null" : Productitem_id.ToString()), 
				__jsonIgnore.ContainsKey("Content") ? string.Empty : string.Format(", Content : {0}", Content == null ? "null" : string.Format("'{0}'", Content.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), 
				__jsonIgnore.ContainsKey("Create_time") ? string.Empty : string.Format(", Create_time : {0}", Create_time == null ? "null" : Create_time.Value.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds.ToString()), 
				__jsonIgnore.ContainsKey("Nickname") ? string.Empty : string.Format(", Nickname : {0}", Nickname == null ? "null" : string.Format("'{0}'", Nickname.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), 
				__jsonIgnore.ContainsKey("Star_price") ? string.Empty : string.Format(", Star_price : {0}", Star_price == null ? "null" : Star_price.ToString()), 
				__jsonIgnore.ContainsKey("Star_quality") ? string.Empty : string.Format(", Star_quality : {0}", Star_quality == null ? "null" : Star_quality.ToString()), 
				__jsonIgnore.ContainsKey("Star_value") ? string.Empty : string.Format(", Star_value : {0}", Star_value == null ? "null" : Star_value.ToString()), 
				__jsonIgnore.ContainsKey("State") ? string.Empty : string.Format(", State : {0}", State == null ? "null" : string.Format("'{0}'", State.ToDescriptionOrString().Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), 
				__jsonIgnore.ContainsKey("Title") ? string.Empty : string.Format(", Title : {0}", Title == null ? "null" : string.Format("'{0}'", Title.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), 
				__jsonIgnore.ContainsKey("Upload_image_url") ? string.Empty : string.Format(", Upload_image_url : {0}", Upload_image_url == null ? "null" : string.Format("'{0}'", Upload_image_url.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), " }");
			return string.Concat("{", json.Substring(1));
		}
		public IDictionary ToBson(bool allField = false) {
			IDictionary ht = new Hashtable();
			if (allField || !__jsonIgnore.ContainsKey("Id")) ht["Id"] = Id;
			if (allField || !__jsonIgnore.ContainsKey("Member_id")) ht["Member_id"] = Member_id;
			if (allField || !__jsonIgnore.ContainsKey("Order_id")) ht["Order_id"] = Order_id;
			if (allField || !__jsonIgnore.ContainsKey("Product_id")) ht["Product_id"] = Product_id;
			if (allField || !__jsonIgnore.ContainsKey("Productitem_id")) ht["Productitem_id"] = Productitem_id;
			if (allField || !__jsonIgnore.ContainsKey("Content")) ht["Content"] = Content;
			if (allField || !__jsonIgnore.ContainsKey("Create_time")) ht["Create_time"] = Create_time;
			if (allField || !__jsonIgnore.ContainsKey("Nickname")) ht["Nickname"] = Nickname;
			if (allField || !__jsonIgnore.ContainsKey("Star_price")) ht["Star_price"] = Star_price;
			if (allField || !__jsonIgnore.ContainsKey("Star_quality")) ht["Star_quality"] = Star_quality;
			if (allField || !__jsonIgnore.ContainsKey("Star_value")) ht["Star_value"] = Star_value;
			if (allField || !__jsonIgnore.ContainsKey("State")) ht["State"] = State?.ToDescriptionOrString();
			if (allField || !__jsonIgnore.ContainsKey("Title")) ht["Title"] = Title;
			if (allField || !__jsonIgnore.ContainsKey("Upload_image_url")) ht["Upload_image_url"] = Upload_image_url;
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
		/// 订单
		/// </summary>
		[JsonProperty] public uint? Order_id {
			get { return _Order_id; }
			set {
				if (_Order_id != value) _obj_order = null;
				_Order_id = value;
			}
		}
		public OrderInfo Obj_order {
			get {
				if (_obj_order == null) _obj_order = Order.GetItem(_Order_id.Value);
				return _obj_order;
			}
			internal set { _obj_order = value; }
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
		/// 产品项目
		/// </summary>
		[JsonProperty] public uint? Productitem_id {
			get { return _Productitem_id; }
			set {
				if (_Productitem_id != value) _obj_productitem = null;
				_Productitem_id = value;
			}
		}
		public ProductitemInfo Obj_productitem {
			get {
				if (_obj_productitem == null) _obj_productitem = Productitem.GetItem(_Productitem_id.Value);
				return _obj_productitem;
			}
			internal set { _obj_productitem = value; }
		}
		/// <summary>
		/// 评价内容
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
		/// 昵称
		/// </summary>
		[JsonProperty] public string Nickname {
			get { return _Nickname; }
			set { _Nickname = value; }
		}
		/// <summary>
		/// 价格评价
		/// </summary>
		[JsonProperty] public byte? Star_price {
			get { return _Star_price; }
			set { _Star_price = value; }
		}
		/// <summary>
		/// 质量评价
		/// </summary>
		[JsonProperty] public byte? Star_quality {
			get { return _Star_quality; }
			set { _Star_quality = value; }
		}
		/// <summary>
		/// 满意评价
		/// </summary>
		[JsonProperty] public byte? Star_value {
			get { return _Star_value; }
			set { _Star_value = value; }
		}
		/// <summary>
		/// 状态
		/// </summary>
		[JsonProperty] public Product_commentSTATE? State {
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
		/// 图片
		/// </summary>
		[JsonProperty] public string Upload_image_url {
			get { return _Upload_image_url; }
			set { _Upload_image_url = value; }
		}
		#endregion

		public pifa.DAL.Product_comment.SqlUpdateBuild UpdateDiy {
			get { return Product_comment.UpdateDiy(this, _Id.Value); }
		}
		public Product_commentInfo Save() {
			if (this.Id != null) {
				Product_comment.Update(this);
				return this;
			}
			this.Create_time = DateTime.Now;
			return Product_comment.Insert(this);
		}
	}
	public enum Product_commentSTATE {
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

