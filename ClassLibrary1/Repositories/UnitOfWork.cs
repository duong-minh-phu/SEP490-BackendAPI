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
        private IGenericRepository<Cart> _CartRepository;
        private IGenericRepository<CartItem> _CartItemRepository;
        private IGenericRepository<Category> _CategoryRepository;
        private IGenericRepository<DiscountCode> _DiscountCodeRepository;
        private IGenericRepository<Order> _OrderRepository;
        private IGenericRepository<OrderDiscount> _OrderDiscountRepository;
        private IGenericRepository<OrderItem> _OrderItemRepository;
        private IGenericRepository<Payment> _PaymentRepository;
        private IGenericRepository<PaymentMethod> _PaymentMethodRepository;
        private IGenericRepository<Product> _ProductRepository;
        private IGenericRepository<ProductImage> _ProductImageRepository;
        private IGenericRepository<Role> _RoleRepository;

        private IGenericRepository<Shop> _ShopRepository;

        private IGenericRepository<StudentInfo> _StudentInfoRepository;

        private IGenericRepository<User> _UserRepository;



        public UnitOfWork(LkmContext context)
        {
            _context = context;
        }

        public IGenericRepository<Cart> Carts =>
            _CartRepository ??= new GenericRepository<Cart>(_context);


        public IGenericRepository<CartItem> CartItems => _CartItemRepository ??= new GenericRepository<CartItem>(_context);

        public IGenericRepository<Category> Categorys => _CategoryRepository ??= new GenericRepository<Category>(_context);

        public IGenericRepository<DiscountCode> DiscountCodes => _DiscountCodeRepository ??= new GenericRepository<DiscountCode>(_context);

        public IGenericRepository<Order> Orders => _OrderRepository ??= new GenericRepository<Order>(_context);

        public IGenericRepository<OrderDiscount> OrderDiscounts => _OrderDiscountRepository ??= new GenericRepository<OrderDiscount>(_context);

        public IGenericRepository<OrderItem> OrderItems => _OrderItemRepository ??= new GenericRepository<OrderItem>(_context);

        public IGenericRepository<Payment> Payments => _PaymentRepository ??= new GenericRepository<Payment>(_context);

        public IGenericRepository<PaymentMethod> PaymentMethods => _PaymentMethodRepository ??= new GenericRepository<PaymentMethod>(_context);

        public IGenericRepository<Product> Products => _ProductRepository ??= new GenericRepository<Product>(_context);

        public IGenericRepository<ProductImage> ProductImages => _ProductImageRepository ??= new GenericRepository<ProductImage>(_context);

        public IGenericRepository<Role> Roles => _RoleRepository ??= new GenericRepository<Role>(_context);

        public IGenericRepository<Shop> Shops => _ShopRepository ??= new GenericRepository<Shop>(_context);

        public IGenericRepository<StudentInfo> StudentInfos => _StudentInfoRepository ??= new GenericRepository<StudentInfo>(_context);

        public IGenericRepository<User> Users => _UserRepository ??= new GenericRepository<User>(_context);

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
