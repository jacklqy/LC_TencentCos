using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TencentCos.Basic
{
    /// <summary>
    /// Cos消息响应体
    /// </summary>
    public class CosResponse
    {
        /// <summary>
        /// 响应编码
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 响应消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 上传文件Key
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 响应数据
        /// </summary>
        public dynamic Data { get; set; }
    }
}
