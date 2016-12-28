using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace pifa.Model {
	public static partial class ExtensionMethods {
		public static string ToJson(this AreaInfo item) { return string.Concat(item); }
		public static string ToJson(this AreaInfo[] items) { return GetJson(items); }
		public static string ToJson(this IEnumerable<AreaInfo> items) { return GetJson(items); }
		public static IDictionary[] ToBson(this AreaInfo[] items, Func<AreaInfo, object> func = null) { return GetBson(items, func); }
		public static IDictionary[] ToBson(this IEnumerable<AreaInfo> items, Func<AreaInfo, object> func = null) { return GetBson(items, func); }

		public static string ToJson(this Area_categoryInfo item) { return string.Concat(item); }
		public static string ToJson(this Area_categoryInfo[] items) { return GetJson(items); }
		public static string ToJson(this IEnumerable<Area_categoryInfo> items) { return GetJson(items); }
		public static IDictionary[] ToBson(this Area_categoryInfo[] items, Func<Area_categoryInfo, object> func = null) { return GetBson(items, func); }
		public static IDictionary[] ToBson(this IEnumerable<Area_categoryInfo> items, Func<Area_categoryInfo, object> func = null) { return GetBson(items, func); }

		public static string ToJson(this CategoryInfo item) { return string.Concat(item); }
		public static string ToJson(this CategoryInfo[] items) { return GetJson(items); }
		public static string ToJson(this IEnumerable<CategoryInfo> items) { return GetJson(items); }
		public static IDictionary[] ToBson(this CategoryInfo[] items, Func<CategoryInfo, object> func = null) { return GetBson(items, func); }
		public static IDictionary[] ToBson(this IEnumerable<CategoryInfo> items, Func<CategoryInfo, object> func = null) { return GetBson(items, func); }

		public static string ToJson(this ExpressInfo item) { return string.Concat(item); }
		public static string ToJson(this ExpressInfo[] items) { return GetJson(items); }
		public static string ToJson(this IEnumerable<ExpressInfo> items) { return GetJson(items); }
		public static IDictionary[] ToBson(this ExpressInfo[] items, Func<ExpressInfo, object> func = null) { return GetBson(items, func); }
		public static IDictionary[] ToBson(this IEnumerable<ExpressInfo> items, Func<ExpressInfo, object> func = null) { return GetBson(items, func); }

		public static string ToJson(this ExpressdescInfo item) { return string.Concat(item); }
		public static string ToJson(this ExpressdescInfo[] items) { return GetJson(items); }
		public static string ToJson(this IEnumerable<ExpressdescInfo> items) { return GetJson(items); }
		public static IDictionary[] ToBson(this ExpressdescInfo[] items, Func<ExpressdescInfo, object> func = null) { return GetBson(items, func); }
		public static IDictionary[] ToBson(this IEnumerable<ExpressdescInfo> items, Func<ExpressdescInfo, object> func = null) { return GetBson(items, func); }

		public static string ToJson(this FactoryInfo item) { return string.Concat(item); }
		public static string ToJson(this FactoryInfo[] items) { return GetJson(items); }
		public static string ToJson(this IEnumerable<FactoryInfo> items) { return GetJson(items); }
		public static IDictionary[] ToBson(this FactoryInfo[] items, Func<FactoryInfo, object> func = null) { return GetBson(items, func); }
		public static IDictionary[] ToBson(this IEnumerable<FactoryInfo> items, Func<FactoryInfo, object> func = null) { return GetBson(items, func); }

		public static string ToJson(this Factory_franchisingInfo item) { return string.Concat(item); }
		public static string ToJson(this Factory_franchisingInfo[] items) { return GetJson(items); }
		public static string ToJson(this IEnumerable<Factory_franchisingInfo> items) { return GetJson(items); }
		public static IDictionary[] ToBson(this Factory_franchisingInfo[] items, Func<Factory_franchisingInfo, object> func = null) { return GetBson(items, func); }
		public static IDictionary[] ToBson(this IEnumerable<Factory_franchisingInfo> items, Func<Factory_franchisingInfo, object> func = null) { return GetBson(items, func); }

		public static string ToJson(this FactorydescInfo item) { return string.Concat(item); }
		public static string ToJson(this FactorydescInfo[] items) { return GetJson(items); }
		public static string ToJson(this IEnumerable<FactorydescInfo> items) { return GetJson(items); }
		public static IDictionary[] ToBson(this FactorydescInfo[] items, Func<FactorydescInfo, object> func = null) { return GetBson(items, func); }
		public static IDictionary[] ToBson(this IEnumerable<FactorydescInfo> items, Func<FactorydescInfo, object> func = null) { return GetBson(items, func); }

		public static string ToJson(this FaqInfo item) { return string.Concat(item); }
		public static string ToJson(this FaqInfo[] items) { return GetJson(items); }
		public static string ToJson(this IEnumerable<FaqInfo> items) { return GetJson(items); }
		public static IDictionary[] ToBson(this FaqInfo[] items, Func<FaqInfo, object> func = null) { return GetBson(items, func); }
		public static IDictionary[] ToBson(this IEnumerable<FaqInfo> items, Func<FaqInfo, object> func = null) { return GetBson(items, func); }

		public static string ToJson(this FaqdescInfo item) { return string.Concat(item); }
		public static string ToJson(this FaqdescInfo[] items) { return GetJson(items); }
		public static string ToJson(this IEnumerable<FaqdescInfo> items) { return GetJson(items); }
		public static IDictionary[] ToBson(this FaqdescInfo[] items, Func<FaqdescInfo, object> func = null) { return GetBson(items, func); }
		public static IDictionary[] ToBson(this IEnumerable<FaqdescInfo> items, Func<FaqdescInfo, object> func = null) { return GetBson(items, func); }

		public static string ToJson(this FaqtypeInfo item) { return string.Concat(item); }
		public static string ToJson(this FaqtypeInfo[] items) { return GetJson(items); }
		public static string ToJson(this IEnumerable<FaqtypeInfo> items) { return GetJson(items); }
		public static IDictionary[] ToBson(this FaqtypeInfo[] items, Func<FaqtypeInfo, object> func = null) { return GetBson(items, func); }
		public static IDictionary[] ToBson(this IEnumerable<FaqtypeInfo> items, Func<FaqtypeInfo, object> func = null) { return GetBson(items, func); }

		public static string ToJson(this FranchisingInfo item) { return string.Concat(item); }
		public static string ToJson(this FranchisingInfo[] items) { return GetJson(items); }
		public static string ToJson(this IEnumerable<FranchisingInfo> items) { return GetJson(items); }
		public static IDictionary[] ToBson(this FranchisingInfo[] items, Func<FranchisingInfo, object> func = null) { return GetBson(items, func); }
		public static IDictionary[] ToBson(this IEnumerable<FranchisingInfo> items, Func<FranchisingInfo, object> func = null) { return GetBson(items, func); }

		public static string ToJson(this MarketInfo item) { return string.Concat(item); }
		public static string ToJson(this MarketInfo[] items) { return GetJson(items); }
		public static string ToJson(this IEnumerable<MarketInfo> items) { return GetJson(items); }
		public static IDictionary[] ToBson(this MarketInfo[] items, Func<MarketInfo, object> func = null) { return GetBson(items, func); }
		public static IDictionary[] ToBson(this IEnumerable<MarketInfo> items, Func<MarketInfo, object> func = null) { return GetBson(items, func); }

		public static string ToJson(this MarketdescInfo item) { return string.Concat(item); }
		public static string ToJson(this MarketdescInfo[] items) { return GetJson(items); }
		public static string ToJson(this IEnumerable<MarketdescInfo> items) { return GetJson(items); }
		public static IDictionary[] ToBson(this MarketdescInfo[] items, Func<MarketdescInfo, object> func = null) { return GetBson(items, func); }
		public static IDictionary[] ToBson(this IEnumerable<MarketdescInfo> items, Func<MarketdescInfo, object> func = null) { return GetBson(items, func); }

		public static string ToJson(this MarkettypeInfo item) { return string.Concat(item); }
		public static string ToJson(this MarkettypeInfo[] items) { return GetJson(items); }
		public static string ToJson(this IEnumerable<MarkettypeInfo> items) { return GetJson(items); }
		public static IDictionary[] ToBson(this MarkettypeInfo[] items, Func<MarkettypeInfo, object> func = null) { return GetBson(items, func); }
		public static IDictionary[] ToBson(this IEnumerable<MarkettypeInfo> items, Func<MarkettypeInfo, object> func = null) { return GetBson(items, func); }

		public static string ToJson(this Markettype_categoryInfo item) { return string.Concat(item); }
		public static string ToJson(this Markettype_categoryInfo[] items) { return GetJson(items); }
		public static string ToJson(this IEnumerable<Markettype_categoryInfo> items) { return GetJson(items); }
		public static IDictionary[] ToBson(this Markettype_categoryInfo[] items, Func<Markettype_categoryInfo, object> func = null) { return GetBson(items, func); }
		public static IDictionary[] ToBson(this IEnumerable<Markettype_categoryInfo> items, Func<Markettype_categoryInfo, object> func = null) { return GetBson(items, func); }

		public static string ToJson(this MemberInfo item) { return string.Concat(item); }
		public static string ToJson(this MemberInfo[] items) { return GetJson(items); }
		public static string ToJson(this IEnumerable<MemberInfo> items) { return GetJson(items); }
		public static IDictionary[] ToBson(this MemberInfo[] items, Func<MemberInfo, object> func = null) { return GetBson(items, func); }
		public static IDictionary[] ToBson(this IEnumerable<MemberInfo> items, Func<MemberInfo, object> func = null) { return GetBson(items, func); }

		public static string ToJson(this Member_addressbookInfo item) { return string.Concat(item); }
		public static string ToJson(this Member_addressbookInfo[] items) { return GetJson(items); }
		public static string ToJson(this IEnumerable<Member_addressbookInfo> items) { return GetJson(items); }
		public static IDictionary[] ToBson(this Member_addressbookInfo[] items, Func<Member_addressbookInfo, object> func = null) { return GetBson(items, func); }
		public static IDictionary[] ToBson(this IEnumerable<Member_addressbookInfo> items, Func<Member_addressbookInfo, object> func = null) { return GetBson(items, func); }

		public static string ToJson(this Member_marketInfo item) { return string.Concat(item); }
		public static string ToJson(this Member_marketInfo[] items) { return GetJson(items); }
		public static string ToJson(this IEnumerable<Member_marketInfo> items) { return GetJson(items); }
		public static IDictionary[] ToBson(this Member_marketInfo[] items, Func<Member_marketInfo, object> func = null) { return GetBson(items, func); }
		public static IDictionary[] ToBson(this IEnumerable<Member_marketInfo> items, Func<Member_marketInfo, object> func = null) { return GetBson(items, func); }

		public static string ToJson(this Member_productInfo item) { return string.Concat(item); }
		public static string ToJson(this Member_productInfo[] items) { return GetJson(items); }
		public static string ToJson(this IEnumerable<Member_productInfo> items) { return GetJson(items); }
		public static IDictionary[] ToBson(this Member_productInfo[] items, Func<Member_productInfo, object> func = null) { return GetBson(items, func); }
		public static IDictionary[] ToBson(this IEnumerable<Member_productInfo> items, Func<Member_productInfo, object> func = null) { return GetBson(items, func); }

		public static string ToJson(this Member_securityInfo item) { return string.Concat(item); }
		public static string ToJson(this Member_securityInfo[] items) { return GetJson(items); }
		public static string ToJson(this IEnumerable<Member_securityInfo> items) { return GetJson(items); }
		public static IDictionary[] ToBson(this Member_securityInfo[] items, Func<Member_securityInfo, object> func = null) { return GetBson(items, func); }
		public static IDictionary[] ToBson(this IEnumerable<Member_securityInfo> items, Func<Member_securityInfo, object> func = null) { return GetBson(items, func); }

		public static string ToJson(this Member_shopInfo item) { return string.Concat(item); }
		public static string ToJson(this Member_shopInfo[] items) { return GetJson(items); }
		public static string ToJson(this IEnumerable<Member_shopInfo> items) { return GetJson(items); }
		public static IDictionary[] ToBson(this Member_shopInfo[] items, Func<Member_shopInfo, object> func = null) { return GetBson(items, func); }
		public static IDictionary[] ToBson(this IEnumerable<Member_shopInfo> items, Func<Member_shopInfo, object> func = null) { return GetBson(items, func); }

		public static string ToJson(this NewsInfo item) { return string.Concat(item); }
		public static string ToJson(this NewsInfo[] items) { return GetJson(items); }
		public static string ToJson(this IEnumerable<NewsInfo> items) { return GetJson(items); }
		public static IDictionary[] ToBson(this NewsInfo[] items, Func<NewsInfo, object> func = null) { return GetBson(items, func); }
		public static IDictionary[] ToBson(this IEnumerable<NewsInfo> items, Func<NewsInfo, object> func = null) { return GetBson(items, func); }

		public static string ToJson(this News_newstagInfo item) { return string.Concat(item); }
		public static string ToJson(this News_newstagInfo[] items) { return GetJson(items); }
		public static string ToJson(this IEnumerable<News_newstagInfo> items) { return GetJson(items); }
		public static IDictionary[] ToBson(this News_newstagInfo[] items, Func<News_newstagInfo, object> func = null) { return GetBson(items, func); }
		public static IDictionary[] ToBson(this IEnumerable<News_newstagInfo> items, Func<News_newstagInfo, object> func = null) { return GetBson(items, func); }

		public static string ToJson(this NewsdescInfo item) { return string.Concat(item); }
		public static string ToJson(this NewsdescInfo[] items) { return GetJson(items); }
		public static string ToJson(this IEnumerable<NewsdescInfo> items) { return GetJson(items); }
		public static IDictionary[] ToBson(this NewsdescInfo[] items, Func<NewsdescInfo, object> func = null) { return GetBson(items, func); }
		public static IDictionary[] ToBson(this IEnumerable<NewsdescInfo> items, Func<NewsdescInfo, object> func = null) { return GetBson(items, func); }

		public static string ToJson(this NewstagInfo item) { return string.Concat(item); }
		public static string ToJson(this NewstagInfo[] items) { return GetJson(items); }
		public static string ToJson(this IEnumerable<NewstagInfo> items) { return GetJson(items); }
		public static IDictionary[] ToBson(this NewstagInfo[] items, Func<NewstagInfo, object> func = null) { return GetBson(items, func); }
		public static IDictionary[] ToBson(this IEnumerable<NewstagInfo> items, Func<NewstagInfo, object> func = null) { return GetBson(items, func); }

		public static string ToJson(this OrderInfo item) { return string.Concat(item); }
		public static string ToJson(this OrderInfo[] items) { return GetJson(items); }
		public static string ToJson(this IEnumerable<OrderInfo> items) { return GetJson(items); }
		public static IDictionary[] ToBson(this OrderInfo[] items, Func<OrderInfo, object> func = null) { return GetBson(items, func); }
		public static IDictionary[] ToBson(this IEnumerable<OrderInfo> items, Func<OrderInfo, object> func = null) { return GetBson(items, func); }

		public static string ToJson(this Order_addressInfo item) { return string.Concat(item); }
		public static string ToJson(this Order_addressInfo[] items) { return GetJson(items); }
		public static string ToJson(this IEnumerable<Order_addressInfo> items) { return GetJson(items); }
		public static IDictionary[] ToBson(this Order_addressInfo[] items, Func<Order_addressInfo, object> func = null) { return GetBson(items, func); }
		public static IDictionary[] ToBson(this IEnumerable<Order_addressInfo> items, Func<Order_addressInfo, object> func = null) { return GetBson(items, func); }

		public static string ToJson(this Order_productitemInfo item) { return string.Concat(item); }
		public static string ToJson(this Order_productitemInfo[] items) { return GetJson(items); }
		public static string ToJson(this IEnumerable<Order_productitemInfo> items) { return GetJson(items); }
		public static IDictionary[] ToBson(this Order_productitemInfo[] items, Func<Order_productitemInfo, object> func = null) { return GetBson(items, func); }
		public static IDictionary[] ToBson(this IEnumerable<Order_productitemInfo> items, Func<Order_productitemInfo, object> func = null) { return GetBson(items, func); }

		public static string ToJson(this Order_refundInfo item) { return string.Concat(item); }
		public static string ToJson(this Order_refundInfo[] items) { return GetJson(items); }
		public static string ToJson(this IEnumerable<Order_refundInfo> items) { return GetJson(items); }
		public static IDictionary[] ToBson(this Order_refundInfo[] items, Func<Order_refundInfo, object> func = null) { return GetBson(items, func); }
		public static IDictionary[] ToBson(this IEnumerable<Order_refundInfo> items, Func<Order_refundInfo, object> func = null) { return GetBson(items, func); }

		public static string ToJson(this PattrInfo item) { return string.Concat(item); }
		public static string ToJson(this PattrInfo[] items) { return GetJson(items); }
		public static string ToJson(this IEnumerable<PattrInfo> items) { return GetJson(items); }
		public static IDictionary[] ToBson(this PattrInfo[] items, Func<PattrInfo, object> func = null) { return GetBson(items, func); }
		public static IDictionary[] ToBson(this IEnumerable<PattrInfo> items, Func<PattrInfo, object> func = null) { return GetBson(items, func); }

		public static string ToJson(this ProductInfo item) { return string.Concat(item); }
		public static string ToJson(this ProductInfo[] items) { return GetJson(items); }
		public static string ToJson(this IEnumerable<ProductInfo> items) { return GetJson(items); }
		public static IDictionary[] ToBson(this ProductInfo[] items, Func<ProductInfo, object> func = null) { return GetBson(items, func); }
		public static IDictionary[] ToBson(this IEnumerable<ProductInfo> items, Func<ProductInfo, object> func = null) { return GetBson(items, func); }

		public static string ToJson(this Product_attrInfo item) { return string.Concat(item); }
		public static string ToJson(this Product_attrInfo[] items) { return GetJson(items); }
		public static string ToJson(this IEnumerable<Product_attrInfo> items) { return GetJson(items); }
		public static IDictionary[] ToBson(this Product_attrInfo[] items, Func<Product_attrInfo, object> func = null) { return GetBson(items, func); }
		public static IDictionary[] ToBson(this IEnumerable<Product_attrInfo> items, Func<Product_attrInfo, object> func = null) { return GetBson(items, func); }

		public static string ToJson(this Product_buyruleInfo item) { return string.Concat(item); }
		public static string ToJson(this Product_buyruleInfo[] items) { return GetJson(items); }
		public static string ToJson(this IEnumerable<Product_buyruleInfo> items) { return GetJson(items); }
		public static IDictionary[] ToBson(this Product_buyruleInfo[] items, Func<Product_buyruleInfo, object> func = null) { return GetBson(items, func); }
		public static IDictionary[] ToBson(this IEnumerable<Product_buyruleInfo> items, Func<Product_buyruleInfo, object> func = null) { return GetBson(items, func); }

		public static string ToJson(this Product_commentInfo item) { return string.Concat(item); }
		public static string ToJson(this Product_commentInfo[] items) { return GetJson(items); }
		public static string ToJson(this IEnumerable<Product_commentInfo> items) { return GetJson(items); }
		public static IDictionary[] ToBson(this Product_commentInfo[] items, Func<Product_commentInfo, object> func = null) { return GetBson(items, func); }
		public static IDictionary[] ToBson(this IEnumerable<Product_commentInfo> items, Func<Product_commentInfo, object> func = null) { return GetBson(items, func); }

		public static string ToJson(this Product_questionInfo item) { return string.Concat(item); }
		public static string ToJson(this Product_questionInfo[] items) { return GetJson(items); }
		public static string ToJson(this IEnumerable<Product_questionInfo> items) { return GetJson(items); }
		public static IDictionary[] ToBson(this Product_questionInfo[] items, Func<Product_questionInfo, object> func = null) { return GetBson(items, func); }
		public static IDictionary[] ToBson(this IEnumerable<Product_questionInfo> items, Func<Product_questionInfo, object> func = null) { return GetBson(items, func); }

		public static string ToJson(this ProductdescInfo item) { return string.Concat(item); }
		public static string ToJson(this ProductdescInfo[] items) { return GetJson(items); }
		public static string ToJson(this IEnumerable<ProductdescInfo> items) { return GetJson(items); }
		public static IDictionary[] ToBson(this ProductdescInfo[] items, Func<ProductdescInfo, object> func = null) { return GetBson(items, func); }
		public static IDictionary[] ToBson(this IEnumerable<ProductdescInfo> items, Func<ProductdescInfo, object> func = null) { return GetBson(items, func); }

		public static string ToJson(this ProductitemInfo item) { return string.Concat(item); }
		public static string ToJson(this ProductitemInfo[] items) { return GetJson(items); }
		public static string ToJson(this IEnumerable<ProductitemInfo> items) { return GetJson(items); }
		public static IDictionary[] ToBson(this ProductitemInfo[] items, Func<ProductitemInfo, object> func = null) { return GetBson(items, func); }
		public static IDictionary[] ToBson(this IEnumerable<ProductitemInfo> items, Func<ProductitemInfo, object> func = null) { return GetBson(items, func); }

		public static string ToJson(this RentsubletInfo item) { return string.Concat(item); }
		public static string ToJson(this RentsubletInfo[] items) { return GetJson(items); }
		public static string ToJson(this IEnumerable<RentsubletInfo> items) { return GetJson(items); }
		public static IDictionary[] ToBson(this RentsubletInfo[] items, Func<RentsubletInfo, object> func = null) { return GetBson(items, func); }
		public static IDictionary[] ToBson(this IEnumerable<RentsubletInfo> items, Func<RentsubletInfo, object> func = null) { return GetBson(items, func); }

		public static string ToJson(this Rentsublet_franchisingInfo item) { return string.Concat(item); }
		public static string ToJson(this Rentsublet_franchisingInfo[] items) { return GetJson(items); }
		public static string ToJson(this IEnumerable<Rentsublet_franchisingInfo> items) { return GetJson(items); }
		public static IDictionary[] ToBson(this Rentsublet_franchisingInfo[] items, Func<Rentsublet_franchisingInfo, object> func = null) { return GetBson(items, func); }
		public static IDictionary[] ToBson(this IEnumerable<Rentsublet_franchisingInfo> items, Func<Rentsublet_franchisingInfo, object> func = null) { return GetBson(items, func); }

		public static string ToJson(this ShopInfo item) { return string.Concat(item); }
		public static string ToJson(this ShopInfo[] items) { return GetJson(items); }
		public static string ToJson(this IEnumerable<ShopInfo> items) { return GetJson(items); }
		public static IDictionary[] ToBson(this ShopInfo[] items, Func<ShopInfo, object> func = null) { return GetBson(items, func); }
		public static IDictionary[] ToBson(this IEnumerable<ShopInfo> items, Func<ShopInfo, object> func = null) { return GetBson(items, func); }

		public static string ToJson(this Shop_franchisingInfo item) { return string.Concat(item); }
		public static string ToJson(this Shop_franchisingInfo[] items) { return GetJson(items); }
		public static string ToJson(this IEnumerable<Shop_franchisingInfo> items) { return GetJson(items); }
		public static IDictionary[] ToBson(this Shop_franchisingInfo[] items, Func<Shop_franchisingInfo, object> func = null) { return GetBson(items, func); }
		public static IDictionary[] ToBson(this IEnumerable<Shop_franchisingInfo> items, Func<Shop_franchisingInfo, object> func = null) { return GetBson(items, func); }

		public static string ToJson(this Shop_friendly_linksInfo item) { return string.Concat(item); }
		public static string ToJson(this Shop_friendly_linksInfo[] items) { return GetJson(items); }
		public static string ToJson(this IEnumerable<Shop_friendly_linksInfo> items) { return GetJson(items); }
		public static IDictionary[] ToBson(this Shop_friendly_linksInfo[] items, Func<Shop_friendly_linksInfo, object> func = null) { return GetBson(items, func); }
		public static IDictionary[] ToBson(this IEnumerable<Shop_friendly_linksInfo> items, Func<Shop_friendly_linksInfo, object> func = null) { return GetBson(items, func); }

		public static string ToJson(this ShopsecurityInfo item) { return string.Concat(item); }
		public static string ToJson(this ShopsecurityInfo[] items) { return GetJson(items); }
		public static string ToJson(this IEnumerable<ShopsecurityInfo> items) { return GetJson(items); }
		public static IDictionary[] ToBson(this ShopsecurityInfo[] items, Func<ShopsecurityInfo, object> func = null) { return GetBson(items, func); }
		public static IDictionary[] ToBson(this IEnumerable<ShopsecurityInfo> items, Func<ShopsecurityInfo, object> func = null) { return GetBson(items, func); }

		public static string ToJson(this ShopstatInfo item) { return string.Concat(item); }
		public static string ToJson(this ShopstatInfo[] items) { return GetJson(items); }
		public static string ToJson(this IEnumerable<ShopstatInfo> items) { return GetJson(items); }
		public static IDictionary[] ToBson(this ShopstatInfo[] items, Func<ShopstatInfo, object> func = null) { return GetBson(items, func); }
		public static IDictionary[] ToBson(this IEnumerable<ShopstatInfo> items, Func<ShopstatInfo, object> func = null) { return GetBson(items, func); }

		public static string GetJson(IEnumerable items) {
			StringBuilder sb = new StringBuilder();
			sb.Append("[");
			IEnumerator ie = items.GetEnumerator();
			if (ie.MoveNext()) {
				while (true) {
					sb.Append(string.Concat(ie.Current));
					if (ie.MoveNext()) sb.Append(",");
					else break;
				}
			}
			sb.Append("]");
			return sb.ToString();
		}
		public static IDictionary[] GetBson(IEnumerable items, Delegate func = null) {
			List<IDictionary> ret = new List<IDictionary>();
			IEnumerator ie = items.GetEnumerator();
			while (ie.MoveNext()) {
				if (ie.Current == null) ret.Add(null);
				else if (func == null) ret.Add(ie.Current.GetType().GetMethod("ToBson").Invoke(ie.Current, null) as IDictionary);
				else {
					object obj = func.GetMethodInfo().Invoke(func.Target, new object[] { ie.Current });
					if (obj is IDictionary) ret.Add(obj as IDictionary);
					else {
						Hashtable ht = new Hashtable();
						PropertyInfo[] pis = obj.GetType().GetProperties();
						foreach (PropertyInfo pi in pis) ht[pi.Name] = pi.GetValue(obj);
						ret.Add(ht);
					}
				}
			}
			return ret.ToArray();
		}
	}
}