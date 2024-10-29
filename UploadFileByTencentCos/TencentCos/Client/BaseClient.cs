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
    /// Client基类
    /// </summary>
    public class BaseClient : IBaseClient
    {
        public async Task<string> GetBuildKey(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                throw new Exception("文件名不能为空！");
            }

            if (!CosConstArgs.CosOption.DirPath.EndsWith('/'))
            {
                CosConstArgs.CosOption.DirPath += "/";
            }

            return await Task.FromResult($"{CosConstArgs.CosOption.DirPath}{DateTime.Now:yyyy/MM/dd}/{fileName}");
        }

        public async Task<string> GetBuildKey(string dirName, string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                throw new Exception("文件名不能为空！");
            }

            if (!CosConstArgs.CosOption.DirPath.EndsWith('/'))
            {
                CosConstArgs.CosOption.DirPath += "/";
            }

            return await Task.FromResult($"{CosConstArgs.CosOption.DirPath}{dirName}/{DateTime.Now:yyyy/MM/dd}/{fileName}");
        }
    }
}
