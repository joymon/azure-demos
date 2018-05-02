using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    public class BlobItem
    {
        public string FileName { get; set; }
        public string Url { get; set; }
        public long Size { get; set; }
        public DateTimeOffset? DateModified { get; set; }
    }
    public class BlobService
    {
        public IList<BlobItem> GetBlobs(string connectionstring, string containerName)
        {
            CloudStorageAccount backupStorageAccount = CloudStorageAccount.Parse(connectionstring);

            var backupBlobClient = backupStorageAccount.CreateCloudBlobClient();
            var backupContainer = backupBlobClient.GetContainerReference(containerName);

            var blobNames = backupContainer.ListBlobs().OfType<CloudBlockBlob>().Select(i => new BlobItem { FileName = i.Name, DateModified = i.Properties.LastModified, Size = i.Properties.Length, Url = i.Uri.ToString() }).ToList();
            return blobNames;
        }
        public bool CheckContainerExists(string connectionstring, string containerName)
        {
            //Parse the connection string and return a reference to the storage account.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionstring);
            //Create the blob client object.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            return blobClient.GetContainerReference(containerName)
                         .Exists();
        }
        public bool CheckBlobExists(string connectionstring, string containerName, string fileName)
        {
            //Parse the connection string and return a reference to the storage account.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionstring);
            //Create the blob client object.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            return blobClient.GetContainerReference(containerName).GetBlobReference(fileName)
                         .Exists();
        }
        public bool CreateContainer(string connectionstring, string containerName)
        {
            var container = GetContainer(connectionstring, containerName);
            var conainterCreate = container.CreateIfNotExists();
            //SASToken _sasToken = new SASToken();
            //_sasToken.Create(connectionstring,"mypolicy", containerName);
            return true;
        }

        private CloudBlobContainer GetContainer(string connectionstring, string name)
        {
            //Parse the connection string and return a reference to the storage account.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionstring);
            //Create the blob client object.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            //Get a reference to a container to use for the sample code, and create it if it does not exist.
            CloudBlobContainer container = blobClient.GetContainerReference(name);
            return container;
        }
        public bool DeleteBlob(string containerName, string fileName, string connectionString)
        {
            // Retrieve storage account from connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);

            // Create the blob client.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve reference to a previously created container.
            CloudBlobContainer container = blobClient.GetContainerReference(containerName);

            // Retrieve reference to a blob named "myblob.txt".
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(fileName);

            // Delete the blob.
            blockBlob.Delete();
            return true;
        }
        public string GetBlobClientUri(string connectionString)
        {
            //Parse the connection string and return a reference to the storage account.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
            //Create the blob client object.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            return blobClient.BaseUri.AbsoluteUri.ToString();
        }

    }
}
