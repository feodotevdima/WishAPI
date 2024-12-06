using Application.interfases;
using Core;
using Presistence;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;

namespace Application
{
    public class WishService: IWishService
    {
        private readonly IWishRepository _wishRepository;
        private readonly HttpClient _httpClient;

        public WishService(IWishRepository wishRepository, HttpClient httpClient)
        {
            _wishRepository = wishRepository;
            _httpClient = httpClient;
        }

        public async Task<WishModel> CreateNewWishAsync(string token, WishList wishList)
        {
            var userIdStr = getUserIdFromToken(token);
            Guid.TryParse(userIdStr, out var userId);
            //var existWish = await _wishRepository.GetWishByUserIdAsync(userId);

            //if ((existWish != null))
            //    await _wishRepository.RemoveWishAsync(existWish.Id);

            var response = await _httpClient.GetAsync($"https://localhost:7001/User/id/{userId}");
            if (!response.IsSuccessStatusCode) 
                return null;
            
            var user = await response.Content.ReadFromJsonAsync<UserModel>();
            
            WishModel wish = new WishModel(user.Id, user.Name, wishList);
            await _wishRepository.AddWishAsync(wish);
            return wish;
        }

        public string? getUserIdFromToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            if (handler.CanReadToken(token))
            {
                JwtSecurityToken jwtToken = handler.ReadJwtToken(token);
                string? userId = jwtToken.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;

                return userId;
            }
            return null;
        }
    }
}
