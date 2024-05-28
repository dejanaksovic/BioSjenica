﻿using System.ComponentModel.DataAnnotations;

namespace bioSjenica.Models
{
    public class Animal
    {
        public int Id { get; set; }
        [Required]
        public string RingNumber { get; set; }
        [Required]
        public string LatinicName { get; set; }
        [Required]
        public string CommonName { get; set; }
        [Required]
        public string ImageUrl { get; set; }
        public List<Region>? Regions { get; set; }
        public List<FeedingGround>? FeedingGrounds { get; set; }
    }
}
