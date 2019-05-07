using LandonApi.Models;
using Microsoft.EntityFrameworkCore;

namespace LandonApi.Repository
{
    public class ApiContext : DbContext
    {
        public ApiContext(DbContextOptions opts)
            : base(opts) {}
        
        public DbSet<RoomEntity> Rooms { get; set; }
    }
}
