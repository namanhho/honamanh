using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MISA.ApplicationCore.Entities;
using MISA.ApplicationCore.Enums;
using MISA.ApplicationCore.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MISA.CukCuk.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EntitiesController<TEntity> : ControllerBase
    {
        #region Declare
        IBaseService<TEntity> _baseService;
        #endregion

        #region Constructor
        public EntitiesController(IBaseService<TEntity> baseService)
        {
            _baseService = baseService;
        }
        #endregion

        #region Method
        // GET: api/v1/<EntitiesController>
        /// <summary>
        /// Lấy danh sách đối tượng
        /// </summary>
        /// <returns>Trạng thái HTTP và danh sách thực thể</returns>
        /// CreatedBy HNANH (25/11/2020)
        [HttpGet]
        public IActionResult Get()
        {
            var entities = _baseService.GetEntities();
            return Ok(entities);
        }

        // GET api/v1/<EntitiesController>/5
        /// <summary>
        /// Lấy danh sách đối tượng theo Id của đối tượng
        /// </summary>
        /// <param name="entityId">Id của đối tượng</param>
        /// <returns>Trạng thái HTTP và danh sách đối tượng</returns>
        /// CreatedBy: HNANH (25/11/2020)
        [HttpGet("{entityId}")]
        public IActionResult Get(string entityId)
        {
            var entity = _baseService.GetEntityById(entityId);
            return Ok(entity);
        }

        // POST api/v1/<EntitiesController>
        /// <summary>
        /// Thêm mới đối tượng
        /// </summary>
        /// <param name="entity">Thông tin đối tượng</param>
        /// <returns>Trạng thái HTTP và thông tin nghiệp vụ</returns>
        /// CreatedBy: HNANH (25/11/2020)
        [HttpPost]
        public IActionResult Post(TEntity entity)
        {
            var serviceResult = _baseService.Insert(entity);
            if (serviceResult.MISACode == MISACode.NotValid)
            {
                return BadRequest(serviceResult);
            }
            else return Created("Thêm thành công", serviceResult);
        }

        // PUT api/v1/<EntitiesController>/5
        /// <summary>
        /// Cập nhật thông tin đối tượng
        /// </summary>
        /// <param name="id">Mã đối tượng</param>
        /// <param name="entity">Thông tin đối tượng</param>
        /// <returns>Trạng thái HTTP và thông tin nghiệp vụ</returns>
        /// CreatedBy: HNANH (25/11/2020)
        [HttpPut("{id}")]
        public IActionResult Put([FromRoute]string id, TEntity entity)
        {
            var entityIdType = entity.GetType().GetProperty($"{typeof(TEntity).Name}Id").PropertyType;
            if (entityIdType == typeof(Guid))
            {
                entity.GetType().GetProperty($"{typeof(TEntity).Name}Id").SetValue(entity, Guid.Parse(id));
            }
            else
            {
                var entiIdConvert = Convert.ChangeType(id, entityIdType);
                entity.GetType().GetProperty($"{typeof(TEntity).Name}Id").SetValue(entity, entiIdConvert);
            }

            var serviceResult = _baseService.Update(entity);
            //Kiểm tra dữ liệu không hợp lệ
            if (serviceResult.MISACode == MISACode.NotValid)
            {
                return BadRequest(serviceResult);
            }
            //Kiểm tra đường dẫn có tồn tại không
            else if(serviceResult.MISACode == MISACode.NotFound)
            {
                return NotFound(serviceResult);
            }
            else return Ok(serviceResult);
        }

        // DELETE api/v1/<EntitiesController>/5
        /// <summary>
        /// Xóa đối tượng
        /// </summary>
        /// <param name="entityId">Id của đối tượng</param>
        /// <returns>Trạng thái HTTP và dữ liệu nghiệp vụ</returns>
        /// CreatedBy: HNANH (25/11/2020)
        [HttpDelete("{entityId}")]
        public IActionResult Delete(string entityId)
        {
            var serviceResult = _baseService.Delete(entityId);
            if (serviceResult.MISACode == MISACode.NotFound)
            {
                return NotFound(serviceResult);
            }
            else return Ok(serviceResult);
        }
        #endregion
    }
}
