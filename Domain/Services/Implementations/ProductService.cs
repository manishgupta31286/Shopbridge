using Microsoft.Extensions.Logging;
using Shopbridge_base.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shopbridge_base.Domain.Models;
using Shopbridge_base.Data.Repository;

namespace Shopbridge_base.Domain.Services
{
    public class ProductService : IProductService
    {
        private readonly ILogger<ProductService> logger;
        private readonly IRepository _repo;

        public ProductService(IRepository repo)
        {
            this._repo = repo;
        }

        public IEnumerable<Product> Get(){
            return _repo.Get<Product>();
        }

        public IEnumerable<Product> Get(string upc)
        {
            return _repo.Get<Product>(x=>x.ProductUpc==upc);
        }
    }
}
