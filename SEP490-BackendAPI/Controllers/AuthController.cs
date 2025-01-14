using ClassLibrary1.DTO.Login;
using ClassLibrary1.DTO.Register;
using ClassLibrary1.Interface;
using ClassLibrary1.Models;
using ClassLibrary1.Security;
using ClassLibrary1.Services;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _config;
    private readonly IJWTService _jwtService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IUnitOfWork unitOfWork, IConfiguration config, IJWTService jwtService, ILogger<AuthController> logger)
    {
        _unitOfWork = unitOfWork;
        _config = config;
        _jwtService = jwtService;
        _logger = logger;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDTO registerUserDTO)
    {
        if (registerUserDTO == null)
        {
            return BadRequest("Invalid request data");
        }

        try
        {
            // Check if required fields are provided
            if (string.IsNullOrEmpty(registerUserDTO.FullName) || string.IsNullOrEmpty(registerUserDTO.Email) || string.IsNullOrEmpty(registerUserDTO.Password))
            {
                return BadRequest("Full name, email, and password are required.");
            }

            // Check if the email already exists
            var existingUser = await _unitOfWork.Users.GetByEmailAsync(registerUserDTO.Email);
            if (existingUser != null)
            {
                return BadRequest("Email already exists");
            }

            // Validate RoleId
            var role = await _unitOfWork.Roles.GetByIdAsync(registerUserDTO.RoleId);
            if (role == null)
            {
                return BadRequest("Invalid role ID.");
            }

            // Hash the password
            var passwordHash = PasswordHasher.HashPassword(registerUserDTO.Password);

            // Create new User
            var newUser = new User
            {
                FullName = registerUserDTO.FullName,
                Email = registerUserDTO.Email,
                PasswordHash = passwordHash,
                Phone = registerUserDTO.Phone,
                Address = registerUserDTO.Address,
                RoleId = registerUserDTO.RoleId,
                IsActive = true,
                CreatedDate = DateTime.Now,
                IsStudent = registerUserDTO.IsStudent,
            };

            // Save the new user
            await _unitOfWork.Users.AddAsync(newUser);
            await _unitOfWork.SaveAsync();

            // If the user is a student, create StudentInfo entry
            if (registerUserDTO.IsStudent == true)
            {
                var studentInfo = new StudentInfo
                {
                    UserId = newUser.UserId,
                    StudentCardImage = registerUserDTO.StudentCardImage,
                    StudentCode = registerUserDTO.StudentCode,
                    University = registerUserDTO.University,
                    CreatedDate = DateTime.Now,
                };

                await _unitOfWork.StudentInfos.AddAsync(studentInfo);
                await _unitOfWork.SaveAsync();
            }

            // Generate JWT token
            var token = _jwtService.GenerateJWT(newUser);

            return Ok(new { Message = "Registration successful"});
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error during registration: {ex.Message}");
            return BadRequest(new { Message = ex.Message });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginUserDTO)
    {
        if (loginUserDTO == null)
        {
            return BadRequest("Invalid request data");
        }

        try
        {
            var user = await _unitOfWork.Users.GetByEmailAsync(loginUserDTO.Email);
            if (user == null || !PasswordHasher.VerifyPassword(loginUserDTO.Password, user.PasswordHash))
            {
                return Unauthorized("Invalid email or password");
            }

            var token = _jwtService.GenerateJWT(user);
            var response = new LoginResponseDTO
            {
                Token = token,
            };

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error during login: {ex.Message}");
            return BadRequest(new { Message = ex.Message });
        }
    }
}
