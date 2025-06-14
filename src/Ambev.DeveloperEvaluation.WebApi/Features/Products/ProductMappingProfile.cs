using AutoMapper;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.CreateProduct;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.GetProduct;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.GetProducts;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.UpdateProduct;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.DeleteProduct;
using Ambev.DeveloperEvaluation.Application.Products.CreateProduct;
using Ambev.DeveloperEvaluation.Application.Products.GetProduct;
using Ambev.DeveloperEvaluation.Application.Products.GetProducts;
using Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;
using Ambev.DeveloperEvaluation.Application.Products.DeleteProduct;
using ApplicationProductDto = Ambev.DeveloperEvaluation.Application.Products.GetProducts.ProductDto;
using WebApiProductDto = Ambev.DeveloperEvaluation.WebApi.Features.Products.GetProducts.ProductDto;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products;

/// <summary>
/// AutoMapper profile for Product feature mappings between WebApi and Application layers
/// </summary>
public class ProductMappingProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for Product feature
    /// </summary>
    public ProductMappingProfile()
    {
        // Request to Command/Query mappings
        CreateMap<CreateProductRequest, CreateProductCommand>();
        CreateMap<GetProductRequest, GetProductQuery>();
        CreateMap<GetProductsRequest, GetProductsQuery>()
            .ForMember(dest => dest.Search, opt => opt.MapFrom(src => src.Name));
        CreateMap<UpdateProductRequest, UpdateProductCommand>();
        CreateMap<DeleteProductRequest, DeleteProductCommand>();

        // Result to Response mappings
        CreateMap<CreateProductResult, CreateProductResponse>();
        CreateMap<GetProductResult, GetProductResponse>();
        CreateMap<GetProductsResult, GetProductsResponse>()
            .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.Products));
        CreateMap<ApplicationProductDto, WebApiProductDto>();
        CreateMap<UpdateProductResult, UpdateProductResponse>();
    }
} 