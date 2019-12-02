using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestService.Models;
using TestService.Data;
using Newtonsoft.Json;


namespace TestService.Actions
{
    public class ActPosition
    {

        public Result Add(Request req)
        {
            /*
            Request request = new Request
            {
                RequestIdent = "1231231",
                Data = "werwerwe"
            };

            string str = JsonConvert.SerializeObject(request);

            string str2 = JsonConvert.DeserializeObject<string>(request.ToString());
            */
            return DBPosition.Add(req.Data.ToString());
        }

        public Result GetAll(Request req)
        {
            return DBPosition.GetAll();
        }

    }
}
