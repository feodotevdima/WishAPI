using Core;
using Microsoft.EntityFrameworkCore;
using Presistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public class WishRepository
    {
        public async Task<c> AddUserAsync(WishModel wish)
        {
            if (wish == null) return null;
            using (WishContext db = new WishContext())
            {
                await db.Wishs.AddAsync(wish);
                await db.SaveChangesAsync();
            }
            return wish;
        }

        public async Task<WishModel> RemoveWishAsync(Guid id)
        {
            var wish = await GetWishByIdAsync(id);
            if (wish == null) return null;
            using (WishContext db = new WishContext())
            {
                db.Wishs.Remove(wish);
                await db.SaveChangesAsync();
            }
            return wish;
        }

        public async Task<WishModel> UpdateUserAsync(WishModel wish)
        {
            if (wish == null) return null;
            using (WishContext db = new WishContext())
            {
                db.Wishs.Update(wish);
                await db.SaveChangesAsync();
            }
            return wish;
        }

        public async Task<List<WishModel>> GetWishsAsync()
        {
            using (WishContext db = new WishContext())
            {
                var wishs = await db.Wishs.ToListAsync();
                return wishs;
            }
        }

        public async Task<WishModel?> GetWishByIdAsync(Guid id)
        {
            List<WishModel> wishs = await GetWishsAsync();
            var wish = wishs.FirstOrDefault(item => item.Id == id);
            return wish;
        }

        public async Task<WishModel?> GetWishByEmailAsync(Guid userId)
        {
            List<WishModel> wishs = await GetWishsAsync();
            var wish = wishs.FirstOrDefault(item => item.UserId == userId);
            return wish;
        }
    }
}
