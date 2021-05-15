using System.ComponentModel.DataAnnotations;
using EntitiesLayer.Abstract;

namespace EntityLayer.Concrete
{
    public class About:IEntity
    {
        [Key]
        public int AboutId { get; set; }
        [StringLength(1000)]
        public string AboutDetails1 { get; set; }
        [StringLength(1000)]
        public string AboutDetails2 { get; set; }
        [StringLength(100)]
        public string AboutImage { get; set; }
        public string AboutImage2 { get; set; }

    }
}
