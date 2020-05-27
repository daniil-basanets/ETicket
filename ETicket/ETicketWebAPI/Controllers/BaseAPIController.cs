using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ETicket.WebAPI.Controllers
{
    public class BaseAPIController : ControllerBase
    {
        protected virtual IActionResult Json(object dtoObject, int statusCode = StatusCodes.Status200OK)
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
