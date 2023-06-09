using Chat.Models;
using Chat.Models.Data;
using Microsoft.EntityFrameworkCore;

namespace Chat.Data
{
    public class ChatContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Channel> Channels { get; set; }

        public DbSet<Group> Groups { get; set; }

        public DbSet<Message> Messages { get; set; }


        public ChatContext(DbContextOptions<ChatContext> options) : base(options)
        {

        }


        public DbSet<UserFriends> UserFriends { get; set; }

        public DbSet<UserGroups> UserGroups { get; set; }

        public DbSet<GroupInviteCode> GroupInviteCode { get; set; }

    }
}
