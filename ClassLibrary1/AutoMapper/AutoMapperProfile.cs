﻿using AutoMapper;
using ClassLibrary1.DTO.Category;
using ClassLibrary1.DTO.Order;
using ClassLibrary1.DTO.Product;
using ClassLibrary1.DTO.ProductImage;
using ClassLibrary1.DTO.Role;
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
            CreateMap<Product, ProductDTO>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category ))
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User ));

            CreateMap<Order, OrderDto>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.FullName));

            // Mapping for OrderCreateDto -> Order
            CreateMap<OrderCreateDto, Order>();

            // Mapping for OrderUpdateDto -> Order
            CreateMap<OrderUpdateDto, Order>();
            CreateMap<CreateProductDTO, Product>();
            CreateMap<UpdateProductDTO, Product>();
        }
    }
}
