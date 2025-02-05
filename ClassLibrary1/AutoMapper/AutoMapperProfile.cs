using AutoMapper;
using ClassLibrary1.DTO.Cart;
using ClassLibrary1.DTO.CartItem;
using ClassLibrary1.DTO.Category;
using ClassLibrary1.DTO.Order;
using ClassLibrary1.DTO.OrderItem;
using ClassLibrary1.DTO.Product;
using ClassLibrary1.DTO.ProductImage;
using ClassLibrary1.DTO.Role;
using ClassLibrary1.DTO.User;
using ClassLibrary1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ClassLibrary1.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            
            CreateMap<ProductImage, ProductImageDTO>();
            CreateMap<CreateProductImageDTO, ProductImage>();
            CreateMap<UpdateProductImageDTO, ProductImage>();
            CreateMap<Role, RoleDTO>();
            CreateMap<CreateRoleDTO, Role>();
            CreateMap<UpdateRoleDTO, Role>();
            CreateMap<Category, CategoryDTO>();
            CreateMap<CreateCategoryDTO, Category>();
            CreateMap<UpdateCategoryDTO, Category>();
            

            CreateMap<Order, OrderDto>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.FullName));

            CreateMap<OrderCreateDto, Order>();

            CreateMap<OrderUpdateDto, Order>();
            CreateMap<CreateProductDTO, Product>();
            CreateMap<UpdateProductDTO, Product>();


            CreateMap<OrderItem, OrderItemDTO>()
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
            .ForMember(dest => dest.OrderStatus, opt => opt.MapFrom(src => src.Order.Status));

            CreateMap<OrderItemCreateDTO, OrderItem>();
            CreateMap<OrderItemUpdateDTO, OrderItem>();

            CreateMap<Cart, CartDTO>().ReverseMap();
            CreateMap<CartItem, CartItemDTO>()
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Product.Price));
            CreateMap<CartItemCreateDTO, CartItem>();
            CreateMap<CartItemUpdateDTO, CartItem>();


            CreateMap<Product, ProductDTO>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name));


            CreateMap<CreateProductDTO, Product>();
            CreateMap<UpdateProductDTO, Product>();

   // User mappings
            CreateMap<User, UserResponseDTO>()
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role.RoleName));
                
            CreateMap<UpdateUserRequestDTO, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore()) 
                .ForMember(dest => dest.RoleId, opt => opt.Ignore())       
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore()) 
                .ForMember(dest => dest.IsActive, opt => opt.Ignore());   


        }
    }
}
