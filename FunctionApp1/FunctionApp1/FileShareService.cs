using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Azure.Storage.Files.Shares;
using GithubToFileShareApp.DataObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GithubToFileShareApp
{
    public class FileShareService
    {

        //-------------------------------------------------
        // Create a file share
        //-------------------------------------------------
        public async Task<string> uploadCode(List<CodeChanges> filesList)
        {

            string KeyVaultUri = "https://integrationappkeyvault.vault.azure.net/";
            var KeyVaultSecret = new SecretClient(new Uri(KeyVaultUri), new DefaultAzureCredential());
            var connectionString = KeyVaultSecret.GetSecret("StorageAccountConn");

            // string connectionString = "DefaultEndpointsProtocol=https;AccountName=demostoragefileshare;AccountKey=B0IEZaVifPbDxQKVQ3j9RteZ6kTqW9GaUCov1JmAHe74FPWKjFzF3m9+WWOD77hFt4EwIFETc22y+AStauwhfQ==;EndpointSuffix=core.windows.net";

            // Instantiate a ShareClient which will be used to create and manipulate the file share
            ShareClient share = new ShareClient(connectionString.Value.Value, "fileshare");

            // Create the share if it doesn't already exist
            share.CreateIfNotExistsAsync();

            // Ensure that the share exists

            if (share.Exists())
            {
                Console.WriteLine($"Share created: {share.Name}");

                // Get a reference to the sample directory
                ShareDirectoryClient rootDirectory = share.GetDirectoryClient("ProductManagementApi");

                // Create the directory if it doesn't already exist
                rootDirectory.CreateIfNotExistsAsync();

                ShareFileClient shareFileClient;
                // Ensure that the directory exists
                if (rootDirectory.Exists())
                {
                    for (int i = 0; i < filesList.Count; i++)
                    {
                        string filePath = filesList[i].filename;
                        string content = filesList[i].patch;

                        string fileName = Path.GetFileName(filePath);
                        Console.WriteLine("GetFileName('{0}') returns '{1}'", filePath, fileName);

                        /* string directoryPath = Path.GetDirectoryName(filePath);
                        string[] dirs = directoryPath.Split("//");
                        string finalDirPath = "";
                        for (int j = 0; j < dirs.Length; j++)
                        {
                            ShareDirectoryClient directory = share.GetDirectoryClient(dirs[i]);
                            directory.CreateIfNotExistsAsync();
                            Console.WriteLine(directory.Name + " created...");
                             finalDirPath = finalDirPath + "/";
                        }*/
                        shareFileClient = new ShareFileClient(connectionString.Value.Value, share.Name, rootDirectory.Name + "/" + fileName);

                        // Ensure that the file exists then overwrite
                        if (shareFileClient.Exists())
                        {
                            //shareFileClient.Delete();
                            string filecontent = content;
                            byte[] byteArray = Encoding.UTF8.GetBytes(filecontent);
                            MemoryStream stream1 = new MemoryStream(byteArray);
                            stream1.Position = 0;
                            shareFileClient.Upload(stream1);
                            await stream1.FlushAsync();
                            stream1.Close();
                        }

                        else
                        {
                            shareFileClient.Create(500);
                            string filecontent = content;
                            byte[] byteArray = Encoding.UTF8.GetBytes(filecontent);
                            MemoryStream stream1 = new MemoryStream(byteArray);
                            stream1.Position = 0;
                            shareFileClient.Upload(stream1);
                            await stream1.FlushAsync();
                            stream1.Close();

                        }
                    }


                }
                return "File Uploaded";
            }
            else
            {
                Console.WriteLine($"CreateShareAsync failed");
                return null;
            }
        }

    }
}
