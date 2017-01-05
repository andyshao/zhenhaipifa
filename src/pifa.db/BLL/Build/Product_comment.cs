using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using pifa.Model;

namespace pifa.BLL {

	public partial class Product_comment {

		protected static readonly pifa.DAL.Product_comment dal = new pifa.DAL.Product_comment();
		protected static readonly int itemCacheTimeout;

		static Product_comment() {
			if (!int.TryParse(RedisHelper.Configuration["pifa_BLL_ITEM_CACHE:Timeout_Product_comment"], out itemCacheTimeout))
				int.TryParse(RedisHelper.Configuration["pifa_BLL_ITEM_CACHE:Timeout"], out itemCacheTimeout);
		}

		#region delete, update, insert

		public static int Delete(uint Id) {
			if (itemCacheTimeout > 0) RemoveCache(GetItem(Id));
			return dal.Delete(Id);
		}
		public static int DeleteByMember_id(uint? Member_id) {
			return dal.DeleteByMember_id(Member_id);
		}
		public static int DeleteByOrder_id(uint? Order_id) {
			return dal.DeleteByOrder_id(Order_id);
		}
		public static int DeleteByProductitem_id(uint? Productitem_id) {
			return dal.DeleteByProductitem_id(Productitem_id);
		}
		public static int DeleteByProduct_id(uint? Product_id) {
			return dal.DeleteByProduct_id(Product_id);
		}

		public static int Update(Product_commentInfo item) {
			if (itemCacheTimeout > 0) RemoveCache(item);
			return dal.Update(item);
		}
		public static pifa.DAL.Product_comment.SqlUpdateBuild UpdateDiy(uint Id) {
			return UpdateDiy(null, Id);
		}
		public static pifa.DAL.Product_comment.SqlUpdateBuild UpdateDiy(Product_commentInfo item, uint Id) {
			if (itemCacheTimeout > 0) RemoveCache(item != null ? item : GetItem(Id));
			return new pifa.DAL.Product_comment.SqlUpdateBuild(item, Id);
		}
		/// <summary>
		/// 用于批量更新
		/// </summary>
		public static pifa.DAL.Product_comment.SqlUpdateBuild UpdateDiyDangerous {
			get { return new pifa.DAL.Product_comment.SqlUpdateBuild(); }
		}

		/// <summary>
		/// 适用字段较少的表；避规后续改表风险，字段数较大请改用 Product_comment.Insert(Product_commentInfo item)
		/// </summary>
		[Obsolete]
		public static Product_commentInfo Insert(uint? Member_id, uint? Order_id, uint? Product_id, uint? Productitem_id, string Content, DateTime? Create_time, string Nickname, byte? Star_price, byte? Star_quality, byte? Star_value, Product_commentSTATE? State, string Title, string Upload_image_url) {
			return Insert(new Product_commentInfo {
				Member_id = Member_id, 
				Order_id = Order_id, 
				Product_id = Product_id, 
				Productitem_id = Productitem_id, 
				Content = Content, 
				Create_time = Create_time, 
				Nickname = Nickname, 
				Star_price = Star_price, 
				Star_quality = Star_quality, 
				Star_value = Star_value, 
				State = State, 
				Title = Title, 
				Upload_image_url = Upload_image_url});
		}
		public static Product_commentInfo Insert(Product_commentInfo item) {
			item = dal.Insert(item);
			if (itemCacheTimeout > 0) RemoveCache(item);
			return item;
		}
		private static void RemoveCache(Product_commentInfo item) {
			if (item == null) return;
			RedisHelper.Remove(string.Concat("pifa_BLL_Product_comment_", item.Id));
		}
		#endregion

		public static Product_commentInfo GetItem(uint Id) {
			if (itemCacheTimeout <= 0) return Select.WhereId(Id).ToOne();
			string key = string.Concat("pifa_BLL_Product_comment_", Id);
			string value = RedisHelper.Get(key);
			if (!string.IsNullOrEmpty(value))
				try { return Product_commentInfo.Parse(value); } catch { }
			Product_commentInfo item = Select.WhereId(Id).ToOne();
			if (item == null) return null;
			RedisHelper.Set(key, item.Stringify(), itemCacheTimeout);
			return item;
		}

