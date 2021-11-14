using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace BeerDiary.Domain.Models
{
    public partial class Review
    {
        public int Id  {get;set;}
        [Required]
        public int BeerId { get; set; }
        [Required]
        public float ReviewScore {get;set;}
        public string ReviewComment {get;set;}
        public double ReviewLat {get;set;}
        public double ReviewLong { get; set; }
        [Required]
        public int UserId {get;set;}
        public DateTime ReviewDone {get;set;}
    }
}
