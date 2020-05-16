using System.Threading.Tasks;
using ETicket.ApplicationServices.Services.Interfaces;
using ETicket.WebAPI.LiqPayApi;
using ETicket.WebAPI.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ETicket.WebAPI.Controllers.Payments
{
    [Route("api/payments")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        #region Private Members

        private readonly PaymentsService paymentsService;

        #endregion

        public PaymentsController(
            IUserService userService,
            IPriceListService priceListService,
            ITicketTypeService ticketTypeService,
            ITransactionService transactionAppService,
            ITicketService ticketService,
            IMerchant merchant
        )
        {
            paymentsService = new PaymentsService(
                transactionAppService, priceListService, ticketTypeService, ticketService, userService, merchant);
        }

        // /api/payments/getTicketPrice
        [HttpPost]
        [Route("[action]")]
        public IActionResult GetTicketPrice(GetTicketPriceRequest request)
        {
            var totalPrice = paymentsService.GetTicketPrice(request.AreasId, request.TicketTypeId);

            return Ok(new { totalPrice });
        }

        // /api/payments/Buy
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Buy(BuyTicketRequest request)
        {
            var response = await paymentsService.MakePayment(
                    request.Price,
                    request.Description,
                    request.CardNumber,
                    request.ExpirationMonth,
                    request.ExpirationYear,
                    request.CVV2,
                    request.TicketTypeId,
                    request.Email);

            var buyTicketResponse = new BuyTicketResponse
            {
                PayResult = response.Result,
                TotalPrice = (decimal)response.Amount,
                BoughtAt = response.CreateDate
            };

            return Ok(buyTicketResponse);
        }
    }
}