using Chat.Data;
using Chat.Models;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace Chat.UsersChat
{
    public class ChatHub : Hub
    {
        private readonly ChatContext _db;
        public ChatHub(ChatContext context)
        {
            _db = context;
        }

        public async Task SendMessage(string ChannelId, string UserMessage)
        {
            if (string.IsNullOrEmpty(UserMessage))
            {
                return;
            }

            if(!Context.User.Identity.IsAuthenticated)
            {
                return;
            }

            int ChannelIdInt;

            if (!int.TryParse(ChannelId, out ChannelIdInt))
            {
                return;
            }

            var user = JsonConvert.DeserializeObject<User>(Context.GetHttpContext().Session.GetString("User"));

            User fullUser = _db.Users.Find(user.Id);

            Channel mainChannel = _db.Channels.Find(ChannelIdInt);

            UserMessage = UserMessage.Trim();

            Message message = new Message() { IdSentUser = fullUser, MainChannel = mainChannel, Text = UserMessage};

            _db.Messages.Add(message);

            _db.SaveChanges();

            string massageForm = TakeForm(fullUser, UserMessage);

            await Clients.Group(ChannelId).SendAsync("Receive", massageForm);
        }


        public async Task SendImage(int ChannelId, string imageName)
        {
            var userId = JsonConvert.DeserializeObject<User>(Context.GetHttpContext().Session.GetString("User"));

            string PathToImage = $"data/users/{userId.Id}/{imageName}";

            User mainUser = _db.Users.Find(userId.Id);

            string ChannelIdInt = ChannelId.ToString();

            Channel mainChannel = _db.Channels.Find(ChannelId);

            SendMessage(ChannelIdInt, FormForImage(PathToImage));
        }


        private string TakeForm(User User ,string message)
        {
            string avatarImagePath = User.AvatarImg ?? "img/user_icon.png";

            string form = $"<div class=\"user-message\">\r\n" +
                $"            <div class=\"user-message-photo\"><img src=\"{avatarImagePath}\" /></div>\r\n " +
                $"           <div class=\"user-message-column\">\r\n" +
                $"                <div class=\"user-message-nameData\">\r\n" +
                $"                    <div class=\"user-message-name\">{User.Name}</div>\r\n" +
                $"                    <div class=\"user-message-dateTime\">{DateTime.Now.ToLongTimeString()}</div>\r\n" +
                $"                </div>\r\n                <div class=\"user-message-text\">{message}\r\n" +
                $"            </div>\r\n" +
                $"        </div>";

            return form;
            //\U0001F600
        }

        private string FormForImage(string imagePath)
        {
            string form = $"<div class=\"user-message-image\"><img src=\"{imagePath}\"/></div>";
            return form;
        }

        public async Task AddUserToChannel(string ChannelId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, ChannelId);
            await Clients.Group(ChannelId).SendAsync("Send", $"{Context.ConnectionId} has joined the group {ChannelId}.");
        }

        public async Task RemoveUserFromChannel(string ChannelId)
        {
            await Clients.Group(ChannelId).SendAsync("Send", $"{Context.ConnectionId} has left the group {ChannelId}.");
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, ChannelId);
        }

    }
}
