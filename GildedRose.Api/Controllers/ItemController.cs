using System;
using System.Threading.Tasks;
using AutoMapper;
using GildedRose.Api.Models;
using GildedRose.BusinessLogic.DTOs;
using GildedRose.BusinessLogic.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GildedRose.BusinessLogic.Exceptions;

namespace GildedRose.Api.Controllers
{
    [Route("api/items")]
    [ApiController]
    public class ItemController :  BaseApiController<ItemDto, ItemModel>
    {
        private readonly IItemService _itemService;
        public ItemController(IItemService itemService, IMapper mapper) : base(itemService, mapper)
        {
            _itemService = itemService;
        }

        [HttpPut("buy/{countOfItemsToBuy}")]
        [Authorize]
        public async Task<IActionResult> BuyItem([FromBody] ItemModel model, int countOfItemsToBuy)
        {
            try
            {
                var result = await _itemService.BuyItem(model.Id, countOfItemsToBuy);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ThereAreNoItemsOnStockException ex)
            {
                return Ok(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
