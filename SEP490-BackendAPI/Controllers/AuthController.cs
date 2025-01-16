using ClassLibrary1.DTO.Login;
using ClassLibrary1.DTO.Password;
using ClassLibrary1.DTO.Register;
using ClassLibrary1.Interface;
using ClassLibrary1.Models;
using ClassLibrary1.Security;
using ClassLibrary1.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _config;
    private readonly IJWTService _jwtService;
    private readonly IEmailService _emailService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IUnitOfWork unitOfWork, IConfiguration config, IJWTService jwtService, ILogger<AuthController> logger, IEmailService emailService)
    {
        _unitOfWork = unitOfWork;
        _config = config;
        _jwtService = jwtService;
        _logger = logger;
        _emailService = emailService;
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

            return Ok(new { Message = "Registration successful" });
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

    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequestDTO changePasswordDTO)
    {
        if (changePasswordDTO == null)
        {
            return BadRequest("Invalid request data");
        }

        try
        {
            // Get the current user from the token
            var userId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("Invalid user token");
            }

            var user = await _unitOfWork.Users.GetByIdAsync(int.Parse(userId));
            if (user == null)
            {
                return NotFound("User not found");
            }

            // Verify the current password
            if (!PasswordHasher.VerifyPassword(changePasswordDTO.CurrentPassword, user.PasswordHash))
            {
                return BadRequest("Current password is incorrect");
            }

            // Hash the new password
            var newHashedPassword = PasswordHasher.HashPassword(changePasswordDTO.NewPassword);

            // Update the password
            user.PasswordHash = newHashedPassword;
            _unitOfWork.Users.Update(user);
            await _unitOfWork.SaveAsync();

            return Ok(new { Message = "Password changed successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error during password change: {ex.Message}");
            return BadRequest(new { Message = ex.Message });
        }
    }

    [HttpPost("reset-password-request")]
    public async Task<IActionResult> ResetPasswordRequest([FromBody] ResetPasswordRequestDTO resetPasswordRequestDTO)
    {
        if (resetPasswordRequestDTO == null || string.IsNullOrEmpty(resetPasswordRequestDTO.Email))
        {
            return BadRequest("Email is required");
        }

        try
        {
            var user = await _unitOfWork.Users.GetByEmailAsync(resetPasswordRequestDTO.Email);
            if (user == null)
            {
                return BadRequest("User not found");
            }

            // Generate a new random password (e.g., 8 characters long)
            var newPassword = GenerateRandomPassword(8);

            // Hash the new password
            var newHashedPassword = PasswordHasher.HashPassword(newPassword);

            // Update user's password
            user.PasswordHash = newHashedPassword;
            _unitOfWork.Users.Update(user);
            await _unitOfWork.SaveAsync();

            // Send new password via email
            await _emailService.SendNewPasswordEmail(user.FullName, user.Email, newPassword);

            return Ok(new { Message = "A new password has been sent to your email" });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error during password reset request: {ex.Message}");
            return BadRequest(new { Message = ex.Message });
        }
    }

    private string GenerateRandomPassword(int length)
    {
        const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
        var random = new Random();
        var password = new char[length];

        for (int i = 0; i < length; i++)
        {
            password[i] = validChars[random.Next(validChars.Length)];
        }

        return new string(password);
    }

}
