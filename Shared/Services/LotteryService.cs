using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace Pakhd.Shared.Services
{
    public class LotteryService
    {
        private readonly PakhdContext _context;

        public LotteryService(PakhdContext context)
        {
            _context = context;
        }

        public int Count { get; set; }
         
        public async Task<Lottery[]> GetAsync()
        {
            return await _context.Lottery.Include(l => l.Items).Include(l => l.Currency).OrderByDescending(l => l.LotteryId).ToArrayAsync();
        }

        public async Task<Lottery?> GetByIdAsync(int id)
        {
            return await _context.Lottery.Include(l => l.Currency).FirstOrDefaultAsync(l => l.LotteryId == id);
        }

        public async Task<Lottery[]> CreateAsync(Lottery lottery, int count)
        {
            _context.Lottery.Add(lottery);
            await _context.SaveChangesAsync();

            for(int i = 1; i <= Count; i++)
            {
                var item = new Item()
                {
                    LotteryId = lottery.LotteryId,
                    Number = i,
                };

                _context.Item.Add(item);
                await _context.SaveChangesAsync();
            }

            return await GetAsync();
        }
    }
}
