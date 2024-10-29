using COSXML;
using COSXML.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TencentCos.Basic
{
    /// <summary>
    /// 构造Cos客户端工具类
    /// </summary>
    public class CosBuilder
    {
        /// <summary>
        /// APPID
        /// </summary>
        private string _appid;

        /// <summary>
        /// 区域
        /// </summary>
        private string _region;

        /// <summary>
        /// 提供配置 SDK 接口
        /// </summary>
        private CosXmlConfig _cosXmlConfig;

        /// <summary>
        /// 提供各种 COS API 服务接口
        /// </summary>
        private CosXmlServer _cosXmlServer;

        /// <summary>
        /// 提供设置密钥信息接口
        /// </summary>
        private QCloudCredentialProvider _cosCredentialProvider;

        public CosBuilder() { }

        /// <summary>
        /// 设置账号和区域
        /// </summary>
        /// <param name="appid">APPID</param>
        /// <param name="region">区域</param>
        /// <returns></returns>
        public CosBuilder SetAccount(string appid, string region)
        {
            _appid = appid;
            _region = region;
            return this;
        }

        /// <summary>
        /// 设置CosXmlServer
        /// </summary>
        /// <param name="ConnectionTimeoutMs">连接超时时间</param>
        /// <param name="ReadWriteTimeoutMs">读写超时时间</param>
        /// <param name="IsHttps">是否启用Https</param>
        /// <param name="SetDebugLog">是否开启调试日志</param>
        /// <returns></returns>
        public CosBuilder SetCosXmlServer(int ConnectionTimeoutMs = 60000, int ReadWriteTimeoutMs = 40000, bool IsHttps = true, bool SetDebugLog = true)
        {
            _cosXmlConfig = new CosXmlConfig.Builder()
                .SetConnectionTimeoutMs(ConnectionTimeoutMs)
                .SetReadWriteTimeoutMs(ReadWriteTimeoutMs)
                .IsHttps(IsHttps)
                .SetAppid(_appid)
                .SetRegion(_region)
                .SetDebugLog(SetDebugLog)
                .Build();
            return this;
        }

        /// <summary>
        /// 设置密钥
        /// </summary>
        /// <param name="secretId">密钥ID</param>
        /// <param name="secretKey">密钥Key</param>
        /// <param name="durationSecond">持续时间(秒)</param>
        /// <returns></returns>
        public CosBuilder SetSecret(string secretId, string secretKey, long durationSecond = 600)
        {
            _cosCredentialProvider = new DefaultQCloudCredentialProvider(secretId, secretKey, durationSecond);
            return this;
        }

        /// <summary>
        /// 构造CosXmlServer
        /// </summary>
        /// <returns></returns>
        public CosXmlServer Builder()
        {
            //初始化 CosXmlServer
            _cosXmlServer = new CosXmlServer(_cosXmlConfig, _cosCredentialProvider);
            return _cosXmlServer;
        }
    }
}
