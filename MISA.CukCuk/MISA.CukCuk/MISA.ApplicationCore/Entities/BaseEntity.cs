using MISA.ApplicationCore.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MISA.ApplicationCore.Entities
{
    /// <summary>
    /// Khai báo thuộc tính bắt buộc nhập
    /// CreatedBy: HNANH (20/11/2020)
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class Required: Attribute
    {

    }
    /// <summary>
    /// Khai báo thuộc tính kiểm tra trùng
    /// CreatedBy: HNANH (20/11/2020)
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class CheckDuplicate : Attribute
    {

    }
    /// <summary>
    /// Khai báo thuộc tính khóa chính
    /// CreatedBy: HNANH (20/11/2020)
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class PrimaryKey: Attribute
    {

    }
    /// <summary>
    /// Khai báo thuộc tính kiểm tra độ dài tối đa
    /// CreatedBy: HNANH (20/11/2020)
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class MaxLength:Attribute
    {
        public int Value { get; set; }
        public string ErrorMsg { get; set; }
        public MaxLength(int maxLength, string msg = null)
        {
            this.Value = maxLength;
            this.ErrorMsg = msg;
        }
    }
    /// <summary>
    /// Khai báo thuộc tính nhằm giải thích tên trưng bày
    /// CreatedBy: HNANH (20/11/2020)
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class DisplayName:Attribute
    {
        public string  Name { get; set; }
        public DisplayName(string name= null)
        {
            this.Name = name;
        }
    }

    /// <summary>
    /// Đối tượng dùng chung
    /// </summary>
    public class BaseEntity
    {
        /// <summary>
        /// Thuộc tính trạng thái của đối tượng
        /// </summary>
        public EntityState EntityState { get; set; } = EntityState.AddNew;
        /// <summary>
        /// Ngày giờ tạo bản ghi
        /// </summary>
        public DateTime? CreatedDate { get; set; }
        /// <summary>
        /// Người tạo bản ghi
        /// </summary>
        public string CreatedBy { get; set; }
        /// <summary>
        /// Ngày giờ sửa bản ghi
        /// </summary>
        public DateTime?  ModifiedDate { get; set; }
        /// <summary>
        /// Tên người sửa bản ghi
        /// </summary>
        public string ModifiedBy { get; set; }
    }
}
