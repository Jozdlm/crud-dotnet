﻿using Microsoft.EntityFrameworkCore;
using PointOfSale.Api.Core;
using PointOfSale.Api.Features.Sales.Repositories.Interfaces;
using PointOfSale.Api.Models;

namespace PointOfSale.Api.Features.Sales.Repositories;

public class SaleRepository : ISaleRepository
{
    private readonly PointOfSaleContext _dbContext;
    
    public SaleRepository(PointOfSaleContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<IEnumerable<Sale>> FindAll()
    {
        return await _dbContext.Sale
            .Include(x => x.User)
            .Include(x => x.Customer).ToListAsync();
    }

    public async Task<Sale?> FindById(int id)
    {
        return await _dbContext.Sale
            .Include(x => x.User)
            .Include(x => x.Customer)
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<decimal> GetTotalByDay(DateTime dateTime)
    {
        return await _dbContext.Sale
            .Where(x => x.Date == dateTime)
            .Select(x => x.Total).SumAsync();
    }

    public async Task<IEnumerable<Sale>> FindByDateRange(DateTime start, DateTime end)
    {
        return await _dbContext.Sale
            .Where(x => x.Date >= start && x.Date <= end)
            .Include(x => x.User)
            .Include(x => x.Customer).ToListAsync();
    }
}