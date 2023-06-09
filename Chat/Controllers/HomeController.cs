using Chat.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Chat.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Chat.Models.Data;

namespace Chat.Controllers
{
    public class HomeController : Controller
    {
        private readonly ChatContext _db;
        public HomeController(ChatContext context)
        {
            _db = context;
        }

        public IActionResult Index()
        {
            bool userIsAuthenticated = HttpContext.User.Identity.IsAuthenticated;

            if (userIsAuthenticated && HttpContext.Session.GetString("User") != null)
            {
                User user = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("User"));

                if (user.Id == 0)
                {
                    LoadDefaultUser();
                    return LocalRedirect("/");
                }

                ViewData["UserIsAuthenticated"] = HttpContext.User.Identity.IsAuthenticated; 

                return View(user);
            }
            else
            {
                LoadDefaultUser();

                User user = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("User"));

                ViewData["UserIsAuthenticated"] = HttpContext.User.Identity.IsAuthenticated;

                return View(user);
            }
        }

        private void LoadDefaultUser()
        {
            HttpContext.SignOutAsync();

            CreateInitialGroupAndAdmin();

            Group DefaultGroup = _db.Groups.Include(i => i.Channels).First(f => f.Id == 1); // 1 = Id общей группы Можно еще сделать
                                                                                            // так .FirstOrDefault(fod => fod.Name == "Общая")
            User DefaultUser = new User() { Groups = new List<Group>() { DefaultGroup } };

            HttpContext.Session.SetString("User", JsonConvert.SerializeObject(DefaultUser));
        }

        private void CreateInitialGroupAndAdmin()
        {
            if (_db.Groups.FirstOrDefault(fod => fod.Name == "Общая") != null)
                return;

            User AdminUser = new User() { Name = "Admin", Email = "Admin", Password = "Admin" };

            Group InitialGroup = new Group() { Name = "Общая", IdAdministrator = AdminUser };

            UserGroups userGroups = new UserGroups() { IdUser = AdminUser, IdGroup = InitialGroup };

            _db.Users.Add(AdminUser);
            _db.Groups.Add(InitialGroup);
            _db.UserGroups.Add(userGroups);
            _db.SaveChanges();
        }
    }
}
