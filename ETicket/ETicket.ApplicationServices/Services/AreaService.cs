using System.Collections.Generic;
using System.Linq;
using ETicket.ApplicationServices.DTOs;
using ETicket.ApplicationServices.Services.Interfaces;
using ETicket.DataAccess.Domain.Entities;
using ETicket.DataAccess.Domain.Interfaces;

namespace ETicket.ApplicationServices.Services
{
    public class AreaService : IAreaService
    {

        #region Private fields

        private readonly IUnitOfWork unitOfWork;
        private readonly MapperService mapper;

        #endregion

        #region Constructors

        public AreaService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            mapper = new MapperService();
        }

        #endregion
                
        public IEnumerable<AreaDto> GetAreas()
        {
            var areas = unitOfWork.Areas.GetAll();
            
            return mapper.Map<IQueryable<Area>, IEnumerable<AreaDto>>(areas).ToList();
        }

        public AreaDto GetAreaById(int id)
        {
            return mapper.Map<Area,AreaDto>(unitOfWork.Areas.Get(id));
        }

        public void Create(AreaDto areaDto)
        {
            var area = mapper.Map<AreaDto,Area>(areaDto);
            
            unitOfWork.Areas.Create(area);
            unitOfWork.Save();
        }

        public void Update(AreaDto areaDto)
        {
            var area = mapper.Map<AreaDto,Area>(areaDto);
            
            unitOfWork.Areas.Update(area);
            unitOfWork.Save();
        }

        public void Delete(int id)
        {
            unitOfWork.Areas.Delete(id);
            unitOfWork.Save();
        }
    }
}