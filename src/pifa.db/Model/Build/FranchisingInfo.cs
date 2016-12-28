using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using pifa.BLL;

namespace pifa.Model {

	public partial class FranchisingInfo {
		#region fields
		private uint? _Id;
		private string _Title;
		#endregion

		public FranchisingInfo() { }

		#region 独创的序列化，反序列化
		protected static readonly string StringifySplit = "@<Franchising(Info]?#>";
		public string Stringify() {
			return string.Concat(
				_Id == null ? "null" : _Id.ToString(), "|",
				_Title == null ? "null" : _Title.Replace("|", StringifySplit));
		}
		public FranchisingInfo(string stringify) {
			string[] ret = stringify.Split(new char[] { '|' }, 2, StringSplitOptions.None);
			if (ret.Length != 2) throw new Exception("格式不正确，FranchisingInfo：" + stringify);
			if (string.Compare("null", ret[0]) != 0) _Id = uint.Parse(ret[0]);
			if (string.Compare("null", ret[1]) != 0) _Title = ret[1].Replace(StringifySplit, "|");
		}
		#endregion

		#region override
		private static Dictionary<string, bool> __jsonIgnore;
		private static object __jsonIgnore_lock = new object();
		public override string ToString() {
			this.Init__jsonIgnore();
			string json = string.Concat(
				__jsonIgnore.ContainsKey("Id") ? string.Empty : string.Format(", Id : {0}", Id == null ? "null" : Id.ToString()), 
				__jsonIgnore.ContainsKey("Title") ? string.Empty : string.Format(", Title : {0}", Title == null ? "null" : string.Format("'{0}'", Title.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), " }");
			return string.Concat("{", json.Substring(1));
		}
		public IDictionary ToBson() {
			this.Init__jsonIgnore();
			IDictionary ht = new Hashtable();
			if (!__jsonIgnore.ContainsKey("Id")) ht["Id"] = Id;
			if (!__jsonIgnore.ContainsKey("Title")) ht["Title"] = Title;
			return ht;
		}
		private void Init__jsonIgnore() {
			if (__jsonIgnore == null) {
				lock (__jsonIgnore_lock) {
					if (__jsonIgnore == null) {
						FieldInfo field = typeof(FranchisingInfo).GetField("JsonIgnore");
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
			FranchisingInfo item = obj as FranchisingInfo;
			if (item == null) return false;
			return this.ToString().Equals(item.ToString());
		}
		public override int GetHashCode() {
			return this.ToString().GetHashCode();
		}
		public static bool operator ==(FranchisingInfo op1, FranchisingInfo op2) {
			if (object.Equals(op1, null)) return object.Equals(op2, null);
			return op1.Equals(op2);
		}
		public static bool operator !=(FranchisingInfo op1, FranchisingInfo op2) {
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
		/// 经营
		/// </summary>
		public string Title {
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
			get { return Franchising.UpdateDiy(this, _Id); }
		}
		public Factory_franchisingInfo FlagFactory(FactoryInfo Factory) {
			return FlagFactory(Factory.Id);
		}
		public Factory_franchisingInfo FlagFactory(uint? Factory_id) {
			Factory_franchisingInfo item = Factory_franchising.GetItem(Factory_id, this.Id);
			if (item == null) item = Factory_franchising.Insert(new Factory_franchisingInfo {
				Factory_id = Factory_id, 
				Franchising_id = this.Id});
			return item;
		}

		public int UnflagFactory(FactoryInfo Factory) {
			return UnflagFactory(Factory.Id);
		}
		public int UnflagFactory(uint? Factory_id) {
			return Factory_franchising.Delete(Factory_id, this.Id);
		}
		public int UnflagFactoryALL() {
			return Factory_franchising.DeleteByFranchising_id(this.Id);
		}

		public Rentsublet_franchisingInfo FlagRentsublet(RentsubletInfo Rentsublet) {
			return FlagRentsublet(Rentsublet.Id);
		}
		public Rentsublet_franchisingInfo FlagRentsublet(uint? Rentsublet_id) {
			Rentsublet_franchisingInfo item = Rentsublet_franchising.GetItem(this.Id, Rentsublet_id);
			if (item == null) item = Rentsublet_franchising.Insert(new Rentsublet_franchisingInfo {
				Franchising_id = this.Id, 
				Rentsublet_id = Rentsublet_id});
			return item;
		}

		public int UnflagRentsublet(RentsubletInfo Rentsublet) {
			return UnflagRentsublet(Rentsublet.Id);
		}
		public int UnflagRentsublet(uint? Rentsublet_id) {
			return Rentsublet_franchising.Delete(this.Id, Rentsublet_id);
		}
		public int UnflagRentsubletALL() {
			return Rentsublet_franchising.DeleteByFranchising_id(this.Id);
		}

		public Shop_franchisingInfo FlagShop(ShopInfo Shop) {
			return FlagShop(Shop.Id);
		}
		public Shop_franchisingInfo FlagShop(uint? Shop_id) {
			Shop_franchisingInfo item = Shop_franchising.GetItem(this.Id, Shop_id);
			if (item == null) item = Shop_franchising.Insert(new Shop_franchisingInfo {
				Franchising_id = this.Id, 
				Shop_id = Shop_id});
			return item;
		}

		public int UnflagShop(ShopInfo Shop) {
			return UnflagShop(Shop.Id);
		}
		public int UnflagShop(uint? Shop_id) {
			return Shop_franchising.Delete(this.Id, Shop_id);
		}
		public int UnflagShopALL() {
			return Shop_franchising.DeleteByFranchising_id(this.Id);
		}

	}
}

