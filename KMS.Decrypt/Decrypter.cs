using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.KeyManagementService;
using Amazon.KeyManagementService.Model;

namespace KMS.Decrypt
{
    internal class Decrypter : IDecrypter
    {
        private AmazonKeyManagementServiceClient _kmsClient;
        private string _kmsKey;

        public Decrypter(string kmsKey, string awsAccessKeyId, string awsSecretAccessKey, string serviceURL)
        {
            _kmsClient = new AmazonKeyManagementServiceClient(
            awsAccessKeyId: awsAccessKeyId,
            awsSecretAccessKey: awsSecretAccessKey,
            new AmazonKeyManagementServiceConfig
            {
                ServiceURL = serviceURL
            });

            _kmsKey = kmsKey;
        }

        public async Task<string> DecryptAsync(MemoryStream encryptedText)
        {
            var decryptResponse = await _kmsClient.DecryptAsync(new DecryptRequest
            {
                KeyId = _kmsKey,
                CiphertextBlob = encryptedText
            });

            using (var reader = new StreamReader(decryptResponse.Plaintext))
            {
                return await reader.ReadToEndAsync();
            }
        }
    }
}
