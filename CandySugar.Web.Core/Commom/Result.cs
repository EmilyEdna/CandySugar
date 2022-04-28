using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Web.Core.Commom
{
    public class Result
    {
        public int Code { get; set; } = 200;
        public object Response { get; set; }
    }
}
