using MISA.ApplicationCore.Entities;
using MISA.ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MISA.ApplicationCore.Services
{
    /// <summary>
    /// Xử lý nghiệp vụ cho riêng Employee
    /// </summary>
    public class EmployeeService : BaseService<Employee>, IEmployeeService
    {
        #region Declare
        IEmployeeRepository _employeeRepository;
        #endregion

        #region Constructor
        public EmployeeService(IEmployeeRepository employeeRepository) : base(employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }
        #endregion

        #region Method
        public IEnumerable<Employee> GetEmployeeByProc(string procedureName)
        {
            return _employeeRepository.GetEntities(procedureName);
        }

        public IEnumerable<Employee> GetEmployeesFilter(string searchText, Guid? departmentId, Guid? positionId)
        {
            return _employeeRepository.GetEmployeesFilter(searchText, departmentId, positionId);
        }
        #endregion
    }
}
