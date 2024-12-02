using CafeReactApp.Server.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CafeReactApp.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EmployeesController(ApplicationDbContext context)

        {

            _context = context;

        }



        [HttpGet(Name = "GetEmployees")]

        public async Task<IActionResult> GetEmployees([FromQuery(Name = "cafe")] string cafe = null)

        {

            if (!string.IsNullOrEmpty(cafe))

            {

                var employees = await _context.Employees

                    .Where(e => e.Cafe.CafeName == cafe)

                    .Include(e => e.Cafe)

                    .Select(e => new

                    {

                        e.EmployeeId,

                        e.Name,

                        e.Email,

                        e.Phone,

                        e.JoinDate,

                       // DaysWorked = (int)(DateTime.Now.Date - e.JoinDate.Date).TotalDays,

                        e.Cafe.CafeName

                    })

                   // .OrderByDescending(e => e.DaysWorked)

                    .ToListAsync();



                return Ok(employees);

            }

            else

            {

                var employees = await _context.Employees

                    .Include(e => e.Cafe)

                    .Select(e => new

                    {

                        e.EmployeeId,

                        e.Name,

                        e.Email,

                        e.Phone,

                        e.JoinDate,

                        //DaysWorked = (int)(DateTime.Now.Date - e.JoinDate.Date).TotalDays,

                        e.Cafe.CafeName

                    })

                   // .OrderByDescending(e => e.DaysWorked)

                    .ToListAsync();



                return Ok(employees);

            }

        }



        [HttpPost]

        public async Task<IActionResult> AddEmployee([FromBody] Employee employee)

        {

            var existingEmployee = await _context.Employees

                .FirstOrDefaultAsync(e => e.Name == employee.Name || e.Email == employee.Email);



            if (existingEmployee != null)

            {

                if (existingEmployee.Name == employee.Name)

                {

                    return Conflict("Employee name already exists!");

                }

                else

                {

                    return BadRequest("Employee email already exists!");

                }

            }



            employee.EmployeeId = GenerateUniqueId();

            _context.Employees.Add(employee);

            await _context.SaveChangesAsync();

            return Ok("Employee added!");

        }



        [HttpPut]

        public async Task<IActionResult> EditEmployee([FromBody] Employee employee)

        {

            _context.Employees.Update(employee);

            await _context.SaveChangesAsync();

            return Ok(new { success = true });

        }



        [HttpDelete("{id}")]

        public async Task<IActionResult> DeleteEmployee(string id)

        {

            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)

            {

                return NotFound();

            }



            _context.Employees.Remove(employee);

            await _context.SaveChangesAsync();

            return Ok(new { message = $"Employee with ID {id} has been deleted." });

        }



        private string GenerateUniqueId()

        {

            var random = new Random();

            var randomNumber = random.Next(1000000, 9999999);

            return "UI" + randomNumber.ToString();

        }
    }
}
