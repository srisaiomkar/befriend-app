using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ErrorTestController : BaseApiController
    {
        private readonly DataContext _context;
        public ErrorTestController(DataContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpGet("auth")]
        public ActionResult<string> GetSecret(){
            return "Secret";
        }

        [HttpGet("not-found")]
        public ActionResult<AppUser> GetNotFound(){
            var user = _context.Users.Find(-1);
            if(user == null) return NotFound();
            return Ok(user);
        }

        [HttpGet("server-error")]
        public ActionResult<string> GetServerError(){
            var user = _context.Users.Find(-1);
            var toReturn = user.ToString();
            return toReturn;
        }

        [HttpGet("bad-request")]
        public ActionResult<string> GetBadRequest(){
            return BadRequest("bad request");
        }

    }
}