using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BeerDiary.Domain.Models
{
    public partial class User
    {
        public int Id { get; set; }
        [Required]
        public string UserName { get; set; }
        public int UserReviewCount {get;set;}
        public ICollection<Review> Reviews { get; set; }
}
}