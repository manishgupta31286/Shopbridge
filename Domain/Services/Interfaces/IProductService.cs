using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shopbridge_base.Domain.Models;

namespace Shopbridge_base.Domain.Services.Interfaces
{
    public interface IProductService
    {
        public IEnumerable<Product> Get();

        public IEnumerable<Product> Get(string upc);
    }
}
