using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMS.Decrypt
{
    internal interface IDecrypter
    {
        Task<string> DecryptAsync(MemoryStream encryptedText);
    }
}
