using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ETicketMobile.Business.Mapping;
using ETicketMobile.Business.Model.Tickets;
using ETicketMobile.Business.Services.Interfaces;
using ETicketMobile.WebAccess;
using ETicketMobile.WebAccess.DTO;
using ETicketMobile.WebAccess.Network.Endpoints;
using ETicketMobile.WebAccess.Network.WebServices.Interfaces;
using Java.Net;

namespace ETicketMobile.Business.Services
{
    public class TicketsService : ITicketsService
    {
        #region Fields

        private readonly ITokenService tokenService;
        private readonly IHttpService httpService;

        #endregion

        public TicketsService(ITokenService tokenService, IHttpService httpService)
        {
            this.tokenService = tokenService
                ?? throw new ArgumentNullException(nameof(tokenService));

            this.httpService = httpService
                ?? throw new ArgumentNullException(nameof(httpService));
        }

        public async Task<IEnumerable<Ticket>> GetTicketsAsync(string accessToken, string email)
        {
            var getTicketsByEmail = TicketsEndpoint.GetTicketsByEmail(email);

            try
            {
                var ticketsDto = await httpService.GetAsync<IEnumerable<TicketDto>>(
                    getTicketsByEmail, accessToken);

                if (ticketsDto == null)
                {
                    accessToken = await tokenService.RefreshTokenAsync();

                    ticketsDto = await httpService.GetAsync<IEnumerable<TicketDto>>(
                        getTicketsByEmail, accessToken);
                }

                var tickets = AutoMapperConfiguration.Mapper.Map<IEnumerable<Ticket>>(ticketsDto);

                return tickets;
            }
            catch (System.Net.WebException ex)
            {
                throw new Exceptions.WebException("Server exception", ex);
            }
            catch (SocketException ex)
            {
                throw new Exceptions.WebException("Server exception", ex);
            }
        }

        public async Task<IList<TicketType>> GetTicketTypesAsync(string accessToken)
        {
            try
            {
                var ticketTypesDto = await httpService.GetAsync<IList<TicketTypeDto>>(
                        TicketTypesEndpoint.GetTicketTypes, accessToken);

                if (ticketTypesDto == null)
                {
                    accessToken = await tokenService.RefreshTokenAsync();

                    ticketTypesDto = await httpService.GetAsync<IList<TicketTypeDto>>(
                        TicketTypesEndpoint.GetTicketTypes, accessToken);
                }

                var ticketTypes = AutoMapperConfiguration.Mapper.Map<IList<TicketType>>(ticketTypesDto);

                return ticketTypes;
            }
            catch (System.Net.WebException ex)
            {
                throw new Exceptions.WebException("Server exception", ex);
            }
            catch (SocketException ex)
            {
                throw new Exceptions.WebException("Server exception", ex);
            }
        }

        public async Task<IList<AreaDto>> GetAreasDtoAsync(string accessToken)
        {
            try
            {
                var areasDto = await httpService.GetAsync<IList<AreaDto>>(AreasEndpoint.GetAreas, accessToken);
                
                if (areasDto == null)
                {
                    accessToken = await tokenService.RefreshTokenAsync();

                    areasDto = await httpService.GetAsync<IList<AreaDto>>(AreasEndpoint.GetAreas, accessToken);
                }

                return areasDto;

            }
            catch (System.Net.WebException ex)
            {
                throw new Exceptions.WebException("Server exception", ex);
            }
            catch (SocketException ex)
            {
                throw new Exceptions.WebException("Server exception", ex);
            }
        }

        public async Task<GetTicketPriceResponseDto> RequestGetTicketPriceAsync(IEnumerable<int> areasId, int ticketTypeId)
        {
            var getTicketPriceRequestDto = new GetTicketPriceRequestDto
            {
                AreasId = areasId,
                TicketTypeId = ticketTypeId
            };

            try
            {
                var response = await httpService.PostAsync<GetTicketPriceRequestDto, GetTicketPriceResponseDto>(
                        TicketsEndpoint.GetTicketPrice, getTicketPriceRequestDto);

                return response;
            }
            catch (System.Net.WebException ex)
            {
                throw new Exceptions.WebException("Server exception", ex);
            }
            catch (SocketException ex)
            {
                throw new Exceptions.WebException("Server exception", ex);
            }
        }

        public async Task<BuyTicketResponseDto> RequestBuyTicketAsync(BuyTicketRequestDto buyTicketRequestDto)
        {
            try
            {
                var response = await httpService.PostAsync<BuyTicketRequestDto, BuyTicketResponseDto>(
                TicketsEndpoint.BuyTicket, buyTicketRequestDto);

                return response;
            }
            catch (System.Net.WebException ex)
            {
                throw new Exceptions.WebException("Server exception", ex);
            }
            catch (SocketException ex)
            {
                throw new Exceptions.WebException("Server exception", ex);
            }
        }
    }
}