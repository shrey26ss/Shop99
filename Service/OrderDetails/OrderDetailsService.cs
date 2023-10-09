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
                if (!string.IsNullOrEmpty(req.SearchText) && req.SearchText.Length > 7)
                {
                    req.SearchText = req.SearchText.Remove(req.SearchText.Length - 1).Remove(0, 3);
                }
                else
                {
                    req.SearchText = "";
                }
                res.Result = await _dapper.GetAllAsync<OrderDetailsColumn>(sp, new 
                {
                    ID = req.Id,
                    LoginId = loginId,
                    req.StatusID,
                    req.Top,
                    FromDate = req.FromDate ?? "",
                    ToDate = req.ToDate ?? "",
                    SearchText = req.SearchText ?? ""
                }, CommandType.StoredProcedure);
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
        public async Task<IResponse> ChengeStatusAsync(int loginId, OrderDetailsRow req)
        {
            var res = new Response()
            {
                StatusCode = ResponseStatus.Failed,
                ResponseText = ResponseStatus.Failed.ToString()
            };
            try
            {
                if (req.StatusID == StatusType.CancelRequestRejected)
                {
                    if (loginId == 1)
                    {
                        res = await _dapper.GetAsync<Response>("declare @PreviousStatusID int select @PreviousStatusID = PreviousStatusID from Orders where ID =@ID; UPdate Orders set PreviousStatusID = StatusID,StatusID = @PreviousStatusID , Remark = @Remark,@StatusCode = 1,@ResponseText = 'Cancel Request Rejected Successfully!' where ID=@ID; Select @StatusCode StatusCode, @ResponseText ResponseText", new { req.ID, req.StatusID, Remark = req.Remark ?? string.Empty, StatusCode = -1, ResponseText = "Failed" }, CommandType.Text);
                    }
                    else
                    {
                        res.StatusCode = ResponseStatus.Failed;
                        res.ResponseText = "Invalid access!";
                        return res;
                    }
                }
                if (req.StatusID == StatusType.CancelRequest || req.StatusID == StatusType.ReturnCanceled)
                {
                    res = await _dapper.GetAsync<Response>("UPdate Orders set PreviousStatusID = StatusID, StatusID = @StatusID, Remark = @Remark,@StatusCode = 1,@ResponseText = 'Cancel Requested Successfully!' where ID=@ID; Select @StatusCode StatusCode, @ResponseText ResponseText",
                       new { req.ID, req.StatusID, Remark = req.Remark ?? string.Empty, StatusCode = -1, ResponseText = "Failed" },
                       CommandType.Text);
                }
                else if (req.StatusID == StatusType.Cancel)
                {
                    string sp = "proc_OrderCancel";
                    res = await _dapper.GetAsync<Response>(sp, new { req.ID, req.StatusID, Remark = req.Remark ?? string.Empty, LoginID = loginId }, CommandType.StoredProcedure);
                }
                else if (req.StatusID == StatusType.Delivered)
                {
                    string sp = "proc_OrderShipped";
                    res = await _dapper.GetAsync<Response>(sp, new { req.ID, req.StatusID, Remark = req.Remark ?? string.Empty, LoginID = loginId }, CommandType.StoredProcedure);
                }
                else if (req.StatusID == StatusType.OrderCompleted)
                {
                    string sp = "proc_OrderCompleted";
                    res = await _dapper.GetAsync<Response>(sp, new { req.ID, req.StatusID, Remark = req.Remark ?? string.Empty, LoginID = loginId }, CommandType.StoredProcedure);
                }
                else if (req.StatusID == StatusType.OrderReplaceInitiated)
                {
                    res = await _dapper.GetAsync<Response>("UPDATE Orders SET PreviousStatusID = StatusID, StatusID=@StatusID,@StatusCode = 1,@ResponseText = 'Replace Initiated Successfully',ReturnRemark=@Remark  Where ID=@ID; Select @StatusCode StatusCode, @ResponseText ResponseText", new { req.ID, req.StatusID, Remark = req.Remark ?? string.Empty, StatusCode = -1, ResponseText = "Failed" }, CommandType.Text);
                }
                else if (req.StatusID == StatusType.ReplaceRejected)
                {
                    if (loginId == 1)
                    {
                        res = await _dapper.GetAsync<Response>("UPDATE Orders SET PreviousStatusID = StatusID, StatusID=@StatusID,@StatusCode = 1,@ResponseText = 'Replace Rejected Successfully',ReturnRemark=@Remark  Where ID=@ID; Select @StatusCode StatusCode, @ResponseText ResponseText", new { req.ID, req.StatusID, Remark = req.Remark ?? string.Empty, StatusCode = -1, ResponseText = "Failed" }, CommandType.Text);
                    }
                    else
                    {
                        res.StatusCode = ResponseStatus.Failed;
                        res.ResponseText = "Invalid access!";
                        return res;
                    }

                }
                else if (req.StatusID == StatusType.ReplacementAccepted)
                {
                    if (loginId == 1)
                    {
                        res = await _dapper.GetAsync<Response>("UPDATE Orders SET PreviousStatusID = StatusID, StatusID=@StatusID,@StatusCode = 1,@ResponseText = 'Order Replacement Accepted Successfully',ReturnRemark=@Remark  Where ID=@ID; Select @StatusCode StatusCode, @ResponseText ResponseText", new { req.ID, req.StatusID, Remark = req.Remark ?? string.Empty, StatusCode = -1, ResponseText = "Failed" }, CommandType.Text);
                    }
                    else
                    {
                        res.StatusCode = ResponseStatus.Failed;
                        res.ResponseText = "Invalid access!";
                        return res;
                    }
                }
                else if (req.StatusID == StatusType.OrderReplaced)
                {
                    res = await _dapper.GetAsync<Response>("UPDATE Orders SET PreviousStatusID = StatusID, StatusID=@StatusID,@StatusCode = 1,@ResponseText = 'Order Replaced Successfully',ReturnRemark=@Remark  Where ID=@ID; Select @StatusCode StatusCode, @ResponseText ResponseText", new { req.ID, req.StatusID, Remark = req.Remark ?? string.Empty, StatusCode = -1, ResponseText = "Failed" }, CommandType.Text);
                }
                else if (req.StatusID == StatusType.ReturnReceived)
                {
                    res = await _dapper.GetAsync<Response>("UPDATE Orders SET PreviousStatusID = StatusID, StatusID=@StatusID,@StatusCode = 1,@ResponseText = 'Return Recived Successfully',ReturnRemark=@Remark  Where ID=@ID; update ReturnAndReplaceProducts set StatusID = @StatusID,UpdatedOn = Getdate() where OrderID = @ID; Select @StatusCode StatusCode, @ResponseText ResponseText", new { req.ID, req.StatusID, Remark = req.Remark ?? string.Empty, StatusCode = -1, ResponseText = "Failed" }, CommandType.Text);
                }
                else if (req.StatusID == StatusType.Confirmed)
                {
                    if (loginId == 1)
                    {
                        res = await _dapper.GetAsync<Response>("UPDATE Orders SET PreviousStatusID = StatusID, StatusID=@StatusID,InvoiceNo = @InvoiceNumber ,@StatusCode = 1,@ResponseText = 'Order Confirmed Successfully',ReturnRemark=@ReturnRemark Where ID=@ID; Select @StatusCode StatusCode, @ResponseText ResponseText", new { req.ID, req.StatusID, req.InvoiceNumber, ReturnRemark = req.Remark ?? string.Empty, LoginID = loginId, StatusCode = -1, ResponseText = "Failed" }, CommandType.Text);
                    }
                    else
                    {
                        res.StatusCode = ResponseStatus.Failed;
                        res.ResponseText = "Invalid access!";
                        return res;
                    }
                }
                else
                {
                    res = await _dapper.GetAsync<Response>("UPDATE Orders SET PreviousStatusID = StatusID, StatusID=@StatusID, SourceImage = @SourceImage, @StatusCode = 1,@ResponseText = 'Return Initiated Successfully',ReturnRemark=@ReturnRemark Where ID=@ID; Select @StatusCode StatusCode, @ResponseText ResponseText", new { req.ID, req.StatusID, ReturnRemark = req.Remark ?? string.Empty, LoginID = loginId, StatusCode = -1, ResponseText = "Failed", SourceImage = req.ImagePaths }, CommandType.Text);
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
            catch (Exception ex)
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
        public async Task<IResponse<OrderInvoice>> GetInvoiceDetails(int Id)
        {
            string sp = "ProcGetOrderInvoice";
            var res = new Response<OrderInvoice>();
            try
            {
                res.Result = await _dapper.GetAsync<OrderInvoice>(sp, new { Id }, CommandType.StoredProcedure);
                res.StatusCode = ResponseStatus.Success;
                res.ResponseText = "";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return res;
        }
        public async Task<IResponse> OrderReplacedConform(OrderReplacedConformReq req)
        {
            var res = new Response();
            try
            {
                string sqlQuery = @"ProcOrderReplacedConform";
                res = await _dapper.GetAsync<Response>(sqlQuery, new
                {
                    req.ID,
                    req.Role,
                    req.StatusID
                }, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return res;
        }
        public async Task<UsersOrderTrakingViewModel> GetUsersOrderTraking(OrderReplacedConformReq req)
        {
            var res = new UsersOrderTrakingViewModel();
            try
            {
                string sqlQuery = "proc_GetOrderTrackingVariantDetails";
                res.usersOrderTrakingRes = await _dapper.GetAsync<UsersOrderTrakingRes>(sqlQuery, new
                {
                    req.ID
                }, CommandType.StoredProcedure);
                if (res.usersOrderTrakingRes == null)
                {
                    res.usersOrderTrakingRes= new UsersOrderTrakingRes();
                }
                string Query = @";WITH CTE AS (
                                            SELECT DISTINCT st.StatusType, dbo.CustomFormat(ot.CreatedOn) EntryOn
                                            FROM StatusTypes st (NOLOCK)
                                            LEFT JOIN OrderTimeline ot (NOLOCK) ON ot.StatusID = st.Id
                                            WHERE OrderID = @ID AND st.IsShowTimeLine = 1
                                            GROUP BY st.StatusType,ot.CreatedOn
                                        )
                                        SELECT st.Id, c.StatusType, c.EntryOn
                                        FROM CTE c
                                        LEFT JOIN StatusTypes st (NOLOCK) ON c.StatusType = st.StatusType
                                        ORDER BY c.EntryOn";
                res.OrderTimeline = await _dapper.GetAllAsync<OrderTimeline>(Query, new
                {
                    req.ID
                }, CommandType.Text);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return res;
        }
        public async Task<IResponse<IEnumerable<ReturnRequestList>>> GetReturnRequest(dynamic T = null)
        {
            string sp = @"select os.ID,vg.Thumbnail,ps.Name,ps.Title,st.StatusType,os.Qty,os.Rate,os.MRP,os.EntryOn,pm.Name as PaymentMode from ReturnAndReplaceProducts rp
                  inner join VariantGroup vg on rp.VarriantID = vg.Id
                  inner join Products ps on vg.ProductId = ps.Id
                  inner join Orders os on rp.OrderID = os.ID
                  inner join PaymentMode pm on os.PaymentMode = pm.ID
                  inner join StatusTypes st on rp.StatusID = st.Id
				  where (rp.StatusID = @StatusID or @StatusID = 0)";
            var res = new Response<IEnumerable<ReturnRequestList>>();
            try
            {
                var req = (OrderDetailsRequest)T;
                res.Result = await _dapper.GetAllAsync<ReturnRequestList>(sp, new { req.StatusID }, CommandType.Text);
                res.StatusCode = ResponseStatus.Success;
                res.ResponseText = "";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return res;
        }
        public async Task<IResponse<ReturnRequestList>> GetReturnRequestByOrderId(OrderDetailsRequest req)
        {
            string sp = @"select os.ID,vg.Thumbnail,ps.Name,st.StatusType,os.Qty,os.Rate,os.MRP,os.EntryOn,pm.Name as PaymentMode,os.SourceImage,os.ReturnRemark Remark from ReturnAndReplaceProducts rp
                  inner join VariantGroup vg on rp.VarriantID = vg.Id
                  inner join Products ps on vg.ProductId = ps.Id
                  inner join Orders os on rp.OrderID = os.ID
                  inner join PaymentMode pm on os.PaymentMode = pm.ID
                  inner join StatusTypes st on rp.StatusID = st.Id
				  where rp.OrderID = @Id";
            var res = new Response<ReturnRequestList>();
            try
            {
                res.Result = await _dapper.GetAsync<ReturnRequestList>(sp, new { req.Id }, CommandType.Text);
                res.StatusCode = ResponseStatus.Success;
                res.ResponseText = "";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return res;
        }
        public async Task<IResponse<IEnumerable<OrderGSTDetails>>> GetOrderGST(int loginId = 0, dynamic T = null)
        {
            string sp = "Proc_OrderGSTDetails";
            var res = new Response<IEnumerable<OrderGSTDetails>>();
            try
            {
                var req = (OrderDetailsRequest)T;
                if (!string.IsNullOrEmpty(req.SearchText) && req.SearchText.Length > 7)
                {
                    req.SearchText = req.SearchText.Remove(req.SearchText.Length - 1).Remove(0, 3);
                }
                else
                {
                    req.SearchText = "";
                }
                res.Result = await _dapper.GetAllAsync<OrderGSTDetails>(sp, new
                {
                    LoginId = loginId,
                    req.StatusID,
                    req.Top,
                    FromDate = req.FromDate ?? "",
                    ToDate = req.ToDate ?? "",
                    SearchText = req.SearchText ?? ""
                }, CommandType.StoredProcedure);
                res.StatusCode = ResponseStatus.Success;
                res.ResponseText = "";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return res;
        }
    }
}
