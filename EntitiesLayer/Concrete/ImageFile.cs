using System.ComponentModel.DataAnnotations;
using EntityLayer.Abstract;

namespace EntityLayer.Concrete
{
    public class ImageFile:IEntity
    {
        [Key]
        public int ImageId { get; set; }
        [StringLength(100)]
        public string ImageName { get; set; }
        [StringLength(250)]
        public string ImagePath { get; set; }
    }
}
