// See https://aka.ms/new-console-template for more information
using System.Collections.Generic;
using System.IO;
using TencentCos.Client;
Console.WriteLine("Hello, World!");

//if (CosClient.Instance.UpFileStream(null, null).Result.Code == 200)
//{
//    Console.WriteLine("Success");
//}
//else
//{
//    Console.WriteLine("Error");
//}

if (CosClient.Instance.UpFile(@"E:\Tool\11.txt").Result.Code == 200)
{
    Console.WriteLine("Success");
}
else
{
    Console.WriteLine("Error");
}
