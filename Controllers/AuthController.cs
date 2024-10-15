using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using TechnicalTest.Models; 
using System.Linq;           
using System;

namespace TechnicalTest.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class AuthController : ControllerBase
  {
    private readonly IConfiguration _configuration;
    private readonly AppDbContext _context;

    public AuthController(IConfiguration configuration, AppDbContext context)
    {
      _configuration = configuration;
      _context = context;
    }

    [HttpPost("login")]
    public ActionResult<string> Login([FromBody] LoginModel login)
    {
      // Validate that the model is not null and that the fields are not empty
      if (login == null || string.IsNullOrEmpty(login.Username) || string.IsNullOrEmpty(login.Password))
      {
        return BadRequest("Username and password are required.");
      }

      var user = _context.Employees
                         .FirstOrDefault(e => e.Name == login.Username);

      if (user == null)
      {
        return Unauthorized("Username not found.");
      }

      if (!VerifyPassword(login.Password, user.PasswordHash))
      {
        return Unauthorized("Incorrect password.");
      }

      string role = user.CurrentPosition > 1 ? "Admin" : "User";
      var token = GenerateJwtToken(role);

      return Ok(new { token });
    }


    private bool VerifyPassword(string password, string passwordHash)
    {
      return password == passwordHash;
    }

    private string GenerateJwtToken(string role)
    {
      var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
      var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

      var claims = new[]
      {
        new Claim(ClaimTypes.Role, role)
      };

      var token = new JwtSecurityToken(
          issuer: _configuration["Jwt:Issuer"],
          audience: _configuration["Jwt:Issuer"],
          claims: claims,
          expires: DateTime.Now.AddMinutes(30),
          signingCredentials: creds);

      return new JwtSecurityTokenHandler().WriteToken(token);
    }
  }
  public class LoginModel
  {
    public string Username { get; set; }
    public string Password { get; set; }
  }

}