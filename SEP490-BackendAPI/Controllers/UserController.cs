using AutoMapper;
using ClassLibrary1.DTO.User;
using ClassLibrary1.Interface;
using ClassLibrary1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SEP490_BackendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _unitOfWork.Users.GetAllAsync(includeProperties: u => u.Role);
            if (users == null) 
            {
                return NotFound(new { Message = "No users found." });
            }
            var userDtos = _mapper.Map<IEnumerable<UserResponseDTO>>(users);
            return Ok(userDtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound(new { Message = $"User with ID {id} was not found." });

            }
            else 
            {
                var userDto = _mapper.Map<UserResponseDTO>(user);
                return Ok(userDto);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserRequestDTO updateUserRequestDTO )
        {
            if (!ModelState.IsValid) 
            { 
                return BadRequest(new { Message = "The provided data is invalid." });
            }

            var user = await _unitOfWork.Users.GetByIdAsync(id);
            if(user == null)
            {
                return NotFound(new { MessMessage = $"User with ID {id} was not found." });
            }
            _mapper.Map(updateUserRequestDTO, user);
            _unitOfWork.Users.Update(user);
            return Ok(new { Message = "User updated successfully." });

        }



    }
}