		public static List<Product_commentInfo> GetItems() {
			return Select.ToList();
		}
		public static Product_commentSelectBuild Select {
			get { return new Product_commentSelectBuild(dal); }
		}
		public static List<Product_commentInfo> GetItemsByMember_id(params uint?[] Member_id) {
			return Select.WhereMember_id(Member_id).ToList();
		}
		public static List<Product_commentInfo> GetItemsByMember_id(uint?[] Member_id, int limit) {
			return Select.WhereMember_id(Member_id).Limit(limit).ToList();
		}
		public static Product_commentSelectBuild SelectByMember_id(params uint?[] Member_id) {
			return Select.WhereMember_id(Member_id);
		}
		public static List<Product_commentInfo> GetItemsByOrder_id(params uint?[] Order_id) {
			return Select.WhereOrder_id(Order_id).ToList();
		}
		public static List<Product_commentInfo> GetItemsByOrder_id(uint?[] Order_id, int limit) {
			return Select.WhereOrder_id(Order_id).Limit(limit).ToList();
		}
		public static Product_commentSelectBuild SelectByOrder_id(params uint?[] Order_id) {
			return Select.WhereOrder_id(Order_id);
		}
		public static List<Product_commentInfo> GetItemsByProductitem_id(params uint?[] Productitem_id) {
			return Select.WhereProductitem_id(Productitem_id).ToList();
		}
		public static List<Product_commentInfo> GetItemsByProductitem_id(uint?[] Productitem_id, int limit) {
			return Select.WhereProductitem_id(Productitem_id).Limit(limit).ToList();
		}
		public static Product_commentSelectBuild SelectByProductitem_id(params uint?[] Productitem_id) {
			return Select.WhereProductitem_id(Productitem_id);
		}
		public static List<Product_commentInfo> GetItemsByProduct_id(params uint?[] Product_id) {
			return Select.WhereProduct_id(Product_id).ToList();
		}
		public static List<Product_commentInfo> GetItemsByProduct_id(uint?[] Product_id, int limit) {
			return Select.WhereProduct_id(Product_id).Limit(limit).ToList();
		}
		public static Product_commentSelectBuild SelectByProduct_id(params uint?[] Product_id) {
			return Select.WhereProduct_id(Product_id);
		}
	}
	public partial class Product_commentSelectBuild : SelectBuild<Product_commentInfo, Product_commentSelectBuild> {
		public Product_commentSelectBuild WhereMember_id(params uint?[] Member_id) {
			return this.Where1Or("a.`member_id` = {0}", Member_id);
		}
		public Product_commentSelectBuild WhereOrder_id(params uint?[] Order_id) {
			return this.Where1Or("a.`order_id` = {0}", Order_id);
		}
		public Product_commentSelectBuild WhereProductitem_id(params uint?[] Productitem_id) {
			return this.Where1Or("a.`productitem_id` = {0}", Productitem_id);
		}
		public Product_commentSelectBuild WhereProduct_id(params uint?[] Product_id) {
			return this.Where1Or("a.`product_id` = {0}", Product_id);
		}
		public Product_commentSelectBuild WhereId(params uint?[] Id) {
			return this.Where1Or("a.`id` = {0}", Id);
		}
		public Product_commentSelectBuild WhereContent(params string[] Content) {
			return this.Where1Or("a.`content` = {0}", Content);
		}
		public Product_commentSelectBuild WhereContentLike(params string[] Content) {
			if (Content == null || Content.Where(a => !string.IsNullOrEmpty(a)).Any() == false) return this;
			return this.Where1Or(@"a.`content` LIKE {0}", Content.Select(a => "%" + a + "%").ToArray());
		}
		public Product_commentSelectBuild WhereCreate_timeRange(DateTime? begin) {
			return base.Where("a.`create_time` >= {0}", begin) as Product_commentSelectBuild;
		}
		public Product_commentSelectBuild WhereCreate_timeRange(DateTime? begin, DateTime? end) {
			if (end == null) return WhereCreate_timeRange(begin);
			return base.Where("a.`create_time` between {0} and {1}", begin, end) as Product_commentSelectBuild;
		}
		public Product_commentSelectBuild WhereNickname(params string[] Nickname) {
			return this.Where1Or("a.`nickname` = {0}", Nickname);
		}
		public Product_commentSelectBuild WhereNicknameLike(params string[] Nickname) {
			if (Nickname == null || Nickname.Where(a => !string.IsNullOrEmpty(a)).Any() == false) return this;
			return this.Where1Or(@"a.`nickname` LIKE {0}", Nickname.Select(a => "%" + a + "%").ToArray());
		}
		public Product_commentSelectBuild WhereStar_price(params byte?[] Star_price) {
			return this.Where1Or("a.`star_price` = {0}", Star_price);
		}
		public Product_commentSelectBuild WhereStar_quality(params byte?[] Star_quality) {
			return this.Where1Or("a.`star_quality` = {0}", Star_quality);
		}
		public Product_commentSelectBuild WhereStar_value(params byte?[] Star_value) {
			return this.Where1Or("a.`star_value` = {0}", Star_value);
		}
		public Product_commentSelectBuild WhereState_IN(params Product_commentSTATE?[] States) {
			return this.Where1Or("a.`state` = {0}", States);
		}
		public Product_commentSelectBuild WhereState(Product_commentSTATE State1) {
			return this.WhereState_IN(State1);
		}
		#region WhereState
		public Product_commentSelectBuild WhereState(Product_commentSTATE State1, Product_commentSTATE State2) {
			return this.WhereState_IN(State1, State2);
		}
		public Product_commentSelectBuild WhereState(Product_commentSTATE State1, Product_commentSTATE State2, Product_commentSTATE State3) {
			return this.WhereState_IN(State1, State2, State3);
		}
		public Product_commentSelectBuild WhereState(Product_commentSTATE State1, Product_commentSTATE State2, Product_commentSTATE State3, Product_commentSTATE State4) {
			return this.WhereState_IN(State1, State2, State3, State4);
		}
		public Product_commentSelectBuild WhereState(Product_commentSTATE State1, Product_commentSTATE State2, Product_commentSTATE State3, Product_commentSTATE State4, Product_commentSTATE State5) {
			return this.WhereState_IN(State1, State2, State3, State4, State5);
		}
		#endregion
		public Product_commentSelectBuild WhereTitle(params string[] Title) {
			return this.Where1Or("a.`title` = {0}", Title);
		}
		public Product_commentSelectBuild WhereTitleLike(params string[] Title) {
			if (Title == null || Title.Where(a => !string.IsNullOrEmpty(a)).Any() == false) return this;
			return this.Where1Or(@"a.`title` LIKE {0}", Title.Select(a => "%" + a + "%").ToArray());
		}
		public Product_commentSelectBuild WhereUpload_image_url(params string[] Upload_image_url) {
			return this.Where1Or("a.`upload_image_url` = {0}", Upload_image_url);
		}
		public Product_commentSelectBuild WhereUpload_image_urlLike(params string[] Upload_image_url) {
			if (Upload_image_url == null || Upload_image_url.Where(a => !string.IsNullOrEmpty(a)).Any() == false) return this;
			return this.Where1Or(@"a.`upload_image_url` LIKE {0}", Upload_image_url.Select(a => "%" + a + "%").ToArray());
		}
		protected new Product_commentSelectBuild Where1Or(string filterFormat, Array values) {
			return base.Where1Or(filterFormat, values) as Product_commentSelectBuild;
		}
		public Product_commentSelectBuild(IDAL dal) : base(dal, SqlHelper.Instance) { }
	}
}