using System.Text;
using KMS.Encrypt;
using KMS.Model;
using Newtonsoft.Json;

Console.WriteLine("Encryption service");

var kmsKey = Environment.GetEnvironmentVariable("KMS_KEY");
var awsAccessKeyId = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID");
var awsSecretAccessKey = Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY");
var serviceURL = Environment.GetEnvironmentVariable("KMS_SERVICE_URL");

var encrypter = new Encrypter(kmsKey, awsAccessKeyId, awsSecretAccessKey, serviceURL);

var myJsonObject = new MyTestClass
{
    Id = Guid.NewGuid().ToString(),
    CreatedOn = DateTime.UtcNow,
    MetaData = new()
    {
        { "Prop1", "Test" },
        { "UDF1", "1235abdgdty" },
        { "UDF2", "testing prop to be encrypted" }
    }
};

foreach (var metadata in myJsonObject.MetaData)
{
    if (metadata.Key == "UDF1" || metadata.Key == "UDF2")
    {
        byte[] byteArray = Encoding.ASCII.GetBytes(metadata.Value.ToString());

        using (var stream = new MemoryStream(byteArray))
        {
            var result = await encrypter.EncryptAsync(stream);

            myJsonObject.MetaData[metadata.Key] = result;
        }
    }    
}

string tempPath = Path.GetTempPath();
JsonFileUtils.SimpleWrite(myJsonObject, $"{tempPath}myobjects.json");

class JsonFileUtils
{
    private static readonly JsonSerializerSettings _options
        = new() { NullValueHandling = NullValueHandling.Ignore };

    public static void SimpleWrite(object obj, string fileName)
    {
        var jsonString = JsonConvert.SerializeObject(obj, _options);
        File.WriteAllText(fileName, jsonString);
    }
}

