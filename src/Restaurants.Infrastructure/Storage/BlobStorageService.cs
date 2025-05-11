using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using Microsoft.Extensions.Options;
using Restaurants.Domain.Interface;
using Restaurants.Infrastructure.Configuration;

namespace Restaurants.Infrastructure.Storage
{
    public class BlobStorageService(IOptions<BlobStorageSettings> blobStorageSettingsOptions) : IBlobStorageService
    {
        private readonly BlobStorageSettings _blobStorageSettings = blobStorageSettingsOptions.Value;
        public async Task<string> UploadToBlobAsync(Stream data, string fileName)
        {
            var blobServiceClient = new BlobServiceClient(_blobStorageSettings.ConnectionString);
            var containerclient = blobServiceClient.GetBlobContainerClient(_blobStorageSettings.LogosContainerName);

            var blobClient = containerclient.GetBlobClient(fileName);

            await blobClient.UploadAsync(data);

            var blobUrl = blobClient.Uri.ToString();
            return blobUrl;
        }

        public string? GetBlobSasUrl(string? blobUrl)
        {
            if (blobUrl == null) return null;
            var sasBuilder = new BlobSasBuilder()
            {
                BlobContainerName = _blobStorageSettings.LogosContainerName,
                Resource = "b",
                StartsOn = DateTimeOffset.UtcNow,
                ExpiresOn = DateTimeOffset.UtcNow.AddMinutes(30),
                BlobName = GetBlobNameFromUri(blobUrl)
            };
            sasBuilder.SetPermissions(BlobSasPermissions.Read);
            var blobServiceClient = new BlobServiceClient(_blobStorageSettings.ConnectionString);


            var sasToken = sasBuilder
                .ToSasQueryParameters(new Azure.Storage.StorageSharedKeyCredential(blobServiceClient.AccountName, _blobStorageSettings.AccountKey))
                  .ToString();

            return $"{blobUrl}?{sasToken}";
        }

        private string GetBlobNameFromUri(string blobUrl)
        {
            var uri = new Uri(blobUrl);
            return uri.Segments.Last();
        }
    }
}
