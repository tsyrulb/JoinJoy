using Azure.Storage.Blobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;

namespace JoinJoy.Core.Services
{
    public class BlobStorageService
    {
        private readonly string _connectionString;
        private readonly string _containerName;

        public BlobStorageService(string connectionString, string containerName)
        {
            _connectionString = connectionString;
            _containerName = containerName;
        }

        /// <summary>
        /// Uploads a profile photo to Azure Blob Storage.
        /// </summary>
        /// <param name="photoStream">The stream of the photo to be uploaded.</param>
        /// <param name="fileName">The name of the file to be saved in the blob container.</param>
        /// <returns>The URI of the uploaded photo with a SAS token for secure access.</returns>
        public async Task<string> UploadProfilePhotoAsync(Stream photoStream, string fileName)
        {
            BlobContainerClient containerClient = new BlobContainerClient(_connectionString, _containerName);

            // Create the container if it doesn't exist. This will not change the public access level.
            await containerClient.CreateIfNotExistsAsync(PublicAccessType.None);

            BlobClient blobClient = containerClient.GetBlobClient(fileName);

            // Upload the photo. Set overwrite to true to allow replacing an existing blob with the same name.
            await blobClient.UploadAsync(photoStream, overwrite: true);

            // Generate and return the SAS URI with read access
            return GenerateBlobSasUri(fileName);
        }

        /// <summary>
        /// Generates a SAS URI for accessing a blob with read permissions.
        /// </summary>
        /// <param name="blobName">The name of the blob for which to generate the SAS URI.</param>
        /// <param name="expiryInHours">The expiration time in hours for the SAS token.</param>
        /// <returns>A URI with SAS token granting read access.</returns>
        private string GenerateBlobSasUri(string blobName, int expiryInHours = 1)
        {
            BlobContainerClient containerClient = new BlobContainerClient(_connectionString, _containerName);
            BlobClient blobClient = containerClient.GetBlobClient(blobName);

            // Check if the blob exists
            if (!blobClient.Exists())
            {
                throw new FileNotFoundException("The specified blob does not exist.");
            }

            // Generate SAS token with read permissions
            BlobSasBuilder sasBuilder = new BlobSasBuilder
            {
                BlobContainerName = containerClient.Name,
                BlobName = blobClient.Name,
                Resource = "b", // "b" stands for blob
                ExpiresOn = DateTimeOffset.UtcNow.AddHours(expiryInHours)
            };

            // Set permissions (Read access only)
            sasBuilder.SetPermissions(BlobSasPermissions.Read);

            // Generate the SAS URI
            return blobClient.GenerateSasUri(sasBuilder).ToString();
        }

        /// <summary>
        /// Deletes a profile photo from Azure Blob Storage.
        /// </summary>
        /// <param name="fileName">The name of the file to be deleted from the blob container.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task DeleteProfilePhotoAsync(string fileName)
        {
            BlobContainerClient containerClient = new BlobContainerClient(_connectionString, _containerName);
            BlobClient blobClient = containerClient.GetBlobClient(fileName);

            // Delete the blob if it exists
            await blobClient.DeleteIfExistsAsync();
        }
    }

}
