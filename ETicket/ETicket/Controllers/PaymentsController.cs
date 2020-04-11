using DBContextLibrary.Domain.Interfaces;
using ETicket.Models.Interfaces;
using ETicket.PrivatBankApi;
using ETicket.Services.BuyTicket;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ETicket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly PrivatBankApiClient client;
        private readonly IUnitOfWork eTicketData;

        public PaymentsController(IMerchant merchant, IUnitOfWork eTicketData)
        {
            client = new PrivatBankApiClient(merchant.MerchantId, merchant.Password);

            this.eTicketData = eTicketData;
        }

        // /api/Payments/Buy
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Buy(BuyTicketRequest request)
        {
            var mediator = new PaymentsAppService(eTicketData, client);
            var response = await mediator.ProcessAsync(request);

            return Ok(response);
        }
    }
}