﻿using Microsoft.EntityFrameworkCore;
using PointOfSale.Api.Core;
using PointOfSale.Api.Features.Products.Contracts;
using PointOfSale.Api.Features.Products.Models;

namespace PointOfSale.Api.Features.Products.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly PointOfSaleContext _dbContext;

    public ProductRepository(PointOfSaleContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Product>> FindAll()
    {
        return await _dbContext.Products
            .Include(x => x.Category)
            .ToListAsync();
    }

    public async Task<Product?> FindById(int id)
    {
        return await _dbContext.Products
            .Include(x => x.Category)
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<IEnumerable<Product>> FindByName(string productName)
    {
        return await _dbContext.Products
            .Include(x => x.Category)
            .Where(x => x.Name.Contains(productName))
            .ToListAsync();
    }

    public async Task<IEnumerable<Product>> FindByCategory(string categoryName)
    {
        return await _dbContext.Products
            .Include(x => x.Category)
            .Where(x => x.Category.Name == categoryName)
            .ToListAsync();
    }

    public async Task<bool> AlreadyExists(int id)
    {
        return await _dbContext.Products.AnyAsync(x => x.Id == id);
    }

    public async Task<int> Add(Product product)
    {
        _dbContext.Add(product);
        return await _dbContext.SaveChangesAsync();
    }

    public async Task<int> Update(Product product)
    {
        _dbContext.Update(product);
        return await _dbContext.SaveChangesAsync();
    }
}