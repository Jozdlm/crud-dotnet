﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PointOfSale.Api.Domain.Entities;

[Table("sale_items")]
public class SaleItem
{
    [Key]
    public int Id { get; set; }

    public int SaleId { get; set; }
    public virtual Sale Sale { get; set; } = null!;

    public int ProductId { get; set; }
    public virtual Product Product { get; set; } = null!;

    public int Quantity { get; set; }
    public decimal PurchasePrice { get; set; }
    public decimal SellingPrice { get; set; }
    public decimal Discount { get; set; }

    public decimal Tax => SellingPrice * 0.12m;
    public decimal Total => (Quantity * SellingPrice) + Tax;
    public decimal Earned => (SellingPrice - PurchasePrice) * Quantity;
}
