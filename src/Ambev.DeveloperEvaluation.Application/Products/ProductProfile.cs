using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Products.CreateProduct;
using Ambev.DeveloperEvaluation.Application.Products.GetProduct;
using Ambev.DeveloperEvaluation.Application.Products.GetProducts;
using Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Products;

/// <summary>
/// AutoMapper profile for Product feature mappings
/// </summary>
public class ProductProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for Product feature
    /// </summary>
    public ProductProfile()
    {
        // Command to Domain entity mappings
        CreateMap<CreateProductCommand, Product>()
            .ConstructUsing(src => new Product(src.Name, src.Price));

        // Domain entity to Application results mappings
        CreateMap<Product, CreateProductResult>();
        CreateMap<Product, GetProductResult>();
        CreateMap<Product, ProductDto>();
        CreateMap<Product, UpdateProductResult>();
    }
} 