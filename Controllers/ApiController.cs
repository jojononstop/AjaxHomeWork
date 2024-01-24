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
        private readonly IWebHostEnvironment _host;
        public ApiController(MyDBContext dbContext, IWebHostEnvironment host)
        {
            _dbContext = dbContext;
            _host = host;
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

        public IActionResult Avator(int id = 1)
        {
            Member? member = _dbContext.Members.Find(id);
            if (member != null)
            {
                byte[] img = member.FileData;
                return File(img, "image/jpeg");
            }

            return NotFound();
        }

        [HttpPost]
        public IActionResult Spots([FromBody] SerchDtos _serch)
        {
            //根據分類編號讀取景點資料
            var spots = _serch.categoryId == 0 ? _dbContext.SpotImagesSpot : _dbContext.SpotImagesSpot.Where(s => s.CategoryId == _serch.categoryId);
            //根據關鍵字搜尋景點資料
            if (!string.IsNullOrEmpty(_serch.keyword))
            {
                spots = spots.Where(s => s.SpotTitle.Contains(_serch.keyword));
            }

            //排序
            switch (_serch.sortBy)
            {
                case "spotTitle":
                    spots = _serch.sortType == "asc" ? spots.OrderBy(s => s.SpotTitle) : spots.OrderByDescending(s => s.SpotTitle);
                    break;
                default:
                    spots = _serch.sortType == "asc" ? spots.OrderBy(s => s.SpotId) : spots.OrderByDescending(s => s.SpotId);
                    break;
            }

            //分頁
            //總共有多少筆資料
            int TotalCount = spots.Count();
            //設定每頁顯示多少筆資料
            int pageSize = _serch.pageSize ?? 9;
            //目前要顯示第幾頁
            int page = _serch.page ?? 1;
            //計算總共有幾頁
            int TotalPages = (int)Math.Ceiling((decimal)TotalCount / pageSize);

            //取出分頁資料
            spots = spots.Skip((int)((page - 1) * pageSize)).Take(pageSize);

            SpotsPagingDtos spotsPaging = new SpotsPagingDtos();
            spotsPaging.TotalPages = TotalPages;
            spotsPaging.SpotsResult = spots.ToList();

            return Json(spotsPaging);
        }

    public IActionResult Register(Member member, IFormFile Avatar)
    {
        string fileName = "empty.jpg";
        if (Avatar != null)
        {
            fileName = Avatar.FileName;
        }

        //取得檔案上傳的實際路徑
        string uploadPath = Path.Combine(_host.WebRootPath, "uploads", fileName);
        //檔案上傳
        using (var fileStream = new FileStream(uploadPath, FileMode.Create))
        {
            Avatar?.CopyTo(fileStream);
        }

        //轉成二進位
        byte[]? imgByte = null;
        using (var memoryStream = new MemoryStream())
        {
            Avatar?.CopyTo(memoryStream);
            imgByte = memoryStream.ToArray();
        }

        member.FileName = fileName;
        member.FileData = imgByte;

        //新增
        _dbContext.Members.Add(member);
        _dbContext.SaveChanges();
        return Content("新增成功");

        // return Content($"Hello {_user.Name}, {_user.Age}歲了,電子郵件是{_user.Email}");
        //return Content($"{_user.Avatar?.FileName}-{_user.Avatar?.Length}-{_user.Avatar?.ContentType}");
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

    //public IActionResult Spot()
    //{
    //    return Content("spots");
    //}
}
}

