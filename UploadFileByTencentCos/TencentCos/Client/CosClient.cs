using COSXML;
using COSXML.CosException;
using COSXML.Model;
using COSXML.Model.Bucket;
using COSXML.Model.Object;
using COSXML.Transfer;
using COSXML.Utils;
using TencentCos.Basic;
using TencentCos.IClient;
using TencentCos.Tool;

namespace TencentCos.Client
{
    /// <summary>
    /// Cos客户端
    /// </summary>
    public class CosClient : BaseClient, ICosClient
    {
        private readonly CosXmlServer _cosXmlServer;

        private readonly string _buketName;

        private readonly string _appid;

        private readonly string _buketAppid;

        public static CosClient Instance { get; } = new CosClient(CosConstArgs.CosOption.BucketName);


        public CosClient(string buketName)
        {
            _buketName = buketName;
            _cosXmlServer = CosSingleton.GetCosXmlServer();
            _appid = CosConstArgs.CosOption.AppId;
            _buketAppid = _buketName + "-" + _appid;
        }

        public async Task<CosResponse> UpFile(string fileAbsolutePath)
        {
            string key = await GetBuildKey(Path.GetFileName(fileAbsolutePath));
            byte[] data = FileHelper.FileToBytes(fileAbsolutePath);
            return await UpFileBytes(key, data);
        }

        public async Task<CosResponse> UpFile(string dirName, string fileAbsolutePath)
        {
            string key = await GetBuildKey(dirName, Path.GetFileName(fileAbsolutePath));
            byte[] data = FileHelper.FileToBytes(fileAbsolutePath);
            return await UpFileBytes(key, data);
        }

        public async Task<CosResponse> UpFileStream(string key, Stream stream)
        {
            try
            {
                PutObjectRequest request = new PutObjectRequest(_buketAppid, key, stream);
                PutObjectResult putObjectResult = await Task.FromResult(_cosXmlServer.PutObject(request));
                return new CosResponse
                {
                    Code = 200,
                    Message = putObjectResult.GetResultInfo(),
                    Data = _cosXmlServer.GetObjectUrl(_buketAppid, key)
                };
            }
            catch (CosClientException ex)
            {
                return new CosResponse
                {
                    Code = 0,
                    Message = "CosClientException: " + ex.Message
                };
            }
            catch (CosServerException ex2)
            {
                return new CosResponse
                {
                    Code = 0,
                    Message = "CosServerException: " + ex2.GetInfo()
                };
            }
            finally
            {
                stream?.Close();
            }
        }

        public async Task<CosResponse> UpFileBytes(string key, byte[] data)
        {
            try
            {
                PutObjectRequest request = new PutObjectRequest(_buketAppid, key, data);
                PutObjectResult putObjectResult = await Task.FromResult(_cosXmlServer.PutObject(request));
                return new CosResponse
                {
                    Code = 200,
                    Message = putObjectResult.GetResultInfo(),
                    Data = _cosXmlServer.GetObjectUrl(_buketAppid, key),
                    Key = key
                };
            }
            catch (CosClientException ex)
            {
                return new CosResponse
                {
                    Code = 0,
                    Message = "CosClientException: " + ex.Message
                };
            }
            catch (CosServerException ex2)
            {
                return new CosResponse
                {
                    Code = 0,
                    Message = "CosServerException: " + ex2.GetInfo()
                };
            }
        }

        public async Task<CosResponse> UpBigFile(string key, string srcPath, Action<long, long> progressCallback, Action<CosResult> successCallback)
        {
            CosResponse CosXmlResult = new CosResponse();
            TransferManager transferManager = new TransferManager(_cosXmlServer, new TransferConfig());
            COSXMLUploadTask uploadTask = new COSXMLUploadTask(_buketAppid, key);
            uploadTask.SetSrcPath(srcPath);
            uploadTask.progressCallback = delegate (long completed, long total)
            {
                progressCallback(completed, total);
            };
            uploadTask.successCallback = delegate (CosResult cosResult)
            {
                COSXMLUploadTask.UploadTaskResult uploadTaskResult = cosResult as COSXMLUploadTask.UploadTaskResult;
                successCallback(cosResult);
                CosXmlResult.Code = 200;
                CosXmlResult.Key = key;
                CosXmlResult.Message = uploadTaskResult.GetResultInfo();
                CosXmlResult.Data = _cosXmlServer.GetObjectUrl(_buketAppid, key);
            };
            uploadTask.failCallback = delegate (CosClientException clientEx, CosServerException serverEx)
            {
                if (clientEx != null)
                {
                    CosXmlResult.Code = 0;
                    CosXmlResult.Message = clientEx.Message;
                }

                if (serverEx != null)
                {
                    CosXmlResult.Code = 0;
                    CosXmlResult.Message = "CosServerException: " + serverEx.GetInfo();
                }
            };
            await Task.Run(delegate
            {
                transferManager.UploadAsync(uploadTask);
            });
            return CosXmlResult;
        }

