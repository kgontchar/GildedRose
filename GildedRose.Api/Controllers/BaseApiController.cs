using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using GildedRose.BusinessLogic.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GildedRose.Api.Controllers
{
    public abstract class BaseApiController<TResource, TModel> : ControllerBase where TResource : class where TModel : class
    {
        private readonly IBaseService<TResource> _service;
        protected readonly IMapper Mapper;
        protected BaseApiController(IBaseService<TResource> baseService, IMapper mapper)
        {
            _service = baseService;
            Mapper = mapper;
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var resource = await _service.GetByIdAsync(id);
                var model = Mapper.Map<TModel>(resource);
                return Ok(model);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create([FromBody] TModel model)
        {
            try
            {
                var resource = await _service.CreateAsync(Mapper.Map<TResource>(model));
                var result = Mapper.Map<TModel>(resource);
                return StatusCode(201, result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var resources = await _service.GetAllAsync();
                var models = Mapper.Map<IEnumerable<TModel>>(resources);
                return Ok(models);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Update([FromBody] TModel model)
        {
            try
            {
                var resource = await _service.UpdateAsync(Mapper.Map<TResource>(model));
                var result = Mapper.Map<TModel>(resource);
                return StatusCode(200, result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _service.DeleteAsync(id);
                return StatusCode(200);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}