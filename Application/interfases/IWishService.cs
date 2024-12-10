using Core;
using Presistence;

namespace Application.interfases
{
    public interface IWishService
    {
        public Task<WishModel> CreateNewWishAsync(string token, string present, string price);
        public string? GetUserIdFromToken(string token);
        public List<WishModel> GetWishsWisoutReserv(List<WishModel> wishs);
        public Task<List<List<WishModel>>> GetListWishsAsync();
    }
}
