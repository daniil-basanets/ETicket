using System.Collections.Generic;
using System.Threading.Tasks;
using ETicketMobile.Business.Model.Tickets;
using ETicketMobile.WebAccess.DTO;

namespace ETicketMobile.Business.Services.Interfaces
{
    public interface ITicketsService
    {
        Task<IEnumerable<Ticket>> GetTicketsAsync(string accessToken, string email);

        Task<IList<TicketType>> GetTicketTypesAsync(string accessToken);

        Task<IList<AreaDto>> GetAreasDtoAsync(string accessToken);

        Task<GetTicketPriceResponseDto> RequestGetTicketPriceAsync(IEnumerable<int> areasId, int ticketTypeId);

        Task<BuyTicketResponseDto> RequestBuyTicketAsync(BuyTicketRequestDto buyTicketRequestDto);
    }
}