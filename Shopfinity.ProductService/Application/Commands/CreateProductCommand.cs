﻿using MediatR;

namespace Shopfinity.ProductService.Application.Commands
{
    public class CreateProductCommand : IRequest<int>
    {
        // Properties for creating a product
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public int StockQuantity { get; set; }
        public bool Active { get; set; }
    }
}
