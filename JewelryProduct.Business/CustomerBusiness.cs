using JewelryProduct.Business.Base;
using JewelryProduct.Common;
using JewelryProduct.Data;
using JewelryProduct.Data.Models;
using System;
using System.Threading.Tasks;

namespace JewelryProduct.Business
{
    public interface ICustomerBusiness
    {
        Task<IBusinessResult> GetAll();
        Task<IBusinessResult> GetById(string Id);
        Task<IBusinessResult> Save(Customer customer);
        Task<IBusinessResult> Update(Customer customer);
        Task<IBusinessResult> DeleteById(int Id);

        Task<IBusinessResult> SearchById(int Id);
    }

    public class CustomerBusiness : ICustomerBusiness
    {
        private readonly UnitOfWork _unitOfWork;

        public CustomerBusiness(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IBusinessResult> GetAll()
        {
            try
            {
                var customers = await _unitOfWork.CustomerRepository.GetAllAsync();
                if (customers == null || !customers.Any())
                {
                    return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA__MSG);
                }
                else
                {
                    return new BusinessResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, customers);
                }
            }
            catch (Exception ex)
            {
                return new BusinessResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<IBusinessResult> GetById(string Id)
        {
            try
            {
                var customers = await _unitOfWork.CustomerRepository.GetByIdAsync(Id);

                if (customers == null)
                {
                    return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA__MSG);
                }
                else
                {
                    return new BusinessResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, customers);
                }
            }
            catch (Exception ex)
            {
                return new BusinessResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<IBusinessResult> Save(Customer customer)
        {
            try
            {
                var existingCustomer = await _unitOfWork.CustomerRepository.GetByIdAsync(customer.Id);

                if (existingCustomer == null)
                {
                    _unitOfWork.CustomerRepository.PrepareCreate(customer);
                }
                else
                {
                    // Update existing customer
                    existingCustomer.CustomerName = customer.CustomerName;
                    existingCustomer.CustomerPhone = customer.CustomerPhone;
                    existingCustomer.CustomerAddress = customer.CustomerAddress;
                    _unitOfWork.CustomerRepository.PrepareUpdate(existingCustomer);
                }

                int result = await _unitOfWork.SaveChangesAsync();

                if (result > 0)
                {
                    return new BusinessResult(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG);
                }
                else
                {
                    return new BusinessResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG);
                }
            }
            catch (Exception ex)
            {
                return new BusinessResult(Const.ERROR_EXCEPTION, ex.ToString());
            }
        }





        public async Task<IBusinessResult> Update(Customer customer)
        {
            try
            {
                int result = await _unitOfWork.CustomerRepository.UpdateAsync(customer);

                if (result > 0)
                {
                    return new BusinessResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG);
                }
                else
                {
                    return new BusinessResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG);
                }
            }
            catch (Exception ex)
            {
                return new BusinessResult(-4, ex.ToString());
            }
        }

        public async Task<IBusinessResult> DeleteById(int Id)
        {
            try
            {
                var customers = await _unitOfWork.CustomerRepository.GetByIdAsync(Id);
                if (customers != null)
                {
                    var result = await _unitOfWork.CustomerRepository.RemoveAsync(customers);
                    if (result)
                    {
                        return new BusinessResult(Const.SUCCESS_DELETE_CODE, Const.SUCCESS_DELETE_MSG);
                    }
                    else
                    {
                        return new BusinessResult(Const.FAIL_DELETE_CODE, Const.FAIL_DELETE_MSG);
                    }
                }
                else
                {
                    return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA__MSG);
                }
            }
            catch (Exception ex)
            {
                return new BusinessResult(-4, ex.ToString());
            }
        }

        public async Task<IBusinessResult> SearchById(int Id)
        {
            try
            {
                var customer = await _unitOfWork.CustomerRepository.GetByIdAsync(Id);

                if (customer == null)
                {
                    return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA__MSG);
                }
                else
                {
                    return new BusinessResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, customer);
                }
            }
            catch (Exception ex)
            {
                return new BusinessResult(Const.ERROR_EXCEPTION, ex.ToString());
            }
        }
    }
}
