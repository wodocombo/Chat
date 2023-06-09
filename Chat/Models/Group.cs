using Chat.Models.Data;
using Newtonsoft.Json;
using System.Threading.Channels;

namespace Chat.Models
{
    public class Group
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string? AvataImg { get; set; }

        public List<Channel> Channels { get; set; }

        [JsonIgnore]
        public User IdAdministrator { get; set; }

    }
}
