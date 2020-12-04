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
    public class PositionsController : ControllerBase
    {
        #region Declare
        IBaseService<Position> _baseService;
        #endregion

        #region Constructor
        public PositionsController(IBaseService<Position> baseService)
        {
            _baseService = baseService;
        }
        #endregion

        #region Method
        // GET: api/<EntitiesController>
        /// <summary>
        /// Lấy danh sách vị trí/chức vụ
        /// </summary>
        /// <returns>Trạng thái HTTP và danh sách vị trí/chức vụ</returns>
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
