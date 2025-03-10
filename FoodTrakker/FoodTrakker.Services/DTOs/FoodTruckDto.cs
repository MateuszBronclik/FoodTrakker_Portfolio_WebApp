﻿using FoodTrakker.Core.LinkingClasses;
using FoodTrakker.Core.Model;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodTrakker.Services.DTOs
{
    public class FoodTruckDto
    {
        public int Id { get; set; }
        public ICollection<ReviewDto> Reviews { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string OwnerId { get; set; }
        public int LocationId { get; set; }
        public Location? Location { get; set; }
        public int TypeId { get; set; }
        public FoodTruckType? Type { get; set; }
        public ICollection<FoodTruckEvent>? FoodTruckEvents { get; set; }
        public IFormFile ImageFile { get; set; }

        public string ImageName { get; set; }

        public double AvgRating { get; set; }
        public bool HasCurrentUserReview   { get; set; }
        public bool IsAddedToFav { get; set; }
        public int ReviewsTotalCount    { get; set; }
    }

}
