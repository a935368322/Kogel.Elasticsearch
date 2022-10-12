using Kogel.Elasticsearch.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Kogel.Elasticsearch
{
    /// <summary>
    /// 
    /// </summary>
    public class EsClient : IEsClient
    {
        /// <summary>
        /// es节点地址
        /// </summary>
        private readonly string _nodeAddress;

        /// <summary>
        /// 
        /// </summary>
        private readonly string _accessToken;

        /// <summary>
        /// 无密码创建
        /// </summary>
        /// <param name="nodeAddress">es节点地址</param>
        public EsClient(string nodeAddress)
        {
            _nodeAddress = nodeAddress;
        }

        /// <summary>
        /// basic授权创建
        /// </summary>
        /// <param name="nodeAddress"></param>
        /// <param name="userName"></param>
        /// <param name="passWord"></param>
        public EsClient(string nodeAddress, string userName, string passWord)
        {
            _nodeAddress = nodeAddress;
            string accessToken = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes($"{userName}:{passWord}"))).ToString();
            _accessToken = accessToken;
        }

        /// <summary>
        /// 查询设置
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IQuerySet<T> QuerySet<T>()
        {
            return new QuerySet<T>(this);
        }
    }
}
