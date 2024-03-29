﻿using Microsoft.EntityFrameworkCore;
using PointOfSale.Api.Domain.Entities;
using PointOfSale.Api.Domain.Interfaces;
using PointOfSale.Api.Infrastructure.Data;

namespace PointOfSale.Api.Infrastructure.Repositories;

public class SaleItemRepository : ISaleItemRepository
{
    private readonly PointOfSaleContext _dbContext;

    public SaleItemRepository(PointOfSaleContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<IEnumerable<SaleItem>> FindSaleItems(int saleId)
    {
        return await _dbContext.SaleItem
            .Include(x => x.Product)
            .Where(x => x.SaleId == saleId).ToListAsync();
    }

    public async Task<IEnumerable<SaleItem>> FindSaleItemsByProduct(int productId)
    {
        return await _dbContext.SaleItem
            .Include(x => x.Sale)
            .Where(x => x.ProductId == productId).ToListAsync();
    }
}