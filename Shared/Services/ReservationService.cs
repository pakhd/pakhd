
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using Pakhd.Shared.Data;
using Microsoft.AspNetCore.Http;
using Pakhd.Shared.Models;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace Pakhd.Shared.Services
{
    public class ReservationService
    {
        private readonly PakhdContext _context;
        private readonly ItemService _item;
        private readonly SalesOrderService _salesOrder;
        private readonly SalesLineService _salesLine;
        private readonly UserService _user;
        private readonly IEmailSender _emailSender;

        public ReservationService(PakhdContext context,
                                    ItemService item,
                                    SalesOrderService salesOrder,
                                    SalesLineService salesLine,
                                    UserService user,
                                    IEmailSender emailSender)
        {
            _context = context;
            _item = item;
            _salesOrder = salesOrder;
            _salesLine = salesLine;
            _user = user;
            _emailSender = emailSender;
        }


        //Get Reservation by Lottery ID
        public async Task<Reservation[]> GetByLotteryAsync(int id)
        {
            return await _context.Reservation.Include(r => r.Item).ThenInclude(i => i.Lottery)
                                    .Include(r => r.PakhdUser)
                                    .Where(r => r.Item.LotteryId == id && r.Status == ReservationStatus.Active)
                                    .OrderBy(r => r.CreatedOn)
                                    .ToArrayAsync();
        }

        public async Task<Reservation[]> GetByUserAsync(string username)
        {
            var user = await _user.FindByUsernameAsync(username);

            return await _context.Reservation.Include(r => r.Item).Where(r => r.PakhdUserId == user.Id).ToArrayAsync();
        }

        public async Task<Reservation?> GetByIdAsync(int id)
        {
            return await _context.Reservation.Include(r => r.Item).FirstOrDefaultAsync(r => r.ReservationId == id);
        }

        public async Task<Reservation[]> GetByItemAsync(int lotteryId, int no)
        {
            return await _context.Reservation.Include(r => r.Item).Where(r => r.Item.LotteryId == lotteryId && r.Item.Number == no && r.Status == ReservationStatus.Active).ToArrayAsync();
        }

        public async Task UpdateAsync(Reservation reservation)
        {
            _context.Reservation.Update(reservation);
            await _context.SaveChangesAsync();
        }

        public async Task<Item[]> ReserveAsync(int itemId, string username)
        {
            var now = DateTime.UtcNow;

            // get the titem
            var item = await _item.GetByIdAsync(itemId);

            // Get current user
            //var claim = _httpContextAccessor.HttpContext.User;
            var user = await _user.FindByUsernameAsync(username);

            var isReservedByUser = await ItemIsReservedByUser(itemId, username);
            var itemIsSold = await _item.IsSoldAsync(itemId);
            var userExceededMaxReservation = await UserExceededMaxReservation(username);

            if(!isReservedByUser && !itemIsSold && !userExceededMaxReservation)
            {
                // create new reservation
                var reservation = new Reservation()
                {
                    ItemId = itemId,
                    PakhdUserId = user.Id,
                };


                // add reservation to db
                _context.Reservation.Add(reservation);
                await _context.SaveChangesAsync();

                // update the item
                item.Status = ItemStatus.Reserved;
                await _item.UpdateAsync(item);

                var message = $"Hello {user.UserName}" + "<br>" +
                    $"Thank you for reserving the ticket #{item.Number} in Lottery #{item.LotteryId}" + "<br>" +
                    "Please make sure that you added your <a href='https://pakhd.app/identity/account/manage'>phone number</a> so we can contact you as soon as possible." + "<br>" +
                    "You can always contact us to buy the ticket. 😁";



                await _emailSender.SendEmailAsync(user.Email, $"Pakhd - Ticket #{item.Number}", message);
                Console.WriteLine($"---------------------Email: {user.Email}------------------");
            }

            


            return await _item.GetByLotteryAsync(item.LotteryId);
        }

        public async Task DeleteAsync(Reservation reservation)
        {
            _context.Reservation.Remove(reservation);
            await _context.SaveChangesAsync();

            var reservations = await GetByItemAsync(reservation.Item.LotteryId, reservation.Item.Number);

            // If there is no remaining reservation
            // Update item status
            var isStillReserved = reservations.Any(r => r.Status == ReservationStatus.Active);
            if (!isStillReserved)
            {
                var item = await _item.GetByNumberAsync(reservation.Item.LotteryId, reservation.Item.Number);
                item.Status = ItemStatus.Free;

                await _item.UpdateAsync(item);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Reservation[]> SellAsync(Reservation reservation)
        {
            // Update Reservation
            reservation.Status = ReservationStatus.Sold;
            await UpdateAsync(reservation);

            //Find remaining reservations
            var remaininReservations = await GetByItemAsync(reservation.Item.LotteryId ,reservation.Item.Number);

            foreach (var res in remaininReservations.Where(r => r.Status == ReservationStatus.Active))
            {
                res.Status = ReservationStatus.Removed;
                res.UpdatedOn = DateTime.Now;
                _context.Reservation.Update(res);
                await _context.SaveChangesAsync();
            }

            var salesOrders = await _salesOrder.GetByUserIdAsync(reservation.PakhdUserId);
            var openOrders = salesOrders.Where(s => s.Status != SalesOrderStatus.Posted);

            // Create Sales Line
            SalesLine salesLine = new SalesLine()
            {
                ItemId = reservation.Item.Number,
                Price = reservation.Item.Lottery.TicketPrice
            };

            if (openOrders.Count() == 0)
            {
                //Create Sales Order
                SalesOrder salesOrder = new SalesOrder()
                {
                    PostingDate = DateTime.Now,
                    PakhdUserId = reservation.PakhdUserId,
                };

                await _salesOrder.CreateAsync(salesOrder);

                // Update Sales Line
                salesLine.SalesOrderId = salesOrder.SalesOrderId;
                salesLine.LineNo = 1;
            }
            else
            {
                var openOrder = openOrders.First();
                var salesLines = await _salesLine.GetByOrderAsync(openOrder.SalesOrderId);
                int lastLineNo = salesLines.OrderBy(s => s.LineNo).Last().LineNo;

                salesLine.SalesOrderId = openOrder.SalesOrderId;
                salesLine.LineNo = lastLineNo + 1;
            }


            await _salesLine.CreateAsync(salesLine);

            var text = $"Hello {salesLine.SalesOrder.PakhdUser.UserName}" + "<br>" +
                    "<br>" +
                    $"Thank you for pruchasing the ticket #{salesLine.Item.Number} in Lottery #{salesLine.Item.LotteryId}" + "<br>" +
                    $"We will be declaring the winners on New Year's Eve! 🎅🏻" + "<br>" +
                    "Make sure to follow us on <a href='https://instagram.com/pakhdapp'>Instagram</a> for the updates and the announcements";

            await _emailSender.SendEmailAsync(salesLine.SalesOrder.PakhdUser.Email, $"Pakhd - Ticket #{salesLine.Item.Number}", text);
            

            return await GetByLotteryAsync(reservation.Item.LotteryId);
        }

        public async Task<bool> ItemIsReservedByUser(int itemId, string username)
        {
            var user = await _user.FindByUsernameAsync(username);

            return await _context.Reservation.AnyAsync(r => r.PakhdUserId == user.Id && r.ItemId == itemId && r.Status == ReservationStatus.Active);
        }

        public async Task<bool> UserExceededMaxReservation(string username)
        {
            var user = await _user.FindByUsernameAsync(username);
            var userReservations = await GetByUserAsync(username);

            if(userReservations.Length >= user.MaxReservation)
            {
                return true;
            }
            else
            {
                return false;
            }
        }



    }
}
