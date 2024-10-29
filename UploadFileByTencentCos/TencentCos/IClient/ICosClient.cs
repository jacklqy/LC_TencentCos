using COSXML.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TencentCos.Basic;

namespace TencentCos.IClient
{
    /// <summary>
    /// Cos客户端抽象
    /// </summary>
    public interface ICosClient : IBaseClient
    {
        // 上传文件流
        Task<CosResponse> UpFileStream(string key, Stream stream);

        // 上传二进制
        Task<CosResponse> UpFileBytes(string key, byte[] data);

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="fileAbsolutePath">文件绝对路径</param>
        Task<CosResponse> UpFile(string fileAbsolutePath);

        // 上传文件
        Task<CosResponse> UpFile(string key, string srcPath);

        // 分块上传大文件
        Task<CosResponse> UpBigFile(string key, string srcPath, Action<long, long> progressCallback, Action<CosResult> successCallback);

        // 查询存储桶的文件列表
        Task<CosResponse> SelectObjectList();

        // 下载文件
        Task<CosResponse> DownObject(string key, string localDir, string localFileName);

        // 删除文件
        Task<CosResponse> DeleteObject(string key);

        // 获取对象URL
        Task<CosResponse> GetObjectUrl(string key);

        //检查对象是否存在
        bool ObjectIsExist(string key);

        //期待更多扩展todo...
    }
}
