using MISA.ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MISA.ApplicationCore.Interfaces
{
    /// <summary>
    /// Interface xử lý nghiệp vụ cho riêng Employee
    /// </summary>
    public interface IEmployeeService: IBaseService<Employee>
    {
        /// <summary>
        /// Lấy danh sách nhân viên theo StoreProcedure
        /// </summary>
        /// <param name="procedureName">Tên StoreProcedure</param>
        /// <returns>Danh sách nhân viên</returns>
        /// CreatedBy: HNANH (3/12/2020)
        IEnumerable<Employee> GetEmployeeByProc(string procedureName);

        /// <summary>
        /// Lấy danh sách nhân viên theo từ khóa tìm kiếm, phòng ban, vị trí/chức vụ
        /// </summary>
        /// <param name="searchText">Từ khóa tìm kiếm: mã nhân viên, tên nhân viên hoặc số điện thoại</param>
        /// <param name="departmentId">Id phòng ban</param>
        /// <param name="positionId">Id vị trí/chức vụ</param>
        /// <returns>Danh sách nhân viên</returns>
        /// CreatedBy: HNANH (03/12/2020)
        IEnumerable<Employee> GetEmployeesFilter(string searchText, Guid? departmentId, Guid? positionId);
    }
}
