using BusinessLayer.Concrete;

using DataAccessLayer.EntityFramework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLayer.Abstract;
using EntityLayer.Concrete;
using Microsoft.Ajax.Utilities;
using MVCProjeKampi.Models.ViewModels;

namespace MVCProjeKampi.Controllers.AdminController
{
    [Authorize(Roles = "Administrator")]
    public class AdminStatisticsController : Controller
    {
        #region Business Katmanı Instance lar
        private IBaseService<Category> _categoryService = new CategoryManager(new EfCategoryDal());
        private IBaseService<Heading> _headingService = new HeadingManager(new EfHeadingDal());
        private IBaseService<User> _userService = new UserManager(new EfUserDal(), new EfSkillDal(), new RoleManager(new EfRoleDal(), new EfUserDal(), new EfUserRoleDal()));
        #endregion

        public ActionResult Index()
        {
            #region Toplam kategori sayısı getirme
            ViewBag.CategorySum = _categoryService.GetCount();
            #endregion

            #region Kategori tablosunda durumu true olan kategoriler ile false olan kategoriler arasındaki sayısal farkı getirme
            int categoryValuesTrue = _categoryService.GetCount(x => x.CategoryStatus == true);
            int categoryValuesFalse = _categoryService.GetCount(x => x.CategoryStatus == false);
            ViewBag.DifferenceCategoryValues = Math.Abs(categoryValuesFalse - categoryValuesTrue);

            #endregion

            #region Yazar adında 'a' harfi geçen yazar sayısı getirme

            ViewBag.WriterSum = _userService.GetCount(x => x.UserFirstName.Contains("a"));

            #endregion

            #region Başlık tablosunda "yazılım" kategorisine ait başlık sayısını getirme

            int CategoryId = _categoryService.Get(x => x.CategoryName == "Yazılım").CategoryId;

            ViewBag.HeadingSum = _categoryService.GetCount(x => x.CategoryId == CategoryId);

            #endregion

            #region En fazla başlığa sahip kategori adı getirme

            var headerList = _headingService.GetList();

            var result = headerList.GroupBy(x => x.CategoryId)
                .OrderByDescending(g => g.Count())
                .Select(y => y.FirstOrDefault().Category)
                .First();

            ViewBag.AmountMostCategoryName = _categoryService.Get(x => x.CategoryId == result.CategoryId).CategoryName;

            #endregion

            return View();
        }


        public ActionResult GetCategoryGraphInformations()
        {
            List<CategoryAndHeadingCountViewModel> viewmodel = new List<CategoryAndHeadingCountViewModel>();

            var categories = _categoryService.GetList();

            var headings = _headingService.GetList();

            foreach (var category in categories)
            {
                viewmodel.Add(new CategoryAndHeadingCountViewModel()
                {
                    Category = category,
                    HeadingCount = 0
                });
            }

            foreach (var item in viewmodel)
            {
                foreach (var heading in headings)
                {
                    if (heading.CategoryId == item.Category.CategoryId)
                    {
                        item.HeadingCount++;
                    }
                }
            }
            

            return Json(viewmodel, JsonRequestBehavior.AllowGet);
        }


        public ActionResult CategoryDonutGraph()
        {
            return View();
        }

        public ActionResult CategoryBarGraph()
        {
            return View();
        }


    }
}