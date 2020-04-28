using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using HoleyForkingShirt.Data;
using HoleyForkingShirt.Models;
using HoleyForkingShirt.Models.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HoleyForkingShirt.Pages.Cart
{
    public class CartModel : PageModel
    {
        private StoreDbContext _context; 
        public List<CartItems> InCart;
        private SignInManager<ApplicationUser> _signInManager;
        private UserManager<ApplicationUser> _userManager;
        public CartModel(StoreDbContext context, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
        }
        public IActionResult OnGetAsync()
        {
            var signedIn = _signInManager.IsSignedIn(User);
            if (signedIn)
            {
                var userId = _userManager.GetUserId(User);
                Models.Cart cart = _context.Carts.First(c => c.UserId == userId);
                if(cart.CartItems == null)
                {
                    ModelState.AddModelError("", "You have no items in your cart.");
                    return RedirectToPage("/Products/Shop");
                }
                InCart = _context.CartItems.Where(i => i.CartID == cart.ID).ToList();

                return Page();
            }
            else
            {
                ModelState.AddModelError("", "Please Login to view your Cart.");
                return RedirectToPage("/Account/Login");
            }

        }
    }
}
