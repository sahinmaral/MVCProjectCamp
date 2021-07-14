using BusinessLayer.Abstract;
using BusinessLayer.Concrete;

using DataAccessLayer.EntityFramework;

using MVCProjeKampi.Models.ViewModels;

using System.Linq;
using System.Web.Mvc;

namespace MVCProjeKampi.Controllers.SiteController
{

    public class HomepageController : Controller
    {
        private IHeadingService headingService = new HeadingManager(new EfHeadingDal());
        private IContentService contentService = new ContentManager(new EfContentDal());
        private IWriterService writerService = new WriterManager(new EfWriterDal(), new EfUserDal());
        private IRoleService roleService = new RoleManager(new EfRoleDal(), new EfUserDal(), new EfUserRoleDal());


        [AllowAnonymous]
        public ActionResult Index()
        {
            var headings = headingService.GetList().OrderByDescending(x => x.HeadingDate).ToList();
            var contents = contentService.GetList().OrderByDescending(x => x.ContentDate).ToList();
            var writers = writerService.GetWriterDetails();


            foreach (var content in contents)
            {
                foreach (var heading in headings)
                {
                    if (content.HeadingId == heading.HeadingId)
                        content.Heading = heading;
                }
            }

            HomepageViewModel viewmodel = new HomepageViewModel();
            viewmodel.Headings = headings;
            viewmodel.Contents = contents;
            viewmodel.Writers = writers;

            return View(viewmodel);
        }

        [AllowAnonymous]
        public PartialViewResult Sidebar()
        {
            var headings = headingService.GetList().OrderByDescending(x => x.HeadingDate).ToList();
            return PartialView(headings);
        }

        [Authorize(Roles = "Writer,User")]
        [Authorize(Roles = "Administrator,User")]
        [Authorize(Roles = "Writer,User")]
        [Authorize(Roles = "Writer,User")]
        public PartialViewResult Logon()
        {
            var username = Session["Username"].ToString();

            var roles = roleService.GetRolesForUser(username);
            
            return PartialView(roles);
        }

        [AllowAnonymous]
        public PartialViewResult Logoff()
        {
            return PartialView();
        }
    }
}