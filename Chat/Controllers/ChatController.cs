using Chat.Data;
using Chat.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Chat.Controllers
{
    public class ChatController : Controller
    {
        private readonly ChatContext _db;

        public ChatController(ChatContext context)
        {
            _db = context;
        }

        [HttpGet]
        public IActionResult Form(int Id)
        {
            if (HttpContext.Session.GetString("User") == null)
            {
                return BadRequest("Ошибка: Некоректный запрос");
            }

            User? user = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("User"));

            Channel channel = _db.Channels.Include(i => i.IdMainGroup).FirstOrDefault(fod => fod.Id == Id);

            if (channel == null)
            {
                return BadRequest("Ошибка: Такой группы нет");
            }

            _db.Messages.Where(w => w.MainChannel == channel).Include(i => i.IdSentUser).ToList();

            Group mainGroup = channel.IdMainGroup;

            ViewBag.CountUsersInGroup = _db.UserGroups.Count(w => w.IdGroup == mainGroup);

            ViewBag.UserIsAuthenticated = HttpContext.User.Identity.IsAuthenticated;

            return View(channel);
        }
    }
}
