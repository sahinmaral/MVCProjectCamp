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
        private IBaseService<Category> categoryManager = new CategoryManager(new EfCategoryDal());
        private IBaseService<Writer> writerManager = new WriterManager(new EfWriterDal(),new EfUserDal());
        private IBaseService<Heading> headingManager = new HeadingManager(new EfHeadingDal());
        #endregion

        public ActionResult Index()
        {
            #region Toplam kategori sayısı getirme
            ViewBag.CategorySum = categoryManager.GetCount();
            #endregion

            #region Kategori tablosunda durumu true olan kategoriler ile false olan kategoriler arasındaki sayısal farkı getirme
            int categoryValuesTrue = categoryManager.GetCount(x => x.CategoryStatus == true);
            int categoryValuesFalse = categoryManager.GetCount(x => x.CategoryStatus == false);
            ViewBag.DifferenceCategoryValues = Math.Abs(categoryValuesFalse - categoryValuesTrue);

            #endregion

            #region Yazar adında 'a' harfi geçen yazar sayısı getirme

            ViewBag.WriterSum = writerManager.GetCount(x => x.User.UserFirstName.Contains("a"));

            #endregion

            #region Başlık tablosunda "yazılım" kategorisine ait başlık sayısını getirme

            int CategoryId = categoryManager.Get(x => x.CategoryName == "Yazılım").CategoryId;

            ViewBag.HeadingSum = categoryManager.GetCount(x => x.CategoryId == CategoryId);

            #endregion

            #region En fazla başlığa sahip kategori adı getirme

            var headerList = headingManager.GetList();

            var result = headerList.GroupBy(x => x.CategoryId)
                .OrderByDescending(g => g.Count())
                .Select(y => y.FirstOrDefault().Category)
                .First();

            ViewBag.AmountMostCategoryName = categoryManager.Get(x => x.CategoryId == result.CategoryId).CategoryName;

            #endregion

            return View();
        }


        public ActionResult GetCategoryDonutGraph()
        {
            List<CategoryAndHeadingCountViewModel> viewmodel = new List<CategoryAndHeadingCountViewModel>();

            var categories = categoryManager.GetList();

            var headings = headingManager.GetList();

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


    }
}