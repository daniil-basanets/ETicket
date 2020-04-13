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
        private readonly IUnitOfWork eTicketData;
        private readonly PrivatBankApiClient privatBankApiClient;

        public PaymentsController(IUnitOfWork eTicketData, IMerchant merchant)
        {
            this.eTicketData = eTicketData;

            privatBankApiClient = new PrivatBankApiClient(merchant.MerchantId, merchant.Password);
        }

        // /api/Payments/Buy
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Buy(BuyTicketRequest request)
        {
            var paymentsAppService = new PaymentsAppService(eTicketData, privatBankApiClient);
            var response = await paymentsAppService.ProcessAsync(request);

            return Ok(response);
        }
    }
}