﻿namespace FoodTrakker.Api.Models
{
    public class EventApiPost
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? OwnerId { get; set; }
        public List<int>? FoodTruckIds { get; set; }
    }
}