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

        public async Task<WishModel> CreateNewWishAsync(string token, string present, string price)
        {
            var userIdStr = GetUserIdFromToken(token);
            Guid.TryParse(userIdStr, out var userId);

            var response = await _httpClient.GetAsync($"https://localhost:7001/User/id/{userId}");
            if (!response.IsSuccessStatusCode) 
                return null;

            var user = await response.Content.ReadFromJsonAsync<UserModel>();

            WishModel wish = new WishModel(user.Id, user.Name, present, price);
            await _wishRepository.AddWishAsync(wish);
            return wish;
        }

        public string? GetUserIdFromToken(string token)
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
