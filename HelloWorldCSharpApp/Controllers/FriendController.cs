using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HelloWorldCSharpApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FriendController : ControllerBase
    {

        static HttpClient client = new HttpClient();

        //private readonly ILogger<FriendController> _logger;

        //public FriendController(ILogger<FriendController> logger)
        //{
        //    _logger = logger;
        //}

        [HttpGet("test1", Name = "test1")]
        public Uri test1()
        {
            return new Uri("https://blog.followparis.com/");
        }
        [HttpGet("test2", Name = "test2")]
        public async Task<Object> test2()
        {
            // Update port # in the following line.
            client.BaseAddress = new Uri("http://localhost:8080/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            Object data = null;
            HttpResponseMessage response = await client.GetAsync("/demo");
            if (response.IsSuccessStatusCode)
            {
                //data = await response.Content.ReadAsAsync<Object>();
                data = await response.Content.ReadAsStringAsync();
            } else
            {
                data = response.StatusCode;
                System.Diagnostics.Debug.WriteLine(response.Content);
            }

            System.Diagnostics.Debug.WriteLine("This is a log");
            
            return data;
        }

        [HttpGet("getFriends", Name = "GetFriends")]
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
