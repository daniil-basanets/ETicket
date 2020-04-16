using ETicketWebAPI.PrivatBankApi.Interfaces;

namespace ETicketWebAPI.PrivatBankApi.PrivatBank
{
    public class SendToPrivatBankCardRequest : IRequestData
    {
        public string PaymentId { get; set; }

        public string CardNumber { get; set; }

        public decimal Amount { get; set; }

        public string Currency { get; set; }

        public string Details { get; set; }

        public string Endpoint => PrivatBankApiEndpoint.PayInsidePrivatBank;

        public string GetXML()
        {
            return "<oper>cmt</oper>"
                 + "<wait>0</wait>"
                 + "<test>1</test>"
                 + $@"<payment id=""{PaymentId}"">"
                 + $@"<prop name=""b_card_or_acc"" value=""{CardNumber}""/>"
                 + $@"<prop name=""amt"" value=""{Amount.ToString().Replace(',','.')}""/>"
                 + $@"<prop name=""ccy"" value=""{Currency}""/>"
                 + $@"<prop name=""details"" value=""{Details}"" />"
                 + "</payment>";
        }
    }
}