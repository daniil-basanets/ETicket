using System;
using System.Collections.Generic;
using System.Linq;
using ETicket.ApplicationServices.DTOs;
using ETicket.ApplicationServices.Services.DocumentTypes;
using ETicket.DataAccess.Domain.Entities;
using ETicket.DataAccess.Domain.Interfaces;
using Moq;
using Xunit;

namespace ETicket.ApplicationServicesTests.ServicesTests
{
    public class DocumentTypesServiceTests
    {
        #region Private fields

        private readonly DocumentTypesService documentTypesService;
        private readonly IList<DocumentType> fakeDocumentTypes;
        private readonly DocumentTypeDto documentTypesDto;
        private DocumentType documentType;

        #endregion

        #region Constructor

        public DocumentTypesServiceTests()
        {
            var mockRepository = new Mock<IRepository<DocumentType, int>>();
            var mockUnitOfWork = new Mock<IUnitOfWork>();

            fakeDocumentTypes = new List<DocumentType>
            {
                new DocumentType {Id = 5, Name = "Test1"},
                new DocumentType {Id = 7, Name = "Test2"},
                new DocumentType {Id = int.MaxValue, Name = "Test2"}
            };

            documentTypesDto = new DocumentTypeDto
            {
                Id = 3,
                Name = "TestDTO"
            };

            mockRepository.Setup(m => m.GetAll()).Returns(fakeDocumentTypes.AsQueryable);
            mockRepository.Setup(m => m.Get(It.IsAny<int>()))
                .Returns<int>(id => fakeDocumentTypes.Single(t => t.Id == id));
            mockRepository.Setup(r => r.Create(It.IsAny<DocumentType>()))
                .Callback<DocumentType>(t => fakeDocumentTypes.Add(documentType = t));
            mockRepository.Setup(r => r.Update(It.IsAny<DocumentType>()))
                .Callback<DocumentType>(t => documentType = t);
            mockRepository.Setup(x => x.Delete(It.IsAny<int>()))
                .Callback<int>(id => fakeDocumentTypes.Remove(fakeDocumentTypes.Single(t => t.Id == id)));

            mockUnitOfWork.Setup(m => m.DocumentTypes).Returns(mockRepository.Object);

            documentTypesService = new DocumentTypesService(mockUnitOfWork.Object);
        }

        #endregion

        #region GetDocumentTypes

        [Fact]
        public void GetDocumentTypes_CheckNull_ShouldBeNotNull()
        {
            var actual = documentTypesService.GetDocumentTypes();

            Assert.NotNull(actual);
        }

        [Fact]
        public void GetDocumentTypes_CompareCount_ShouldBeEqual()
        {
            var documentTypes = documentTypesService.GetDocumentTypes();

            var expected = fakeDocumentTypes.Count;
            var actual = documentTypes.Count();

            Assert.Equal(expected, actual);
        }

        #endregion

        #region GetDocumentTypeById

        [Theory]
        [InlineData(5)]
        [InlineData(7)]
        public void GetDocumentTypeById_CheckName_ShouldBeTheSameAsInFake(int id)
        {
            var expected = fakeDocumentTypes.Single(t => t.Id == id).Name;
            var actual = documentTypesService.GetDocumentTypeById(id).Name;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetDocumentTypeById_Id_ShouldBeGreaterZero()
        {
            var expectedMessage = "id should be greater than zero (Parameter 'id')";

            var exception = Assert.Throws<ArgumentOutOfRangeException>(() => documentTypesService.GetDocumentTypeById(int.MinValue));

            Assert.Equal(expectedMessage, exception.Message);
        }

        #endregion

        #region Delete

        [Fact]
        public void Delete_DocumentType_CountShouldDecrease()
        {
            var expected = fakeDocumentTypes.Count - 1;

            documentTypesService.Delete(fakeDocumentTypes.First().Id);

            var actual = fakeDocumentTypes.Count;

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-100)]
        public void Delete_DocumentType_IdShouldBeGreaterZero(int id)
        {
            var expectedParamName = "id"; 

            var exception = Assert.Throws<ArgumentOutOfRangeException>(() => documentTypesService.Delete(id));

            Assert.Equal(expectedParamName, exception.ParamName);
        }

        #endregion

        #region Create

        [Fact]
        public void Create_DocumentType_ShouldBeNotNull()
        {
            documentTypesService.Create(documentTypesDto);

            var actual = documentType;

            Assert.NotNull(actual);
        }

        [Fact]
        public void Create_CheckNameInNewObject_ShouldBeTheSameAsInFake()
        {
            documentTypesService.Create(documentTypesDto);

            var expected = documentTypesDto.Name;
            var actual = documentType.Name;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Create_AddNewObject_CountShouldIncrease()
        {
            var expected = fakeDocumentTypes.Count + 1;

            documentTypesService.Create(documentTypesDto);

            var actual = fakeDocumentTypes.Count;

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(" \r \t \n")]
        public void Create_DocumentType_ShouldFailTypeNameIsEmpty(string name)
        {
            var expectedMessage = "Name is empty";
            documentTypesDto.Name = name;
            Action action = () => documentTypesService.Create(documentTypesDto);

            var exception = Assert.Throws<ArgumentException>(action);

            Assert.Equal(expectedMessage, exception.Message);
        }

        [Theory]
        [InlineData("q")]
        [InlineData("qwertyuioplkjhgfdsazxcvbnmetretertretertertsadasdsadsa")]
        public void Create_DocumentType_ShouldFailNameIsInvalid(string typeName)
        {
            var expectedMessage = $"Length {typeName.Length} of Name is invalid";
            documentTypesDto.Name = typeName;
            Action action = () => documentTypesService.Create(documentTypesDto);

            var exception = Assert.Throws<ArgumentException>(action);

            Assert.Equal(expectedMessage, exception.Message);
        }

        [Fact]
        public void Create_DocumentType_ShouldFailDtoShouldNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => documentTypesService.Create(null));
        }

        #endregion

        #region Update

        [Theory]
        [InlineData("  f   ")]
        [InlineData("\r f \t \n ")]
        public void Update_DocumentType_ShouldFailNameIsInvalid(string typeName)
        {
            documentTypesDto.Name = typeName;
            Action action = () => documentTypesService.Update(documentTypesDto);

            Assert.Throws<ArgumentException>(action);
        }

        [Fact]
        public void Update_DocumentType_NameShouldBeEqualDTOsName()
        {
            documentTypesDto.Name = "UpdatedName";
            var expected = documentTypesDto.Name;

            documentTypesService.Update(documentTypesDto);

            var actual = documentType.Name;

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
