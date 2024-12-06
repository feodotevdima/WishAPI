using Application.interfases;
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
    public class WishRepository : IWishRepository
    {
        public async Task<WishModel?> AddWishAsync(WishModel wish)
        {
            if (wish == null) return null;
            using (WishContext db = new WishContext())
            {
                await db.Wishs.AddAsync(wish);
                await db.SaveChangesAsync();
            }
            return wish;
        }

        public async Task<WishModel?> RemoveWishAsync(Guid id)
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

        public async Task<WishModel?> UpdateWishAsync(WishModel wish)
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

        public async Task<List<WishModel>> GetWishByUserIdAsync(Guid userId)
        {
            List<WishModel> wishs = await GetWishsAsync();
            var wish = wishs.Where(item => item.UserId == userId).ToList();
            return wish;
        }
    }
}
