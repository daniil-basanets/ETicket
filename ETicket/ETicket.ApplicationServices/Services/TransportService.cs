﻿using System.Collections.Generic;
using System.Linq;
using ETicket.ApplicationServices.DTOs;
using ETicket.ApplicationServices.Services.Interfaces;
using ETicket.DataAccess.Domain.Entities;
using ETicket.DataAccess.Domain.Interfaces;


namespace ETicket.ApplicationServices.Services
{
    public class TransportService: ITransportService
    {
        #region private members

        private readonly IUnitOfWork uow;

        private readonly MapperService mapper;

        #endregion

        public TransportService(IUnitOfWork uow)
        {
            this.uow = uow;
            mapper = new MapperService();
        }

        public IEnumerable<TransportDto> GetAll()
        {
            return mapper.Map<IQueryable<Transport>, IEnumerable<TransportDto>>(uow.Transports.GetAll()).ToList();
        }

        public Transport Get(int id)
        {
            return uow.Transports.Get(id);
        }

        public void Create(TransportDto transportDto)
        {
            var transport = mapper.Map<TransportDto, Transport>(transportDto);
            uow.Transports.Create(transport);
            uow.Save();
        }

        public void Update(TransportDto transportDto)
        {
            var transport = mapper.Map<TransportDto, Transport>(transportDto);
            uow.Transports.Update(transport);
            uow.Save();
        }

        public void Delete(int id)
        {
            uow.Transports.Delete(id);
            uow.Save();
        }

        public bool Exists(int id)
        {
            return uow.Transports.Get(id) != null;
        }
    }
}
