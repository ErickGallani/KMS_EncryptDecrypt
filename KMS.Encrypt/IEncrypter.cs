using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMS.Encrypt
{
    internal interface IEncrypter
    {
        Task<string> EncryptAsync(MemoryStream plainText);
    }
}
