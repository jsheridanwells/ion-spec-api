using System;
using System.Threading.Tasks;
using LandonApi.Models;
using LandonApi.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LandonApi.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class RoomsController : ControllerBase
    {
        private readonly ApiContext _ctx;

        public RoomsController(ApiContext ctx)
        {
            _ctx = ctx;
        }
        [HttpGet(Name = nameof(GetRooms))]
        public IActionResult GetRooms()
        {
            throw new NotImplementedException();
        }

        [HttpGet("{id}", Name = nameof(GetRoom))]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Room>> GetRoom(Guid id)
        {
            var entity = await _ctx.Rooms.SingleOrDefaultAsync(
                r => r.Id == id
            );

            if (entity == null)
                return NotFound();
            
            var resource = new Room
            {
                Href = Url.Link(nameof(GetRoom), new { roomId = entity.Id }),
                Name = entity.Name,
                Rate = entity.Rate / 100.0m
            };         
            return resource;
        }
    }
}
