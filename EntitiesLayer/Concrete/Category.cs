﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EntityLayer.Abstract;

namespace EntityLayer.Concrete
{
    public class Category : IEntity
    {
        [Key]
        public int CategoryId { get; set; }
        [StringLength(50)]
        public string CategoryName { get; set; }
        [StringLength(200)]
        public string CategoryDescription { get; set; }
        public bool CategoryStatus { get; set; }
        public ICollection<Heading> Headings { get; set; }
    }
}
