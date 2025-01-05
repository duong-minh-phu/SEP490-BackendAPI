using ClassLibrary1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Cart> Carts { get; }
        IGenericRepository<CartItem> CartItems { get; }
        IGenericRepository<Category> Categories { get; }
        IGenericRepository<DiscountCode> DiscountCodes { get; }
        IGenericRepository<Order> Orders { get; }
        IGenericRepository<OrderDiscount> OrderDiscounts { get; }
        IGenericRepository<OrderItem> OrderItems { get; }
        IGenericRepository<Payment> Payments { get; }
        IGenericRepository<PaymentMethod> PaymentMethods { get; }
        IGenericRepository<Product> Products { get; }
        IGenericRepository<ProductImage> ProductImages { get; }
        IGenericRepository<Role> Roles { get; }
        IGenericRepository<Shop> Shops { get; }
        IGenericRepository<StudentInfo> StudentInfos { get; }
        IGenericRepository<User> Users { get; }

        Task<int> SaveAsync();
    }
}
