using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatOverflow.V1.Models.ResultModels
{
    public class ErrorCodeResult
    {
        public string Code { get; set; }
        public string Description { get; set; }
    }

    public class ErrorResult
    {
        public object Data { get; set; }
        public ICollection<ErrorCodeResult> Errors { get; set; }

        public ErrorResult()
        {
            Errors = new List<ErrorCodeResult>();
        }
    }
}
