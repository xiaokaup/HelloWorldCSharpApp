using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HelloWorldCSharpApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FriendController : ControllerBase
    {

        //private readonly ILogger<FriendController> _logger;

        //public FriendController(ILogger<FriendController> logger)
        //{
        //    _logger = logger;
        //}

        [HttpGet(Name = "GetFriends")]
        public List<Friend> GetFriends()
        {
            List<Friend> firends = new List<Friend>();

            firends.Add(new Friend("firstName a", "lastName a", "location a"));
            firends.Add(new Friend("firstName b", "lastName b", "location b"));
            firends.Add(new Friend("firstName c", "lastName c", "location c"));

            return firends;
        }
    }
}
