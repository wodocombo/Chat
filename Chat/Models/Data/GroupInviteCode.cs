using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chat.Models.Data
{
    public class GroupInviteCode
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("GroupID")]
        public Group MainGroup { get; set; }

        public string Code { get; set; }

        public int CountUserCanJoin { get; set; }
    }
}
