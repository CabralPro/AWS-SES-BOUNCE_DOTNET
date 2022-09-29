namespace aws.credentials.Models
{
    public class AwsDataModel
    {
        public SmptModel Smpt { get; set; }
        public AwsKeysModel AwsKeys { get; set; }
        public string AwsRegionEndpoint { get; set; }
        public SqsModel Sqs { get; set; }
        public SesModel Ses { get; set; }
    }

    public class AwsKeysModel
    {
        public string AwsAccessKeyId { get; set; }
        public string AwsSecretAccessKey { get; set; }
    }

    public class SesModel
    {
        public string ConfigSet { get; set; }
        public string FromEmailAddress { get; set; }
    }

    public class SmptModel
    {
        public string Host { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int Port { get; set; }
    }

    public class SqsModel
    {
        public string Url { get; set; }
        public int MaxNumberOfMessages { get; set; }
        public int WaitTimeSeconds { get; set; }
    }
}
