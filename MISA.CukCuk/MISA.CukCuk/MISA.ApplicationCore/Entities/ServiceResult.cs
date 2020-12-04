using MISA.ApplicationCore.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MISA.ApplicationCore.Entities
{
    public class ServiceResult
    {
        /// <summary>
        /// Dữ liệu kết quả nhằm giúp dev có thể xác định rõ vị trí, thuộc tính, trường bị lỗi
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        /// Thông báo
        /// </summary>
        public string Messenger { get; set; }

        /// <summary>
        /// Mã kết quả
        /// </summary>
        public MISACode MISACode { get; set; }
    }
}
