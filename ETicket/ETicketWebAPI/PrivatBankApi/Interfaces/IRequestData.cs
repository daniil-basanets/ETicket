namespace ETicket.WebAPI.PrivatBankApi.Interfaces
{
    public interface IRequestData
    {
        string Endpoint { get; }
        string GetXML();
    }
}