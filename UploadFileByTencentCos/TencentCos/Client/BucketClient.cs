using COSXML;
using COSXML.CosException;
using COSXML.Model.Bucket;
using COSXML.Model.Service;
using COSXML.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TencentCos.Basic;
using TencentCos.IClient;

namespace TencentCos.Client
{
    /// <summary>
    /// Bucket客户端
    /// </summary>
    public class BucketClient : BaseClient, IBucketClient
    {
        /// <summary>
        /// CosXmlServer对象
        /// </summary>
        private readonly CosXmlServer _cosXmlServer;

        /// <summary>
        /// AppId
        /// </summary>
        private readonly string _appid;

        /// <summary>
        /// 构造函数
        /// </summary>
        public BucketClient()
        {
            _cosXmlServer = CosSingleton.GetCosXmlServer();
            _appid = CosConstArgs.CosOption.AppId;
        }

        public static BucketClient Instance { get; } = new BucketClient();


        /// <summary>
        /// 创建存储桶
        /// </summary>
        /// <param name="buketName">存储桶名称</param>
        /// <returns></returns>
        public async Task<CosResponse> CreateBucket(string buketName)
        {
            try
            {
                string bucket = buketName + "-" + _appid; //存储桶名称 格式：BucketName-APPID
                PutBucketRequest request = new PutBucketRequest(bucket);
                //设置签名有效时长
                //request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.Seconds), 600);
                //执行请求
                PutBucketResult result = await Task.FromResult(_cosXmlServer.PutBucket(request));

                return new CosResponse { Code = 200, Message = result.GetResultInfo() };
            }
            catch (CosClientException clientEx)
            {
                return new CosResponse { Code = 0, Message = "CosClientException: " + clientEx.Message + clientEx.InnerException };
            }
            catch (CosServerException serverEx)
            {
                return new CosResponse { Code = 200, Message = "CosServerException: " + serverEx.GetInfo() };
            }
        }

        /// <summary>
        /// 获取存储桶
        /// </summary>
        /// <param name="tokenTome"></param>
        /// <returns></returns>
        public async Task<CosResponse> SelectBucket(int tokenTome = 600)
        {
            try
            {
                GetServiceRequest request = new GetServiceRequest();
                //设置签名有效时长
                request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.Seconds), tokenTome);
                //执行请求
                GetServiceResult result = await Task.FromResult(_cosXmlServer.GetService(request));
                //得到所有的buckets
                var allBuckets = result.listAllMyBuckets.buckets;

                return new CosResponse { Code = 200, Message = "Success", Data = allBuckets };
            }
            catch (CosClientException clientEx)
            {
                return new CosResponse { Code = 0, Message = "CosClientException: " + clientEx.Message };
            }
            catch (CosServerException serverEx)
            {
                return new CosResponse { Code = 0, Message = "CosServerException: " + serverEx.GetInfo() };
            }
        }

        /// <summary>
        /// 检查存储桶是否存在
        /// </summary>
        /// <param name="buketName">存储桶名称</param>
        /// <returns></returns>
        public async Task<CosResponse> CheckBucket(string buketName)
        {
            try
            {
                // 存储桶名称，此处填入格式必须为 BucketName-APPID, 其中 APPID 获取参考 https://console.cloud.tencent.com/developer
                string bucket = buketName + "-" + _appid; //存储桶名称 格式：BucketName-APPID
                DoesBucketExistRequest request = new DoesBucketExistRequest(bucket);
                //执行请求
                bool existState = await Task.FromResult(_cosXmlServer.DoesBucketExist(request));

                return new CosResponse { Code = 200, Data = existState };
            }
            catch (CosClientException clientEx)
            {
                return new CosResponse { Code = 0, Message = "CosClientException: " + clientEx.Message };
            }
            catch (CosServerException serverEx)
            {
                return new CosResponse { Code = 0, Message = "CosServerException: " + serverEx.GetInfo() };
            }
        }

    }
}
