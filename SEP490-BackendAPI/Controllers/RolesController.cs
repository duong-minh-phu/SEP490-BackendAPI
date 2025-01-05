using AutoMapper;
using ClassLibrary1.DTO.Role;
using ClassLibrary1.Interface;
using ClassLibrary1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SEP490_BackendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RolesController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

       
        [HttpGet]
        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await _unitOfWork.Roles.GetAllAsync();
            var roleDtos = _mapper.Map<IEnumerable<RoleDTO>>(roles);
            return Ok(roleDtos);
        }

        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoleById(int id)
        {
            var role = await _unitOfWork.Roles.GetAllAsync(
                pi => pi.RoleId == id
            );

            return Ok(role);
        }

        
        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleDTO createRoleDto)
        {
            if (createRoleDto == null)
                return BadRequest();

            var role = _mapper.Map<Role>(createRoleDto);
            await _unitOfWork.Roles.AddAsync(role);
            await _unitOfWork.SaveAsync();

            return CreatedAtAction(nameof(GetRoleById), new { id = role.RoleId }, role);
        }

        
        

        
        
    }
}
