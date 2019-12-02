using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TestService.Models;
using TestService.Actions;
using Newtonsoft.Json;

namespace TestService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public JsonResult Post([FromBody] Request value)
        {

            Random rand = new Random();
            var x = rand.Next(10000000, 99999999);
            string session = x.ToString();

            Program.Logger.Debug(" > "+session +" > "+JsonConvert.SerializeObject(value));


            Result res = new Result();
            switch (value.RequestIdent)
            {
                case "PositionAdd":
                    ActPosition actPosition = new ActPosition();
                    res = actPosition.Add(value);
                    break;
                case "PositionsGet":
                    ActPosition actPositionGetAll = new ActPosition();
                    res = actPositionGetAll.GetAll(value);
                    break;
                case "PersonAdd":
                    ActPerson actPerson = new ActPerson();
                    res = actPerson.Add(value);
                    break;
                case "PersonsGet":
                    ActPerson actPersonGetAll = new ActPerson();
                    res = actPersonGetAll.GetAll(value);
                    break;
                default:
                    res.Code = 105;
                    res.Info = "Не найден метод";
                    break;
            }

            Program.Logger.Debug(" < " + session + " < " + JsonConvert.SerializeObject(res));
            return new JsonResult(res);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
