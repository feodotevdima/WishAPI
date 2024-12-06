using Core;
using Presistence;

namespace Application.interfases
{
    public interface IWishService
    {
        public Task<WishModel> CreateNewWishAsync(string token, WishList wishList);
        public string? getUserIdFromToken(string token);
    }
}
