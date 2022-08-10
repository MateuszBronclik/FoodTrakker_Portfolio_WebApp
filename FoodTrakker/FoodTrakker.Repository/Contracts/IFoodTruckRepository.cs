﻿using FoodTrakker.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodTrakker.Repository.Contracts
{
    public interface IFoodTruckRepository : IRepository<FoodTruck, int>
    {
        public Task<List<FoodTruck>> GetFullFoodTruckInfoAsync();
        public Task<FoodTruck> GetFullFoodTruckInfoAsync(int Id);
        public Task<List<FoodTruck>> GetOwnerFoodTrucks(string ownerId);
        Task SaveChanges();
    }
}
