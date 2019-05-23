using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GildedRose.BusinessLogic.DTOs;
using GildedRose.BusinessLogic.Exceptions;
using GildedRose.BusinessLogic.Interfaces.Services;
using GildedRose.BusinessLogic.Services;
using GildedRose.DataAccess.DatabaseContext;
using GildedRose.DataAccess.Entities;
using GildedRose.DataAccess.Interfaces.Repositories;
using GildedRose.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;

namespace GildedRose.BusinessLogic.Tests.ServiceTests
{
    public class ItemServiceTests
    {
        private const string CANT_FIND_ITEM_EXCEPTION_MESSAGE = "Can't find entity with those id";
        private const string VALUE_IS_ZERO_OR_LESS_EXCEPTION_MESSAGE = "Value can't be zero or less";
        private IItemService _itemService;
        private Mock<IMapper> _mapper;

        [SetUp]
        public void Setup()
        {
            _mapper = new Mock<IMapper>();
        }

        #region HappyCases
        [Test]
        public void Get_item_should_return_item_if_item_existis()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<GildedRoseContext>()
                .UseInMemoryDatabase(nameof(Get_item_should_return_item_if_item_existis))
                .Options;

            using (var context = new GildedRoseContext(options))
            {
                context.Items.Add(GetTestEntity());
                context.SaveChanges();
            }

            var expected = GetTestDto();

            _mapper.Setup(mapper => mapper.Map<ItemDto>(It.IsAny<ItemEntity>()))
                .Returns(GetTestDto());

            //Act
            ItemDto result;
            using (var context = new GildedRoseContext(options))
            {
                var itemRepository = new ItemRepository(context);
                _itemService = new ItemService(itemRepository, _mapper.Object);
                result = _itemService.GetByIdAsync(expected.Id).Result;
            }

            //Assert
            using (var context = new GildedRoseContext(options))
            {
                Assert.AreEqual(1, context.Items.Count());
                Assert.NotNull(result);
                Assert.AreEqual(expected.Id, context.Items.Single().Id);
                Assert.AreEqual(expected.Name, context.Items.Single().Name);
            }
        }

        [Test]
        public void Get_all_should_return_all_items_from_dbcontext()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<GildedRoseContext>()
                .UseInMemoryDatabase(nameof(Get_all_should_return_all_items_from_dbcontext))
                .Options;


            using (var context = new GildedRoseContext(options))
            {
                context.Items.AddRange(GetTestEntityItems());
                context.SaveChanges();
            }

            _mapper.Setup(mapper => mapper.Map<IEnumerable<ItemDto>>(It.IsAny<IEnumerable<ItemEntity>>()))
                .Returns(GetTestDtoItems());

            //Act
            IEnumerable<ItemDto> items;
            using (var context = new GildedRoseContext(options))
            {
                var itemRepository = new ItemRepository(context);
                _itemService = new ItemService(itemRepository, _mapper.Object);
                items = _itemService.GetAllAsync().Result.ToList();
            }

