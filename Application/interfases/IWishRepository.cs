using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.interfases
{
    public interface IWishRepository
    {
        public Task<WishModel?> AddWishAsync(WishModel wish);
        public Task<WishModel?> RemoveWishAsync(Guid id);
        public Task<WishModel?> UpdateWishAsync(WishModel wish);
        public Task<List<WishModel>> GetWishsAsync();
        public Task<WishModel?> GetWishByIdAsync(Guid id);
        public Task<WishModel?> GetWishByUserIdAsync(Guid userId);
    }
}
