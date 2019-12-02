using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestService.Models;
using TestService.Data;
using Newtonsoft.Json;

namespace TestService.Actions
{
    public class ActPerson
    {
        public Result Add(Request req)
        {
            dynamic data = JsonConvert.DeserializeObject(req.Data.ToString());
            return DBPerson.Add(data?.Name?.ToString(), data?.Position?.ToString());
        }

        public Result GetAll(Request req)
        {
            return DBPerson.GetAll();
        }
    }
}
