﻿using Dapper;
using Microsoft.Extensions.Configuration;
using MISA.ApplicationCore.Entities;
using MISA.ApplicationCore.Enums;
using MISA.ApplicationCore.Interfaces;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MISA.Infarstructure
{
    /// <summary>
    /// Thành phần dùng chung xử lý chung thao tác với Database
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class BaseRepository<TEntity> : IBaseRepository<TEntity>, IDisposable where TEntity:BaseEntity
    {
        #region Declare
        IConfiguration _configuration;
        string _connectionString = string.Empty;
        protected IDbConnection _dbConnection = null;
        protected string _tableName = "";
        string _entityId = "";
        #endregion

        #region Constructor
        public BaseRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("MISACukCukConnectionString");
            _dbConnection = new MySqlConnection(_connectionString);
            _tableName = typeof(TEntity).Name;
            _entityId = _tableName + "Id";
        }
        #endregion

        #region Method
        public IEnumerable<TEntity> GetEntities()
        {
            //Thực hiện câu lệnh truy vấn
            var entities = _dbConnection.Query<TEntity>($"Proc_Get{_tableName}s", commandType: CommandType.StoredProcedure);
            //Trả về dữ liệu
            return entities;
        }

        public IEnumerable<TEntity> GetEntities(string procedureName)
        {
            //Thực hiện câu lệnh truy vấn
            var entities = _dbConnection.Query<TEntity>(procedureName, commandType: CommandType.StoredProcedure);
            //Trả về dữ liệu
            return entities;
        }

        public TEntity GetEntityById(string entityId)
        {
            var param = new DynamicParameters();
            param.Add($"@{_entityId}", entityId, DbType.String);
            //var entity = _dbConnection.Query<TEntity>($"Proc_Get{_entity}ById", new { _entityId = entityId }, commandType: CommandType.StoredProcedure).FirstOrDefault();
            var entity = _dbConnection.Query<TEntity>($"Proc_Get{_tableName}ById", param, commandType: CommandType.StoredProcedure).FirstOrDefault();

            //var a = $"SELECT * FROM {_entity} WHERE {_entity}Id = '{entityId}'";
            //var entity = _dbConnection.Query<TEntity>($"SELECT * FROM {_entity} WHERE {_entity}Id = '{entityId}'", commandType: CommandType.Text).FirstOrDefault();
            return entity;
        }

        public int Insert(TEntity entity)
        {
            var rowAffected = 0;
            _dbConnection.Open();
            using (var transaction= _dbConnection.BeginTransaction())
            {
                var param = MappingDbType(entity);
                //Thực thi các mã lệnh
                //var customers = dbConnection.Query<Customer>("SELECT * FROM View_Customer ORDER BY CreatedDate ASC", commandType: CommandType.Text);
                rowAffected = _dbConnection.Execute($"Proc_Insert{_tableName}", param, commandType: CommandType.StoredProcedure);
                transaction.Commit();
            }
            //Trả về số bản ghi thêm mới được 

            return rowAffected;
        }

        public int Update(TEntity entity)
        {
            var param = MappingDbType(entity);
            //Thực thi các mã lệnh
            var rowAffected = _dbConnection.Execute($"Proc_Update{_tableName}", param, commandType: CommandType.StoredProcedure);
            //Trả về số bản ghi sửa    
            return rowAffected;
        }

        public int Delete(string entityId)
        {
            var rowAffected = 0;
            _dbConnection.Open();
            using (var transaction = _dbConnection.BeginTransaction())
            {
                rowAffected = _dbConnection.Execute($"DELETE FROM {_tableName} WHERE {_tableName}Id = '{entityId}'", commandType: CommandType.Text);
                transaction.Commit();
            }
            return rowAffected;

        }

        public TEntity GetEntityByProperty(TEntity entity, PropertyInfo property)
        {
            var propertyName = property.Name;
            var propertyValue = property.GetValue(entity);
            var keyValue = entity.GetType().GetProperty($"{_tableName}Id").GetValue(entity);
            var query = string.Empty;
            if (entity.EntityState == EntityState.AddNew)
            {
                query = $"SELECT * FROM {_tableName} WHERE {propertyName} = '{propertyValue}'";
            }
            else if (entity.EntityState == EntityState.Update)
            {
                query = $"SELECT * FROM {_tableName} WHERE {propertyName} = '{propertyValue}' AND {_tableName}Id != '{keyValue}'";
            }
            else return null;
            var entityDuplicate = _dbConnection.Query<TEntity>(query, commandType: CommandType.Text).FirstOrDefault();
            return entityDuplicate;
        }

        /// <summary>
        /// Xử lý chuyển kiểu từ GUID sang string
        /// </summary>
        /// <typeparam name="TEntity">Kiểu Generic - kiểu dùng cho tất cả</typeparam>
        /// <param name = "entity" > Thực thể</param>
        /// <returns>Đối tượng sau khi được chuyển kiểu</returns>
        /// CreatedBy: HNANH(26/11/2020)
        private DynamicParameters MappingDbType<TEntity>(TEntity entity)
        {
            var properties = entity.GetType().GetProperties();
            var param = new DynamicParameters();
            foreach (var property in properties)
            {
                var propertyName = property.Name;
                var propertyValue = property.GetValue(entity);
                var propertyType = property.PropertyType;
                if (propertyType == typeof(Guid) || propertyType == typeof(Guid?))
                {
                    param.Add($"@{propertyName}", propertyValue, DbType.String);

                }
                else if (propertyType == typeof(bool) || propertyType == typeof(bool?))
                {
                    propertyValue = ((bool)propertyValue == true) ? 1 : 0;
                    param.Add($"@{propertyName}", propertyValue);
                }
                else
                {
                    param.Add($"@{propertyName}", propertyValue);
                }
            }
            return param;
        }

        public void Dispose()
        {
            if(_dbConnection.State == ConnectionState.Open)
            {
                _dbConnection.Close();
            }
        }
        #endregion
    }
}
