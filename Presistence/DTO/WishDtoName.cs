using Core;
using System.Net.Http.Json;


namespace Presistence.DTO
{
    public class WishDtoName
    {
        private readonly HttpClient _httpClient;
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string? UserName { get; set; }
        public string? Image { get; set; }
        public string Present { get; set; }
        public string Price { get; set; }
        public Guid? ReservUser { get; set; }

        public WishDtoName(Guid id, Guid userId, string present, string price, HttpClient httpClient, Guid? reservUser)
        {
            _httpClient = httpClient;
            Id = id;
            UserId = userId;
            Present = present;
            Price = price;
            ReservUser = reservUser;
        }

        public async Task<bool> GetNameAsync()
        {
            var response = await _httpClient.GetAsync($"https://localhost:7001/User/id/{UserId}");
            if (response.IsSuccessStatusCode)
            {
                var user = await response.Content.ReadFromJsonAsync<UserModel>();
                UserName = user.Name;
                Image = user.Image;
                return true;
            }
            return false;
        }
    }
}
