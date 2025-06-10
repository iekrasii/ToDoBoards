using System;
using System.ComponentModel.DataAnnotations;

namespace ProductApi.Models;

public class ProductVariant
{
    public Guid Id { get; set; }

    [Required]
    public Guid ProductId { get; set; }

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public Product? Product { get; set; }
}
