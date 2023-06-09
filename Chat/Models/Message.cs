using Newtonsoft.Json;

namespace Chat.Models
{
    public class Message
    {
        public int Id { get; set; }

        [JsonIgnore]
        public User IdSentUser { get; set; }

        [JsonIgnore]
        public Channel MainChannel { get; set; }

        public string Text { get; set; }

        public DateTime CreatedMessage { get; set; } = DateTime.Now;

    }
}
