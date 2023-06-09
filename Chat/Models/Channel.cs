using Chat.Models.Data;
using Newtonsoft.Json;

namespace Chat.Models
{
    public class Channel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [JsonIgnore]
        public Group IdMainGroup { get; set; }

        public List<Message> Messages { get; set; }
    }
}
