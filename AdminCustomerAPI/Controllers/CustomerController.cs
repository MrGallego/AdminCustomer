using AdminCustomerAPI.Dto;
using AdminCustomerAPI.Models;
using AdminCustomerAPI.Repository;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AdminCustomerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : Controller
    {
        private readonly ILogger<CustomerController> _logger;
        private readonly ApplicationDbContext _context;

        public CustomerController(ILogger<CustomerController> logger, ApplicationDbContext context)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<CustomerDto>> GetCustomer()
        {
            _logger.LogInformation("obtener clientes");
            return Ok(_context.Customers.ToList());
        }

        [HttpGet("iden:int")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<CustomerDto> GetCustomer(int iden)
        {
            if (iden == 0)
            {
                _logger.LogError("error al traer el cliente con el id  " + iden);
                return BadRequest();
            }
            var customer = _context.Customers.FirstOrDefault(x => x.NumeroIdentificacion == iden);
            if (customer == null)
            {
                return NotFound();
            }
            return Ok(customer);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<CustomerDto> CreateCustomer([FromBody] CustomerDto customerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (_context.Customers.FirstOrDefault(c => c.NumeroIdentificacion == customerDto.NumeroIdentificacion) != null)
            {
                ModelState.AddModelError("IdentificacionExiste", "Verifique el documento, se encuentra ya registrado");
                return BadRequest(ModelState);
            }
            if (customerDto == null)
            {
                return BadRequest(customerDto);
            }
            Customer customer = new()
            {
                TipoIdentificacion = customerDto.TipoIdentificacion,
                NumeroIdentificacion = customerDto.NumeroIdentificacion,
                Nombres = customerDto.Nombres,
                Apellidos = customerDto.Apellidos,
                Correo = customerDto.Correo,
                FechaNacimiento = customerDto.FechaNacimiento,
                FechaCreacion = DateTime.Now,
                FechaModificacion = DateTime.Now
            };
            _context.Customers.Add(customer);
            _context.SaveChanges();
            return Ok(customerDto);
        }

        [HttpDelete("iden:int")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteCustomer(int iden)
        {
            if(iden == 0)
            {
                return BadRequest();
            }
             var customer = _context.Customers.FirstOrDefault(x => x.NumeroIdentificacion == iden);
            if(customer == null)
            {
                return NotFound();
            }
            _context.Customers.Remove(customer);
            _context.SaveChanges();
            return NoContent();
        }



        [HttpPut("iden:int")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Update(int iden, [FromBody] CustomerDto customerDto)
        {
            if (customerDto == null || iden != customerDto.NumeroIdentificacion)
            {
                return BadRequest();
            }
            Customer customer = new()
            {
                TipoIdentificacion = customerDto.TipoIdentificacion,
                NumeroIdentificacion = customerDto.NumeroIdentificacion,
                Nombres = customerDto.Nombres,
                Apellidos = customerDto.Apellidos,
                Correo = customerDto.Correo,
                FechaNacimiento = customerDto.FechaNacimiento
            };
            _context.Customers.Update(customer);
            _context.SaveChanges();
            return NoContent(); 
        }

        [HttpPatch("{iden:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public IActionResult UpdatePartialCustomer(int iden, JsonPatchDocument<CustomerDto> jsonPatch)
        {
            if (jsonPatch == null || iden == 0)
            {
                return BadRequest();
            }
            var data = _context.Customers.FirstOrDefault(c => c.NumeroIdentificacion == iden);
            CustomerDto customerDto = new()
            {
                TipoIdentificacion = data.TipoIdentificacion,
                NumeroIdentificacion = data.NumeroIdentificacion,
                Nombres = data.Nombres,
                Apellidos = data.Apellidos,
                Correo = data.Correo,
                FechaNacimiento = data.FechaNacimiento
            };

            if (data == null) return BadRequest();
            jsonPatch.ApplyTo(customerDto, (Microsoft.AspNetCore.JsonPatch.Adapters.IObjectAdapter)ModelState);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Customer customer = new()
            {
                TipoIdentificacion = customerDto.TipoIdentificacion,
                NumeroIdentificacion = customerDto.NumeroIdentificacion,
                Nombres = customerDto.Nombres,
                Apellidos = customerDto.Apellidos,
                Correo = customerDto.Correo,
                FechaNacimiento = customerDto.FechaNacimiento
            };
            _context.Customers.Update(customer);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
