using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chat.Models.Data
{
    public class UserGroups
    {
        [Key]
        public int Id { get; set; }


        [ForeignKey("UserID")]
        public virtual User IdUser { get; set; }

        [ForeignKey("GroupID")]
        public virtual Group? IdGroup { get; set; }
    }
}
