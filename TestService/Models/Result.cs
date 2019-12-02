using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestService.Models
{
    /// <summary>
    /// Результат запроса
    /// </summary>
    public class Result
    {
        public int Code { get; set; }
        public string Info { get; set; }
        public object Data { get; set; }

        public Result()
        {
            Code = 101;
            Info = "Неизвестная ошибка";
            Data = null;
        }
    }
}
