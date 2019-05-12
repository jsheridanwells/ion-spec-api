using System;
using System.Linq;
using System.Threading.Tasks;
using LandonApi.Models;
using LandonApi.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace LandonApi
{
    public static class SeedData
    {
        public static async Task IntializeAsync(IServiceProvider services)
        {
            await AddTestData(
                services.GetRequiredService<ApiContext>()
            );
        }
        public static async Task AddTestData(ApiContext ctx)
        {
            if (ctx.Rooms.Any())
            {
                return; // we already have data
            }

            ctx.Rooms.Add(new RoomEntity
            {
                Id = Guid.NewGuid(),
                Name = "Oxford",
                Rate = 10199,
            });    
            ctx.Rooms.Add(new RoomEntity
            {
                Id = Guid.NewGuid(),
                Name = "Milan",
                Rate = 5000,
            });  
            ctx.Rooms.Add(new RoomEntity
            {
                Id = Guid.NewGuid(),
                Name = "Derbyshire",
                Rate = 30000,
            });    
            await ctx.SaveChangesAsync();
        }
    }
}
