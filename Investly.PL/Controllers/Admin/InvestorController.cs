using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Investly.PL.Controllers.Admin
{
    [Route("api/admin/[controller]")]
    [ApiController]
    public class InvestorController : ControllerBase
    {
        // GET: api/<InvestorController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "hii", "value2" };
        }

        // GET api/<InvestorController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<InvestorController>
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<InvestorController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<InvestorController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
