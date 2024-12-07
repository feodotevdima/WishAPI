using Application.interfases;
using Core;
using Microsoft.AspNetCore.Authorization;
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
        [HttpGet("{id}")]
        public async Task<IResult> GetUserWishsAsync()
        {
            var token = Request.Headers["Authorization"].ToString();
            token = token.Substring(7);
            var userIdStr = _wishService.GetUserIdFromToken(token);
            Guid.TryParse(userIdStr, out var userId);
            List<WishModel> Wish = await _wishRepository.GetWishByUserIdAsync(userId);
            if (Wish == null) return Results.BadRequest();
            return Results.Json(Wish);
        }

        [Authorize]
        [HttpPost]
        public async Task<IResult> AddWishAsync([FromBody] WishList wishList)
        {
            var token = Request.Headers["Authorization"].ToString();
            token = token.Substring(7);
            var userIdStr = _wishService.GetUserIdFromToken(token);
            Console.WriteLine(token);
            Console.WriteLine(userIdStr);
            var wish = await _wishService.CreateNewWishAsync(token, wishList.Present, wishList.Price.ToString());
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

        [Route("update/name")]
        [HttpPut]
        public async Task<IResult> UpdateNameAsync([FromBody] CreateName newName)
        {
            List<WishModel> oldWish = await _wishRepository.GetWishByUserIdAsync(newName.UserId);
            var wishs= new List<WishModel>();
            foreach (WishModel w in oldWish)
            {
                var newWish = new WishModel(newName.UserId, newName.Name, w.Present, w.Price);
                wishs.Add(await _wishRepository.UpdateWishAsync(newWish));
            }
            if (wishs == null) return Results.BadRequest();
            return Results.Json(wishs);
        }

        [Authorize]
        [HttpDelete]
        public async Task<IResult> RemoveWishAsync([FromBody] Guid wishId)
        {
            var wish = await _wishRepository.RemoveWishAsync(wishId);
            if (wish == null) return Results.BadRequest();
            return Results.Json(wish);
        }
    }
}
