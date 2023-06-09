using Chat.Data;
using Chat.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Chat.Controllers
{
    public class MessageController : Controller
    {
        private readonly ChatContext _db;

        public MessageController(ChatContext context)
        {
            _db = context;
        }

        [HttpPost]
        public string Image([FromForm] ImageUploadModel model)
        {
            if (HttpContext.Session.GetString("User") == null)
            {
                return "Ошибка: Вы не зарегистрированы";
            }

            IFormFile image = model.Image;

            User user = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("User"));

            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);

            string pathToImage = $"wwwroot\\data\\users\\{user.Id}\\";

            string filePath = Path.Combine(pathToImage, fileName);

            if (!Directory.Exists(pathToImage))
            {
                Directory.CreateDirectory(pathToImage);
            }

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                image.CopyTo(fileStream);
            }

            return fileName;
        }

        public record class ImageUploadModel(int ChannelId, IFormFile Image);
    }
}
