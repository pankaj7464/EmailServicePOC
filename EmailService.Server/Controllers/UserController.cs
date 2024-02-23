
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using EmailService.Server.Models;
using EmailService.Server.Services;
using System.Security.Cryptography;
using EmailService.Server.DatabaseConfig;
using Microsoft.EntityFrameworkCore;
using EmailService.Server.Uttils;
using EmailService.Server.Utils;
namespace EmailService.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly string _domain;
        private readonly IEmailServices _emailServices;
        private readonly DBContext _context;
        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger, IConfiguration configuration, IOptions<SmtpSettings> smtpSettings, IEmailServices emailService, DBContext context)
        {
            _domain = "https://127.0.0.1:4200";
            _emailServices = emailService;
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// This code handles user registration, checking for existing users, 
        /// creating password hashes, and sending verification emails. 
        /// </summary>
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterRequestDto request)
        {

            try
            {
                // Check if user already exists
                if (_context.Users.Any(u => u.Email == request.Email))
                {
                    _logger.LogInformation("User registration failed. User already exists.");
                    return BadRequest("User already exists.");
                }

                // Create password hash and salt
                CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

                // Create new user object
                var user = new User
                {
                    Email = request.Email,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                    VerificationToken = CreateRandomToken()
                };

                // Add user to database
                _context.Users.Add(user);
              
                // Send verification email
                var emailDto = new EmailDto
                {
                    To = user.Email,
                    Subject = "Please click link to verify your account",
                    Body = EmailTemplate.GenerateRegistrationConfirmationEmail(_domain, user.VerificationToken)
            };
                _emailServices.SendEmail(emailDto);

                // Save changes to database
                await _context.SaveChangesAsync();

                _logger.LogInformation("User registration successful.");
                return Ok("User successfully created!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during user registration.");
                return StatusCode(500, "An error occurred during user registration.");
            }
        }

        /// <summary>
        ///  This endpoint verifies user accounts using a provided token, 
        /// updating their verification status in the database.
        /// It sends a success email upon verification and handles errors appropriately.
        /// </summary>
        [HttpPost("verify")]
        public async Task<IActionResult> Verify(string token)
        {
            try
            {
                // Find user by verification token
                var user = await _context.Users.FirstOrDefaultAsync(u => u.VerificationToken == token);
                if (user == null)
                {
                    _logger.LogInformation("Invalid verification token.");
                    return BadRequest("Invalid token.");
                }

                // Set user as verified
                user.VerifiedAt = DateTime.Now;
                await _context.SaveChangesAsync();

                // Send verification success email
                var emailDto = new EmailDto
                {
                    To = user.Email,
                    Subject = "Account Verification Successful",
                    Body = EmailTemplate.GenerateVerifiedAccountEmail(user.Email)
                };
                _emailServices.SendEmail(emailDto);

                _logger.LogInformation("User account verified.");
                return Ok("User verified! :)");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during account verification.");
                return StatusCode(500, "An error occurred during account verification.");
            }
        }

        /// <summary>
        /// This endpoint verifies user accounts using a token, updating their status and sending a success email.
        /// It handles errors gracefully, logging them and returning appropriate responses.
        /// </summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginRequestDto request)
        {
            try
            {
                // Find user by email
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
                if (user == null)
                {
                    _logger.LogInformation("User not found.");
                    return BadRequest("User not found.");
                }

                // Verify password
                if (!VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
                {
                    _logger.LogInformation("Incorrect password.");
                    return BadRequest("Password is incorrect.");
                }

                // Check if user is verified
                if (user.VerifiedAt == null)
                {
                    _logger.LogInformation("User not verified.");
                    return BadRequest("Not verified!");
                }

                // Send login success email
                var emailDto = new EmailDto
                {
                    To = user.Email,
                    Subject = "Successful Login Notification",
                    Body = EmailTemplate.LoginEmailTemplate(user.Email)
                };
                _emailServices.SendEmail(emailDto);

                _logger.LogInformation("User logged in successfully.");
                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during user login.");
                return StatusCode(500, "An error occurred during user login.");
            }
        }

        /// <summary>
        /// This endpoint handles password reset requests, generating a reset token and sending an email with instructions.
        ///It logs successful email sending and gracefully handles errors.
        /// </summary>

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            try
            {
                // Find user by email
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
                if (user == null)
                {
                    _logger.LogInformation("User not found for password reset request.");
                    return BadRequest("User not found.");
                }

                // Generate password reset token and expiration
                user.PasswordResetToken = CreateRandomToken();
                user.ResetTokenExpires = DateTime.Now.AddDays(1);
                await _context.SaveChangesAsync();


                var emailDto = new EmailDto
                {
                    To = user.Email,
                    Subject = "Password Reset Request",
                    Body = EmailTemplate.ForgotPasswordTemplate(user.Email, _domain, user.PasswordResetToken)
                };

                // Send password reset email
                _emailServices.SendEmail(emailDto);

                _logger.LogInformation("Password reset email sent successfully.");
                return Ok("You may now reset your password.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during password reset request.");
                return StatusCode(500, "An error occurred during password reset request.");
            }
        }

        /// <summary>
        ///This endpoint handles password resets, validating the reset token and updating the password if valid.
        ///It sends a confirmation email upon successful reset and handles errors gracefully,
        ///logging them appropriately.
        /// </summary>       
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequestDto request)
        {
            try
            {
                // Find user by password reset token
                var user = await _context.Users.FirstOrDefaultAsync(u => u.PasswordResetToken == request.Token);
                if (user == null || user.ResetTokenExpires < DateTime.Now)
                {
                    _logger.LogInformation("Invalid password reset token.");
                    return BadRequest("Invalid Token.");
                }

                // Generate new password hash and salt
                CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

                // Update user's password
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
                user.PasswordResetToken = null;
                user.ResetTokenExpires = null;

                // Save changes to database
                await _context.SaveChangesAsync();


                var emailDto = new EmailDto
                {
                    To = user.Email,
                    Subject = "Password Reset Confirmation",
                    Body = EmailTemplate.GeneratePasswordResetConfirmationEmail(user.Email)
            };

                // Send password reset confirmation email
                _emailServices.SendEmail(emailDto);

                _logger.LogInformation("Password reset successfully.");
                return Ok("Password successfully reset.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during password reset.");
                return StatusCode(500, "An error occurred during password reset.");
            }
        }


        /// <summary>
        /// Retrieves all users from the database.
        /// </summary>
        ///<returns>
        ///An IActionResult containing a list of users with email createdAt and id if successful, or a 
        ///500 status code with an error message if an exception occurs.
        /// </returns>
        [HttpGet]
        public IActionResult GetAllUsers()
        {
            try
            {
                var users = _context.Users
                    .Select(u => new {
                        u.Email,
                        u.VerifiedAt,
                        u.Id
                    })
                    .ToList();

                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving users.");
                return StatusCode(500, "An error occurred while retrieving users.");
            }
        }


        /// <summary>
        /// Deletes a user by their ID.
        /// </summary>
        /// 
        /// <param name="id">The ID of the user to delete.</param>

        /// <returns>
        ///   Returns a 404 status code if the user with the specified ID is not found,
        ///   or a 200 status code with a success message if the user is successfully deleted.
        ///   Returns a 500 status code with an error message if an exception occurs.
        /// </returns>
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            try
            {
                var user = _context.Users.Find(id);
                if (user == null)
                {
                    return NotFound();
                }

                _context.Users.Remove(user);
                _context.SaveChanges();

                return Ok("User deleted successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting user.");
                return StatusCode(500, "An error occurred while deleting user.");
            }
        }


        /// <summary>
        /// This helper method creates a password hash and salt using HMACSHA512 encryption.
        /// It takes the password as input, computes the hash, and stores both the hash and salt as byte arrays.
        /// </summary>
        /// 

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        /// <summary>
        /// This helper method verifies a password hash by computing the hash of the provided password using the given salt and comparing it to the stored hash. 
        /// It returns true if the hashes match, indicating a valid password.
        /// </summary>
        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

        /// <summary>
        /// This private method generates a random token by creating a hexadecimal string 
        /// representation of 64 random bytes generated by RandomNumberGenerator.
        /// </summary>
        private string CreateRandomToken()
        {
            return Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
        }
    }
}
