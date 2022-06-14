using System.Text.Json.Serialization;

namespace aws.ses.bounce.Models
{
    public class Bounce
    {
        [JsonPropertyName("feedbackId")]
        public string FeedbackId { get; set; }

        [JsonPropertyName("bounceType")]
        public string BounceType { get; set; }

        [JsonPropertyName("bouncesubType")]
        public string BouncesubType { get; set; }


        //[JsonPropertyName("bouncedRecipients")]
        //public BouncedRecipient[] BouncedRecipients { get; set; }

        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }

        [JsonPropertyName("reportinguTA")]
        public string ReportingNTA { get; set; }
    }
}
