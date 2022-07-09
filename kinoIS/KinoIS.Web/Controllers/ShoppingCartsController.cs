﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KinoIS.Domain.Models;
using KinoIS.Repository;
using KinoIS.Service.Interface;
using System.Security.Claims;

namespace KinoIS.Web.Controllers
{
    public class ShoppingCartsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ShoppingCartService shoppingCartService;
        private readonly TicketInShoppingCartService ticketInShoppingCartService;
        private readonly KinoUserService kinoUserService;

        public ShoppingCartsController(ShoppingCartService shoppingCartService, TicketInShoppingCartService ticketInShoppingCartService, ApplicationDbContext context)
        {
            this.ticketInShoppingCartService = ticketInShoppingCartService;
            this.shoppingCartService = shoppingCartService;
            _context = context;
        }

        /*
         public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ShoppingCart shoppingCart = this.shoppingCartService.findByOwnerId(userId);
            List<TicketInShoppingCart> ticketsInShoppingCart = this.ticketInShoppingCartService.findAllByShoppingCartId(shoppingCart.Id);
            ViewBag.Owner = shoppingCart.Owner;
            return View(ticketsInShoppingCart);
        }
         */

        // GET: ShoppingCarts
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var applicationDbContext = _context.shoppingCarts.Include(s => s.Owner);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ShoppingCarts/Details/5

        /*
          public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shoppingCart = await _context.shoppingCarts
                .Include(s => s.Owner)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shoppingCart == null)
            {
                return NotFound();
            }

            return View(shoppingCart);
        }
         */
        public IActionResult Details (string email)
        {
            KinoUser user = this._context.users.Where(x => x.Email.Equals(email)).FirstOrDefault();
            ShoppingCart shoppingCart = this._context.shoppingCarts.Where(x => x.OwnerId.Equals(user.Id)).FirstOrDefault();
            return View(shoppingCart);

        }

        // GET: ShoppingCarts/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shoppingCart = await _context.shoppingCarts.FindAsync(id);
            if (shoppingCart == null)
            {
                return NotFound();
            }
            ViewData["OwnerId"] = new SelectList(_context.users, "Id", "Id", shoppingCart.OwnerId);
            return View(shoppingCart);
        }

        // POST: ShoppingCarts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,OwnerId")] ShoppingCart shoppingCart)
        {
            if (id != shoppingCart.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(shoppingCart);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShoppingCartExists(shoppingCart.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["OwnerId"] = new SelectList(_context.users, "Id", "Id", shoppingCart.OwnerId);
            return View(shoppingCart);
        }

        // GET: ShoppingCarts/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shoppingCart = await _context.shoppingCarts
                .Include(s => s.Owner)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shoppingCart == null)
            {
                return NotFound();
            }

            return View(shoppingCart);
        }

        // POST: ShoppingCarts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var shoppingCart = await _context.shoppingCarts.FindAsync(id);
            _context.shoppingCarts.Remove(shoppingCart);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ShoppingCartExists(Guid id)
        {
            return _context.shoppingCarts.Any(e => e.Id == id);
        }
    }
}