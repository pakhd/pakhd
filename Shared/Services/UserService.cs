using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace Pakhd.Shared.Services
{
    public class UserService
    {
        private readonly PakhdContext _context;

        public UserService(PakhdContext context)
        {
            _context = context;
        }
        

        public async Task<PakhdUser[]> GetAsync()
		{
            return await _context.PakhdUser.Include(p => p.Reservations).Include(p => p.SalesOrders).ThenInclude(p => p.SalesLines).ToArrayAsync();
		}

        public string[] GetReferralCodes()
        {
            return _context.PakhdUser.Select(u => u.ReferralCode).ToArray();
        }


        public async Task<PakhdUser> FindByIdAsync(string id)
        {
            return await _context.PakhdUser.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<PakhdUser> FindByUsernameAsync(string username)
        {
            return await _context.PakhdUser.FirstOrDefaultAsync(p => p.UserName == username);
        }
    }
}
