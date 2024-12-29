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
            var wishs = await _wishService.GetListWishsAsync();
            if (wishs == null) return Results.BadRequest();

            var newWishs = new List<List<WishModel>>();
            foreach (var wish in wishs)
            {
                var w = _wishService.GetWishsWisoutReserv(wish);
                if (w.Count>0)
                    newWishs.Add(w);
            }

            var token = Request.Headers["Authorization"].ToString();
            if (token.Length >= 7)
            {
                token = token.Substring(7);
                var userIdStr = _wishService.GetUserIdFromToken(token);
                Guid.TryParse(userIdStr, out var Id);
                return Results.Json(newWishs.Where(item => item[0].UserId != Id));
            }

            return Results.Json(newWishs);
        }


        [HttpGet("{id}")]
        public async Task<IResult> GetUserWishsAsync(string id)
        {
            Guid.TryParse(id, out var GuidId);
            List<WishModel> Wish = await _wishRepository.GetWishByUserIdAsync(GuidId);
            if (Wish == null) return Results.BadRequest();

            var token = Request.Headers["Authorization"].ToString();
            if (token.Length > 7)
            {
                token = token.Substring(7);
                var userIdStr = _wishService.GetUserIdFromToken(token);
                if (userIdStr == id)
                    return Results.Json(Wish);
            }
            return Results.Json(_wishService.GetWishsWisoutReserv(Wish));
        }

        [Route("reserv/{id}")]
        [Authorize]
        [HttpGet]
        public async Task<IResult> GetReservWishsAsync(string id)
        {
            var wishs = await _wishRepository.GetWishsAsync();
            var reservWish = wishs.Where(item => item.ReservUser.ToString() == id);
            return Results.Json(reservWish);
        }

        [Authorize]
        [HttpPost]
        public async Task<IResult> AddWishAsync([FromBody] WishList wishList)
        {
            var token = Request.Headers["Authorization"].ToString();
            token = token.Substring(7);
            var userIdStr = _wishService.GetUserIdFromToken(token);
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
            List<WishModel> oldWishs = await _wishRepository.GetWishByUserIdAsync(newName.UserId);
            var wishs= new List<WishModel>();
            foreach (WishModel w in oldWishs)
            {
                var newWish = new WishModel(newName.UserId, newName.Name, w.Present, w.Price, w.ReservUser, w.ReservUserName);
                wishs.Add(await _wishRepository.UpdateWishAsync(newWish));
            }
            if (wishs == null) return Results.BadRequest();
            return Results.Json(wishs);
        }

        [Route("reserv/add/{id}")]
        [Authorize]
        [HttpPut]
        public async Task<IResult> UpdateAddReservAsync(string id)
        {
            Guid.TryParse(id, out var wishId);
            var oldWish = await _wishRepository.GetWishByIdAsync(wishId);

            var token = Request.Headers["Authorization"].ToString();
            token = token.Substring(7);
            var userId = _wishService.GetUserIdFromToken(token);

            Guid.TryParse(userId, out var Id);
            oldWish.ReservUser = Id;
            oldWish.ReservUserName = await _wishService.GetUserById(userId);
            var wish = _wishRepository.UpdateWishAsync(oldWish);

            if (wish == null) return Results.BadRequest();
            return Results.Json(wish);
        }

        [Route("reserv/del/{id}")]
        [Authorize]
        [HttpPut]
        public async Task<IResult> UpdateDelReservAsync(string id)
        {
            Guid.TryParse(id, out var wishId);
            var oldWish = await _wishRepository.GetWishByIdAsync(wishId);

            var token = Request.Headers["Authorization"].ToString();
            token = token.Substring(7);
            var userId = _wishService.GetUserIdFromToken(token);
            if(oldWish.ReservUser.ToString() != userId) return Results.BadRequest();

            oldWish.ReservUser = null;
            oldWish.ReservUser = null;

            var wish = _wishRepository.UpdateWishAsync(oldWish);

            if (wish == null) return Results.BadRequest();
            return Results.Json(wish);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IResult> RemoveWishAsync(string id)
        {
            Guid.TryParse(id, out var Id);
            var wish = await _wishRepository.RemoveWishAsync(Id);
            if (wish == null) return Results.BadRequest();
            return Results.Json(wish);
        }
    }
}
