using curso.api.Models.Cursos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace curso.api.Controllers
{
    [Route("api/v1/courses")]
    [ApiController]
    [Authorize]
    public class CursoController : ControllerBase
    {
        /// <summary>
        /// This service allows register course for the authenticated user.
        /// </summary>
        /// <returns>Return 201 status and user course data</returns>
        [SwaggerResponse(statusCode: 201, description: "Sucess in registering a couse")]
        [SwaggerResponse(statusCode: 401, description: "Not authorized")]

        [HttpPost]
        [Route("")]

        public async Task<IActionResult> Post(CursoViewModelInput cursoViewModelInput)
        {
            var userCode = int.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
            return Created("", cursoViewModelInput);
        }

        /// <summary>
        /// This service allows to obtain all user actives courses.
        /// </summary>
        /// <returns>Return status ok and user course datas</returns>
        [SwaggerResponse(statusCode: 200, description: "Sucess in getting the courses")]
        [SwaggerResponse(statusCode: 401, description: "Not authorized")]
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Get()
        {
            var courses = new List<CursoViewModelOutput>();

            //var userCode = int.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
            courses.Add(new CursoViewModelOutput()
            {
                Login = "",
                Description = "teste",
                Name = "teste"
            });

            return Ok(courses);
        }
    }
}