            //Assert
            using (var context = new GildedRoseContext(options))
            {
                Assert.NotNull(items);
                Assert.AreEqual(context.Items.Count(), items.Count());
                Assert.AreEqual(context.Items.First().Name, items.First().Name);
            }
        }

        [Test]
        public void Get_all_should_return_empty_collection_if_dbcontext_returns_empty_collection()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<GildedRoseContext>()
                .UseInMemoryDatabase(nameof(Get_all_should_return_empty_collection_if_dbcontext_returns_empty_collection))
                .Options;

            //Act
            IEnumerable<ItemDto> items;
            using (var context = new GildedRoseContext(options))
            {
                var itemRepository = new ItemRepository(context);
                _itemService = new ItemService(itemRepository, _mapper.Object);
                items = _itemService.GetAllAsync().Result.ToList();
            }

            //Assert
            using (var context = new GildedRoseContext(options))
            {
                Assert.NotNull(items);
                Assert.AreEqual(context.Items.Count(), items.Count());
            }
        }

        [Test]
        public void Create_item_should_just_create_new_item()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<GildedRoseContext>()
                .UseInMemoryDatabase(nameof(Create_item_should_just_create_new_item))
                .Options;

            var itemDtoToCreate = GetTestDto();

            var itemEntityToCreate = GetTestEntity();

            _mapper.Setup(mapper => mapper.Map<ItemEntity>(itemDtoToCreate)).Returns(itemEntityToCreate);

            //Act
            using (var context = new GildedRoseContext(options))
            {
                var itemRepository = new ItemRepository(context);
                _itemService = new ItemService(itemRepository, _mapper.Object);
                _itemService.CreateAsync(itemDtoToCreate);
            }

            //Assert
            using (var context = new GildedRoseContext(options))
            {
                Assert.AreEqual(1, context.Items.Count());
                Assert.AreEqual(itemDtoToCreate.Name, context.Items.Single().Name);
                Assert.AreEqual(itemDtoToCreate.Price, context.Items.Single().Price);
            }
        }

        [Test]
        public void Update_item_should_not_create_new_entity_and_dont_throw_exception_if_entity_is_exists()
        {
            //Arrange
            var newPriceOfItem = 80000;
            var newNameOfItem = "MyFavouriteItem";

            var options = new DbContextOptionsBuilder<GildedRoseContext>()
                .UseInMemoryDatabase(nameof(Update_item_should_not_create_new_entity_and_dont_throw_exception_if_entity_is_exists))
                .Options;

            using (var context = new GildedRoseContext(options))
            {
                context.AddRange(GetTestEntityItems());
                context.SaveChanges();
            }

            var itemDtoToUpdate = GetTestDto();
            itemDtoToUpdate.Price = newPriceOfItem;
            itemDtoToUpdate.Name = newNameOfItem;

            var itemEntityToUpdate = GetTestEntity();
            itemEntityToUpdate.Price = newPriceOfItem;
            itemEntityToUpdate.Name = newNameOfItem;

            _mapper.Setup(mapper => mapper.Map<ItemEntity>(itemDtoToUpdate)).Returns(itemEntityToUpdate);

            //Act
            using (var context = new GildedRoseContext(options))
            {
                var itemRepository = new ItemRepository(context);
                _itemService = new ItemService(itemRepository, _mapper.Object);
                _itemService.UpdateAsync(itemDtoToUpdate);
            }

            //Assert
            using (var context = new GildedRoseContext(options))
            {
                Assert.AreEqual(5, context.Items.Count());
                Assert.NotNull(context.Items.FirstOrDefault(x => x.Id == itemDtoToUpdate.Id));
                Assert.AreEqual(newPriceOfItem, context.Items.First(x => x.Id == itemDtoToUpdate.Id).Price);
            }
        }

        [Test]
        public void Delete_item_should_not_throw_exception_if_entity_is_exists()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<GildedRoseContext>()
                .UseInMemoryDatabase(nameof(Delete_item_should_not_throw_exception_if_entity_is_exists))
                .Options;

            var entities = GetTestEntityItems();
            using (var context = new GildedRoseContext(options))
            {
                context.AddRange(entities);
                context.SaveChanges();
            }

            //Act
            using (var context = new GildedRoseContext(options))
            {
                var itemRepository = new ItemRepository(context);
                _itemService = new ItemService(itemRepository, _mapper.Object);
                _itemService.DeleteAsync(entities.First().Id);
            }

            //Assert
            using (var context = new GildedRoseContext(options))
            {
                Assert.AreEqual(entities.Count - 1, context.Items.Count());
                Assert.IsNull(context.Items.FirstOrDefault(x => x.Id == entities.First().Id));
            }
        }

        [TestCase(350, 99)]
        [TestCase(1000, 50)]
        [TestCase(39, 12)]
        [TestCase(148, 50)]
        [TestCase(32564, 6545)]
        public void Buy_item_should_update_itemsLeft_in_context(int itemsLeft, int countOfItemsToBuy)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<GildedRoseContext>()
                .UseInMemoryDatabase(nameof(Buy_item_should_update_itemsLeft_in_context) + itemsLeft)
                .Options;

            var item = GetTestEntity();
            item.ItemsLeft = itemsLeft;
            var dto = GetTestDto();
            dto.ItemsLeft = itemsLeft;

            using (var context = new GildedRoseContext(options))
            {
                context.Items.Add(item);
                context.SaveChanges();
            }

            _mapper.Setup(mapper => mapper.Map<ItemEntity>(It.IsAny<ItemDto>()))
                .Returns(() =>
                {
                    item.ItemsLeft -= countOfItemsToBuy;
                    return item;
                });
            _mapper.Setup(mapper => mapper.Map<ItemDto>(It.IsAny<ItemEntity>()))
                .Returns(dto);

            //Assert
            using (var context = new GildedRoseContext(options))
            {
                var itemRepository = new ItemRepository(context);
                _itemService = new ItemService(itemRepository, _mapper.Object);
                var result = _itemService.BuyItem(item.Id, countOfItemsToBuy).Result;
                Assert.AreEqual(itemsLeft - countOfItemsToBuy, context.Items.Single().ItemsLeft);
            }
        }
        #endregion

        #region UnhappyCases
        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(-10)]
        [TestCase(-100)]
        [TestCase(-1000)]
        public void Get_item_should_throw_exception_if_id_is_zero_or_less(int id)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<GildedRoseContext>()
                .UseInMemoryDatabase(nameof(Get_item_should_throw_exception_if_id_is_zero_or_less) + id)
                .Options;

            using (var context = new GildedRoseContext(options))
            {
                context.Items.AddRange(GetTestEntityItems());
                context.SaveChanges();
            }

            //Assert
            using (var context = new GildedRoseContext(options))
            {
                var itemRepository = new ItemRepository(context);
                _itemService = new ItemService(itemRepository, _mapper.Object);
                Assert.ThrowsAsync<ArgumentException>(async () => await _itemService.GetByIdAsync(id), VALUE_IS_ZERO_OR_LESS_EXCEPTION_MESSAGE);
            }
        }

        [TestCase(25)]
        [TestCase(35)]
        [TestCase(45)]
        [TestCase(3000)]
        [TestCase(10000)]
        public void Get_item_should_throw_exception_if_repository_doesnt_found_entity(int id)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<GildedRoseContext>()
                .UseInMemoryDatabase(nameof(Get_item_should_throw_exception_if_id_is_zero_or_less) + id)
                .Options;

            using (var context = new GildedRoseContext(options))
            {
                context.Items.AddRange(GetTestEntityItems());
                context.SaveChanges();
            }

            //Assert
            using (var context = new GildedRoseContext(options))
            {
                var itemRepository = new ItemRepository(context);
                _itemService = new ItemService(itemRepository, _mapper.Object);
                Assert.ThrowsAsync<ItemNotFoundException>(async () => await _itemService.GetByIdAsync(id), CANT_FIND_ITEM_EXCEPTION_MESSAGE);
            }
        }

        [Test]
        public void Create_item_should_throw_exception_if_item_to_create_was_null()
        {
            //Assert
            var itemRepository = new Mock<IItemRepository>();
            _itemService = new ItemService(itemRepository.Object, _mapper.Object);
            Assert.ThrowsAsync<ArgumentNullException>(async () => await _itemService.CreateAsync(null), "Item to create can't be null");
        }

        [Test]
        public void Update_item_should_throw_exception_if_item_to_create_was_null()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<GildedRoseContext>()
                .UseInMemoryDatabase(nameof(Update_item_should_throw_exception_if_item_to_create_was_null))
                .Options;

            using (var context = new GildedRoseContext(options))
            {
                context.AddRange(GetTestEntityItems());
                context.SaveChanges();
            }

            //Assert
            using (var context = new GildedRoseContext(options))
            {
                var itemRepository = new ItemRepository(context);
                _itemService = new ItemService(itemRepository, _mapper.Object);
                Assert.ThrowsAsync<ArgumentNullException>(async () => await _itemService.UpdateAsync(null), "Item to update can't be null");
            }
        }

        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(-10)]
        [TestCase(-100)]
        [TestCase(-1000)]
        public void Delete_item_should_throw_exception_if_id_is_zero_or_less(int id)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<GildedRoseContext>()
                .UseInMemoryDatabase(nameof(Delete_item_should_throw_exception_if_id_is_zero_or_less) + id)
                .Options;

            using (var context = new GildedRoseContext(options))
            {
                context.AddRange(GetTestEntityItems());
                context.SaveChanges();
            }

            //Assert
            using (var context = new GildedRoseContext(options))
            {
                var itemRepository = new ItemRepository(context);
                _itemService = new ItemService(itemRepository, _mapper.Object);
                Assert.ThrowsAsync<ArgumentException>(async () => await _itemService.DeleteAsync(id));
            }
        }

        [TestCase(25)]
        [TestCase(35)]
        [TestCase(45)]
        [TestCase(3000)]
        [TestCase(10000)]
        public void Delete_item_should_throw_exception_if_repository_doesnt_found_entity(int id)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<GildedRoseContext>()
                .UseInMemoryDatabase(nameof(Delete_item_should_throw_exception_if_repository_doesnt_found_entity) + id)
                .Options;

            using (var context = new GildedRoseContext(options))
            {
                context.Items.AddRange(GetTestEntityItems());
                context.SaveChanges();
            }

            //Assert
            using (var context = new GildedRoseContext(options))
            {
                var itemRepository = new ItemRepository(context);
                _itemService = new ItemService(itemRepository, _mapper.Object);
                Assert.ThrowsAsync<ArgumentNullException>(async () => await _itemService.DeleteAsync(id), VALUE_IS_ZERO_OR_LESS_EXCEPTION_MESSAGE);
            }
        }

        [TestCase(25)]
        [TestCase(35)]
        [TestCase(45)]
        [TestCase(3000)]
        [TestCase(10000)]
        public void Buy_item_should_throw_exception_if_repository_doesnt_found_entity(int id)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<GildedRoseContext>()
                .UseInMemoryDatabase(nameof(Buy_item_should_throw_exception_if_repository_doesnt_found_entity) + id)
                .Options;

            using (var context = new GildedRoseContext(options))
            {
                context.Items.AddRange(GetTestEntityItems());
                context.SaveChanges();
            }

            //Assert
            using (var context = new GildedRoseContext(options))
            {
                var itemRepository = new ItemRepository(context);
                _itemService = new ItemService(itemRepository, _mapper.Object);
                Assert.ThrowsAsync<ItemNotFoundException>(async () => await _itemService.BuyItem(id, 100), CANT_FIND_ITEM_EXCEPTION_MESSAGE);
            }
        }

        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(-10)]
        [TestCase(-100)]
        [TestCase(-1000)]
        public void Buy_item_should_throw_exception_if_items_left_zero_or_less(int itemsLeft)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<GildedRoseContext>()
                .UseInMemoryDatabase(nameof(Buy_item_should_throw_exception_if_items_left_zero_or_less) + itemsLeft)
                .Options;

            var item = GetTestEntity();
            item.ItemsLeft = itemsLeft;
            var dto = GetTestDto();
            _mapper.Setup(mapper => mapper.Map<ItemDto>(It.IsAny<ItemEntity>())).Returns(dto);

            using (var context = new GildedRoseContext(options))
            {
                context.Items.Add(item);
                context.SaveChanges();
            }

            //Assert
            using (var context = new GildedRoseContext(options))
            {
                var itemRepository = new ItemRepository(context);
                _itemService = new ItemService(itemRepository, _mapper.Object);
                Assert.ThrowsAsync<ArgumentException>(async () => await _itemService.BuyItem(item.Id, itemsLeft), CANT_FIND_ITEM_EXCEPTION_MESSAGE);
            }
        }

        [TestCase(25)]
        [TestCase(35)]
        [TestCase(45)]
        [TestCase(3000)]
        [TestCase(10000)]
        public void Buy_item_should_throw_exception_if_user_wants_to_by_items_more_then_left(int countOfitemsToBuy)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<GildedRoseContext>()
                .UseInMemoryDatabase(nameof(Buy_item_should_throw_exception_if_user_wants_to_by_items_more_then_left) + countOfitemsToBuy)
                .Options;

            var item = GetTestEntity();
            item.ItemsLeft = 10;
            var dto = GetTestDto();
            dto.ItemsLeft = 10;
            _mapper.Setup(mapper => mapper.Map<ItemDto>(It.IsAny<ItemEntity>())).Returns(dto);
            using (var context = new GildedRoseContext(options))
            {
                context.Items.Add(item);
                context.SaveChanges();
            }

            //Assert
            using (var context = new GildedRoseContext(options))
            {
                var itemRepository = new ItemRepository(context);
                _itemService = new ItemService(itemRepository, _mapper.Object);
                Assert.ThrowsAsync<ThereAreNoItemsOnStockException>(async () => await _itemService.BuyItem(item.Id, countOfitemsToBuy), CANT_FIND_ITEM_EXCEPTION_MESSAGE);
            }
        }
        #endregion

        #region GetTestItems
        private List<ItemEntity> GetTestEntityItems()
        {
            var testEntities = new List<ItemEntity>
            {
                new ItemEntity { Id = 1, Name = "Item1", Price = 10000 },
                new ItemEntity { Id = 2, Name = "Item2", Price = 20000 },
                new ItemEntity { Id = 3, Name = "Item3", Price = 30000 },
                new ItemEntity { Id = 4, Name = "Item4", Price = 40000 },
                new ItemEntity { Id = 5, Name = "Item5", Price = 50000 }
            };
            return testEntities;
        }

        private List<ItemDto> GetTestDtoItems()
        {
            var testDtos = new List<ItemDto>
            {
                new ItemDto { Id = 1, Name = "Item1", Price = 10000, ItemsLeft = 100 },
                new ItemDto { Id = 2, Name = "Item2", Price = 20000, ItemsLeft = 100 },
                new ItemDto { Id = 3, Name = "Item3", Price = 30000, ItemsLeft = 100 },
                new ItemDto { Id = 4, Name = "Item4", Price = 40000, ItemsLeft = 100 },
                new ItemDto { Id = 5, Name = "Item5", Price = 50000, ItemsLeft = 100 }
            };
            return testDtos;
        }

        private ItemEntity GetTestEntity()
        {
            return new ItemEntity { Id = 1, Name = "Item1", Price = 10000, ItemsLeft = 100 };
        }

        private ItemDto GetTestDto()
        {
            return new ItemDto { Id = 1, Name = "Item1", Price = 10000, ItemsLeft = 100 };
        }
        #endregion
    }
}
