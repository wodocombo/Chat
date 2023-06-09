using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Chat.Models;
using Microsoft.EntityFrameworkCore;
using Chat.Data;
using Chat.Models.Data;
using Newtonsoft.Json;

namespace Chat.Areas.Identity.Controllers
{
    public class Identity : Controller
    {
        private readonly ChatContext _db;

        public Identity(ChatContext context)
        {
            _db = context;
        }


        [HttpPost]
        [Route("/Login")]
        public IActionResult Login(IFormCollection model)
        {
            string? email = model["email"];
            string? password = model["password"];

            email = email.Trim();
            password = password.Trim();

            if (email == "" || password == "")
            {
                return BadRequest("Поля не должны быть пустыми");
            }

            User? user = _db.Users.FirstOrDefault(u => u.Email == email && u.Password == password);

            if (user == null)
            {
                return BadRequest("Такого пользователя не существует");
            }

            var allUserGroups = _db.UserGroups.Where(ug => ug.IdUser == user).Include(ug => ug.IdGroup).ToList();

            user.Groups = allUserGroups.Select(s => s.IdGroup).ToList();

            foreach (var item in user.Groups)
            {
                _db.Channels.Where(c => c.IdMainGroup == item).ToList();
            }

            IdentityUser(user);

            return LocalRedirect("~/");
        }


        [HttpPost]
        [Route("/Registration")]
        public IActionResult Registration(IFormCollection model)
        {
            string? email = model["email"];
            string? password = model["password"];
            string? nickname = model["nickname"];

            email = email.Trim();
            password = password.Trim();
            nickname = nickname.Trim();

            if (email == "" || password == "" || nickname == "")
            {
                return BadRequest("Поля не должны быть пустыми");
            }

            Group? startGroup = _db.Groups.Find(1); // 1 = ID начальной группы (общей для всех)

            User newUser = new User() { Email = email, Password = password, Name = nickname };
            UserGroups userGroups = new UserGroups() { IdGroup = startGroup, IdUser = newUser };

            _db.Users.Add(newUser);
            _db.UserGroups.Add(userGroups);
            _db.SaveChanges();

            newUser.Groups = new List<Group>() { startGroup };

            IdentityUser(newUser);

            return LocalRedirect("~/");
        }


        private void IdentityUser(User user)
        {
            //Создание сессии
            HttpContext.Session.SetString("User", JsonConvert.SerializeObject(user));

            //Аутентификация с помощью куки
            var clain = new List<Claim> { new Claim(ClaimTypes.Name, user.Id.ToString()) };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(clain, "Cookies");

            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
        }


        [Area("Identity")]
        [Route("/Identity/LoginForm")]
        public IActionResult LoginForm()
        {
            return View();
        }
        

        [Area("Identity")]
        [Route("/Identity/Logout")]
        public void Logout()
        {
            HttpContext.SignOutAsync();
            HttpContext.Session.Clear();
        }
    }
}
