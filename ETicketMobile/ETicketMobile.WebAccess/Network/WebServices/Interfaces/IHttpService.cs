using System;
using System.Threading.Tasks;

namespace ETicketMobile.WebAccess.Network.WebServices.Interfaces
{
    public interface IHttpService
    {
        Task<T> GetAsync<T>(Uri endpoint, string token);

        Task<TDestination> PostAsync<TSource, TDestination>(Uri endpoint, TSource item, string token = "");
    }
}