using System;

namespace SmartFoodPlanner.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Unit { get; set; }
        public decimal Quantity { get; set; }
        public ProductStatus Status { get; set; }
        public DateTime? ExpiryDate { get; set; }
    }

    public enum ProductStatus
    {
        Fresh,
        NeedToUse,
        Leftover,
        InStock
    }
}