using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using pifa.Model;

namespace pifa.BLL {

	public partial class Shopsecurity {

		protected static readonly pifa.DAL.Shopsecurity dal = new pifa.DAL.Shopsecurity();
		protected static readonly int itemCacheTimeout;

		static Shopsecurity() {
			if (!int.TryParse(RedisHelper.Configuration["pifa_BLL_ITEM_CACHE:Timeout_Shopsecurity"], out itemCacheTimeout))
				int.TryParse(RedisHelper.Configuration["pifa_BLL_ITEM_CACHE:Timeout"], out itemCacheTimeout);
		}

		#region delete, update, insert

		public static int Delete(uint? Shop_id) {
			if (itemCacheTimeout > 0) RemoveCache(GetItem(Shop_id));
			return dal.Delete(Shop_id);
		}
		public static int DeleteByIdcard(string Idcard) {
			return dal.DeleteByIdcard(Idcard);
		}

		public static int Update(ShopsecurityInfo item) {
			if (itemCacheTimeout > 0) RemoveCache(item);
			return dal.Update(item);
		}
		public static pifa.DAL.Shopsecurity.SqlUpdateBuild UpdateDiy(uint? Shop_id) {
			return UpdateDiy(null, Shop_id);
		}
		public static pifa.DAL.Shopsecurity.SqlUpdateBuild UpdateDiy(ShopsecurityInfo item, uint? Shop_id) {
			if (itemCacheTimeout > 0) RemoveCache(item != null ? item : GetItem(Shop_id));
			return new pifa.DAL.Shopsecurity.SqlUpdateBuild(item, Shop_id);
		}
		/// <summary>
		/// 用于批量更新
		/// </summary>
		public static pifa.DAL.Shopsecurity.SqlUpdateBuild UpdateDiyDangerous {
			get { return new pifa.DAL.Shopsecurity.SqlUpdateBuild(); }
		}

		public static ShopsecurityInfo Insert(uint? Shop_id, string Idcard, string Idcard_img1, string Idcard_img2, string License_img) {
			return Insert(new ShopsecurityInfo {
				Shop_id = Shop_id, 
				Idcard = Idcard, 
				Idcard_img1 = Idcard_img1, 
				Idcard_img2 = Idcard_img2, 
				License_img = License_img});
		}
		public static ShopsecurityInfo Insert(ShopsecurityInfo item) {
			item = dal.Insert(item);
			if (itemCacheTimeout > 0) RemoveCache(item);
			return item;
		}
		private static void RemoveCache(ShopsecurityInfo item) {
			if (item == null) return;
			RedisHelper.Remove(string.Concat("pifa_BLL_Shopsecurity_", item.Shop_id));
			RedisHelper.Remove(string.Concat("pifa_BLL_ShopsecurityByIdcard_", item.Idcard));
		}
		#endregion

		public static ShopsecurityInfo GetItem(uint? Shop_id) {
			if (Shop_id == null) return null;
			if (itemCacheTimeout <= 0) return dal.GetItem(Shop_id);
			string key = string.Concat("pifa_BLL_Shopsecurity_", Shop_id);
			string value = RedisHelper.Get(key);
			if (!string.IsNullOrEmpty(value))
				try { return new ShopsecurityInfo(value); } catch { }
			ShopsecurityInfo item = dal.GetItem(Shop_id);
			if (item == null) return null;
			RedisHelper.Set(key, item.Stringify(), itemCacheTimeout);
			return item;
		}
		public static ShopsecurityInfo GetItemByIdcard(string Idcard) {
			if (Idcard == null) return null;
			if (itemCacheTimeout <= 0) return dal.GetItemByIdcard(Idcard);
			string key = string.Concat("pifa_BLL_ShopsecurityByIdcard_", Idcard);
			string value = RedisHelper.Get(key);
			if (!string.IsNullOrEmpty(value))
				try { return new ShopsecurityInfo(value); } catch { }
			ShopsecurityInfo item = dal.GetItemByIdcard(Idcard);
			if (item == null) return null;
			RedisHelper.Set(key, item.Stringify(), itemCacheTimeout);
			return item;
		}

		public static List<ShopsecurityInfo> GetItems() {
			return Select.ToList();
		}
		public static ShopsecuritySelectBuild Select {
			get { return new ShopsecuritySelectBuild(dal); }
		}
		public static List<ShopsecurityInfo> GetItemsByShop_id(params uint?[] Shop_id) {
			return Select.WhereShop_id(Shop_id).ToList();
		}
		public static List<ShopsecurityInfo> GetItemsByShop_id(uint?[] Shop_id, int limit) {
			return Select.WhereShop_id(Shop_id).Limit(limit).ToList();
		}
		public static ShopsecuritySelectBuild SelectByShop_id(params uint?[] Shop_id) {
			return Select.WhereShop_id(Shop_id);
		}
	}
	public partial class ShopsecuritySelectBuild : SelectBuild<ShopsecurityInfo, ShopsecuritySelectBuild> {
		public ShopsecuritySelectBuild WhereShop_id(params uint?[] Shop_id) {
			return this.Where1Or("a.`Shop_id` = {0}", Shop_id);
		}
		public ShopsecuritySelectBuild WhereIdcard(params string[] Idcard) {
			return this.Where1Or("a.`idcard` = {0}", Idcard);
		}
		public ShopsecuritySelectBuild WhereIdcardLike(params string[] Idcard) {
			if (Idcard == null) return this;
			return this.Where1Or(@"a.`idcard` LIKE {0}", Idcard.Select(a => "%" + a + "%").ToArray());
		}
		public ShopsecuritySelectBuild WhereIdcard_img1(params string[] Idcard_img1) {
			return this.Where1Or("a.`idcard_img1` = {0}", Idcard_img1);
		}
		public ShopsecuritySelectBuild WhereIdcard_img1Like(params string[] Idcard_img1) {
			if (Idcard_img1 == null) return this;
			return this.Where1Or(@"a.`idcard_img1` LIKE {0}", Idcard_img1.Select(a => "%" + a + "%").ToArray());
		}
		public ShopsecuritySelectBuild WhereIdcard_img2(params string[] Idcard_img2) {
			return this.Where1Or("a.`idcard_img2` = {0}", Idcard_img2);
		}
		public ShopsecuritySelectBuild WhereIdcard_img2Like(params string[] Idcard_img2) {
			if (Idcard_img2 == null) return this;
			return this.Where1Or(@"a.`idcard_img2` LIKE {0}", Idcard_img2.Select(a => "%" + a + "%").ToArray());
		}
		public ShopsecuritySelectBuild WhereLicense_img(params string[] License_img) {
			return this.Where1Or("a.`license_img` = {0}", License_img);
		}
		public ShopsecuritySelectBuild WhereLicense_imgLike(params string[] License_img) {
			if (License_img == null) return this;
			return this.Where1Or(@"a.`license_img` LIKE {0}", License_img.Select(a => "%" + a + "%").ToArray());
		}
		protected new ShopsecuritySelectBuild Where1Or(string filterFormat, Array values) {
			return base.Where1Or(filterFormat, values) as ShopsecuritySelectBuild;
		}
		public ShopsecuritySelectBuild(IDAL dal) : base(dal, SqlHelper.Instance) { }
	}
}