using MISA.ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MISA.ApplicationCore.Interfaces
{
    public interface ICustomerRepository: IBaseRepository<Customer>
    {
        /// <summary>
        /// Lấy khách hàng qua mã khách hàng
        /// </summary>
        /// <param name="customerCode">Mã khách hàng</param>
        /// <returns>Thông tin 1 Khách hàng</returns>
        /// CreatedBy: HNANH (26/11/2020)
        Customer GetCustomerByCode(string customerCode);

    }
}
