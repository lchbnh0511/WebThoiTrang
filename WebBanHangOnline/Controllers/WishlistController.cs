using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using System.Web.UI;
using WebBanHangOnline.Models;
using WebBanHangOnline.Models.EF;

namespace WebBanHangOnline.Controllers
{
 
    public class WishlistController : Controller
    {
        // GET: Wishlist
        public ActionResult Index(int? page)
        {
            var pageSize = 1;
            if (page == null)
            {
                page = 1;
            }
            IEnumerable<Wishlist> items = db.Wishlists.Where(x => x.UserName == User.Identity.Name).OrderByDescending(x => x.CreatedDate);
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            items = items.ToPagedList(pageIndex, pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            return View(items);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult PostWishList(int ProductID)
        {
            if (Request.IsAuthenticated == false)
            {
                return Json(new { Success = false, Message = "Bạn chưa đăng nhập..." });
            }
            var checkItem = db.Wishlists.FirstOrDefault(x=>x.ProductID == ProductID &&  x.UserName == User.Identity.Name);
            if (checkItem != null)
            {
                return Json(new { Success = false, Message = "Sản phẩm đã được thêm vào danh sách yêu thích rồi" });
            }
            var item = new Wishlist();
            item.ProductID = ProductID;
            item.UserName = User.Identity.Name;
            item.CreatedDate = DateTime.Now;
            db.Wishlists.Add(item);
            db.SaveChanges();
            return Json(new {Success = true});
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult PostDeleteWishlist(int ProductID)
        {
            var checkItem = db.Wishlists.FirstOrDefault(x => x.ProductID == ProductID && x.UserName == User.Identity.Name);
            if (checkItem != null)
            {
                db.Wishlists.Remove(checkItem);
                db.SaveChanges();
                return Json(new { Success = true, Message = "Xóa thành công" });
            }
            return Json(new { Success = false, Message = "Xóa thất bại" });
        }

        private ApplicationDbContext db = new ApplicationDbContext();
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}