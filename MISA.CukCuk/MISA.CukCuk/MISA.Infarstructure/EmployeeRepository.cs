using Dapper;
using Microsoft.Extensions.Configuration;
using MISA.ApplicationCore.Entities;
using MISA.ApplicationCore.Interfaces;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace MISA.Infarstructure
{
    public class EmployeeRepository :BaseRepository<Employee>, IEmployeeRepository
    {
        #region Constructor
        public EmployeeRepository(IConfiguration configuration) : base(configuration)
        {

        }
        #endregion

        #region Method
        public Employee GetEmployeeByCode(string employeeCode)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Employee> GetEmployeesFilter(string searchText, Guid? departmentId, Guid? positionId)
        {
            var inputs = (searchText != null) ? searchText : "";
            var param = new DynamicParameters();
            param.Add("@EmployeeCode", inputs, DbType.String);
            param.Add("@FullName", inputs, DbType.String);
            param.Add("@PhoneNumber", inputs, DbType.String);
            param.Add("@DepartmentId", departmentId, DbType.String);
            param.Add("@PositionId", positionId, DbType.String);

            var employees = _dbConnection.Query<Employee>("Proc_GetEmployeesFilter", param, commandType: CommandType.StoredProcedure);
            return employees;

        }
        #endregion
    }
}
