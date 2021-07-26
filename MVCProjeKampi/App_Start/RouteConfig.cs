using System.Security.Policy;
using System.Web.Mvc;
using System.Web.Routing;

namespace MVCProjeKampi
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");


            routes.MapRoute(
                name: "About_Index",
                url: "site-hakkinda",
                defaults: new { controller = "About", action = "Index" }
            );

            routes.MapRoute(
                name: "Logins_Login",
                url: "giris-yap",
                defaults: new { controller = "Logins", action = "Login" }
            );

            routes.MapRoute(
                name: "Logins_Registration",
                url: "kayit-ol",
                defaults: new { controller = "Logins", action = "Registration" }
            );

            routes.MapRoute(
                name: "AdminProfiles_EditProfile",
                url: "admin-profil/profilini-duzenle",
                defaults: new { controller = "AdminProfiles", action = "EditProfile" }
            );

            routes.MapRoute(
                name: "AdminProfiles_Skills",
                url: "admin-profil/becerilerim",
                defaults: new { controller = "AdminProfiles", action = "Skills" }
            );

            routes.MapRoute(
                name: "AdminStatistics_CategoryDonutGraph",
                url: "admin-profil/istatistik/kategori-donut-grafigi",
                defaults: new { controller = "AdminStatistics", action = "CategoryDonutGraph" }
            );

            routes.MapRoute(
                name: "AdminStatistics_CategoryBarGraph",
                url: "admin-profil/istatistik/kategori-bar-grafigi",
                defaults: new { controller = "AdminStatistics", action = "CategoryBarGraph" }
            );

            routes.MapRoute(
                name: "AdminHomepage_Index",
                url: "admin-profil",
                defaults: new { controller = "AdminHomepage", action = "Index" }
            );

            routes.MapRoute(
                name: "AdminAbouts_Index",
                url: "admin-profil/site-hakkinda",
                defaults: new { controller = "AdminAbouts", action = "Index" }
            );

            routes.MapRoute(
                name: "AdminHeadings_MyHeadings",
                url: "admin-profil/basliklarim",
                defaults: new { controller = "AdminHeadings", action = "MyHeadings" }
            );

            routes.MapRoute(
                name: "AdminHeadings_Index/{p}",
                url: "admin-profil/basliklar",
                defaults: new { controller = "AdminHeadings", action = "Index" , p=UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "AdminHeadings_EditHeading",
                url: "admin-profil/basligi-duzenle/{headingNameForFriendlyUrl}",
                defaults: new { controller = "AdminHeadings", action = "EditHeading" }
            );


            routes.MapRoute(
                name: "AdminAbouts_EditAbout",
                url: "admin-profil/hakkinda-duzenle/{aboutHeaderForFriendlyUrl}",
                defaults: new { controller = "AdminAbouts", action = "EditAbout" }
            );

            routes.MapRoute(
                name: "AdminAbouts_EnableAbout",
                url: "admin-profil/hakkinda-aktif-et/{aboutHeaderForFriendlyUrl}",
                defaults: new { controller = "AdminAbouts", action = "EnableAbout" }
            );

            routes.MapRoute(
                name: "AdminHeadings_EnableHeading",
                url: "admin-profil/basligi-onayla/{headingNameForFriendlyUrl}",
                defaults: new { controller = "AdminHeadings", action = "EnableHeading" }
            );

            routes.MapRoute(
                name: "AdminHeadings_DeleteHeading",
                url: "admin-profil/basligi-sil/{headingNameForFriendlyUrl}",
                defaults: new { controller = "AdminHeadings", action = "DeleteHeading" }
            );

            routes.MapRoute(
                name: "AdminHeadings_HeadingRaport",
                url: "admin-profil/istatistik/baslik-raporu",
                defaults: new { controller = "AdminHeadings", action = "HeadingRaport" }
            );

            routes.MapRoute(
                name: "AdminContacts_Index",
                url: "admin-profil/iletisim-ve-mesajlar",
                defaults: new { controller = "AdminContacts", action = "Index" }
            );

            routes.MapRoute(
                name: "AdminContacts_GetContactDetails",
                url: "admin-profil/iletisim-ve-mesajlar",
                defaults: new { controller = "AdminContacts", action = "Index" }
            );

            routes.MapRoute(
                name: "AdminWriters_Index",
                url: "admin-profil/yazarlar",
                defaults: new { controller = "AdminWriters", action = "Index" }
            );

            routes.MapRoute(
                name: "AdminWriters_EditWriter",
                url: "admin-profil/yazar-profili-duzenle/{username}",
                defaults: new { controller = "AdminWriters", action = "EditWriter" }
            );

            routes.MapRoute(
                name: "AdminCategories_HeadingsByCategory",
                url: "admin-profil/baslik/{categoryName}",
                defaults: new { controller = "AdminHeadings", action = "HeadingsByCategory" }
            );

            routes.MapRoute(
                name: "AdminCategories_DeleteCategory",
                url: "admin-profil/kategori-sil/{categoryName}",
                defaults: new { controller = "AdminCategories", action = "DeleteCategory" }
            );

            routes.MapRoute(
                name: "AdminCategories_UpdateCategory",
                url: "admin-profil/kategori-duzenle/{categoryName}",
                defaults: new { controller = "AdminCategories", action = "UpdateCategory" }
            );

            routes.MapRoute(
                name: "AdminCategories_AddCategory",
                url: "admin-profil/kategori-ekle",
                defaults: new { controller = "AdminCategories", action = "AddCategory" }
            );

            routes.MapRoute(
                name: "AdminAbouts_AddAbout",
                url: "admin-profil/hakkimizda-ekle",
                defaults: new { controller = "AdminAbouts", action = "AddAbout" }
            );

            routes.MapRoute(
                name: "AdminHeadings_AddHeading",
                url: "admin-profil/baslik-ekle",
                defaults: new { controller = "AdminHeadings", action = "AddHeading" }
            );

            routes.MapRoute(
                name: "Headings_HeadingByWriterUsername",
                url: "{username}/basliklar/{p}",
                defaults: new { controller = "Headings", action = "HeadingByWriterUsername" ,p=UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "AdminStatistics_Index",
                url: "admin-profil/istatistik/bazi-istatistikler",
                defaults: new { controller = "AdminStatistics", action = "Index" }
            );

            routes.MapRoute(
                name: "AdminAuthorization_Index",
                url: "admin-profil/yetkilendirmeler/{p}",
                defaults: new { controller = "AdminAuthorization", action = "Index" , p=UrlParameter.Optional}
            );

            routes.MapRoute(
                name: "AdminAuthorization_GiveAuthorization",
                url: "admin-profil/yetkilendirmeler/yetki-duzenle/{username}",
                defaults: new { controller = "AdminAuthorization", action = "EditAuthorization" }
            );

            routes.MapRoute(
                name: "AdminAuthorization_BanUser",
                url: "admin-profil/yetkilendirmeler/kullaniciyi-banla/{username}",
                defaults: new { controller = "AdminAuthorization", action = "BanUser" }
            );

            routes.MapRoute(
                name: "AdminAuthorization_UnbanUser",
                url: "admin-profil/yetkilendirmeler/kullanicinin-banini-kaldir/{username}",
                defaults: new { controller = "AdminAuthorization", action = "UnbanUser" }
            );


            routes.MapRoute(
                name: "AdminGalleries_Index",
                url: "admin-profil/site-galerisi",
                defaults: new { controller = "AdminGalleries", action = "Index" }
            );

            routes.MapRoute(
                name: "AdminCategories_Index/{p}",
                url: "admin-profil/kategoriler",
                defaults: new { controller = "AdminCategories", action = "Index" , p=UrlParameter.Optional}
            );

            routes.MapRoute(
                name: "AdminContents_MyContentByHeading",
                url: "admin-profil/yazilarim",
                defaults: new { controller = "AdminContents", action = "MyContentByHeading" }
            );

            routes.MapRoute(
                name: "Logins_Logout",
                url: "cikis-yap",
                defaults: new { controller = "Logins", action = "Logout" }
            );

            routes.MapRoute(
                name: "HeadingByHeadingNameForFriendlyUrl",
                url: "baslik/{headingNameForFriendlyUrl}/{p}",
                defaults: new { controller = "Headings", action = "HeadingByHeadingNameForFriendlyUrl", p = UrlParameter.Optional}
            );


            routes.MapRoute(
                name: "WriterProfiles_EditProfile",
                url: "yazar-profil/profilini-duzenle",
                defaults: new { controller = "WriterProfiles", action = "EditProfile" }
            );

            routes.MapRoute(
                name: "WriterHomepage_Index",
                url: "yazar-profil",
                defaults: new { controller = "WriterHomepage", action = "Index" }
            );


            routes.MapRoute(
                name: "WriterHeadings_Index",
                url: "yazar-profil/basliklarim",
                defaults: new { controller = "WriterHeadings", action = "Index" }
            );

            routes.MapRoute(
                name: "WriterContents_ContentByHeading",
                url: "yazar-profil/yazilarim/{p}",
                defaults: new { controller = "WriterContents", action = "ContentByHeading" , p=UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "WriterHeadings_AddHeading",
                url: "yazar-profil/baslik-ekle",
                defaults: new { controller = "WriterHeadings", action = "AddHeading" }
            );

            routes.MapRoute(
                name: "WriterHeadings_EditHeading",
                url: "yazar-profil/basligini-duzenle/{headingNameForFriendlyUrl}",
                defaults: new { controller = "WriterHeadings", action = "EditHeading" }
            );

            routes.MapRoute(
                name: "WriterHeadings_DeleteHeading",
                url: "yazar-profil/basligini-sil/{headingNameForFriendlyUrl}",
                defaults: new { controller = "WriterHeadings", action = "DeleteHeading" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Homepage", action = "Index", id = UrlParameter.Optional }
            );

        }
    }
}
