using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TencentCos.IClient
{
    /// <summary>
    /// Client基类抽象
    /// </summary>
    public interface IBaseClient
    {
        //获取构造Key
        Task<string> GetBuildKey(string fileName);
    }
}
