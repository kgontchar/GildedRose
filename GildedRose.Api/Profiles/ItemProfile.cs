using AutoMapper;
using GildedRose.Api.Models;
using GildedRose.BusinessLogic.DTOs;
using GildedRose.DataAccess.Entities;

namespace GildedRose.Api.Profiles
{
    public class ItemProfile : Profile
    {
        public ItemProfile()
        {
            CreateMap<ItemEntity, ItemDto>();
            CreateMap<ItemDto, ItemEntity>();

            CreateMap<ItemDto, ItemModel>();
            CreateMap<ItemModel, ItemDto>();
        }
    }
}