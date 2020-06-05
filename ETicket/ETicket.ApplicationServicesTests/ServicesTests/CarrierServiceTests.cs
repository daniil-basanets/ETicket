using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETicket.ApplicationServices.DTOs;
using ETicket.ApplicationServices.Services;
using ETicket.DataAccess.Domain.Entities;
using ETicket.DataAccess.Domain.Interfaces;
using Moq;
using Xunit;

namespace ETicket.ApplicationServicesTests.ServicesTests
{
    public class CarrierServiceTests
    {
        #region Private fields

        private readonly CarrierService carrierService;
        private readonly IList<Carrier> fakeCarriers;
        private readonly CarrierDto carrierDto;
        private Carrier carrier;

        #endregion

        #region Constructor

        public CarrierServiceTests()
        {
            var mockRepository = new Mock<IRepository<Carrier, int>>();
            var mockUnitOfWork = new Mock<IUnitOfWork>();

            fakeCarriers = new List<Carrier>
            {
                new Carrier {Id = 1, Name = "Carrier1", Phone = "380990112347", Address = "Some addr1", IBAN = "100023333113"},
                new Carrier {Id = 5, Name = "Carrier5", Phone = "+380990136974", Address = "Some addr5", IBAN = "108233113"},
                new Carrier {Id = 2, Name = "Carrier2", Phone = "+38(099)0136974", Address = "Some addr2", IBAN = "4655644242244"},
                new Carrier {Id = 6, Name = "Carrier6", Phone = "0990136974", Address = "1", IBAN = "424252522424"}
            };

            carrierDto = new CarrierDto
            {
                Id = 4,
                Name = "Carrier4",
                Phone = "+38(099)0136974",
                Address = "Some addr4",
                IBAN = "44423333113"
            };

            mockRepository.Setup(m => m.GetAll())
                           .Returns(fakeCarriers.AsQueryable);
            mockRepository.Setup(m => m.Get(It.IsAny<int>()))
                          .Returns<int>(id => fakeCarriers.Single(t => t.Id == id));
            mockRepository.Setup(r => r.Create(It.IsAny<Carrier>()))
                          .Callback<Carrier>(t => fakeCarriers.Add(carrier = t));
            mockRepository.Setup(r => r.Update(It.IsAny<Carrier>()))
                          .Callback<Carrier>(t => carrier = t);
            mockRepository.Setup(x => x.Delete(It.IsAny<int>()))
                          .Callback<int>(id => fakeCarriers.Remove(fakeCarriers.Single(t => t.Id == id)));

            mockUnitOfWork.Setup(m => m.Carriers).Returns(mockRepository.Object);

            carrierService = new CarrierService(mockUnitOfWork.Object);
        }

        #endregion

        #region Create

        [Fact]
        public void Create_ValidCarrierDto_EntityModelNotNull()
        {
            carrierService.Create(carrierDto);

            var actual = carrier;

            Assert.NotNull(actual);
        }

        [Fact]
        public void Create_ValidCarrierDto_NameShouldBeTheSameAsInEntityModel()
        {
            carrierService.Create(carrierDto);

            var expected = carrierDto.Name;
            var actual = carrier.Name;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Create_ValidCarrierDto_CarriersCountShouldIncrease()
        {
            var expected = fakeCarriers.Count + 1;

            carrierService.Create(carrierDto);

            var actual = fakeCarriers.Count;

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(" \r \t \n")]
        [InlineData("q")]
        [InlineData("qwertyuioplkjhgfdsazxcvbnmetretertretertertsadasdsd")]
        public void Create_InvalidCarrierName_ShouldFail(string name)
        {
            carrierDto.Name = name;
            Action action = () => carrierService.Create(carrierDto);

            Assert.Throws<ArgumentException>(action);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(" \r \t \n")]
        [InlineData("q")]
        [InlineData("qwertyuioplkjhgfdsazxcvbnmetret")]
        public void Create_InvalidCarrierIban_ShouldFail(string iban)
        {
            carrierDto.IBAN = iban;
            Action action = () => carrierService.Create(carrierDto);

            Assert.Throws<ArgumentException>(action);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(" \r \t \n")]
        [InlineData("q")]
        [InlineData("qwertyuioplkjhgfdsazxcvbnmetretertretertertsadasdsadsa")]
        [InlineData("*/--+//-+-#$(")]
        public void Create_InvalidCarrierPhone_ShouldFail(string phone)
        {
            carrierDto.Phone = phone;
            Action action = () => carrierService.Create(carrierDto);

            Assert.Throws<ArgumentException>(action);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(" \r \t \n")]
        [InlineData("q")]
        [InlineData("qwertyuioplkjhgfdsazxcvbnmetretertretertertsadasdsadsa")]
        public void Create_InvalidCarrierAddress_ShouldFail(string address)
        {
            carrierDto.Address = address;
            Action action = () => carrierService.Create(carrierDto);

            Assert.Throws<ArgumentException>(action);
        }

        [Fact]
        public void Create_NullCarrierDto_ShouldFail()
        {
            Assert.Throws<ArgumentNullException>(() => carrierService.Create(null));
        }

        #endregion

        #region Update

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(" \r \t \n")]
        [InlineData("q")]
        [InlineData("qwertyuioplkjhgfdsazxcvbnmetretertretertertsadasdsadsa")]
        public void Update_InvalidCarrierName_ShouldFail(string name)
        {
            carrierDto.Name = name;
            Action action = () => carrierService.Update(carrierDto);

            Assert.Throws<ArgumentException>(action);
        }

        [Fact]
        public void Update_ValidCarrierDto_NameShouldBeEqualDTOsName()
        {
            carrierDto.Name = "UpdatedName";
            var expected = carrierDto.Name;

            carrierService.Update(carrierDto);

            var actual = carrier.Name;

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Delete

        [Fact]
        public void Delete_ValidCarrierId_CarriersCountShouldDecrease()
        {
            var expected = fakeCarriers.Count - 1;

            carrierService.Delete(fakeCarriers.First().Id);

            var actual = fakeCarriers.Count;

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-100)]
        public void Delete_InvalidCarrierId_ShouldFail(int id)
        {
            Action action = () => carrierService.Delete(id);

            Assert.Throws<InvalidOperationException>(action);
        }

        #endregion

        #region GetAll

        [Fact]
        public void GetAll_NoParameters_ReturnShouldBeNotNull()
        {
            var actual = carrierService.GetAll();

            Assert.NotNull(actual);
        }

        [Fact]
        public void GetAll_NoParameters_ReturnCountShouldBeEqual()
        {
            var carriers = carrierService.GetAll();

            var expected = fakeCarriers.Count;
            var actual = carriers.Count();

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Get

        [Theory]
        [InlineData(5)]
        [InlineData(2)]
        [InlineData(1)]
        [InlineData(6)]
        public void Get_ValidCarrierId_NameShouldBeTheSameAsInFake(int id)
        {
            var expected = fakeCarriers.Single(t => t.Id == id).Name;
            var actual = carrierService.Get(id).Name;

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(int.MinValue)]
        [InlineData(0)]
        [InlineData(100)]
        [InlineData(int.MaxValue)]
        public void Get_InvalidCarrierId_ShouldFail(int id)
        {
            Action action = () => carrierService.Get(id);

            Assert.Throws<InvalidOperationException>(action);
        }

        #endregion
    }
}
