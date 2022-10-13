// See https://aka.ms/new-console-template for more information
using Kogel.Elasticsearch;
using Kogel.Elasticsearch.Test.Models;

Console.WriteLine("Hello, World!");


var client = new EsClient("http://192.168.3.7:9200/");

var list = client.QuerySet<EsProduct>()
    .Where(x => x.materialCode == "M00031734" && x.accuracyInt >= 1 || x.seal=="1")
    .ToList();






/*
 *  /// <summary>
    /// 
    /// </summary>
    public class EsQuerySet
    {
        /// <summary>
        /// 或者集合
        /// </summary>
        private List<EsQuerySetItem> ShouldList { get; set; }

        /// <summary>
        /// 并且集合
        /// </summary>
        private List<EsQuerySetItem> MustList { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        private List<Dictionary<string, object>> SortList { get; set; }

        /// <summary>
        /// 
        /// </summary>
        private IHttpClient _HttpClient { get; set; }

        /// <summary>
        /// 
        /// </summary>
        private string _AccessToken { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static EsQuerySet Create(IHttpClient httpClient)
        {
            return new EsQuerySet
            {
                ShouldList = new List<EsQuerySetItem>(),
                MustList = new List<EsQuerySetItem>(),
                SortList = new List<Dictionary<string, object>>(),
                _HttpClient = httpClient
            };
        }

        /// <summary>
        /// (basic auth)
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static EsQuerySet Create(IHttpClient httpClient, string userName, string password)
        {
            string accessToken = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes($"{userName}:{password}"))).ToString();
            return new EsQuerySet
            {
                ShouldList = new List<EsQuerySetItem>(),
                MustList = new List<EsQuerySetItem>(),
                SortList = new List<Dictionary<string, object>>(),
                _HttpClient = httpClient,
                _AccessToken = accessToken
            };
        }

        /// <summary>
        /// 等于
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="esAction"></param>
        /// <returns></returns>
        public EsQuerySet Term(string key, object value, EsAction esAction = EsAction.Must)
        {
            var termDic = new Dictionary<string, object>();
            termDic.Add(key, value);
            if (esAction == EsAction.Must)
                MustList.Add(new EsQuerySetItem { TermDic = termDic });
            else
                ShouldList.Add(new EsQuerySetItem { TermDic = termDic });
            return this;
        }

        /// <summary>
        /// 模糊匹配
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="esAction"></param>
        /// <returns></returns>
        public EsQuerySet Match(string key, object value, EsAction esAction = EsAction.Must)
        {
            var matchDic = new Dictionary<string, object>();
            matchDic.Add(key, value);
            if (esAction == EsAction.Must)
                MustList.Add(new EsQuerySetItem { MatchDic = matchDic });
            else
                ShouldList.Add(new EsQuerySetItem { MatchDic = matchDic });
            return this;
        }

        /// <summary>
        /// 范围匹配
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="rangeMode"></param>
        /// <param name="esAction"></param>
        /// <returns></returns>
        public EsQuerySet Range(string key, object value, EsRange rangeMode, EsAction esAction = EsAction.Must)
        {
            var rangeDic = new Dictionary<string, (EsRange, object)>();
            rangeDic.Add(key, (rangeMode, value));
            if (esAction == EsAction.Must)
                MustList.Add(new EsQuerySetItem { RangeDic = rangeDic });
            else
                ShouldList.Add(new EsQuerySetItem { RangeDic = rangeDic });
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public dynamic ToParam(int pageIndex = 1, int pageSize = 10)
        {
            if (SortList == null || SortList.Count == 0)
            {
                var sortDic = new Dictionary<string, object>();
                sortDic.Add("Id", new { order = "desc" });
                SortList.Add(sortDic);
            }
            dynamic paramJson = new
            {
                from = (pageIndex - 1) * pageSize,
                size = pageSize,
                sort = SortList,
                query = new
                {
                    @bool = new
                    {
                        must = GetWhereList(MustList),
                        should = GetWhereList(ShouldList)
                    }
                }
            };
            return paramJson;
        }

        /// <summary>
        /// 获取条件列表
        /// </summary>
        /// <param name="whereList"></param>
        /// <returns></returns>
        private List<dynamic> GetWhereList(List<EsQuerySetItem> whereList)
        {
            if (whereList == null)
                return new List<dynamic>();
            List<dynamic> list = new List<dynamic>();
            foreach (var item in whereList)
            {
                if (item.TermDic != null)
                {
                    foreach (var term in item.TermDic)
                    {
                        var termItem = new Dictionary<string, object>();
                        termItem.Add(term.Key, term.Value);
                        list.Add(new { term = termItem });
                    }
                }
                if (item.MatchDic != null)
                {
                    foreach (var match in item.MatchDic)
                    {
                        var matchItem = new Dictionary<string, object>();
                        matchItem.Add(match.Key, match.Value);
                        list.Add(new { match = matchItem });
                    }
                }
                if (item.RangeDic != null)
                {
                    foreach (var range in item.RangeDic)
                    {
                        var rangeItem = new Dictionary<string, object>();
                        if (range.Value.Item1 == EsRange.gte)
                        {
                            rangeItem.Add(range.Key, new { gte = range.Value.Item2 });
                        }
                        else
                        {
                            rangeItem.Add(range.Key, new { lte = range.Value.Item2 });
                        }
                        list.Add(new { range = rangeItem });
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <returns></returns>
        public T Search<T>(string url)
        {
            var param = ToParam();
            if (string.IsNullOrEmpty(_AccessToken))
                return _HttpClient.Post<T>(url, param);
            else
            {
                return _HttpClient.Post<T>(url, param, _AccessToken.Replace("Basic", "").Trim(), "Basic");
            }
               
        }
    }



    /// <summary>
    /// 
    /// </summary>
    public enum EsAction
    {
        /// <summary>
        /// 
        /// </summary>
        Must,

        /// <summary>
        /// 
        /// </summary>
        Should,
    }

    /// <summary>
    /// 
    /// </summary>
    public enum EsRange
    {
        /// <summary>
        /// 大于等于
        /// </summary>
        gte,

        /// <summary>
        /// 小于等于
        /// </summary>
        lte
    }

    /// <summary>
    /// 
    /// </summary>
    public class EsQuerySetItem
    {
        /// <summary>
        /// 全文匹配
        /// </summary>
        public Dictionary<string, object> TermDic { get; set; }

        /// <summary>
        /// 模糊匹配
        /// </summary>
        public Dictionary<string, object> MatchDic { get; set; }

        /// <summary>
        /// 大小判断 (字段,(【gte大于等于,lte小于等于】,值))
        /// </summary>
        public Dictionary<string, (EsRange, object)> RangeDic { get; set; }
    }
*/