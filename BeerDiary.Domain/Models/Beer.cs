using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeerDiary.Domain.Models
{
    public partial class Beer
    {
        public int Id  {get;set;}
        [Required]
        public string BeerName {get;set;}
        public string BeerBrewer {get;set;}
        public int BeerReviewCount {get;set;}
        [Column(TypeName = "decimal(5,2)")]
        public double BeerSize {get;set;}
        public string BeerType {get;set;}
        [Column(TypeName = "decimal(5,2)")]
        public double BeerStrength {get;set;}
        public double BeerAvgScore {get;set;}
        public ICollection<Review> BeerReviews {get;set;}
    }
}