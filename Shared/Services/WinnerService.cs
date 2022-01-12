using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace Pakhd.Shared.Services
{
    public class WinnerService
    {
        private readonly PakhdContext _context;

        public WinnerService(PakhdContext context)
        {
            _context = context;
        }
        

        public async Task<Winner[]> GetAsync()
		{
            return await _context.Winner.ToArrayAsync();
		}

        public async Task<Winner[]> GetByLotteryAsync(int lotteryId)
        {
            return await _context.Winner.Include(w => w.Lottery).Include(w => w.PakhdUser).Where(w => w.LotteryId == lotteryId).ToArrayAsync();
        }

    }
}
