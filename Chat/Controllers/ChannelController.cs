using Chat.Data;
using Chat.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Chat.Controllers
{
    public class ChannelController : Controller
    {
        private readonly ChatContext _db;

        public ChannelController(ChatContext context)
        {
            _db = context;
        }


        [HttpGet]
        public IActionResult GetChannels(int Id)
        {
            if(HttpContext.Session.GetString("User") == null)
            {
                return BadRequest("Ошибка: Некоректный запрос");
            }

            User user = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("User"));

            if (!user.Groups.Any(g => g.Id == Id))
            {
                return BadRequest("У пользователя нет такой группы");
            }

            Models.Group group = _db.Groups.Include(i => i.Channels).FirstOrDefault(fod => fod.Id == Id);         

            User AdminUser = _db.Groups.Where(g => g.Id == group.Id).Select(u => u.IdAdministrator).FirstOrDefault();

            if (user.Id == AdminUser.Id)
            {
                ViewData["UserIsAdmin"] = true;
            }
            else
            {
                ViewData["UserIsAdmin"] = false;
            }

            return View(group);
        }


        public IActionResult AddChannelForm(int Id)
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return BadRequest("Вы не зарегистрированы, вы не можете добавлять каналы.");
            }

            User user = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("User"));

            User fullUser = _db.Users.Find(user.Id);

            Chat.Models.Group groupSettings = _db.Groups.Find(Id);

            if (fullUser != groupSettings.IdAdministrator)
            {
                return BadRequest("Вы не админ, вы не можете добавлять каналы.");
            }

            return View();
        }


        public IActionResult AddChannel(IFormCollection model)
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return BadRequest("Вы не зарегистрированы, вы не можете добавлять каналы.");
            }

            User user = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("User"));

            string? nameChannel = model["channelName"];
            int groupId;

            if (!int.TryParse(model["groupId"].ToString(), out groupId))
            {
                return BadRequest("Id группы недействителен");
            }

            nameChannel = nameChannel.Trim();

            if (nameChannel == "")
            {
                return BadRequest("Имя канала не может быть пустым");
            }

            Chat.Models.Group group = _db.Groups.Include(i => i.IdAdministrator).FirstOrDefault(fod => fod.Id == groupId);

            if (group.IdAdministrator.Id != user.Id)
            {
                return BadRequest("Вы не админ");
            }

            Channel newChannel = new Channel() { IdMainGroup = group, Name = nameChannel };

            _db.Channels.Add(newChannel);
            _db.SaveChanges();

            var localUserGroup = user.Groups.FirstOrDefault(g => g.Id == groupId);

            if (localUserGroup?.Channels == null)
            {
                localUserGroup.Channels = new List<Channel>();
            }

            localUserGroup.Channels.Add(newChannel);

            HttpContext.Session.SetString("User", JsonConvert.SerializeObject(user));

            return Content("ok");
        }

        [HttpGet]
        public IActionResult RenameForm()
        {
            return View();
        }


        [HttpPost]
        public IActionResult RenameChannel(IFormCollection model)
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return BadRequest("Вы не зарегистрированы");
            }

            User user = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("User"));

            string nameChannelNew = model["channelName"];

            int idChannel;

            if (!int.TryParse(model["channelId"], out idChannel))
            {
                return BadRequest("Id недействителен");
            }

            nameChannelNew = nameChannelNew.Trim();

            if (nameChannelNew == "")
            {
                return BadRequest("Имя канала не может быть пустым");
            }

            Channel fullChannel = _db.Channels.Include(i => i.IdMainGroup.IdAdministrator).FirstOrDefault(fod => fod.Id == idChannel);

            if(fullChannel.IdMainGroup.IdAdministrator.Id != user.Id)
            {
                return BadRequest("Вы не админ");

            }

            fullChannel.Name = nameChannelNew;
            _db.SaveChanges();

            return Content("Имя канала измененно");
        }

    }
}
