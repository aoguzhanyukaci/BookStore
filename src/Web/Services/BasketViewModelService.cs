using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Web.Interfaces;

namespace Web.Services
{
    public class BasketViewModelService : IBasketViewModelService
    {
        private readonly IHttpContextAccessor _httpContextAccesor;
        private readonly IAsyncRepository<Basket> _basketRepository;

        public BasketViewModelService(IHttpContextAccessor httpContextAccesor, IAsyncRepository<Basket> basketRepository)
        {
            _httpContextAccesor = httpContextAccesor;
            _basketRepository = basketRepository;
        }
        public async Task<int> GetOrCreateBasketIdAsync()
        {
            var buyerId = GetOrCreateBuyerId();
            var spec = new BasketSpecification(buyerId);
            var basket = await _basketRepository.FirstOrDefaultAsync(spec);

            if (basket == null)
            {
                basket = new Basket() { BuyerId = buyerId };
                basket = await _basketRepository.AddAsync(basket);
            }
            return basket.Id;
        }

        public string GetOrCreateBuyerId()
        {
            var context = _httpContextAccesor.HttpContext;
            var user = context.User;

            // return user id if user is logged in
            if (user.Identity.IsAuthenticated)
            {
                return user.FindFirstValue(ClaimTypes.NameIdentifier);
            }
            else
            {
                // return anonymus user id if a basket cookie exists
                if (context.Request.Cookies.ContainsKey(Constants.BASKET_COOKIE_NAME))
                {
                    return context.Request.Cookies[Constants.BASKET_COOKIE_NAME];
                }
                // create and return an annymous user id
                else
                {
                    string newBuyerId = Guid.NewGuid().ToString();
                    var cookieOptions = new CookieOptions()
                    {
                        IsEssential = true,
                        Expires = DateTime.Now.AddDays(10)
                    };

                    context.Response.Cookies.Append(Constants.BASKET_COOKIE_NAME, newBuyerId, cookieOptions);
                    return newBuyerId;
                }
            }
        }
    }
}
