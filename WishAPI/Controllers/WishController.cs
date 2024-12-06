using Application.interfases;
using Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presistence;

namespace WishAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class WishController : ControllerBase
    {
        private readonly IWishService _wishService;
        private readonly IWishRepository _wishRepository;

        public WishController(IWishRepository wishRepository, IWishService wishService)
        {
            _wishRepository = wishRepository;
            _wishService = wishService;
        }

        [Route("all")]
        [HttpGet]
        public async Task<IResult> GetAllWishsAsync()
        {
            var wishs = await _wishRepository.GetWishsAsync();
            if (wishs == null) return Results.BadRequest();
            return Results.Json(wishs);
        }

        [Authorize]
        [HttpPost]
        public async Task<IResult> AddWishAsync([FromBody] WishList wishList)
        {
            var token = Request.Headers["Authorization"].ToString();
            token = token.Substring(7);
            var userIdStr = _wishService.getUserIdFromToken(token);
            Console.WriteLine(token);
            Console.WriteLine(userIdStr);
            var wish = await _wishService.CreateNewWishAsync(token, wishList);
            if (wish == null) return Results.BadRequest();
            return Results.Json(wish);
        }

        [Route("update/wish")]
        [Authorize]
        [HttpPut]
        public async Task<IResult> UpdateWishAsync([FromBody] WishModel wish)
        {
            var newWish = await _wishRepository.UpdateWishAsync(wish);
            if (newWish == null) return Results.BadRequest();
            return Results.Json(newWish);
        }

        //[Route("update/name")]
        //[HttpPut]
        //public async Task<IResult> UpdateNameAsync([FromBody] string token)
        //{
        //    string name = "";
        //    var userIdStr = _wishService.getUserIdFromToken(token);
        //    Guid.TryParse(userIdStr, out var userId);
        //    var oldWish = await _wishRepository.GetWishByUserIdAsync(userId);
        //    var newWish = new WishModel(userId, name, oldWish.Present, oldWish.Price);
        //    var wish = await _wishRepository.UpdateWishAsync(newWish);
        //    if (wish == null) return Results.BadRequest();
        //    return Results.Json(wish);
        //}
        //todo: update for name
        [Authorize]
        [HttpDelete]
        public async Task<IResult> RemoveWishAsync()
        {
            var token = Request.Headers["Authorization"].ToString();
            token = token.Substring(7);
            var userIdStr = _wishService.getUserIdFromToken(token);
            Guid.TryParse(userIdStr, out var userId);
            var wish = await _wishRepository.RemoveWishAsync(userId);
            if (wish == null) return Results.BadRequest();
            return Results.Json(wish);
        }
    }
}
