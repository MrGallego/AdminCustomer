using AdminCustomerAPI.Models;
using AdminCustomerAPI.Models.Dto;
using AdminCustomerAPI.Models.Models;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace AdminCustomerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : Controller
    {
        private readonly ILogger<CustomerController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CustomerController(ILogger<CustomerController> logger, ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<CustomerDto>>> GetCustomer()
        {
            _logger.LogInformation("obtener clientes");

            IEnumerable<Customer> list = await _context.Customers.ToListAsync();

            return Ok(_mapper.Map<IEnumerable<CustomerDto>>(list));
        }

        [HttpGet("iden:int")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CustomerDto>> GetCustomer(int iden)
        {
            if (iden == 0)
            {
                _logger.LogError("error al traer el cliente con el numero de identificacion:  " + iden);
                return BadRequest();
            }
            var customer = await _context.Customers.FirstOrDefaultAsync(x => x.NumeroIdentificacion == iden);
            if (customer == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<CustomerDto>(customer));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CustomerDto>> CreateCustomer([FromBody] CustomerCreateDto customerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (await _context.Customers.FirstOrDefaultAsync(c => c.NumeroIdentificacion == customerDto.NumeroIdentificacion) != null)
            {
                ModelState.AddModelError("IdentificacionExiste", "Verifique el documento, ya que se encuentra registrado");
                return BadRequest(ModelState);
            }
            if (customerDto == null)
            {
                return BadRequest(customerDto);
            }

            Customer customer = _mapper.Map<Customer>(customerDto);
           
            await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();

            return Ok(customerDto);
        }

        [HttpDelete("iden:int")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCustomer(int iden)
        {
            if(iden == 0)
            {
                return BadRequest();
            }
             var customer = await _context.Customers.FirstOrDefaultAsync(x => x.NumeroIdentificacion == iden);
            if(customer == null)
            {
                return NotFound();
            }
            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            return NoContent();
        }



        [HttpPut("iden:int")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(int iden, [FromBody] CustomerUpdateDto customerDto)
        {
            if (customerDto == null || iden != customerDto.NumeroIdentificacion)
            {
                return BadRequest();
            }
            Customer customer = _mapper.Map<Customer>(customerDto); 

            _context.Customers.Update(customer);
            await _context.SaveChangesAsync();
            return NoContent(); 
        }

        [HttpPatch("{iden:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> UpdateSpecificCustomer(int iden, JsonPatchDocument<CustomerUpdateDto> jsonPatch)
        {
            if (jsonPatch == null || iden == 0)
            {
                return BadRequest();
            }
            var data = await _context.Customers.FirstOrDefaultAsync(c => c.NumeroIdentificacion == iden);
            CustomerUpdateDto customerUpdateDto = _mapper.Map<CustomerUpdateDto>(jsonPatch);
   

            if (data == null) return BadRequest();
            jsonPatch.ApplyTo(customerUpdateDto, (Microsoft.AspNetCore.JsonPatch.Adapters.IObjectAdapter)ModelState);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Customer customer = _mapper.Map<Customer>(jsonPatch);
            
            _context.Customers.Update(customer);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
