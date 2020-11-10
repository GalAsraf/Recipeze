using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace API.Controllers
{
    [EnableCors("*", "*","*")]
    [RoutePrefix("api/user")]
    public class UserController : ApiController
    {
        [Route("GetUserExist"), HttpPost]
        // public IHttpActionResult GetUserExist(string userName, string password)//User user
        public IHttpActionResult GetUserExist([FromBody]UserDTO user)
        {
            int u = BL.UserBL.GetUserExist(user.Email, user.Password);
            if (u != -1)
                return Ok(u);
            else
                return NotFound();
        }

        [Route("AddUser"), HttpPost]
        public IHttpActionResult AddUser(UserDTO user)
        {
            BL.UserBL.AddUser(user);
            return Ok("run");
        }
    }
}
