using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PointOfSale.Api.Application.Contracts;
using PointOfSale.Api.Domain.Entities;
using PointOfSale.Api.Domain.Interfaces;

namespace PointOfSale.Api.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly IProductRepository _repository;
    private readonly IMapper _mapper;

    public ProductsController(
        IProductRepository repository,
        IMapper mapper
    )
    {
        _repository = repository;
        _mapper = mapper;
    }

    [HttpGet("")]
    public async Task<ActionResult> GetProducts(string? q = "")
    {
        List<ProductResponse> productsDto;

        if (q != null)
        {
            var filteredProducts = await _repository.FindByName(q);
            productsDto = _mapper.Map<List<ProductResponse>>(filteredProducts);

            return Ok(new
            {
                Status = 200,
                Data = productsDto,
                productsDto.Count
            });
        }

        var products = await _repository.FindAll();
        productsDto = _mapper.Map<List<ProductResponse>>(products);

        return Ok(new
        {
            Status = 200,
            Data = productsDto,
            productsDto.Count
        });
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProductResponse>> GetProduct(int id)
    {
        var product = await _repository.FindById(id);

        if (product == null)
            return NotFound(new { message = $"El producto con el id {id} no fue encontrado" });

        return _mapper.Map<ProductResponse>(product);
    }

    [HttpPost]
    public async Task<IActionResult> AddProduct(ProductDto productDto)
    {
        var product = _mapper.Map<Product>(productDto);
        var result = await _repository.Add(product);

        if (result == 0)
        {
            return BadRequest(
                new
                {
                    message = "Ha ocurrido un error al intentar crear un producto, comuniquese con sistemas"
                }
            );
        }

        return Ok(new { message = "Producto creado con exito" });
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateProduct(int id, ProductDto productDto)
    {
        var alreadyExists = await _repository.AlreadyExists(id);

        if (!alreadyExists)
        {
            return NotFound();
        }

        var product = _mapper.Map<Product>(productDto);
        product.Id = id;

        var result = await _repository.Update(product);

        if (result == 0)
        {
            return BadRequest(
                new
                {
                    message = "Ha ocurrido un error al intentar actualizar el producto, comuniquese con sistemas"
                }
            );
        }

        return Ok(new { message = "Producto actualizado correctamente" });
    }
}
