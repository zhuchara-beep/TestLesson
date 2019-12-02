using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestService.Models
{
    /// <summary>
    /// Запрос к серверу
    /// </summary>
    public class Request
    {
        public string RequestIdent { get; set; }
        public object Data { get; set; }
    }
}
