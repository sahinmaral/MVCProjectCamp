using System.Collections;
using System.ComponentModel.DataAnnotations;
using EntitiesLayer.Abstract;

namespace EntityLayer.Concrete
{
    public class Contact : IEntity, IEnumerable
    {
        [Key]
        public int ContactId { get; set; }
        [StringLength(50)]
        public string Username { get; set; }
        [StringLength(50)]
        public string UserEmail { get; set; }
        [StringLength(500)]
        public string Subject { get; set; }

        public string Message { get; set; }

        public IEnumerator GetEnumerator()
        {
            throw new System.NotImplementedException();
        }
    }
}
