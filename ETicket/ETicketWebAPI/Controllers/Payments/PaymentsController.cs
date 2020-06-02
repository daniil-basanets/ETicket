using System.Threading.Tasks;
using ETicket.ApplicationServices.Services.Interfaces;
using ETicket.WebAPI.LiqPayApi;
using ETicket.WebAPI.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ETicket.WebAPI.Controllers.Payments
{
    [Route("api/payments")]
    [ApiController]
    [SwaggerTag("Payments service")]
    public class PaymentsController : BaseAPIController
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

        [HttpPost]
        [Route("ticketprice")]
        [SwaggerOperation(Summary = "Get price for ticket", Description = "Allowed: everyone")]
        [SwaggerResponse(200, "Returns if everything is right. Contains an object with total price")]
        public IActionResult GetTicketPrice([FromBody, SwaggerRequestBody("Get ticket price payload", Required = true)] GetTicketPriceRequest request)
        {
            var totalPrice = paymentsService.GetTicketPrice(request.AreasId, request.TicketTypeId);

            return Ok(new { totalPrice });
        }

        [HttpPost]
        [Route("buy")]
        [SwaggerOperation(Summary = "Buy ticket", Description = "Allowed: authorized user")]
        [SwaggerResponse(200, "Returns if everything is right. Contains a BuyTicketResponse")]
        public async Task<IActionResult> Buy([FromBody, SwaggerRequestBody("Buy ticket payload", Required = true)] BuyTicketRequest request)
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