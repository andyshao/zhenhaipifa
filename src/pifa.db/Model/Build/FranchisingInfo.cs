using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using pifa.BLL;

namespace pifa.Model {

	[JsonObject(MemberSerialization.OptIn)]
	public partial class FranchisingInfo {
		#region fields
		private uint? _Id;
		private string _Title;
		#endregion

		public FranchisingInfo() { }

		#region 序列化，反序列化
		protected static readonly string StringifySplit = "@<Franchising(Info]?#>";
		public string Stringify() {
			return string.Concat(
				_Id == null ? "null" : _Id.ToString(), "|",
				_Title == null ? "null" : _Title.Replace("|", StringifySplit));
		}
		public static FranchisingInfo Parse(string stringify) {
			string[] ret = stringify.Split(new char[] { '|' }, 2, StringSplitOptions.None);
			if (ret.Length != 2) throw new Exception("格式不正确，FranchisingInfo：" + stringify);
			FranchisingInfo item = new FranchisingInfo();
			if (string.Compare("null", ret[0]) != 0) item.Id = uint.Parse(ret[0]);
			if (string.Compare("null", ret[1]) != 0) item.Title = ret[1].Replace(StringifySplit, "|");
			return item;
		}
		#endregion

		#region override
		private static Lazy<Dictionary<string, bool>> __jsonIgnoreLazy = new Lazy<Dictionary<string, bool>>(() => {
			FieldInfo field = typeof(FranchisingInfo).GetField("JsonIgnore");
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
				__jsonIgnore.ContainsKey("Title") ? string.Empty : string.Format(", Title : {0}", Title == null ? "null" : string.Format("'{0}'", Title.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), " }");
			return string.Concat("{", json.Substring(1));
		}
		public IDictionary ToBson(bool allField = false) {
			IDictionary ht = new Hashtable();
			if (allField || !__jsonIgnore.ContainsKey("Id")) ht["Id"] = Id;
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
		/// 经营
		/// </summary>
		[JsonProperty] public string Title {
			get { return _Title; }
			set { _Title = value; }
		}
		private List<FactoryInfo> _obj_factorys;
		public List<FactoryInfo> Obj_factorys {
			get {
				if (_obj_factorys == null) _obj_factorys = Factory.SelectByFranchising_id(_Id.Value).ToList();
				return _obj_factorys;
			}
		}
		private List<RentsubletInfo> _obj_rentsublets;
		public List<RentsubletInfo> Obj_rentsublets {
			get {
				if (_obj_rentsublets == null) _obj_rentsublets = Rentsublet.SelectByFranchising_id(_Id.Value).ToList();
				return _obj_rentsublets;
			}
		}
		private List<ShopInfo> _obj_shops;
		public List<ShopInfo> Obj_shops {
			get {
				if (_obj_shops == null) _obj_shops = Shop.SelectByFranchising_id(_Id.Value).ToList();
				return _obj_shops;
			}
		}
		#endregion

		public pifa.DAL.Franchising.SqlUpdateBuild UpdateDiy {
			get { return Franchising.UpdateDiy(this, _Id.Value); }
		}
		public FranchisingInfo Save() {
			if (this.Id != null) {
				Franchising.Update(this);
				return this;
			}
			return Franchising.Insert(this);
		}
		public Factory_franchisingInfo FlagFactory(FactoryInfo Factory) => FlagFactory(Factory.Id);
		public Factory_franchisingInfo FlagFactory(uint? Factory_id) {
			Factory_franchisingInfo item = Factory_franchising.GetItem(Factory_id.Value, this.Id.Value);
			if (item == null) item = Factory_franchising.Insert(new Factory_franchisingInfo {
				Factory_id = Factory_id, 
			Franchising_id = this.Id});
			return item;
		}

		public int UnflagFactory(FactoryInfo Factory) => UnflagFactory(Factory.Id);
		public int UnflagFactory(uint? Factory_id) => Factory_franchising.Delete(Factory_id.Value, this.Id.Value);
		public int UnflagFactoryALL() => Factory_franchising.DeleteByFranchising_id(this.Id);

		public Rentsublet_franchisingInfo FlagRentsublet(RentsubletInfo Rentsublet) => FlagRentsublet(Rentsublet.Id);
		public Rentsublet_franchisingInfo FlagRentsublet(uint? Rentsublet_id) {
			Rentsublet_franchisingInfo item = Rentsublet_franchising.GetItem(this.Id.Value, Rentsublet_id.Value);
			if (item == null) item = Rentsublet_franchising.Insert(new Rentsublet_franchisingInfo {
			Franchising_id = this.Id, 
				Rentsublet_id = Rentsublet_id});
			return item;
		}

		public int UnflagRentsublet(RentsubletInfo Rentsublet) => UnflagRentsublet(Rentsublet.Id);
		public int UnflagRentsublet(uint? Rentsublet_id) => Rentsublet_franchising.Delete(this.Id.Value, Rentsublet_id.Value);
		public int UnflagRentsubletALL() => Rentsublet_franchising.DeleteByFranchising_id(this.Id);

		public Shop_franchisingInfo FlagShop(ShopInfo Shop) => FlagShop(Shop.Id);
		public Shop_franchisingInfo FlagShop(uint? Shop_id) {
			Shop_franchisingInfo item = Shop_franchising.GetItem(this.Id.Value, Shop_id.Value);
			if (item == null) item = Shop_franchising.Insert(new Shop_franchisingInfo {
			Franchising_id = this.Id, 
				Shop_id = Shop_id});
			return item;
		}

		public int UnflagShop(ShopInfo Shop) => UnflagShop(Shop.Id);
		public int UnflagShop(uint? Shop_id) => Shop_franchising.Delete(this.Id.Value, Shop_id.Value);
		public int UnflagShopALL() => Shop_franchising.DeleteByFranchising_id(this.Id);

	}
}

