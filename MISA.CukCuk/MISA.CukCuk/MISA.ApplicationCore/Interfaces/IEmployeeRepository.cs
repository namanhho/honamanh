using MISA.ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MISA.ApplicationCore.Interfaces
{
    public interface IEmployeeRepository: IBaseRepository<Employee>
    {
        /// <summary>
        /// Lấy danh sách hàng theo từ khóa tìm kiếm, phòng ban, vị trí/chức vụ
        /// </summary>
        /// <param name="searchText">Từ khóa tìm kiếm: mã, tên hoặc số điện thoại nhân viên</param>
        /// <param name="departmentId">Id phòng ban</param>
        /// <param name="positionId">Id vị trí/chức vụ</param>
        /// <returns>Danh sách nhân viên</returns>
        /// CreatedBy: HNANH (03/12/2020)
        IEnumerable<Employee> GetEmployeesFilter(string searchText, Guid? departmentId, Guid? positionId);
        
        /// <summary>
        /// Lấy khách hàng qua mã khách hàng
        /// </summary>
        /// <param name="EmployeeCode">Mã khách hàng</param>
        /// <returns>Khách hàng</returns>
        /// CreatedBy: HNANH (26/11/2020)
        Employee GetEmployeeByCode(string employeeCode);
    }
}
