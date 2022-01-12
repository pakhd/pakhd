using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Pakhd.Shared.Services
{
    public class SalesLineService
    {
        private readonly PakhdContext _context;
        private readonly UserService _user;
        private readonly ItemService _item;

        public SalesLineService(PakhdContext context,
                                    UserService user,
                                    ItemService item)
        {
            _context = context;
            _user = user;
            _item = item;
        }
        
        public async Task<SalesLine[]> GetAsync()
        {
            return await _context.SalesLine.Include(s => s.SalesOrder).Include(s => s.Item).ToArrayAsync();
        }

        public async Task<SalesLine[]> GetByOrderAsync(string salesOrderId)
        {
            return await _context.SalesLine.Include(s => s.SalesOrder)
                                            .Include(s => s.Item)
                                            .ThenInclude(i => i.Lottery)
                                            .Where(s => s.SalesOrderId == salesOrderId)
                                            .ToArrayAsync();
        }

        public async Task<SalesLine> GetByUserAsync(string username)
        {
            var user = await _user.FindByUsernameAsync(username);

            return await _context.SalesLine.Include(s => s.SalesOrder).FirstOrDefaultAsync(s => s.SalesOrder.PakhdUserId == user.Id);
        }

        public async Task<SalesLine> GetbyIdAsync(int Id)
        {
            return await _context.SalesLine.FirstOrDefaultAsync(s => s.SalesLineId == Id);
        }

        public async Task<SalesLine> GetbyItemAsync(int itemId)
        {
            return await _context.SalesLine.Include(s => s.SalesOrder).FirstOrDefaultAsync(s => s.ItemId == itemId);
        }

        public async Task<SalesLine[]> CreateAsync(SalesLine salesLine)
        {
            var item = await _item.GetByIdAsync(salesLine.ItemId);
            item.Status = ItemStatus.Sold;

            await _item.UpdateAsync(item);

            _context.SalesLine.Add(salesLine);
            await _context.SaveChangesAsync();

            return await GetAsync();
        }

        public async Task<SalesLine[]> UpdateAsync(SalesLine salesLine)
        {
            _context.SalesLine.Update(salesLine);
            await _context.SaveChangesAsync();

            return await GetAsync();
        }

        public async Task<SalesLine[]> DeleteAsync(SalesLine salesLine)
        {
            _context.SalesLine.Remove(salesLine);
            await _context.SaveChangesAsync();

            return await GetAsync();
        }

    }
}
