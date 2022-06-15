using System.Text.Json;
using aws.credentials.Models;

namespace aws.credentials
{
    /// <summary>
    /// 
    /// Smtp Data: 
    ///     Host, Porta: https://us-east-1.console.aws.amazon.com/ses/home?region=us-east-1#/account
    ///     Username, Password: https://us-east-1.console.aws.amazon.com/iamv2/home#/users
    ///     
    /// AwsKeys:
    ///     https://us-east-1.console.aws.amazon.com/iam/home?region=us-east-1#/security_credentials
    ///     
    /// SQS : 
    ///     https://us-east-1.console.aws.amazon.com/sqs/v2/home?region=us-east-1#/queues
    ///   
    /// 
    /// SES:
    ///     ConfigSet: https://us-east-1.console.aws.amazon.com/ses/home?region=us-east-1#/configuration-sets
    ///     FromEmailAddress: https://us-east-1.console.aws.amazon.com/ses/home?region=us-east-1#/verified-identities    
    /// 
    /// TUTORIALS:
    ///     https://www.youtube.com/watch?v=3Gyvky1196Q&t=168s&ab_channel=MaxoTech
    ///     https://www.msp360.com/resources/blog/how-to-find-your-aws-access-key-id-and-secret-access-key/#:~:text=1%20Go%20to%20Amazon%20Web,and%20Secret%20Access%20Key)%20option.
    ///     https://engage.so/blog/understanding-amazon-ses-configuration-sets-and-how-engage-uses-them/
    /// 
    /// </summary>
    public static class Credentials
    {
        #pragma warning disable CS8601 // Possível atribuição de referência nula.
        private static AwsDataModel AwsData = JsonSerializer.Deserialize<AwsDataModel>(
            @"{
	            ""Smpt"": {
                    ""Host"": ""my-host.amazonaws.com"",
                    ""Port"": 587,
                    ""Username"": ""my-username"",
                    ""Password"": ""my-password""
                },
	            ""AwsKeys"": {
		            ""AwsAccessKeyId"": ""my-access-key-id"",
		            ""AwsSecretAccessKey"": ""my-secret-access-key""
	            },
	            ""Sqs"": {
                    ""Url"": ""my-sqs-url"",
                    ""MaxNumberOfMessages"": 10,
                    ""WaitTimeSeconds"": 20
                },
	            ""Ses"": {
                  ""ConfigSet"": ""my-config-set-name"",
		          ""FromEmailAddress"": ""my-verified-email""
                }
            }");

        public static void AddEnviromentVariable(string enviromentKey = "AWS_DATA")
        {
            Environment.SetEnvironmentVariable(
                enviromentKey, JsonSerializer.Serialize(AwsData));
        }
    }

}