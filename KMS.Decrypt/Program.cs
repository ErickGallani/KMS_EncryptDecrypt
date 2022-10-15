using System.Text;
using System.Text.Json.Nodes;
using KMS.Decrypt;
using KMS.Model;
using Newtonsoft.Json;

Console.WriteLine("Decryption service");

var kmsKey = Environment.GetEnvironmentVariable("KMS_KEY");
var awsAccessKeyId = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID");
var awsSecretAccessKey = Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY");
var serviceURL = Environment.GetEnvironmentVariable("KMS_SERVICE_URL");

var decrypter = new Decrypter(kmsKey, awsAccessKeyId, awsSecretAccessKey, serviceURL);


string tempPath = Path.GetTempPath();

string[] jsonObjects = File.ReadAllLines($"{tempPath}myobjects.json");

foreach (var jsonObject in jsonObjects)
{
    var myTestObject = JsonConvert.DeserializeObject<MyTestClass>(jsonObject);

    foreach (var metadata in myTestObject.MetaData)
    {
        if (metadata.Key == "UDF1" || metadata.Key == "UDF2")
        {
            byte[] byteArray = Convert.FromBase64String(metadata.Value.ToString());

            using (var stream = new MemoryStream(byteArray))
            {
                var result = await decrypter.DecryptAsync(stream);

                myTestObject.MetaData[metadata.Key] = result;
            }
        }
    }

    Console.WriteLine($"Decrypted text - {JsonConvert.SerializeObject(myTestObject)}");
}