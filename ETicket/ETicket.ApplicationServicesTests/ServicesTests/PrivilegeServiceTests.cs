using System;
using System.Collections.Generic;
using System.Linq;
using ETicket.ApplicationServices.DTOs;
using ETicket.ApplicationServices.Services;
using ETicket.DataAccess.Domain.Entities;
using ETicket.DataAccess.Domain.Interfaces;
using ETicket.DataAccess.Domain.Repositories;
using Moq;
using Xunit;

namespace ETicket.ApplicationServicesTests.ServicesTests
{
    public class PrivilegeServiceTests
    {
        #region Private members

        private readonly PrivilegeService privilegeService;

        private readonly IList<Privilege> fakePrivileges;

        private readonly PrivilegeDto privilegeDto;

        private Privilege privilege;

        #endregion

        #region Constructor

        public PrivilegeServiceTests()
        {
            var mockRepository = new Mock<IRepository<Privilege, int>>();
            var mockUnitOfWork = new Mock<IUnitOfWork>();

            fakePrivileges = new List<Privilege>
            {
                new Privilege {Id = 1, Name = "Priv1", Coefficient = 0M},
            new Privilege {Id = 2, Name = "Priv2",  Coefficient = 0.3M},
            new Privilege {Id = 3, Name = "Priv3", Coefficient = 1M}
            };

            privilegeDto = new PrivilegeDto
            {
                Id = 4,
                Name = "Priv4",
                Coefficient = 0.3452M
            };

            mockRepository.Setup(m => m.GetAll()).Returns(fakePrivileges.AsQueryable);
            mockRepository.Setup(m => m.Get(It.IsAny<int>()))
                .Returns<int>(id => fakePrivileges.Single(t => t.Id == id));
            mockRepository.Setup(r => r.Create(It.IsAny<Privilege>()))
                .Callback<Privilege>(t => fakePrivileges.Add(privilege = t));
            mockRepository.Setup(r => r.Update(It.IsAny<Privilege>()))
                .Callback<Privilege>(t => privilege = t);
            mockRepository.Setup(x => x.Delete(It.IsAny<int>()))
                .Callback<int>(id => fakePrivileges.Remove(fakePrivileges.Single(t => t.Id == id)));

            mockUnitOfWork.Setup(m => m.Privileges).Returns(mockRepository.Object);

            privilegeService = new PrivilegeService(mockUnitOfWork.Object);
        }

        #endregion

        #region Create 

        [Fact]
        public void Create_ValidPrivilegeDto_EntityModelNotNull()
        {
            privilegeService.Create(privilegeDto);

            var actual = privilege;

            Assert.NotNull(actual);
        }

        [Fact]
        public void Create_ValidPrivilegeDto_NameTheSameAsInEntityModel()
        {
            privilegeService.Create(privilegeDto);

            var expected = privilegeDto.Name;
            var actual = privilege.Name;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Create_ValidPrivilegeDto_PrivilegesCountIncrement()
        {
            var expected = fakePrivileges.Count + 1;

            privilegeService.Create(privilegeDto);

            var actual = fakePrivileges.Count;

            Assert.Equal(expected, actual);
        }

        //Negative cases

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(" \r \t \n")]
        [InlineData("q")]
        public void Create_InvalidPrivilegeName_Fail(string name)
        {
            privilegeDto.Name = name;
            Action action = () => privilegeService.Create(privilegeDto);

            Assert.Throws<ArgumentException>(action);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(2)]
        [InlineData(-0.2)]
        [InlineData(2.1)]
        public void Create_InvalidPrivilegeCoefficient_Fail(decimal coefficient)
        {
            privilegeDto.Coefficient = coefficient;
            Action action = () => privilegeService.Create(privilegeDto);

            Assert.Throws<ArgumentException>(action);
        }


        [Fact]
        public void Create_NullPrivilegeDto_Fail()
        {
            Assert.Throws<ArgumentNullException>(() => privilegeService.Create(null));
        }

        #endregion

        #region Update 

        [Fact]
        public void Update_ValidPrivilegeDto_NameEqualDTOsName()
        {
            privilegeDto.Name = "UpdatedName";
            var expected = privilegeDto.Name;

            privilegeService.Update(privilegeDto);
            var actual = privilege.Name;

            Assert.Equal(expected, actual);
        }

        //Negative cases
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(" \r \t \n")]
        [InlineData("q")]
        public void Update_InvalidPrivilegeName_Fail(string name)
        {
            privilegeDto.Name = name;
            Action action = () => privilegeService.Update(privilegeDto);

            Assert.Throws<ArgumentException>(action);
        }
        [Theory]
        [InlineData(-1)]
        [InlineData(2)]
        [InlineData(-0.2)]
        [InlineData(2.1)]
        public void Update_InvalidPrivilegeCoefficient_Fail(decimal coefficient)
        {
            privilegeDto.Coefficient = coefficient;
            Action action = () => privilegeService.Update(privilegeDto);

            Assert.Throws<ArgumentException>(action);
        }

        #endregion

        #region Delete

        [Fact]
        public void Delete_ValidPrivilegeId_PrivilegesCountDecrement()
        {
            var expected = fakePrivileges.Count - 1;

            privilegeService.Delete(fakePrivileges.First().Id);

            var actual = fakePrivileges.Count;

            Assert.Equal(expected, actual);
        }

        //Negative case
        [Theory]
        [InlineData(0)]
        [InlineData(-100)]
        public void Delete_InvalidPrivilegeId_Fail(int id)
        {
            Action action = () => privilegeService.Delete(id);

            Assert.Throws<InvalidOperationException>(action);
        }

        #endregion

        #region GetAll 

        [Fact]
        public void GetAll_NoParameters_NotNull()
        {
            var actual = privilegeService.GetPrivileges();

            Assert.NotNull(actual);
        }

        [Fact]
        public void GetAll_NoParameters_EqualCount()
        {
            var privileges = privilegeService.GetPrivileges();

            var expected = fakePrivileges.Count;
            var actual = privileges.Count();

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Get

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void Get_ValidPrivilegeId_NameTheSameAsInFake(int id)
        {
            var expected = fakePrivileges.Single(t => t.Id == id).Name;
            var actual = privilegeService.GetPrivilegeById(id).Name;

            Assert.Equal(expected, actual);
        }

        //Negative case
        [Theory]
        [InlineData(int.MinValue)]
        [InlineData(0)]
        [InlineData(100)]
        [InlineData(int.MaxValue)]
        public void Get_InvalidPrivilegeId_Fail(int id)
        {
            Action action = () => privilegeService.GetPrivilegeById(id);

            Assert.Throws<InvalidOperationException>(action);
        }

        #endregion
    }
}