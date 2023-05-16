﻿using System.ComponentModel.DataAnnotations;

namespace SEHW.Models;

public class Dish
{
    [Key] public int Id { get; set; }

    [Required] [MaxLength(100)] public string Name { get; set; }

    public string? Description { get; set; }

    [Required] public decimal Price { get; set; }

    [Required] public int Quantity { get; set; }

    [Required] public bool IsAvailable { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}