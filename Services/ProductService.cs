using Demo.Grpc.ProductService.Protos;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace Demo.Grpc.ProductService.Services;

public class ProductService : ProductProtoService.ProductProtoServiceBase
{
    private readonly static List<Product> _products = [];
    public override Task<Product?> GetProduct(GetProductRequest request, ServerCallContext context)
    {
        var product = _products.FirstOrDefault(p => p.Id == request.Id);
        if(product == null)
            throw new RpcException(new Status(StatusCode.NotFound, $"Product with ID {request.Id} not found."));
        
        return Task.FromResult(product)!;
    }

    public override Task<ProductListResponse> ListProduct(
        Empty empty,
        ServerCallContext context)
    {
        var response = new ProductListResponse();
        response.Products.AddRange(_products);

        return Task.FromResult(response);
    }

    public override Task<Product> CreateProduct(
        CreateProductRequest request,
        ServerCallContext context)
    {
        var newProduct = new Product
        {
            Id = Guid.NewGuid().ToString(),
            Name = request.Name,
            Description = request.Description,
            Price = request.Price
        };
        _products.Add(newProduct);

        return Task.FromResult(newProduct);
    }

    public override Task<Product> UpdateProduct(
        UpdateProductRequest request,
        ServerCallContext context)
    {
        var product = _products.FirstOrDefault(p => p.Id == request.Id);
        if (product == null)
            throw new RpcException(new Status(StatusCode.NotFound, $"Product with ID {request.Id} not found."));

        product.Name = request.Name;
        product.Description = request.Description;
        product.Price = request.Price;

        return Task.FromResult(product);
    }

    public override Task<Empty> DeleteProduct(
        DeleteProductRequest request,
        ServerCallContext context)
    {
        var product = _products.FirstOrDefault(p => p.Id == request.Id);
        if (product == null)
            throw new RpcException(new Status(StatusCode.NotFound, $"Product with ID {request.Id} not found."));
        
        _products.Remove(product);

        return Task.FromResult(new Empty());
    }
}