using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using Amazon.KeyManagementService;
using Amazon.KeyManagementService.Model;

namespace KMS.Encrypt
{
    internal class Encrypter : IEncrypter
    {
        private AmazonKeyManagementServiceClient _kmsClient;
        private string _kmsKey;

        public Encrypter(string kmsKey, string awsAccessKeyId, string awsSecretAccessKey, string serviceURL)
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

        public async Task<string> EncryptAsync(MemoryStream plainText)
        {
            var encryptResponse = await _kmsClient.EncryptAsync(new EncryptRequest
            {
                KeyId = _kmsKey,
                Plaintext = plainText
            });

            return Convert.ToBase64String(encryptResponse.CiphertextBlob.ToArray());
        }
    }
}
