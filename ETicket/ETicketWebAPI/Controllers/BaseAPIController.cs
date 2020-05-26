using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ETicket.WebAPI.Controllers
{
    public class BaseAPIController : ControllerBase
    {
        protected virtual string GetJson(object dtoObject)
        {
            return JsonConvert.SerializeObject(dtoObject, Formatting.Indented);
        }
    }
}
