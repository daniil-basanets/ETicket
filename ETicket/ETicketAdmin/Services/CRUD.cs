
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;
using ETicket.Admin.Extensions;
using ETicket.DataAccess.Domain;
using ETicket.DataAccess.Domain.Entities;
using ETicket.DataAccess.Domain.Interfaces;
using ETicketAdmin.DTOs;
using Microsoft.EntityFrameworkCore;

namespace ETicket.Admin.Services
{
    public class CRUD
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        //private readonly ETicketDataContext eTicketDataContext;

        public CRUD(IUnitOfWork unitOfWork, IMapper mapper/*, ETicketDataContext eTicketDataContext*/)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            //this.eTicketDataContext = eTicketDataContext;
        }

        private void Add<T>(T documentTypeDto)
        {
            var documentType = mapper.Map<DocumentType>(documentTypeDto);
            unitOfWork.DocumentTypes.Create(documentType);
        }

        public IQueryable Read<T>(Type repository)
        {
            if(repository == typeof(DocumentType))
            {
                return unitOfWork.DocumentTypes.GetAll();
            }
            return null;

            //var types = new Dictionary<IQueryable<object>, Type>();
            //types.Add(unitOfWork.DocumentTypes.GetAll(), typeof(DocumentType));

            //var type = types.Where(t => t.Value == repository);

            //var query = type.Select(t => t.Key.First());

            //return null;

            //var result = new Dictionary<string, Action>{
            //    {typeof(DocumentType).Name, () => unitOfWork.DocumentTypes.GetAll()}
            //}[repository.GetType().Name].Target;
            //return (IQueryable<T>)result;

            //Type[] types = new Type[] { typeof(DocumentType) };

            //var temp = repository;
            //var result = new Switch(repository)
            //.Case<DocumentType>
            //    (action => unitOfWork.DocumentTypes.GetAll()).Object;

            //return (IQueryable<T>)result;


            //return  switch 
        }

        //private T GetType<T>(object type)
        //{

        //    //if(type is DocumentType)


        //    //return type.GetType() switch
        //    //{
        //    //    DocumentType => unitOfWork.DocumentTypes.GetAll()
        //    //}
        //}

        private DocumentType Read(int id) => unitOfWork.DocumentTypes.Get((int)id);

        private void Update(DocumentTypeDto documentTypeDto)
        {
            var documentType = mapper.Map<DocumentType>(documentTypeDto);

            unitOfWork.DocumentTypes.Update(documentType);
        }

        private void Delete(int id)
        {
            unitOfWork.DocumentTypes.Delete(id);
        }

        private void Save() => unitOfWork.Save();

    }
}
