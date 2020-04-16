namespace ETicketWebAPI.PrivatBankApi.Interfaces
{
    public interface IRequestData
    {
        string Endpoint { get; }
        string GetXML();
    }
}