using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TencentCos.Tool
{
    public class FileHelper
    {
        private static readonly object FileLock = new object();

        public static List<string> wordFileTypes = new List<string> { ".doc", ".docx", ".dot", ".dotx", ".rtf", ".wps", ".wpt" };

        public static List<string> excelFileTypes = new List<string> { ".xls", ".xlsx", ".csv", ".et", ".ett" };

        public static List<string> pptFileTypes = new List<string> { ".ppt", ".pptx", ".dps", ".dpt" };

        public static List<string> pdfFileTypes = new List<string> { ".pdf" };

        public static byte[] FileToBytes(string path)
        {
            if (!File.Exists(path))
            {
                return new byte[0];
            }

            FileInfo fileInfo = new FileInfo(path);
            byte[] array = new byte[fileInfo.Length];
            FileStream fileStream = fileInfo.OpenRead();
            fileStream.Read(array, 0, Convert.ToInt32(fileStream.Length));
            fileStream.Close();
            return array;
        }

        public static void BytesToFile(byte[] buff, string savepath)
        {
            if (File.Exists(savepath))
            {
                File.Delete(savepath);
            }

            FileStream fileStream = new FileStream(savepath, FileMode.CreateNew);
            BinaryWriter binaryWriter = new BinaryWriter(fileStream);
            binaryWriter.Write(buff, 0, buff.Length);
            binaryWriter.Close();
            fileStream.Close();
        }

        public static string ReadPageModel(string modelAbsPath)
        {
            return ReadPageModel(modelAbsPath, Encoding.Default);
        }

        public static string ReadPageModel(string modelAbsPath, Encoding encoding)
        {
            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                using (StreamReader streamReader = new StreamReader(modelAbsPath, encoding))
                {
                    stringBuilder.Append(streamReader.ReadToEnd());
                }

                return stringBuilder.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("读出模板'" + modelAbsPath + "'失败。错误信息：" + ex.Message);
            }
        }

        public static bool MoveFile(string strOrignFile, string strNewFile)
        {
            bool result = false;
            if (File.Exists(strOrignFile))
            {
                try
                {
                    string directoryName = Path.GetDirectoryName(strNewFile);
                    if (!Directory.Exists(directoryName))
                    {
                        Directory.CreateDirectory(directoryName);
                    }

                    if (File.Exists(strNewFile))
                    {
                        File.Delete(strNewFile);
                    }

                    File.Move(strOrignFile, strNewFile);
                    result = true;
                }
                catch (Exception ex)
                {
                    throw new Exception("移到文件'" + strOrignFile + "'失败。错误信息：" + ex.Message);
                }
            }

            return result;
        }

        public static void CopyFile(string sources, string dest)
        {
            FileSystemInfo[] fileSystemInfos = new DirectoryInfo(sources).GetFileSystemInfos();
            foreach (FileSystemInfo fileSystemInfo in fileSystemInfos)
            {
                string text = Path.Combine(dest, fileSystemInfo.Name);
                if (fileSystemInfo is FileInfo)
                {
                    File.Copy(fileSystemInfo.FullName, text, overwrite: true);
                    continue;
                }

                Directory.CreateDirectory(text);
                CopyFile(fileSystemInfo.FullName, text);
            }
        }

        public static void WriteAllText(string filePath, string content, Encoding encoding)
        {
            string directoryName = Path.GetDirectoryName(filePath);
            if (directoryName != null && !Directory.Exists(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }

            try
            {
                File.WriteAllText(filePath, content, encoding);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void WriteAllText(string filePath, string content)
        {
            string directoryName = Path.GetDirectoryName(filePath);
            if (directoryName != null && !Directory.Exists(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }

            try
            {
                File.WriteAllText(filePath, content, Encoding.UTF8);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static bool IsWord(string fileType)
        {
            return wordFileTypes.Any((string n) => n.Equals(fileType, StringComparison.OrdinalIgnoreCase));
        }

        public static bool IsExcel(string fileType)
        {
            return excelFileTypes.Any((string n) => n.Equals(fileType, StringComparison.OrdinalIgnoreCase));
        }

        public static bool IsPpt(string fileType)
        {
            return pptFileTypes.Any((string n) => n.Equals(fileType, StringComparison.OrdinalIgnoreCase));
        }

        public static bool IsPdf(string fileType)
        {
            return pdfFileTypes.Any((string n) => n.Equals(fileType, StringComparison.OrdinalIgnoreCase));
        }
    }
}
