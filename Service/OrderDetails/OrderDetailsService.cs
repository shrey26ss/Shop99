using AppUtility.Helper;
using Data;
using Entities.Enums;
using Entities.Models;
using Infrastructure.Interface;
using Microsoft.Extensions.Logging;
using Service.Address;
using Service.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Service.OrderDetails
{
    public class OrderDetailsService : IOrderDetailsService
    {
        public readonly IDapperRepository _dapper;
        public readonly ILogger<OrderDetailsService> _logger;
        public OrderDetailsService(IDapperRepository dapper, ILogger<OrderDetailsService> logger)
        {
            _dapper = dapper;
            _logger = logger;
        }

        public Task<IResponse> AddAsync(RequestBase<OrderDetailsRow> entity)
        {
            throw new NotImplementedException();
        }

        public Task<IResponse> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IResponse<IEnumerable<OrderDetailsColumn>>> GetAsync(int loginId = 0, dynamic T = null)
        {
            string sp = "Proc_OrderdDetails";            
            var res = new Response<IEnumerable<OrderDetailsColumn>>();
            try
            {
                var req = (OrderDetailsRequest)T;
                res.Result = await _dapper.GetAllAsync<OrderDetailsColumn>(sp, new { LoginId = loginId, req.StatusID, req.Top }, CommandType.StoredProcedure);
                res.StatusCode = ResponseStatus.Success;
                res.ResponseText = "";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return res;
        }

        public Task<IResponse<OrderDetailsRow>> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
        public async Task<IResponse> ChengeStatusAsync(OrderDetailsRow req)
        {
            var res = new Response();
            try
            {
                string sqlQuery = @"Update Orders Set StatusID = @StatusID where ID = @ID";
                int i = -5;
                i = await _dapper.ExecuteAsync(sqlQuery, new
                {
                    req.ID,
                    req.StatusID
                }, CommandType.Text);
                var description = Utility.O.GetErrorDescription(i);
                if (i > 0 && i < 10)
                {
                    res.StatusCode = ResponseStatus.Success;
                    res.ResponseText = ResponseStatus.Success.ToString();
                }
                else
                {
                    res.ResponseText = description;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return res;
        }

        public async Task<IResponse<IEnumerable<OrderDetailsColumn>>> GetAsync<OrderDetailsColumn>(int loginId = 0, Expression<Func<OrderDetailsColumn, bool>> predicate = null)
        {
            var res = new Response<IEnumerable<OrderDetailsColumn>>();
            try
            {
                // {x => (Convert(x.StatusID, Int32) == Convert(value(WebAPI.Controllers.OrderDetailsController+<>c__DisplayClass2_0).req.StatusID, Int32))}
                var translator = new MyQueryTranslator();
                StringBuilder whereClasue = translator.Translate(predicate);
                whereClasue.Replace("(", "").Replace(")", "");
                var param = whereClasue.ToString().Split(" AND ");
                foreach (var item in param)
                {

                }
                string sp = $"Select * from Order Where {translator.Translate(predicate)}";
                try
                {
                    res.Result = await _dapper.GetAllAsync<OrderDetailsColumn>(sp, null, CommandType.Text);
                    res.StatusCode = ResponseStatus.Success;
                    res.ResponseText = "";
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }            
            return res;
        }

        public async Task<IResponse> UpdateShippingNInvoice(OrderShippedStatus req)
        {
            var res = new Response();
            try
            {
                string sqlQuery = @"Update Orders Set StatusID =@StatusID,InvoiceNo = @InvoiceNumber,DocketNo= @TrackingId where ID = @Id";
                int i = -5;
                i = await _dapper.ExecuteAsync(sqlQuery, new
                {
                    req.Id,
                    req.TrackingId,
                    req.InvoiceNumber,
                    StatusID = StatusType.Shipped
                }, CommandType.Text);
                var description = Utility.O.GetErrorDescription(i);
                if (i > 0 && i < 10)
                {
                    res.StatusCode = ResponseStatus.Success;
                    res.ResponseText = ResponseStatus.Success.ToString();
                }
                else
                {
                    res.ResponseText = description;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return res;
        }
    }
}
