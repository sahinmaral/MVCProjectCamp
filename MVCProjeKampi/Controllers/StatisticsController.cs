using BusinessLayer.Concrete;

using DataAccessLayer.EntityFramework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLayer.Abstract;
using EntityLayer.Concrete;

namespace MVCProjeKampi.Controllers
{
    public class StatisticsController : Controller
    {
        #region Business Katmanı Instance lar
        private IBaseService<Category> categoryManager = new CategoryManager(new EfCategoryDal());
        private IBaseService<Writer> writerManager = new WriterManager(new EfWriterDal());
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


    }
}