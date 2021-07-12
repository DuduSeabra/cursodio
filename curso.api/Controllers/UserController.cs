using curso.api.Business.Entities;
using curso.api.Business.Repositories;
using curso.api.Filters;
using curso.api.Infraestruture.Data;
using curso.api.Models;
using curso.api.Models.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace curso.api.Controllers
{
    [Route("api/v1/user")]
    [ApiController]
    public class UserController : ControllerBase
    {


        private readonly IUserRepository _userRepository;
        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        /// <summary>
        /// This service allows authenticate a registered and active user
        /// </summary>
        /// <param name="loginViewModelInput"></param>
        /// <returns>status ok, user data and the token in case of success</returns>
        [SwaggerResponse(statusCode: 200, description: "Successful authenticating", Type = typeof(LoginViewModelInput))]
        [SwaggerResponse(statusCode: 400, description: "Obligatory fields", Type = typeof(ValidateFieldViewModelOutput))]
        [SwaggerResponse(statusCode: 500, description: "Internal Error", Type = typeof(GenericErrorViewModel))]

        [HttpPost]
        [Route("login")]
        [ValidationModelStateCustomized]
        public IActionResult Login(LoginViewModelInput loginViewModelInput)
        {
            var userViewModelOutput = new UserViewModelOutput()
            {
                Code = 1,
                Login = "Dudu_13",
                Email = "dudu@email.com"
            };

            var secret = Encoding.ASCII.GetBytes("MzfsT&d9gprP>!9$Es(X!5g@;ef!5sbk:jH\\2.}8ZP'qY#7");
            var symmetricSecurityKey = new SymmetricSecurityKey(secret);
            var securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userViewModelOutput.Code.ToString()),
                    new Claim(ClaimTypes.Name, userViewModelOutput.Login.ToString()),
                    new Claim(ClaimTypes.Email, userViewModelOutput.Email.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature)
            };
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var tokenGenerated = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);
            var token = jwtSecurityTokenHandler.WriteToken(tokenGenerated);

            return Ok(new
            {
                Token = token,
                Usuario = userViewModelOutput
            });
        }
        /// <summary>
        /// This service allows register a registered user nonexistent
        /// </summary>
        /// <param name="registerViewModelInput">Register login view model</param>
        [SwaggerResponse(statusCode: 200, description: "Successful authenticating", Type = typeof(LoginViewModelInput))]
        [SwaggerResponse(statusCode: 400, description: "Obligatory fields", Type = typeof(ValidateFieldViewModelOutput))]
        [SwaggerResponse(statusCode: 500, description: "Internal Error", Type = typeof(GenericErrorViewModel))]
        [HttpPost]
        [Route("register")]
        [ValidationModelStateCustomized]
        public IActionResult Register(RegisterViewModelInput registerViewModelInput)
        {
            //var optionsBuilder = new DbContextOptionsBuilder<CourseDbContext>();
            //optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=COURSE");
            //CourseDbContext context = new CourseDbContext(optionsBuilder.Options);

            //var pendingmigrations = context.Database.GetPendingMigrations();
            //if(pendingmigrations.Count() > 0)
            //{
            //    context.Database.Migrate();
            //}

            var user = new User();
            user.Login = registerViewModelInput.Login;
            user.Password = registerViewModelInput.Password;
            user.Email = registerViewModelInput.Email;
            _userRepository.Add(user);
            _userRepository.Commit();

            return Created("", registerViewModelInput);
        }
    }
}