        public async Task<CosResponse> SelectObjectList()
        {
            try
            {
                GetBucketRequest getBucketRequest = new GetBucketRequest(_buketAppid);
                getBucketRequest.SetSign(TimeUtils.GetCurrentTime(TimeUnit.Seconds), 600L);
                GetBucketResult getBucketResult = await Task.FromResult(_cosXmlServer.GetBucket(getBucketRequest));
                return new CosResponse
                {
                    Code = 200,
                    Data = getBucketResult.GetResultInfo()
                };
            }
            catch (CosClientException ex)
            {
                return new CosResponse
                {
                    Code = 0,
                    Data = "CosClientException: " + ex.Message
                };
            }
            catch (CosServerException ex2)
            {
                return new CosResponse
                {
                    Code = 0,
                    Data = "CosServerException: " + ex2.GetInfo()
                };
            }
        }

        public async Task<CosResponse> DownObject(string key, string localDir, string localFileName)
        {
            try
            {
                GetObjectRequest getObjectRequest = new GetObjectRequest(_buketAppid, key, localDir, localFileName);
                getObjectRequest.SetSign(TimeUtils.GetCurrentTime(TimeUnit.Seconds), 600L);
                GetObjectResult getObjectResult = await Task.FromResult(_cosXmlServer.GetObject(getObjectRequest));
                return new CosResponse
                {
                    Code = 200,
                    Message = getObjectResult.GetResultInfo()
                };
            }
            catch (CosClientException ex)
            {
                return new CosResponse
                {
                    Code = 0,
                    Message = "CosClientException: " + ex.Message
                };
            }
            catch (CosServerException ex2)
            {
                return new CosResponse
                {
                    Code = 0,
                    Message = ex2.GetInfo()
                };
            }
        }

        public async Task<CosResponse> DeleteObject(string key)
        {
            try
            {
                DeleteObjectRequest deleteObjectRequest = new DeleteObjectRequest(_buketAppid, key);
                deleteObjectRequest.SetSign(TimeUtils.GetCurrentTime(TimeUnit.Seconds), 600L);
                DeleteObjectResult deleteObjectResult = await Task.FromResult(_cosXmlServer.DeleteObject(deleteObjectRequest));
                return new CosResponse
                {
                    Code = 200,
                    Message = deleteObjectResult.GetResultInfo()
                };
            }
            catch (CosClientException ex)
            {
                return new CosResponse
                {
                    Code = 0,
                    Message = "CosClientException: " + ex.Message
                };
            }
            catch (CosServerException ex2)
            {
                return new CosResponse
                {
                    Code = 0,
                    Message = "CosServerException: " + ex2.GetInfo()
                };
            }
        }

        public async Task<CosResponse> GetObjectUrl(string key)
        {
            try
            {
                string data = await Task.FromResult(_cosXmlServer.GetObjectUrl(_buketAppid, key));
                return new CosResponse
                {
                    Code = 200,
                    Data = data,
                    Key = key
                };
            }
            catch (CosClientException ex)
            {
                return new CosResponse
                {
                    Code = 0,
                    Message = "CosClientException: " + ex.Message
                };
            }
            catch (CosServerException ex2)
            {
                return new CosResponse
                {
                    Code = 0,
                    Message = "CosServerException: " + ex2.GetInfo()
                };
            }
        }

        public bool ObjectIsExist(string key)
        {
            bool result = false;
            try
            {
                DoesObjectExistRequest request = new DoesObjectExistRequest(_buketAppid, key);
                result = _cosXmlServer.DoesObjectExist(request);
            }
            catch (CosClientException ex)
            {
                Console.WriteLine("CosClientException: " + ex);
            }
            catch (CosServerException ex2)
            {
                Console.WriteLine("CosServerException: " + ex2.GetInfo());
            }

            return result;
        }

    }
}
