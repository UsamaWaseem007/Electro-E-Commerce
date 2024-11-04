using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MartWebApp.Models; // Adjust the namespace according to your project structure

public interface ICartService
{
    void AddToCart(int productId);
    List<CartItem> GetCartItems();
    void RemoveFromCart(int cartItemId);
}

public class CartService : ICartService
{
    private readonly MartWebAppDbContext _context;

    public CartService(MartWebAppDbContext context)
    {
        _context = context;
    }

    public void AddToCart(int productId)
    {
        var cartItem = _context.CartItems.SingleOrDefault(ci => ci.ProductId == productId);
        if (cartItem == null)
        {
            cartItem = new CartItem { ProductId = productId, Quantity = 1 };
            _context.CartItems.Add(cartItem);
        }
        else
        {
            cartItem.Quantity++;
        }

        _context.SaveChanges();
    }

    public List<CartItem> GetCartItems()
    {
        return _context.CartItems.Include(ci => ci.ProductId).ToList();
    }

    public void RemoveFromCart(int cartItemId)
    {
        var cartItem = _context.CartItems.Find(cartItemId);
        if (cartItem != null)
        {
            _context.CartItems.Remove(cartItem);
            _context.SaveChanges();
        }
    }
}
