using ETicketDataAccess.Domain.Interfaces;
using ETicketWebAPI.Models.Interfaces;
using ETicketWebAPI.PrivatBankApi;
using ETicketWebAPI.Services.BuyTicket;
using Microsoft.AspNetCore.Mvc;

namespace ETicketWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        #region Privat Members

        private readonly IUnitOfWork eTicketData;
        private readonly IMerchantSettings merchantSettings;
        private readonly PrivatBankApiClient privatBankApiClient;

        #endregion

        public PaymentsController(
            IUnitOfWork eTicketData, 
            IMerchant merchant,
            IMerchantSettings merchantSettings
        )
        {
            this.eTicketData = eTicketData;
            this.merchantSettings = merchantSettings;

            privatBankApiClient = new PrivatBankApiClient(merchant.MerchantId, merchant.Password);
        }

        // /api/Payments/Buy
        [HttpPost]
        [Route("[action]")]
        public IActionResult Buy(BuyTicketRequest request)
        {
            var paymentsAppService = new PaymentsAppService(eTicketData, merchantSettings, privatBankApiClient);
            var response = paymentsAppService.Process(request);

            return Ok(response);
        }
    }
}