using Chat.Data;
using Chat.Models;
using Chat.Models.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text;

namespace Chat.Controllers
{
    public class GroupController : Controller
    {
        private readonly ChatContext _db;

        public GroupController(ChatContext context)
        {
            _db = context;
        }


        public IActionResult Form()
        {

            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return BadRequest("Ошибка: Вы не зарегистрированы, пожалуйста пройдите авторизацию.");
            }

            return View();
        }


        [HttpPost]
        [Route("/Group/Add")]
        public IActionResult AddGroup(IFormCollection model)
        {
            string groupName = model["groupName"];

            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return BadRequest("Ошибка: Вы не зарегистрированы, пожалуйста пройдите авторизацию.");
            }

            if (groupName.Length > 16)
            {
                return BadRequest("Ошибка: Имя группы не должно быть больше 16");
            }

            User user = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("User"));
            User fullUSer = _db.Users.Find(user.Id);

            Group newGroup = new Group() { Name = groupName, IdAdministrator = fullUSer };
            UserGroups newUserGroups = new UserGroups() { IdGroup = newGroup, IdUser = fullUSer };

            _db.Groups.Add(newGroup);
            _db.UserGroups.Add(newUserGroups);
            _db.SaveChanges();

            user.Groups.Add(newGroup);

            HttpContext.Session.SetString("User", JsonConvert.SerializeObject(user));

            return Content($"Вы создали группу: {groupName}");
        }


        [HttpPost]
        public IActionResult Join(IFormCollection model)
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return BadRequest("Ошибка: Вы не зарегистрированы, пожалуйста пройдите авторизацию.");
            }

            string code = model["code"];

            var groupInvite = _db.GroupInviteCode.Include(gic => gic.MainGroup).FirstOrDefault(gic => gic.Code == code);

            if (groupInvite == null)
            {
                return BadRequest("Ошибка: такого кода не существует.");
            }

            User user = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("User"));
            User userFull = _db.Users.Find(user.Id);

            int GroupId = groupInvite.MainGroup.Id;

            Group group = _db.Groups.Find(GroupId);

            if(!user.Groups.Any(g => g.Id == group.Id))
            {
                UserGroups userGroups = new UserGroups() { IdGroup = group, IdUser = userFull };

                groupInvite.CountUserCanJoin--;

                if (groupInvite.CountUserCanJoin == 0)
                {
                    _db.GroupInviteCode.Remove(groupInvite);
                }

                _db.UserGroups.Add(userGroups);
                _db.SaveChanges();

                user.Groups.Add(group);
                HttpContext.Session.SetString("User", JsonConvert.SerializeObject(user));
            }
            else
            {
                return BadRequest("Вы уже находитесь в группе к которой хотите присоединиться");
            }

            return Content($"Вы вступили в группу {group.Id}");
        }


        public IActionResult SettingsForm(int Id)
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return BadRequest("Ошибка: Вы не зарегистрированы, пожалуйста пройдите авторизацию.");
            }

            User user = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("User"));

            User fullUser = _db.Users.Find(user.Id);
            Group groupSettings = _db.Groups.Include(i => i.IdAdministrator).FirstOrDefault(fod => fod.Id == Id);

            if (groupSettings.IdAdministrator.Id != user.Id)
            {
                return BadRequest("Вы не админ");
            }

            return View();
        }


        [HttpPost]
        public IActionResult Settings(int Id)
        {
            if (!UserIsAdmin(Id))
            {
                return BadRequest("Ошибка: Вы не админ.");
            }

            var form = HttpContext.Request.Form;
            string methodName = form["method"];
            string text = form["text"];

            var result = methodName switch
            {
                "rename" => RenameGroup(Id, text),
                "addUser" => CreateCodeToUserInvite(Id, text),
                "deleteGroup" => DeleteGroup(Id,text)
            };

            return result;
        }


        private bool UserIsAdmin(int idGroup)
        {
            if (HttpContext.Session.GetString("User") == null)
            {
                return false;
            }

            User user = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("User"));

            if (!_db.Groups.Any(g => g.Id == idGroup && g.IdAdministrator.Id == user.Id))
            {
                return false;
            }

            return true;
        }


        private IActionResult DeleteGroup(int id, string textBool)
        {
            if (!UserIsAdmin(id))
            {
                return BadRequest("Ошибка: Вы не админ.");
            }

            if (textBool == "on")
            {
                var deleteGroup = _db.Groups.Find(id);
                _db.Groups.Remove(deleteGroup);

                User user = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("User"));
                user.Groups.Remove(user.Groups.Find(g => g.Id == id));
                HttpContext.Session.SetString("User", JsonConvert.SerializeObject(user));

                return Content("Группа удалена");
            }
            else if (textBool == null)
            {
                return Content("Группа не удалена");
            }
            
            return Content("Группа не знаю");
        }


        private IActionResult CreateCodeToUserInvite(int Id, string text)
        {
            if (!UserIsAdmin(Id))
            {
                return BadRequest("Ошибка: Вы не админ.");
            }

            int CountUserCanJoin = int.Parse(text);

            Group mainGroup = _db.Groups.Find(Id);

            Random random = new Random();
            int randomNumber = random.Next(0, 100000000);
            string codeToInvite = randomNumber.ToString("D8");

            GroupInviteCode groupInviteCode = new GroupInviteCode()
            {
                MainGroup = mainGroup,
                CountUserCanJoin = CountUserCanJoin,
                Code = codeToInvite
            };

            _db.GroupInviteCode.Add(groupInviteCode);
            _db.SaveChanges();

            return Content($"Код для приглашения друзей: {codeToInvite}");
        }


        private IActionResult RenameGroup(int GroupId, string GroupName)
        {
            if (!UserIsAdmin(GroupId))
            {
                return BadRequest("Ошибка: Вы не админ.");
            }

            if (GroupName.Trim() == "")
            {
                return BadRequest("Имя группы не может быть пустым");
            }

            if (GroupName.Length > 16)
            {
                return BadRequest("Ошибка: Имя группы не должно быть больше 16");
            }

            User user = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("User"));
            Group group = _db.Groups.Find(GroupId);

            group.Name = GroupName;
            user.Groups.FirstOrDefault(g => g.Id == GroupId).Name = GroupName;

            _db.SaveChanges();
            HttpContext.Session.SetString("User", JsonConvert.SerializeObject(user));

            return Content($"Группа переименована. Новое название группы: {GroupName} ");
        }

        [HttpGet]
        public IActionResult ImageCutForm()
        {
            return View();
        }


        [HttpPost]
        public IActionResult ChangeGroupAvatar(IFormCollection model)
        {
            int idGroup;

            if (!int.TryParse(model["idGroup"], out idGroup))
            {
                return BadRequest("Ошибка: Id группы неправильный");
            }

            if (!UserIsAdmin(idGroup))
            {
                return BadRequest("Ошибка: Вы не админ.");
            }

            string base64ImageData = model["imageData"];

            // Удаление префикса "data:image/png;base64," из строки base64
            string base64Data = base64ImageData.Substring(base64ImageData.IndexOf(',') + 1);

            // Декодирование base64 обратно в байтовый массив
            byte[] imageData = Convert.FromBase64String(base64Data);

            // Генерация уникального имени файла
            string pathToGroupAva = $"wwwroot\\data\\groups\\{idGroup}\\";

            // Определение пути для сохранения файла на сервере
            string filePath = Path.Combine(pathToGroupAva, "GroupAva.png");

            if (!Directory.Exists(pathToGroupAva))
            {
                Directory.CreateDirectory(pathToGroupAva);
            }

            // Сохранение файла на сервере
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                fileStream.Write(imageData, 0, imageData.Length);
            }

            var dasd = _db.Groups.Find(idGroup);

            string pathWithoutRoot = filePath.Replace("wwwroot\\", "");

            _db.Groups.Find(idGroup).AvataImg = pathWithoutRoot;
            _db.SaveChanges();

            User user = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("User"));

            user.Groups.FirstOrDefault(fod => fod.Id == idGroup).AvataImg = pathWithoutRoot;

            HttpContext.Session.SetString("User", JsonConvert.SerializeObject(user));

            return Content("Аватарка изменена");
        }
    }
}
