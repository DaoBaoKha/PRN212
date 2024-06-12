using JewelryProduct.Data.Models;
using JewelryProduct.Data.Repository;
using System;
using System.Threading.Tasks;

namespace JewelryProduct.Data
{
    public class UnitOfWork : IDisposable
    {
        private readonly Net1810_212_6_JewelryProductContext _unitOfWorkContext;
        private CustomerRepository _customerRepository;

        public UnitOfWork(Net1810_212_6_JewelryProductContext context)
        {
            _unitOfWorkContext = context;
            _customerRepository = new CustomerRepository(_unitOfWorkContext);
        }

        public CustomerRepository CustomerRepository
        {
            get
            {
                return _customerRepository;
            }
        }

        public int SaveChangesWithTransaction()
        {
            int result = -1;

            using (var dbContextTransaction = _unitOfWorkContext.Database.BeginTransaction())
            {
                try
                {
                    result = _unitOfWorkContext.SaveChanges();
                    dbContextTransaction.Commit();
                }
                catch (Exception)
                {
                    result = -1;
                    dbContextTransaction.Rollback();
                }
            }

            return result;
        }

        public async Task<int> SaveChangesWithTransactionAsync()
        {
            int result = -1;

            using (var dbContextTransaction = _unitOfWorkContext.Database.BeginTransaction())
            {
                try
                {
                    result = await _unitOfWorkContext.SaveChangesAsync();
                    dbContextTransaction.Commit();
                }
                catch (Exception)
                {
                    result = -1;
                    dbContextTransaction.Rollback();
                }
            }

            return result;
        }

        public async Task<int> SaveChangesAsync()
        {
            try
            {
                return await _unitOfWorkContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public void Dispose()
        {
            _unitOfWorkContext.Dispose();
        }
    }
}
