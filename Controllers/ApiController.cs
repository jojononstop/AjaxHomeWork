using Ajax0122.Models;
using Ajax0122.Models.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Ajax0122.Controllers
{
    public class ApiController : Controller
    {
        private readonly MyDBContext _dbContext;
        public ApiController(MyDBContext dbContext)
        {
            _dbContext = dbContext;
        }

		//[HttpPost] error 505
		public IActionResult Index()
        {
            ////return Content("Hello Content");
            ////return Content("<h2>Hello Content</h2>", "text/html");
            System.Threading.Thread.Sleep(5000);
            return Content("<h2>Content, 你好</h2>", "text/plain", System.Text.Encoding.UTF8);
            
        }
        public IActionResult Cities()
        {
            var cities = _dbContext.Addresses.Select(a => a.City).Distinct();
            return Json(cities);
        }

        public IActionResult Districts(string city)
        {
            var districts = _dbContext.Addresses.Where(a => a.City == city).Select(a => a.SiteId).Distinct();
            return Json(districts);
        }

        public IActionResult Roads(string siteId)
        {
            var road = _dbContext.Addresses.Where(a => a.SiteId == siteId).Select(a => a.Road).Distinct();
            return Json(road);
        }

        public IActionResult Avator(int id=1)
       {
          Member? member = _dbContext.Members.Find(id);
        if(member != null)
            {
                byte[]img = member.FileData;
                return File(img, "image/jpeg");
            }
            return NotFound();
        }

        public IActionResult Register(UserDtos _user)
        {
            if (string.IsNullOrEmpty(_user.Name))
            {
				_user.Name = "Guest";
            }
            return Content($"Hello {_user.Name}, You are {_user.Age} years old.Also your Email  is {_user.Email}");
        }
        public IActionResult CheckAccount(string name)
		{// 在 _dbContext.Members 資料表中查找具有特定名稱的成員
			var member = _dbContext.Members.FirstOrDefault(a => a.Name == name);
            if (member != null)
            {
                return Content("帳號已存在");
            }
            return Content("帳號可使用");
        }
    }
}

