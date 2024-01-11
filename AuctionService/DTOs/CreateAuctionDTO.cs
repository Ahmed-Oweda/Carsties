﻿using System.ComponentModel.DataAnnotations;

namespace AuctionService.DTOs
{
    public class CreateAuctionDTO
    {
        [Required]
        public string Make { get; set; }
        [Required]
        public string Model { get; set; }
        [Required]
        public int Year { get; set; }
        [Required]
        public string Color { get; set; }
        [Required]
        public int Mileage { get; set; }
        [Required]
        public string ImageURL { get; set; }
        [Required]
        public int ReservedPrice { get; set; }
        [Required]
        public DateTime AuctionEnd { get; set; }
    }
}
