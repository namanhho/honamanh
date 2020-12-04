﻿using MISA.ApplicationCore.Entities;
using MISA.ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace MISA.ApplicationCore.Services
{
    /// <summary>
    /// Xử lý nghiệp vụ chung
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class BaseService<TEntity> : IBaseService<TEntity> where TEntity : BaseEntity
    {
        #region Declare
        IBaseRepository<TEntity> _baseRepository;
        ServiceResult _serviceResult;
        #endregion

        #region Construtor
        public BaseService(IBaseRepository<TEntity> baseRepository)
        {
            _baseRepository = baseRepository;
            _serviceResult = new ServiceResult() { MISACode = Enums.MISACode.Success };
        }
        #endregion

        #region Method
        public IEnumerable<TEntity> GetEntities()
        {
            return _baseRepository.GetEntities();
        }

        public TEntity GetEntityById(string entityId)
        {
            return _baseRepository.GetEntityById(entityId);
        }

        public virtual ServiceResult Insert(TEntity entity)
        {
            entity.EntityState = Enums.EntityState.AddNew;
            // Validate dữ liệu
            var isValidate = Validate(entity);

            if (isValidate == true)
            {
                _serviceResult.Data = _baseRepository.Insert(entity);
                _serviceResult.Messenger = Properties.Resources.Msg_AddSuccess;
                _serviceResult.MISACode = Enums.MISACode.IsValid;
                return _serviceResult;
            }
            else
            {
                return _serviceResult;
            }
        }

        public ServiceResult Update(TEntity entity)
        {
            entity.EntityState = Enums.EntityState.Update;
            // Validate dữ liệu
            var isValidate = Validate(entity);
            if (isValidate == true)
            {
                var rowAfffected = _baseRepository.Update(entity);
                if (rowAfffected == 0)
                {
                    _serviceResult.Data = rowAfffected;
                    _serviceResult.Messenger = Properties.Resources.Msg_NotFound;
                    _serviceResult.MISACode = Enums.MISACode.NotFound;
                }
                else
                {
                    _serviceResult.Data = rowAfffected;
                    _serviceResult.Messenger = Properties.Resources.Msg_EditSuccess;
                    _serviceResult.MISACode = Enums.MISACode.IsValid;
                }
                return _serviceResult;
            }
            else
            {
                return _serviceResult;
            }
        }

        public ServiceResult Delete(string entityId)
        {
            var rowAffected = _baseRepository.Delete(entityId);
            _serviceResult.Data = rowAffected;
            if (rowAffected == 0)
            {
                _serviceResult.Messenger = "Không tìm thấy server";
                _serviceResult.MISACode = Enums.MISACode.NotFound;
            }
            else
            {
                _serviceResult.Messenger = "Xóa thành công";
                _serviceResult.MISACode = Enums.MISACode.Success;
            }
            return _serviceResult;
        }

        /// <summary>
        /// Hàm validate chung của cha mà tất cả các con phải thực hiện, để private để không cho override
        /// </summary>
        /// <param name="entity">Đối tượng</param>
        /// <returns>Trả về đúng nếu dữ liệu hợp lệ, trả về sai nếu dữ liệu ko hợp lệ</returns>
        private bool Validate(TEntity entity)
        {
            var mesArrayError = new List<string>();
            var isValidate = true;
            // Đọc các Property
            var properties = entity.GetType().GetProperties();
            foreach (var property in properties)
            {
                var displayName = string.Empty;
                //Lấy tên attribute đã được gán displayName
                var displayNameAttribute = property.GetCustomAttributes(typeof(DisplayName), true);
                // Kiểm tra xem có attribute cần phải validate không
                if (displayNameAttribute.Length > 0)
                {
                    displayName = (displayNameAttribute[0] as DisplayName).Name;
                }
                if (property.IsDefined(typeof(Required), false))
                {
                    //Check bắt buộc nhập
                    var propertyValue = property.GetValue(entity);
                    if (propertyValue == null)
                    {
                        isValidate = false;
                        mesArrayError.Add(string.Format(Properties.Resources.Msg_EmptyData, displayName));
                        _serviceResult.Messenger = Properties.Resources.Msg_ErrorData;
                        _serviceResult.MISACode = Enums.MISACode.NotValid;

                    }
                }
                if (property.IsDefined(typeof(CheckDuplicate), false))
                {
                    // Check trùng
                    var propertyName = property.Name;
                    var propertyValue = property.GetValue(entity);
                    var entityDuplicate = _baseRepository.GetEntityByProperty(entity, property);

                    if (entityDuplicate != null)
                    {
                        isValidate = false;
                        mesArrayError.Add(string.Format(Properties.Resources.Msg_DuplicateData, displayName));
                        _serviceResult.Messenger = Properties.Resources.Msg_ErrorData;
                        _serviceResult.MISACode = Enums.MISACode.NotValid;
                    }
                }
                if (property.IsDefined(typeof(MaxLength), false))
                {
                    //Lấy độ dài dữ liệu khi người dùng nhập
                    var propertyValue = property.GetValue(entity);

                    // Lấy độ dài max và thông điệp đã khai báo
                    var attibuteMaxLength = property.GetCustomAttributes(typeof(MaxLength), true)[0];
                    var maxLenght = (attibuteMaxLength as MaxLength).Value;
                    var errorMsg = (attibuteMaxLength as MaxLength).ErrorMsg;
                    if (propertyValue.ToString().Trim().Length > maxLenght)
                    {
                        isValidate = false;
                        mesArrayError.Add(errorMsg ?? string.Format(Properties.Resources.Msg_LongData, displayName, maxLenght));
                        _serviceResult.Messenger = Properties.Resources.Msg_LongData;
                        _serviceResult.MISACode = Enums.MISACode.NotValid;
                    }

                }
            }
            _serviceResult.Data = mesArrayError;
            if (isValidate == true)
            {
                isValidate = ValidateCustom(entity);
            }
            return isValidate;
        }

        /// <summary>
        /// Hàm thực hiện kiểm tra dữ liệu/nghiệp vụ tùy chỉnh
        /// </summary>
        /// <param name="entity">Đối tượng</param>
        /// <returns>Trả về đúng nếu dữ liệu hợp lệ, trả về sai nếu dữ liệu không hợp lệ</returns>
        protected virtual bool ValidateCustom(TEntity entity)
        {
            return true;
        }

        //public virtual IEnumerable<TEntity> GetEntitiesByProc(string procedureName)
        //{
        //    return _baseRepository.GetEntities(procedureName);
        //}
        #endregion
    }
}
