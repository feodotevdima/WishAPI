using Core;
using Presistence;

namespace Application.interfases
{
    public interface IWishService
    {
        public Task<WishModel> CreateNewWishAsync(CreateWish reqest);
        public string? getUserIdFromToken(string token);
    }
}
