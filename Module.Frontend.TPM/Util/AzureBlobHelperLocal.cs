﻿using Core.KeyVault;
using DocumentFormat.OpenXml.Packaging;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Azure.Storage.Auth;
using Microsoft.Azure.Storage.Blob;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Module.Frontend.TPM.Util
{
    class AzureBlobHelperLocal
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private static string accountname = KeyStorageManager.GetKeyVault().GetSecret("BlobStorageName", "");
        private static string accesskey = KeyStorageManager.GetKeyVault().GetSecret("BlobStorageKey", "");

        private static string GetAccessTokenAsync2()
        {
            var tokenProvider = new AzureServiceTokenProvider();
            return tokenProvider.GetAccessTokenAsync("https://storage.azure.com/").Result;
        }

        public static CloudBlobContainer GetContainer()
        {
            var accessToken = GetAccessTokenAsync2();
            string containerName = KeyStorageManager.GetKeyVault().GetSecret("BlobStorageContainer", "");

            var tokenCredential = new TokenCredential(accessToken);
            var storageCredentials = new StorageCredentials(tokenCredential);
            CloudBlobContainer container = new CloudBlobContainer(new Uri($"https://{accountname}.blob.core.windows.net/{containerName}"), storageCredentials);
            return container;
        }

        public static void UploadToBlob(string fileName, string filePath, string folderName)
        {
            CloudBlobContainer cont = GetContainer();
            cont.CreateIfNotExists();

            CloudBlockBlob cblob = cont.GetBlockBlobReference(Path.Combine(folderName, fileName));
            using (Stream file = System.IO.File.OpenRead(filePath))
            {
                cblob.UploadFromStream(file);
            }
            File.Delete(filePath);
        }
        public static void DuplicateToBlob(string fileName, string filePath, string folderName)
        {
            CloudBlobContainer cont = GetContainer();
            cont.CreateIfNotExists();
            cont.SetPermissions(new BlobContainerPermissions
            {
                PublicAccess = BlobContainerPublicAccessType.Blob
            });
            CloudBlockBlob cblob = cont.GetBlockBlobReference(Path.Combine(folderName, fileName));
            using (Stream file = System.IO.File.OpenRead(filePath))
            {
                cblob.UploadFromStream(file);
            }
        }

        public static string ReadTextFromBlob(string folderName, string fileName)
        {
            logger.Debug($"Start read from blob file: {fileName}, from folder: {folderName}");
            CloudBlobContainer cont = GetContainer();
            CloudBlockBlob cblob = cont.GetBlockBlobReference(Path.Combine(folderName, fileName));
            bool isExist = cblob.Exists();
            string result = isExist ? cblob.DownloadText() : String.Empty;
            logger.Debug($"BlockBlob is exist: {isExist}, result: {result}");
            return result;
        }
        public static byte[] ReadExcelFromBlob(string folderName, string fileName)
        {
            byte[] byteArray;
            logger.Debug($"Start read from blob file: {fileName}, from folder: {folderName}");
            CloudBlobContainer cont = GetContainer();
            CloudBlockBlob cblob = cont.GetBlockBlobReference(Path.Combine(folderName, fileName));
            using (var memoryStream = new MemoryStream())
            {
                if (cblob.Exists())
                {
                    cblob.DownloadToStream(memoryStream);
                    memoryStream.Position = 0;
                }
                byteArray = new byte[memoryStream.Length];
                byteArray = GetStreamBytes(memoryStream);


            }

            return byteArray;
        }
        public static void DeleteFromBlob(string folderName, string fileName)
        {
            logger.Debug($"Start read from blob file: {fileName}, from folder: {folderName}");
            CloudBlobContainer cont = GetContainer();
            CloudBlockBlob cblob = cont.GetBlockBlobReference(Path.Combine(folderName, fileName));

            cblob.DeleteIfExists();
        }
        public static string ReadDatFromBlob(string folderName, string fileName)
        {
            string result = "";
            logger.Debug($"Start read from blob file: {fileName}, from folder: {folderName}");
            CloudBlobContainer cont = GetContainer();
            CloudBlockBlob cblob = cont.GetBlockBlobReference(Path.Combine(folderName, fileName));

            using (var memoryStream = new MemoryStream())
            {
                if (cblob.Exists())
                {
                    cblob.DownloadToStream(memoryStream);
                    memoryStream.Position = 0;
                    using (StreamReader sr = new StreamReader(memoryStream))
                    {
                        //This allows you to do one Read operation.
                        result = sr.ReadToEnd();
                    }
                }


            }

            return result;
        }
        public static bool Copy(string sourcePath, string destPath)//source - откуда
        {
            CloudBlobContainer cont = GetContainer();
            CloudBlockBlob cblobSource = cont.GetBlockBlobReference(sourcePath);
            CloudBlockBlob cblobDest = cont.GetBlockBlobReference(destPath);
            using (var memoryStream = new MemoryStream())
            {
                if (cblobSource.Exists())
                {
                    cblobSource.DownloadToStream(memoryStream);
                    memoryStream.Position = 0;
                    cblobDest.UploadFromStream(memoryStream);
                    cblobSource.DeleteIfExists();

                }
            }
            return cblobDest.Exists();
            ;
        }
        public static IEnumerable<string> GetAllFileByDirectory(string folderName)
        {
            CloudBlobContainer cont = GetContainer();
            CloudBlobDirectory cblobSource = cont.GetDirectoryReference(folderName);
            var blobs = cblobSource.ListBlobs().ToList();
            List<string> resultBlobs = new List<string>();
            foreach (var item in blobs)
            {
                var blobFileName = item.Uri.Segments.Last().Replace("%20", " ");
                if (blobFileName != "buffer.txt")
                {
                    string blobFilePath = item.Uri.AbsolutePath.Replace(item.Container.Uri.AbsolutePath + "/", "").Replace("%20", " ");
                    resultBlobs.Add(blobFilePath);
                }
            }
            return resultBlobs.AsEnumerable();
        }
        public static bool IsExists(string folderName, string fileName)
        {
            CloudBlobContainer cont = GetContainer();
            CloudBlockBlob cblobSource = cont.GetBlockBlobReference(Path.Combine(folderName, fileName));
            return cblobSource.Exists();
            ;
        }
        private static byte[] GetStreamBytes(Stream stream)
        {
            byte[] readBuffer = new byte[stream.Length];

            int totalBytesRead = 0;
            int bytesRead;

            while ((bytesRead = stream.Read(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0)
            {
                totalBytesRead += bytesRead;

                if (totalBytesRead == readBuffer.Length)
                {
                    int nextByte = stream.ReadByte();
                    if (nextByte != -1)
                    {
                        byte[] temp = new byte[readBuffer.Length * 2];
                        Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
                        Buffer.SetByte(temp, totalBytesRead, (byte)nextByte);
                        readBuffer = temp;
                        totalBytesRead++;
                    }
                }
            }

            byte[] buffer = readBuffer;
            if (readBuffer.Length != totalBytesRead)
            {
                buffer = new byte[totalBytesRead];
                Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
            }

            return buffer;
        }
    }
}
