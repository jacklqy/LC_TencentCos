using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TencentCos.Basic
{
    /// <summary>
    /// Cos相关常量参数
    /// </summary>
    public class CosConstArgs
    {
        /// <summary>
        /// Cos相关配置参数
        /// </summary>
        public static CosOptions CosOption { get; set; }

        /// <summary>
        /// 初始化Cos相关参数
        /// </summary>
        /// <param name="cosOptions"></param>
        public static void Init(CosOptions cosOption)
        {
            CosOption = cosOption;
        }
    }

    /// <summary>
    /// CosOptions参数对应实体
    /// </summary>
    public class CosOptions
    {
        ////方式一：配置文件读取
        //"CosOptions": {
        //"AppId": "4563452452",
        //"Region": "ap-chongqing",
        //"SecretId": "AKI345Ru73ifghZonVw61f286786x82g4B",
        //"SecretKey": "rR3445EBrGO45645631tUp365P77",
        //"DurationSecond": 600,
        //"BucketName": "public",
        //"DirPath": "Dev/SEBR/",
        //"Host": "https://public-4563452452.cos.ap-chongqing.myqcloud.com/"
        //},

        //方式二：设置默认固定值

        /// <summary>
        /// APPID 获取参考 https://console.cloud.tencent.com/developer
        /// </summary>
        public string AppId { get; set; } = null;

        /// <summary>
        /// 设置默认的区域, COS 地域的简称请参照 https://cloud.tencent.com/document/product/436/6224
        /// </summary>
        public string Region { get; set; } = null;

        /// <summary>
        /// 云 API 密钥 SecretId, 获取 API 密钥请参照 https://console.cloud.tencent.com/cam/capi
        /// </summary>
        public string SecretId { get; set; } = null;

        /// <summary>
        /// 云 API 密钥 SecretKey, 获取 API 密钥请参照 https://console.cloud.tencent.com/cam/capi
        /// </summary>
        public string SecretKey { get; set; } = null;

        /// <summary>
        /// 每次请求签名有效时长，单位为秒
        /// </summary>
        public long DurationSecond { get; set; } = 600;

        /// <summary>
        /// 存储桶名称
        /// </summary>
        public string BucketName { get; set; } = null;

        /// <summary>
        /// Cos存储目录
        /// </summary>
        public string DirPath { get; set; } = null;

        public string Host { get; set; } = null;
    }
}
