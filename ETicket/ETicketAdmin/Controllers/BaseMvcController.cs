using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ETicket.Admin.Controllers
{
    public class BaseMvcController : Controller
    {
        protected IActionResult Json(object dtoObject, int statusCode = StatusCodes.Status200OK)
        {
            var jsonString = JsonConvert.SerializeObject(dtoObject, Formatting.Indented);
            var result = new ObjectResult(jsonString)
            {
                StatusCode = statusCode
            };

            return result;
        }
    }
}