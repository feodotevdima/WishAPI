using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Presistence.DTO
{
    public class WishDtoReservUserName
    {
        private readonly HttpClient _httpClient;
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Present { get; set; }
        public string Price { get; set; }
        public Guid? ReservUser { get; set; }
        public string? ReservUserName { get; set; }

        public WishDtoReservUserName(Guid id, Guid userId, string present, string price, HttpClient httpClient, Guid? reservUser)
        {
            _httpClient = httpClient;
            Id = id;
            UserId = userId;
            Present = present;
            Price = price;
            ReservUser = reservUser;
        }

        public async Task<bool> GetReservNameAsync()
        {
            var response = await _httpClient.GetAsync($"https://localhost:7001/User/id/{ReservUser}");
            if (response.IsSuccessStatusCode)
            {
                var user = await response.Content.ReadFromJsonAsync<UserModel>();
                ReservUserName = user.Name;
                return true;
            }
            return false;
        }
    }
}
