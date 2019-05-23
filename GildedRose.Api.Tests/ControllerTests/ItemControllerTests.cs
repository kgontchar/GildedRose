using System;
using System.Collections.Generic;
using AutoMapper;
using GildedRose.Api.Controllers;
using GildedRose.Api.Models;
using GildedRose.BusinessLogic.DTOs;
using GildedRose.BusinessLogic.Exceptions;
using GildedRose.BusinessLogic.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace GildedRose.Api.Tests.ControllerTests
{
    public class ItemControllerTests
    {
        private Mock<IItemService> _itemService;
        private Mock<IMapper> _mapper;

        [SetUp]
        public void Setup()
        {
            _mapper = new Mock<IMapper>();
            _itemService = new Mock<IItemService>();
        }

        #region HappyCases
        [Test]
        public void Get_should_return_ok_if_collection_empty()
        {
            //Arrange
            _itemService.Setup(service => service.GetAllAsync()).ReturnsAsync(new List<ItemDto>());
            var controller = new ItemController(_itemService.Object, _mapper.Object);

            //Act
            var answer = controller.GetAll().Result as ObjectResult;

            //Assert
            Assert.IsNotNull(answer);
            Assert.AreEqual(200, answer.StatusCode);
        }

        [Test]
        public void Get_should_return_ok_and_collection_of_models_if_collection_not_empty()
        {
            //Arrange
            var items = GetTestDtoItems();
            var models = GetTestModelItems();
            _itemService.Setup(service => service.GetAllAsync()).ReturnsAsync(items);
            _mapper.Setup(mapper => mapper.Map<IEnumerable<ItemModel>>(items)).Returns(models);
            var controller = new ItemController(_itemService.Object, _mapper.Object);

            //Act
            var answer = controller.GetAll().Result as ObjectResult;

            //Assert
            Assert.IsNotNull(answer);
            Assert.AreEqual(200, answer.StatusCode);
            var modelsFromAnswer = answer.Value as List<ItemModel>;
            Assert.IsNotNull(modelsFromAnswer);
            Assert.AreEqual(items.Count, modelsFromAnswer.Count);
        }

        [Test]
        public void Create_should_return_201_if_created()
        {
            //Arrange
            var dto = GetTestDto();
            var model = GetTestModel();

            _itemService.Setup(service => service.CreateAsync(It.IsAny<ItemDto>())).ReturnsAsync(dto);
            _mapper.Setup(mapper => mapper.Map<ItemDto>(It.IsAny<ItemModel>())).Returns(dto);
            _mapper.Setup(mapper => mapper.Map<ItemModel>(It.IsAny<ItemDto>())).Returns(model);

            var controller = new ItemController(_itemService.Object, _mapper.Object);

            //Act
            var answer = controller.Create(model).Result as ObjectResult;

            //Assert
            Assert.IsNotNull(answer);
            Assert.AreEqual(201, answer.StatusCode);
        }

        [Test]
        public void Update_should_return_200_if_updated()
        {
            //Arrange
            var dto = GetTestDto();
            var model = GetTestModel();

            _itemService.Setup(service => service.UpdateAsync(It.IsAny<ItemDto>())).ReturnsAsync(dto);
            _mapper.Setup(mapper => mapper.Map<ItemDto>(It.IsAny<ItemModel>())).Returns(dto);
            _mapper.Setup(mapper => mapper.Map<ItemModel>(It.IsAny<ItemDto>())).Returns(model);

            var controller = new ItemController(_itemService.Object, _mapper.Object);

            //Act
            var answer = controller.Update(model).Result as ObjectResult;

            //Assert
            Assert.IsNotNull(answer);
            Assert.AreEqual(200, answer.StatusCode);
        }

        [Test]
        public void Delete_should_return_status_code_200_if_delete_not_thrown()
        {
            //Arrange
            var controller = new ItemController(_itemService.Object, _mapper.Object);

            //Act
            var answer = controller.Delete(10).Result as StatusCodeResult;

            //Assert
            Assert.IsNotNull(answer);
            Assert.AreEqual(200, answer.StatusCode);
        }

        [Test]
        public void Buy_should_return_status_code_200_and_true_if_buying_was_succesfully()
        {
            //Arrange
            _itemService.Setup(service => service.BuyItem(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(true);
            var controller = new ItemController(_itemService.Object, _mapper.Object);

            //Act
            var answer = controller.BuyItem(GetTestModel(), 100).Result as ObjectResult;

            //Assert
            Assert.IsNotNull(answer);
            Assert.AreEqual(200, answer.StatusCode);
            Assert.AreEqual(true, answer.Value);
        }
        #endregion

        #region UnhappyCases
        [Test]
        public void Get_should_return_return_bad_request_if_argument_exception()
        {
            //Arrange
            _itemService.Setup(service => service.GetByIdAsync(It.IsAny<int>())).Throws<ArgumentException>();
            var controller = new ItemController(_itemService.Object, _mapper.Object);

            //Act
            var answer = controller.Get(10).Result as BadRequestObjectResult;

            //Assert
            Assert.IsNotNull(answer);
            Assert.AreEqual(400, answer.StatusCode);
        }

        [Test]
        public void Get_should_return_return_status_code_500_if_exception()
        {
            //Arrange
            _itemService.Setup(service => service.GetByIdAsync(It.IsAny<int>())).Throws<Exception>();
            var controller = new ItemController(_itemService.Object, _mapper.Object);

            //Act
            var answer = controller.Get(10).Result as ObjectResult;

            //Assert
            Assert.IsNotNull(answer);
            Assert.AreEqual(500, answer.StatusCode);
        }

        [Test]
        public void Update_should_return_return_bad_request_if_argument_exception()
        {
            //Arrange
            _itemService.Setup(service => service.UpdateAsync(null)).Throws<ArgumentException>();
            var controller = new ItemController(_itemService.Object, _mapper.Object);

            //Act
            var answer = controller.Update(null).Result as BadRequestObjectResult;

            //Assert
            Assert.IsNotNull(answer);
            Assert.AreEqual(400, answer.StatusCode);
        }

        [Test]
        public void Update_should_return_return_status_code_500_if_exception()
        {
            //Arrange
            _itemService.Setup(service => service.UpdateAsync(null)).Throws<Exception>();
            var controller = new ItemController(_itemService.Object, _mapper.Object);

            //Act
            var answer = controller.Update(null).Result as ObjectResult;

            //Assert
            Assert.IsNotNull(answer);
            Assert.AreEqual(500, answer.StatusCode);
        }

        [Test]
        public void Create_should_return_return_bad_request_if_argument_exception()
        {
            //Arrange
            _itemService.Setup(service => service.CreateAsync(null)).Throws<ArgumentException>();
            var controller = new ItemController(_itemService.Object, _mapper.Object);

            //Act
            var answer = controller.Create(null).Result as BadRequestObjectResult;

            //Assert
            Assert.IsNotNull(answer);
            Assert.AreEqual(400, answer.StatusCode);
        }

        [Test]
        public void Create_should_return_return_status_code_500_if_exception()
        {
            //Arrange
            _itemService.Setup(service => service.CreateAsync(null)).Throws<Exception>();
            var controller = new ItemController(_itemService.Object, _mapper.Object);

            //Act
            var answer = controller.Create(null).Result as ObjectResult;

            //Assert
            Assert.IsNotNull(answer);
            Assert.AreEqual(500, answer.StatusCode);
        }

        [Test]
        public void Delete_should_return_return_bad_request_if_argument_exception()
        {
            //Arrange
            _itemService.Setup(service => service.CreateAsync(null)).Throws<ArgumentException>();
            var controller = new ItemController(_itemService.Object, _mapper.Object);

            //Act
            var answer = controller.Create(null).Result as BadRequestObjectResult;

            //Assert
            Assert.IsNotNull(answer);
            Assert.AreEqual(400, answer.StatusCode);
        }

        [Test]
        public void Delete_should_return_return_status_code_500_if_exception()
        {
            //Arrange
            _itemService.Setup(service => service.CreateAsync(null)).Throws<Exception>();
            var controller = new ItemController(_itemService.Object, _mapper.Object);

            //Act
            var answer = controller.Create(null).Result as ObjectResult;

            //Assert
            Assert.IsNotNull(answer);
            Assert.AreEqual(500, answer.StatusCode);
        }

        [Test]
        public void Buy_item_should_return_bad_request_if_service_throw_argument_exception()
        {
            //Arrange
            _itemService.Setup(service => service.BuyItem(It.IsAny<int>(), It.IsAny<int>())).Throws<ArgumentException>();
            var controller = new ItemController(_itemService.Object, _mapper.Object);

            //Act
            var answer = controller.BuyItem(GetTestModel(), 100).Result as BadRequestObjectResult;

            //Assert
            Assert.IsNotNull(answer);
            Assert.AreEqual(400, answer.StatusCode);
        }

        [Test]
        public void Buy_item_should_return_return_status_code_200_if_service_throw_therearenoitemsexception()
        {
            //Arrange
            _itemService.Setup(service => service.BuyItem(It.IsAny<int>(),It.IsAny<int>())).Throws<ThereAreNoItemsOnStockException>();
            var controller = new ItemController(_itemService.Object, _mapper.Object);

            //Act
            var answer = controller.BuyItem(GetTestModel(), 100).Result as ObjectResult;

            //Assert
            Assert.IsNotNull(answer);
            Assert.AreEqual(200, answer.StatusCode);
        }

        [Test]
        public void Buy_item_should_return_return_status_code_500_if_service_throw_base_exception()
        {
            //Arrange
            _itemService.Setup(service => service.BuyItem(It.IsAny<int>(), It.IsAny<int>())).Throws<Exception>();
            var controller = new ItemController(_itemService.Object, _mapper.Object);

            //Act
            var answer = controller.BuyItem(GetTestModel(), 100).Result as ObjectResult;

            //Assert
            Assert.IsNotNull(answer);
            Assert.AreEqual(500, answer.StatusCode);
        }
        #endregion

        #region GetTestItems
        private List<ItemModel> GetTestModelItems()
        {
            var testModel = new List<ItemModel>
            {
                new ItemModel { Id = 1, Name = "Item1", Price = 10000 },
                new ItemModel { Id = 2, Name = "Item2", Price = 20000 },
                new ItemModel { Id = 3, Name = "Item3", Price = 30000 },
                new ItemModel { Id = 4, Name = "Item4", Price = 40000 },
                new ItemModel { Id = 5, Name = "Item5", Price = 50000 }
            };
            return testModel;
        }

        private List<ItemDto> GetTestDtoItems()
        {
            var testDtos = new List<ItemDto>
            {
                new ItemDto { Id = 1, Name = "Item1", Price = 10000 },
                new ItemDto { Id = 2, Name = "Item2", Price = 20000 },
                new ItemDto { Id = 3, Name = "Item3", Price = 30000 },
                new ItemDto { Id = 4, Name = "Item4", Price = 40000 },
                new ItemDto { Id = 5, Name = "Item5", Price = 50000 }
            };
            return testDtos;
        }

        private ItemModel GetTestModel()
        {
            return new ItemModel { Id = 1, Name = "Item1", Price = 10000 };
        }

        private ItemDto GetTestDto()
        {
            return new ItemDto { Id = 1, Name = "Item1", Price = 10000 };
        }
        #endregion
    }
}
