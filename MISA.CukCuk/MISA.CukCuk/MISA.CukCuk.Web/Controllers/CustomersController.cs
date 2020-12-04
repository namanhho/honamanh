using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Dapper;
using System.Data;
using MySql.Data.MySqlClient;
using MISA.ApplicationCore;
using MISA.ApplicationCore.Interfaces;
using MISA.ApplicationCore.Entities;
using MISA.ApplicationCore.Enums;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MISA.CukCuk.Web.Controllers
{
    /// <summary>
    /// Api Danh mục Khách hàng
    /// CreatedBy HNANH (24/11/2020)
    /// </summary>
    [Route("api/v1/customers")]
    [ApiController]
    public class CustomersController: EntitiesController<Customer>
    {
        #region Declare
        //IBaseService<Customer> _baseService;
        ICustomerService _customerService;
        #endregion
        #region Constructor
        public CustomersController(ICustomerService customerService) :base(customerService)
        {
            //_baseService = baseService;
            _customerService = customerService;
        }
        #endregion
        
    }
}
