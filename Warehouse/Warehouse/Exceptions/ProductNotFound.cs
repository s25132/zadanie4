﻿namespace Warehouse.Exceptions
{
    public class ProductNotFound : Exception
    {
        public ProductNotFound(string message): base(message)
        {
        }
    }
}
