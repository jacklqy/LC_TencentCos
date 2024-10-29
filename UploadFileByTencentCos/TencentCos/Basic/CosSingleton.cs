using COSXML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TencentCos.Basic
{
    /// <summary>
    /// 单例CosXmlServer，推荐使用单例
    /// </summary>
    public class CosSingleton
    {
        /// <summary>
        /// 单例CosXmlServer
        /// </summary>
        private static CosXmlServer _cosXmlServer = null;

        /// <summary>
        /// 私有化构造函数
        /// </summary>
        private CosSingleton() { }

        /// <summary>
        /// 静态构造函数：由CLR保证，在第一次使用这个类之前，自动被调用且只调用一次.
        /// 很多初始化都可以写在这里
        /// </summary>
        static CosSingleton()
        {
            // 构建一个 CoxXmlServer 对象
            _cosXmlServer = new CosBuilder()
                .SetAccount(CosConstArgs.CosOption.AppId, CosConstArgs.CosOption.Region)
                .SetCosXmlServer()
                .SetSecret(CosConstArgs.CosOption.SecretId, CosConstArgs.CosOption.SecretKey, CosConstArgs.CosOption.DurationSecond)
                .Builder();
        }

        /// <summary>
        /// 对外提供获取CosXmlServer实例的统一入口
        /// </summary>
        /// <returns></returns>
        public static CosXmlServer GetCosXmlServer()
        {
            return _cosXmlServer;
        }

    }
}
