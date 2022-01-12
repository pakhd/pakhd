using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Pakhd.Shared.Services
{
    public class ItemService
    {
        private readonly PakhdContext _context;

        public ItemService(PakhdContext context)
        {
            _context = context;
        }

        public async Task<Item[]> GetAsync()
        {
            return await _context.Item.ToArrayAsync();
        }

        public async Task<Item> GetByIdAsync(int Id)
        {
            return await _context.Item.FirstOrDefaultAsync(i => i.ItemId == Id);
        }

        public async Task<Item> GetByNumberAsync(int lotteryId, int no)
        {
            return await _context.Item.FirstOrDefaultAsync(i => i.LotteryId == lotteryId && i.Number == no);
        }

        public async Task<Item[]> GetByLotteryAsync(int lotteryId)
        {
            return await _context.Item.Where(i => i.LotteryId == lotteryId).ToArrayAsync();
        }

        public async Task<Item[]> GetNotSoldItemsByLottery(int id)
        {
            return await _context.Item.Where(i => i.LotteryId == id && i.Status != ItemStatus.Sold).ToArrayAsync();
        }

        public async Task<Item[]> CreateAsync(Item item)
        {
            _context.Item.Add(item);
            await _context.SaveChangesAsync();

            return await GetByLotteryAsync(item.LotteryId);
        }

        public async Task<Item> UpdateAsync(Item item)
        {
            _context.Item.Update(item);
            await _context.SaveChangesAsync();

            return item;
        }

        public async Task<bool> IsReservedAsync(int id)
        {
            return await _context.Reservation.AnyAsync(i => i.ItemId == id);
        }

        public async Task<bool> IsSoldAsync(int id)
        {
            return await _context.Item.AnyAsync(i => i.ItemId == id && i.Status == ItemStatus.Sold);
        }
    }
}
