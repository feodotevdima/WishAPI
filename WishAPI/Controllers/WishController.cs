using Application.interfases;
using Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presistence;
using Presistence.DTO;

namespace WishAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class WishController : ControllerBase
    {
        private readonly IWishService _wishService;
        private readonly IWishRepository _wishRepository;
        private readonly HttpClient _httpClient;

        public WishController(IWishRepository wishRepository, IWishService wishService, HttpClient httpClient)
        {
            _wishRepository = wishRepository;
            _wishService = wishService;
            _httpClient = httpClient;
        }

        [Route("all")]
        [HttpGet]
        public async Task<IResult> GetAllWishsAsync()
        {
            var wishs = await _wishService.GetListWishsAsync();
            if (wishs == null) return Results.BadRequest();

            var newWishs = new List<List<WishDtoName>>();
            foreach (var wish in wishs)
            {
                var wishNotReserv = _wishService.GetWishsWisoutReserv(wish);
                if (wishNotReserv.Count > 0)
                {
                    var wishsNotReserv = new List<WishDtoName>();
                    foreach (var w in wishNotReserv)
                    { 
                        var dto = new WishDtoName(w.Id, w.UserId, w.Present, w.Price, _httpClient, w.ReservUser);
                        var value = await dto.GetNameAsync();
                        wishsNotReserv.Add(dto);
                    }
                    newWishs.Add(wishsNotReserv);
                }
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
                {
                    var wishsReservName = new List<WishDtoReservUserName>();
                    foreach (var w in Wish)
                    {
                        var dto = new WishDtoReservUserName(w.Id, w.UserId, w.Present, w.Price, _httpClient, w.ReservUser);
                        var value = await dto.GetReservNameAsync();
                        wishsReservName.Add(dto);
                    }
                    return Results.Json(wishsReservName);
                }
            }
            return Results.Json(_wishService.GetWishsWisoutReserv(Wish));
        }


        [Route("get-wish/{id}")]
        [Authorize]
        [HttpGet]
        public async Task<IResult> GetWishAsync(string id)
        {
            Guid.TryParse(id, out var GuidId);
            WishModel? Wish = await _wishRepository.GetWishByIdAsync(GuidId);
            if (Wish == null) return Results.BadRequest();

            var token = Request.Headers["Authorization"].ToString();
            token = token.Substring(7);
            var userIdStr = _wishService.GetUserIdFromToken(token);
            if (userIdStr == Wish.UserId.ToString())
                return Results.Json(Wish);
            
            return Results.BadRequest();
        }

        [Route("reserv/{id}")]  
        [Authorize]
        [HttpGet]
        public async Task<IResult> GetReservWishsAsync(string id)
        {
            var wishs = await _wishRepository.GetWishsAsync();
            var reservWish = wishs.Where(item => item.ReservUser.ToString() == id);

            var wishsName = new List<WishDtoName>();
            foreach (var w in reservWish)
            {
                var dto = new WishDtoName(w.Id, w.UserId, w.Present, w.Price, _httpClient, w.ReservUser);
                var value = await dto.GetNameAsync();
                wishsName.Add(dto);
            }
            return Results.Json(wishsName);
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

        [Route("update/wish/{id}")]
        [Authorize]
        [HttpPut]
        public async Task<IResult> UpdateWishAsync(string id, [FromBody] WishList wishList)
        {
            Guid.TryParse(id, out var GuidId);
            var oldWish = await _wishRepository.GetWishByIdAsync(GuidId);
            var newWish = new WishModel(oldWish.UserId, wishList.Present, wishList.Price, oldWish.ReservUser);
            newWish.Id = oldWish.Id;
            var Wish = await _wishRepository.UpdateWishAsync(newWish);
            if (Wish == null) return Results.BadRequest();
            return Results.Json(Wish);
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
