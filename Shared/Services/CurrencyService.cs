using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Pakhd.Shared.Services
{
    public class CurrencyService
    {
        private readonly PakhdContext _context;

        public CurrencyService(PakhdContext context)
        {
            _context = context;
        }
        

        public async Task<Currency[]> GetAsync()
		{
            return await _context.Currency.ToArrayAsync();
		}

        public async Task<Currency[]> CreateCurrencyAsync(Currency currency)
		{
            _context.Currency.Add(currency);
            await _context.SaveChangesAsync();

            return await GetAsync();
		}

    }
}
