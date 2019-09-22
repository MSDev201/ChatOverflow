using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatOverflow.V1.Controller
{
    [ApiVersion("1.0")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        [NonAction]
        protected string GetCurrentUserId()
        {
            return User.FindFirst("UserId")?.Value;
        }
    }
}
