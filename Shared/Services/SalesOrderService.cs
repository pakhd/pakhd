using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Pakhd.Shared.Services
{
    public class SalesOrderService
    {
        private readonly PakhdContext _context;
        private readonly UserService _user;

        public SalesOrderService(PakhdContext context, UserService user)
        {
            _context = context;
            _user = user;
        }
        
        public async Task<SalesOrder[]> GetAsync()
        {
            return await _context.SalesOrder.Include(s => s.PakhdUser).Include(s => s.SalesLines).ToArrayAsync();
        }

        public async Task<SalesOrder[]> GetByUserIdAsync(string userId)
        {
            PakhdUser user = await _user.FindByIdAsync(userId);

            return await _context.SalesOrder.Where(s => s.PakhdUserId == user.Id).ToArrayAsync();
        }

        public async Task<SalesOrder> GetbyIdAsync(string Id)
        {
            return await _context.SalesOrder.FirstOrDefaultAsync(s => s.SalesOrderId == Id);
        }

        public async Task<SalesOrder[]> CreateAsync(SalesOrder salesOrder)
        {
            SalesOrder lastSalesOrder = await GetLastOrderAsync();

            salesOrder.OrderNo = GenerateOrderNo(lastSalesOrder);

            _context.SalesOrder.Add(salesOrder);
            await _context.SaveChangesAsync();

            return await GetAsync();
        }

        public async Task<SalesOrder[]> UpdateAsync(SalesOrder salesOrder)
        {
            _context.SalesOrder.Update(salesOrder);
            await _context.SaveChangesAsync();

            return await GetAsync();
        }

        public async Task<SalesOrder[]> PostAsync(SalesOrder salesOrder)
        {
            salesOrder.Status = SalesOrderStatus.Posted;
            await UpdateAsync(salesOrder);

            return await GetAsync();
        }

        public async Task<SalesOrder[]> DeleteAsync(SalesOrder salesOrder)
        {
            _context.SalesOrder.Remove(salesOrder);
            await _context.SaveChangesAsync();

            return await GetAsync();
        }

        public async Task<SalesOrder> GetLastOrderAsync()
        {
            return await _context.SalesOrder.OrderBy(s => s.OrderNo).LastOrDefaultAsync();
        }

        public string GenerateOrderNo(SalesOrder? lastOrder)
        {
            string lastPart = "";
            string year = DateTime.Now.Year.ToString();

            if (lastOrder is not null)
            {
                int newOrderNo = Convert.ToInt32(lastOrder.OrderNo.Split("-")[2]) + 1;
                
                if (newOrderNo < 10)
                {
                    lastPart = "00000" + newOrderNo.ToString();
                }
                else if (newOrderNo < 100)
                {
                    lastPart = "0000" + newOrderNo.ToString();
                }
                else if (newOrderNo < 1000)
                {
                    lastPart = "000" + newOrderNo.ToString();
                }
                else if (newOrderNo < 10000)
                {
                    lastPart = "00" + newOrderNo.ToString();
                }
                else if (newOrderNo < 100000)
                {
                    lastPart = "0" + newOrderNo.ToString();
                }
                else
                {
                    lastPart = newOrderNo.ToString();
                }
            }
            else
            {
                lastPart = "000001";
            }

            return "SO-" + year.Substring(2) + "-" + lastPart;
        }

    }
}
