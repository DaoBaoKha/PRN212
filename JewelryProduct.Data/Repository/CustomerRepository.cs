using JewelryProduct.Data.Base;
using JewelryProduct.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JewelryProduct.Data.Repository
{
    public class CustomerRepository : GenericRepository<Customer>
    {
        public CustomerRepository() { }

        public CustomerRepository(Net1810_212_6_JewelryProductContext context) => _context = context;
    }

}
