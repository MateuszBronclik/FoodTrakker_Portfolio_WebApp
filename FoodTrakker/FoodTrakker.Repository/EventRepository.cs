﻿
using FoodTrakker.Core;
using FoodTrakker.Repository.Data;

namespace FoodTrakker.Repository
{
    public class EventRepository : Repository<Event>
    {
        public EventRepository(FoodTrakkerContext context) : base(context)
        {
        }
    }
}
