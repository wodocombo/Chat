using Chat.Models.Data;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace Chat.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [JsonIgnore]
        public string Email { get; set; }

        [JsonIgnore]
        public string Password { get; set; }

        public string? AvatarImg { get; set; }

        public List<Group> Groups { get; set; }

        public List<User>? Friends { get; set; } 

        [JsonIgnore]
        public List<Message>? Messages { get; set; }
    }
}
