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

        public List<WishModel> GetWishsWisoutReserv(List<WishModel> wishs)
        {
            return wishs.Where(item => item.ReservUser == null).ToList();
        }

        public async Task<List<List<WishModel>>> GetListWishsAsync()
        {
            var wishs = await _wishRepository.GetWishsAsync();
            var UsersId = new List<Guid>();
            foreach (var wish in wishs)
                UsersId.Add(wish.UserId);

            var users = UsersId.Distinct().ToList();
            var ListWishs = new List<List<WishModel>>();
            foreach(var user in users)
            {
                ListWishs.Add(await _wishRepository.GetWishByUserIdAsync(user));
            }
            return ListWishs;
        }
    }
}
