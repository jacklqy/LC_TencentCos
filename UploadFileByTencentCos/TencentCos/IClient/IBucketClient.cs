using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TencentCos.Basic;

namespace TencentCos.IClient
{
    /// <summary>
    /// Bucket客户端抽象
    /// </summary>
    public interface IBucketClient : IBaseClient
    {
        // 创建存储桶
        Task<CosResponse> CreateBucket(string buketName);

        // 获取存储桶列表
        Task<CosResponse> SelectBucket(int tokenTome = 600);

        // 检查存储桶是否存在
        Task<CosResponse> CheckBucket(string buketName);

        //期待更多扩展todo...
    }
}
