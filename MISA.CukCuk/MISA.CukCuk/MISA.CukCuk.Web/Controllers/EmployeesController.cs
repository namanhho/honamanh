using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MISA.ApplicationCore.Entities;
using MISA.ApplicationCore.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MISA.CukCuk.Web.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class EmployeesController : EntitiesController<Employee>
    {
        #region Declare
        IEmployeeService _employeeService;
        //IBaseService<Employee> _baseService;
        #endregion

        #region Constructor
        public EmployeesController(IEmployeeService employeeService) : base(employeeService)
        {
            _employeeService = employeeService;
        }
        #endregion

        #region Method
        /// <summary>
        /// Lấy danh sách nhân viên theo StoreProcedure
        /// </summary>
        /// <param name="procedureName">Tên StoreProcedure</param>
        /// <returns>Trả về trạng thái HTTP và danh sách nhân viên</returns>
        /// CreatedBy: HNANH (01/12/2020)
        [HttpGet("getbyprocedure")]
        public IActionResult GetEntitiesByProc([FromQuery] string procedureName)
        {
            var entities = _employeeService.GetEmployeeByProc(procedureName);
            return Ok(entities);
        }

        /// <summary>
        /// Tìm kiếm danh sách nhân viên theo từ khóa, phòng ban và vị trí/chức vụ
        /// </summary>
        /// <param name="searchText">Từ khóa tìm kiếm: mã nhân viên, tên hoặc số điện thoại</param>
        /// <param name="departmentId">Id phòng ban</param>
        /// <param name="positionId">Id vị trí/chức vụ</param>
        /// <returns>Trạng thái HTTP và danh sách nhân viên</returns>
        /// CreatedBy: HNANH (02/12/2020)
        [HttpGet("filter")]
        public IActionResult GetEmployeesFilter(string searchText, Guid? departmentId, Guid? positionId)
        {
            return Ok(_employeeService.GetEmployeesFilter(searchText, departmentId, positionId));
        }
        #endregion
    }
}
