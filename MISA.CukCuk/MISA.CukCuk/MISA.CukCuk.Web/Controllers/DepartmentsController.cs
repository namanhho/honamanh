using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.ApplicationCore.Entities;
using MISA.ApplicationCore.Interfaces;

namespace MISA.CukCuk.Web.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        #region Declare
        IBaseService<Department> _baseService;
        #endregion

        #region Constructor
        public DepartmentsController(IBaseService<Department> baseService)
        {
            _baseService = baseService;
        }
        #endregion

        #region Method
        // GET: api/v1/<EntitiesController>
        /// <summary>
        /// Lấy danh sách phòng ban
        /// </summary>
        /// <returns>Trạng thái HTTP và danh sách phòng ban</returns>
        /// CreatedBy: HNANH (01/12/2020)
        [HttpGet]
        public IActionResult Get()
        {
            var entities = _baseService.GetEntities();
            return Ok(entities);
        }
        #endregion
    }
}
