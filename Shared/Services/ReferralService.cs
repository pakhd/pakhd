using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace Pakhd.Shared.Services
{
    public class ReferralService
    {
        private readonly PakhdContext _context;
        private readonly UserService _user;

        public ReferralService(PakhdContext context, UserService user)
        {
            _context = context;
            _user = user;
        }
        

        public async Task<Referral[]> GetAsync()
		{
            return await _context.Referral.ToArrayAsync();
		}

        public async Task<Referral[]> GetByUserAsync(string username)
        {
            var user = await _user.FindByUsernameAsync(username);
            Console.WriteLine($"--------------USER ID: {user.Id}---------------------");
            return _context.Referral.Where(r => r.PakhdUserId == user.Id).ToArray();
        }

        public async Task<int> GetSalesCount(string[] userIds)
        {
            int count = 0;
            foreach(var id in userIds)
            {
                var salesList = await _context.SalesLine.Include(so => so.SalesOrder).Where(sl => sl.SalesOrder.PakhdUserId == id).ToListAsync();
                count += salesList.Count();
            }



            return count;
        }
    }
}
