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
        [Route("GetUserExist/{username,password}"),HttpPost]
       // public IHttpActionResult GetUserExist(string userName, string password)//User user
        public IHttpActionResult GetUserExist(UserDTO user)
        {
            int u = BL.UserBL.GetUserExist(user.UserName, user.Password);
            //todo: check if user exists in database.
            if (u != 0)
                return Ok("not exist");
            else
                return Ok("exist");
        }

        [Route("AddUser"), HttpPost]
        public IHttpActionResult AddUser(UserDTO user)
        {
            BL.UserBL.AddUser(user);
            return Ok("run");
        }
    }
}
