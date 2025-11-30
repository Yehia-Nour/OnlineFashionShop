using ECommerce.ServicesAbstraction;
using ECommerce.Shared.DTOs.BasketDTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Presentation.Controllers
{
    public class BasketsController : ApiBaseController
    {
        private readonly IBasketService _basketService;

        public BasketsController(IBasketService basketService)
        {
            _basketService = basketService;
        }

        [HttpGet]
        public async Task<ActionResult<BasketDTO>> GetBasket(string id)
        {
            var result = await _basketService.GetBasketAsync(id);
            return HandleResult(result);
        }

        [HttpPost]
        public async Task<ActionResult<BasketDTO>> CreateOrUpdate(BasketDTO basket)
        {
            var basketResp = await _basketService.CreateOrUpdateAsync(basket);
            return Ok(basketResp);
        }

        [HttpDelete]
        public async Task<ActionResult<bool>> DeleteBasket(string id)
        {
            var result = await _basketService.DeleteBasketAsync(id);
            return Ok(result);
        }
    }
}
