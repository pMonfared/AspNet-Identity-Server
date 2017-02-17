using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResourceWithIdentityServerWithClient.Model;
using ASPNETCoreIdentitySample.DataLayer.Context;

namespace ResourceWithIdentityServerWithClient.Controllers
{
    [Authorize("admin")]
    [Produces("application/json")]
    [Route("api/UserManagement")]
    public class UserManagementController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserManagementController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var users = _context.Users.ToList();
            var result = new List<UserDto>();

            foreach(var applicationUser in users)
            {
                var user = new UserDto
                {
                    Id = applicationUser.Id.ToString(),
                    Name = applicationUser.Email,
                    IsAdmin = false,
                    IsActive = applicationUser.IsActive
                };

                result.Add(user);
            }

            return Ok(result);
        }
        
        [HttpPut("{id}")]
        public void Put(string id, [FromBody]UserDto userDto)
        {
            var user = _context.Users.First(t => t.Id == Convert.ToInt32(id));

            //user.IsAdmin = userDto.IsAdmin;
            

            _context.Users.Update(user);
            _context.SaveChanges();
        }   
    }
}
