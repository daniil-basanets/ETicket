using System;
using System.Collections.Generic;
using ETicket.Models.Interfaces;
using ETicket.PrivatBankApi;
using ETicket.PrivatBankApi.PrivatBank;
using Microsoft.AspNetCore.Mvc;

namespace ETicket.Controllers
{
    /// <summary>
    /// Pattern State
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    // Context
    public class PaymentController : ControllerBase
    {
        private readonly PrivatBankApiClient client;

        /// <param name="merchant">State</param>
        public PaymentController(IMerchant merchant)
        {
            // depends on merchant state which user account will be used in privatBank api
            client = new PrivatBankApiClient(merchant.MerchantId, merchant.Password);
        }

        // GET: api/Payment
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Payment/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        /// <summary>
        /// State Handler
        /// </summary>
        [HttpGet]
        [Route("[action]")]
        public IActionResult BuyTicket(string cardNum, string amount)
        {
            var privatBankCardRequest = new SendToPrivatBankCardRequest
            {
                PaymentId = Guid.NewGuid().ToString(),
                CardNumber = cardNum,
                Amount = decimal.Parse(amount),
                Currency = "UAH",
                Details = "Test"
            };

            var privatBankCardResponse = client
                .ExecuteAsync<SendToPrivatBankCardRequest, SendToPrivatBankCardResponse>(privatBankCardRequest).Result;

            return Ok(privatBankCardResponse);
        }

        // POST: api/Payment
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Payment/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}