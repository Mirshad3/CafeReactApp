using CafeReactApp.Server.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CafeReactApp.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CafesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CafesController(ApplicationDbContext context)

        {

            _context = context;

        }


        [HttpGet(Name = "GetCafes")]

        public async Task<IActionResult> GetCafes([FromQuery(Name = "location")] string location = null)

        {

            if (!string.IsNullOrEmpty(location))

            {

                var cafes = await _context.Cafes

                    .Where(c => c.Location == location)

                    .Include(c => c.Employees)

                    .Select(c => new

                    {

                        c.CafeName,

                        c.Description,

                        Employees = c.Employees.Count,

                        c.Logo,

                        c.Location,

                        c.CafeId

                    })

                    .OrderByDescending(c => c.Employees)

                    .ToListAsync();



                return Ok(cafes);

            }

            else

            {

                var cafes = await _context.Cafes

                    .Include(c => c.Employees)

                    .Select(c => new

                    {

                        c.CafeName,

                        c.Description,

                        Employees = c.Employees.Count,

                        c.Logo,

                        c.Location,

                        c.CafeId

                    })

                    .OrderByDescending(c => c.Employees)

                    .ToListAsync();



                return Ok(cafes);

            }

        }



        [HttpPost]

        public async Task<IActionResult> AddCafe([FromBody] Cafe cafe)

        {

            cafe.CafeId = GenerateUniqueId();

            _context.Cafes.Add(cafe);

            await _context.SaveChangesAsync();

            return Ok(new { success = true });

        }



        [HttpPut]

        public async Task<IActionResult> EditCafe([FromBody] Cafe cafe)

        {

            _context.Cafes.Update(cafe);

            await _context.SaveChangesAsync();

            return Ok(new { success = true });

        }



        [HttpDelete("{id}")]

        public async Task<IActionResult> DeleteCafe(string id)

        {

            var cafe = await _context.Cafes.Include(c => c.Employees).FirstOrDefaultAsync(c => c.CafeId == id);

            if (cafe == null)

            {

                return NotFound();

            }



            _context.Employees.RemoveRange(cafe.Employees);

            _context.Cafes.Remove(cafe);

            await _context.SaveChangesAsync();

            return NoContent();

        }
        private string GenerateUniqueId()

        {

            var random = new Random();

            var randomNumber = random.Next(1000000, 9999999);

            return "UI" + randomNumber.ToString();

        }
    }
}
