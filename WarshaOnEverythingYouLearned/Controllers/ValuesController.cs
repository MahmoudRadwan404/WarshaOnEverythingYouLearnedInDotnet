using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WarshaOnEverythingYouLearned.Configrations;
using WarshaOnEverythingYouLearned.Data;
using WarshaOnEverythingYouLearned.Data.Entity;
using WarshaOnEverythingYouLearned.dto;

namespace WarshaOnEverythingYouLearned.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {

        public ApplicationDbContext Context { get; set; }
public JWTOptions jwt {  get; set; }
        public ValuesController(ApplicationDbContext context,JWTOptions ops)
        {
            Context = context;
            this.jwt = ops;

        }

        [HttpGet]
        public  IActionResult GetAllEmployees()
        {
           // var employees = await Context.Employees.ToListAsync();
            return Ok();
        }

        [HttpGet("andDepartment")]
       

        public async Task<IActionResult> GetAllemployeesDepartments()
        {
            var data =await Context.Users
       .Include(u=>u.roles)
        .Select(u => new UserDto
        {
            Id = u.Id,
            Name = u.Name,
            Email = u.email,
            IsActived = u.isActived,
            RoleId= u.RoleId,
        })
        .ToListAsync(); // ✅ This works!
            var currentUser=HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

            return Ok("currentUser working right now");
        }

      
        [HttpPost]
        public IActionResult login([FromBody]LoginDto loginDto) { 
        
            string email= loginDto.Email;
            string password= loginDto.Password;
            var userdata= Context.Users.FirstOrDefault(u => u.email == email && u.password == password);
            var tokenHandler=new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor()

            {
                Issuer = jwt.issuer,
                Audience = jwt.audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.key)), SecurityAlgorithms.HmacSha256)
            ,
                Subject = new System.Security.Claims.ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Email,userdata.email),
                })
            };
            var securitytoken = tokenHandler.CreateToken(tokenDescriptor);
            var val=tokenHandler.WriteToken(securitytoken);
            return Ok(val);
        }
    }
}
