using ClassLibrary1.Interface;
using ClassLibrary1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly LkmContext _context;
        private readonly Dictionary<Type, object> _repositories = new();

        public UnitOfWork(LkmContext context)
        {
            _context = context;
        }

        // Generic repository getter
        public IGenericRepository<T> Repository<T>() where T : class
        {
            if (_repositories.ContainsKey(typeof(T)))
                return (IGenericRepository<T>)_repositories[typeof(T)];

            var repository = new GenericRepository<T>(_context);
            _repositories.Add(typeof(T), repository);
            return repository;
        }

        // Specific repositories (for strongly-typed usage if needed)
        public IGenericRepository<Cart> Carts => Repository<Cart>();
        public IGenericRepository<CartItem> CartItems => Repository<CartItem>();
        public IGenericRepository<Category> Categories => Repository<Category>();
        public IGenericRepository<DiscountCode> DiscountCodes => Repository<DiscountCode>();
        public IGenericRepository<Order> Orders => Repository<Order>();
        public IGenericRepository<OrderDiscount> OrderDiscounts => Repository<OrderDiscount>();
        public IGenericRepository<OrderItem> OrderItems => Repository<OrderItem>();
        public IGenericRepository<Payment> Payments => Repository<Payment>();
        public IGenericRepository<PaymentMethod> PaymentMethods => Repository<PaymentMethod>();
        public IGenericRepository<Product> Products => Repository<Product>();
        public IGenericRepository<ProductImage> ProductImages => Repository<ProductImage>();
        public IGenericRepository<Role> Roles => Repository<Role>();
        public IGenericRepository<Shop> Shops => Repository<Shop>();
        public IGenericRepository<StudentInfo> StudentInfos => Repository<StudentInfo>();
        public IGenericRepository<User> Users => Repository<User>();

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
