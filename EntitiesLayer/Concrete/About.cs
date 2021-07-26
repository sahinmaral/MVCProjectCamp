using EntityLayer.Abstract;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace EntityLayer.Concrete
{
    public class About: IEntity
    {
        [Key]
        public int AboutId { get; set; }


        [StringLength(30)]
        public string AboutHeader { get; set; }

        [StringLength(30)]
        public string AboutHeaderForFriendlyUrl { get; set; }

        [StringLength(1000)]
        [AllowHtml]
        public string AboutText { get; set; }


        [DefaultValue("false")]
        public bool AboutStatus { get; set; }

    }
}
