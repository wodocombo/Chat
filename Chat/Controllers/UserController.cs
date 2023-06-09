using Chat.Data;
using Chat.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Chat.Controllers
{
    public class UserController : Controller
    {
        private readonly ChatContext _db;

        public UserController(ChatContext context)
        {
            _db = context;
        }


        public IActionResult Profile()
        {
            if (!UserIsIdentity())
            {
                return BadRequest("Пользователь не залогинин");
            }

            User user = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("User"));
            return View(user);
        }


        public IActionResult ChangeUserAvatar(IFormCollection model)
        {
            if (!UserIsIdentity())
            {
                return BadRequest("Пользователь не залогинин");
            }

            User user = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("User"));

            string base64ImageData = model["imageData"];

            // Удаление префикса "data:image/png;base64," из строки base64
            string base64Data = base64ImageData.Substring(base64ImageData.IndexOf(',') + 1);

            // Декодирование base64 обратно в байтовый массив
            byte[] imageData = Convert.FromBase64String(base64Data);

            // Генерация уникального имени файла
            string pathToUserAva = $"wwwroot\\data\\users\\{user.Id}\\Ava\\";

            // Определение пути для сохранения файла на сервере
            string filePath = Path.Combine(pathToUserAva, "Avatar.png");

            if (!Directory.Exists(pathToUserAva))
            {
                Directory.CreateDirectory(pathToUserAva);
            }

            // Сохранение файла на сервере
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                fileStream.Write(imageData, 0, imageData.Length);
            }

            string pathWithoutRoot = filePath.Replace("wwwroot\\", "");

            _db.Users.Find(user.Id).AvatarImg = pathWithoutRoot;
            _db.SaveChanges();

            user.AvatarImg = pathWithoutRoot;
            HttpContext.Session.SetString("User", JsonConvert.SerializeObject(user));

            return Content("Ваша картинка изменена");
        }

        private bool UserIsIdentity()
        {
            if (HttpContext.Session.GetString("User") == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
