using System;
using System.Collections.Generic;
using System.Text;

namespace MISA.ApplicationCore.Entities
{
    public class Position: BaseEntity
    {
        /// <summary>
        /// Id vị trí/chức vụ
        /// </summary>
        public Guid PositionId { get; set; }
        /// <summary>
        /// Mã vị trí/chức vụ
        /// </summary>
        public string PositionCode { get; set; }
        /// <summary>
        /// Tên vị trí/chức vụ
        /// </summary>
        public string  PositionName { get; set; }
    }
}
