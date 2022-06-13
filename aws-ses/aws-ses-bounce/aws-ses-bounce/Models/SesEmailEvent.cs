using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace aws_ses_bounce.Models
{
    public class SesEmailEvent
    {
        [JsonPropertyName("eventType")]
        public string EventType { get; set; }

        //[JsonPropertyName("mail")]
        //public Mail Mail { get; set; }

        //[JsonPropertyName("delivery")]
        //public Delivery Delivery { get; set; }

        //[JsonPropertyName("bounce")]
        //public Bounce Bounce { get; set; }
    }

}
