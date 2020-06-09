using System;
using System.Collections.Generic;
using System.Linq;
using ETicket.ApplicationServices.DTOs;
using ETicket.ApplicationServices.Services;
using ETicket.DataAccess.Domain.Entities;
using ETicket.DataAccess.Domain.Interfaces;
using Moq;
using Xunit;

namespace ETicket.ApplicationServicesTests.ServicesTests
{
    public class DocumentServiceTests
    {
        #region Private members

        private readonly DocumentService documentService;
        private readonly IList<Document> fakeDocuments;
        private readonly DocumentDto documentDto;
        private Document document;

        #endregion

        #region Constructor

        public DocumentServiceTests()
        {
            DateTime current = new DateTime();
            current = DateTime.UtcNow;
                       
            var mockRepository = new Mock<IRepository<Document, Guid>>();
            var mockUnitOfWork = new Mock<IUnitOfWork>();

            fakeDocuments = new List<Document>
            {
                new Document {Id = Guid.Parse("196d28f2-c6f0-4d70-a872-7a29d5dc79d3"), DocumentTypeId = 2,  DocumentType = new DocumentType {Id = 5, Name = "Test1"}, Number = "Test1", ExpirationDate = new DateTime(), IsValid = false},
                new Document {Id = Guid.Parse("396d28f2-c6f0-4d70-a872-7a29d5dc79d3"), DocumentTypeId = 3,  DocumentType = new DocumentType {Id = 7, Name = "Test2"}, Number = "Test2", ExpirationDate = new DateTime(), IsValid = false},
                new Document {Id = Guid.Parse("396d28f2-c8f0-4d70-a872-7a29d5dc99d3"), DocumentTypeId = 4,  DocumentType = new DocumentType {Id = 7, Name = "Test2"}, Number = "Test2", ExpirationDate = new DateTime(), IsValid = false}
            };

            documentDto = new DocumentDto
            {
                Id = new Guid(),
                DocumentTypeId = 1,
                DocumentTypeName = "DocumentTest",
                Number = "Test",
                ExpirationDate = current,
                IsValid = false
            };

            mockRepository.Setup(m => m.GetAll()).Returns(fakeDocuments.AsQueryable);
            mockRepository.Setup(m => m.Get(It.IsAny<Guid>()))
                .Returns<Guid>(id => fakeDocuments.Single(s => s.Id == id));
            mockRepository.Setup(r => r.Create(It.IsAny<Document>()))
                .Callback<Document>(s => fakeDocuments.Add(document = s));
            mockRepository.Setup(r => r.Update(It.IsAny<Document>()))
                .Callback<Document>(s => document = s);
            mockRepository.Setup(x => x.Delete(It.IsAny<Guid>()))
                .Callback<Guid>(id => fakeDocuments.Remove(fakeDocuments.Single(s => s.Id == id)));

            mockUnitOfWork.Setup(m => m.Documents).Returns(mockRepository.Object);

            documentService = new DocumentService(mockUnitOfWork.Object);
        }

        #endregion

        #region GetStations facts

        [Fact]
        public void GetAll_CheckNull_ShouldBeNotNull()
        {
            var actual = documentService.GetDocuments();

            Assert.NotNull(actual);
        }

        [Fact]
        public void GetAll_CompareCount_ShouldBeEqual()
        {
            var documents = documentService.GetDocuments();

            var expected = fakeDocuments.Count;
            var actual = documents.Count();

            Assert.Equal(expected, actual);
        }

        #endregion

        #region GetDocumentById facts

        [Theory]
        [InlineData("396d28f2-c6f0-4d70-a872-7a29d5dc79d3")]
        [InlineData("396d28f2-c8f0-4d70-a872-7a29d5dc99d3")]
        public void Get_CheckNameInReceivedObject_ShouldBeTheSameAsInFake(string guid)
        {
            Guid id = Guid.Parse(guid);

            var expected = fakeDocuments.Single(t => t.Id == id).Number;
            var actual = documentService.GetDocumentById(id).Number;

            Assert.Equal(expected, actual);
        }
        
        #endregion

        #region CreateStation facts

        [Fact]
        public void Create_Document_ShouldBeNotNull()
        {
            documentService.Create(documentDto);

            var actual = document;

            Assert.NotNull(actual);
        }

        [Fact]
        public void Create_CheckNameInNewObject_ShouldBeTheSameAsInFake()
        {
            documentService.Create(documentDto);

            var expected = documentDto.Number;
            var actual = document.Number;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Create_AddNewObject_CountShouldIncrease()
        {
            var expected = fakeDocuments.Count + 1;

            documentService.Create(documentDto);

            var actual = fakeDocuments.Count;

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(" \r \t \n")]
        public void Create_Document_ShouldFailNameIsEmpty(string name)
        {
            documentDto.Number = name;

            var exception = Assert.Throws<ArgumentException>(() => documentService.Create(documentDto));

            var expectedMessage = "Number is empty";
            Assert.Equal(expectedMessage, exception.Message);
        }

        [Theory]
        [InlineData("hhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhh")]
        public void Create_Document_ShouldFailNameIsInvalid(string name)
        {
            documentDto.Number = name;

            var exception = Assert.Throws<ArgumentException>(() => documentService.Create(documentDto));

            var expectedMessage = "Length 60 of Number is invalid";
            Assert.Equal(expectedMessage, exception.Message);
        }

        #endregion

        #region UpdateStation facts

        [Theory]
        [InlineData("     ")]
        [InlineData("\r  \t \n ")]
        public void Update_Document_ShouldFailNameIsInvalid(string name)
        {
            documentDto.Number = name;

            Assert.Throws<ArgumentException>(() => documentService.Update(documentDto));
        }

        [Fact]
        public void Update_Document_NameShouldBeEqualDTOsName()
        {
            documentDto.Number = "NewName";

            var expected = documentDto.Number;

            documentService.Update(documentDto);

            var actual = document.Number;

            Assert.Equal(expected, actual);
        }

        #endregion

        #region DeleteStation facts

        [Fact]
        public void Delete_Document_CountShouldDecrease()
        {
            var expected = fakeDocuments.Count - 1;

            documentService.Delete(fakeDocuments.First().Id);

            var actual = fakeDocuments.Count;

            Assert.Equal(expected, actual);
        }
        #endregion
    }
}
