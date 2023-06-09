using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chat.Models.Data
{
    public class UserFriends
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("UserID")]
        public User IdUser { get; set; }

        [ForeignKey("FriendID")]
        public User? IdFriend { get; set; }
    }
}
