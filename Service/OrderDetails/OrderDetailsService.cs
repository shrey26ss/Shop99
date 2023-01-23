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
                    res = await _dapper.GetAsync<Response>("declare @PreviousStatusID int select @PreviousStatusID = PreviousStatusID from Orders where ID =@ID; UPdate Orders set PreviousStatusID = StatusID,StatusID = @PreviousStatusID , Remark = @Remark,@StatusCode = 1,@ResponseText = 'Cancel Request Rejected Successfully!' where ID=@ID; Select @StatusCode StatusCode, @ResponseText ResponseText", new { req.ID, req.StatusID, Remark = req.Remark ?? string.Empty, StatusCode = -1, ResponseText = "Failed" }, CommandType.Text);
                }
                if (req.StatusID == StatusType.CancelRequest || req.StatusID == StatusType.ReturnCanceled)
                {
                    res = await _dapper.GetAsync<Response>("UPdate Orders set PreviousStatusID = StatusID, StatusID = @StatusID, Remark = @Remark,@StatusCode = 1,@ResponseText = 'Cancel Requested Successfully!' where ID=@ID; Select @StatusCode StatusCode, @ResponseText ResponseText", new { req.ID, req.StatusID, Remark = req.Remark ?? string.Empty, StatusCode = -1, ResponseText = "Failed" }, CommandType.Text);
                }
                else if (req.StatusID == StatusType.Cancel)
                {
                    string sp = "proc_OrderCancel";
                    res = await _dapper.GetAsync<Response>(sp, new { req.ID, req.StatusID, Remark = req.Remark ?? string.Empty, LoginID = loginId }, CommandType.StoredProcedure);
                }
                else if (req.StatusID == StatusType.OrderReplaceInitiated)
                {
                    res = await _dapper.GetAsync<Response>("UPDATE Orders SET StatusID=@StatusID,@StatusCode = 1,@ResponseText = 'Replace Initiated Successfully',ReturnRemark=@Remark  Where ID=@ID; Select @StatusCode StatusCode, @ResponseText ResponseText", new { req.ID, req.StatusID, Remark = req.Remark ?? string.Empty, StatusCode = -1, ResponseText = "Failed" }, CommandType.Text);
                }
                else if (req.StatusID == StatusType.ReplaceRejected)
                {
                    res = await _dapper.GetAsync<Response>("UPDATE Orders SET StatusID=@StatusID,@StatusCode = 1,@ResponseText = 'Replace Rejected Successfully',ReturnRemark=@Remark  Where ID=@ID; Select @StatusCode StatusCode, @ResponseText ResponseText", new { req.ID, req.StatusID, Remark = req.Remark ?? string.Empty, StatusCode = -1, ResponseText = "Failed" }, CommandType.Text);
                }
                else if (req.StatusID == StatusType.ReplacementAccepted)
                {
                    res = await _dapper.GetAsync<Response>("UPDATE Orders SET StatusID=@StatusID,@StatusCode = 1,@ResponseText = 'Order Replacement Accepted Successfully',ReturnRemark=@Remark  Where ID=@ID; Select @StatusCode StatusCode, @ResponseText ResponseText", new { req.ID, req.StatusID, Remark = req.Remark ?? string.Empty, StatusCode = -1, ResponseText = "Failed" }, CommandType.Text);
                }
                else if (req.StatusID == StatusType.OrderReplaced)
                {
                    res = await _dapper.GetAsync<Response>("UPDATE Orders SET StatusID=@StatusID,@StatusCode = 1,@ResponseText = 'Order Replaced Successfully',ReturnRemark=@Remark  Where ID=@ID; Select @StatusCode StatusCode, @ResponseText ResponseText", new { req.ID, req.StatusID, Remark = req.Remark ?? string.Empty, StatusCode = -1, ResponseText = "Failed" }, CommandType.Text);
                }
                else if (req.StatusID == StatusType.ReturnReceived)
                {
                    res = await _dapper.GetAsync<Response>("UPDATE Orders SET StatusID=@StatusID,@StatusCode = 1,@ResponseText = 'Return Recived Successfully',ReturnRemark=@Remark  Where ID=@ID; update ReturnAndReplaceProducts set StatusID = @StatusID,UpdatedOn = Getdate() where OrderID = @ID; Select @StatusCode StatusCode, @ResponseText ResponseText", new { req.ID, req.StatusID, Remark = req.Remark ?? string.Empty, StatusCode = -1, ResponseText = "Failed" }, CommandType.Text);
                }
                else if(req.StatusID == StatusType.Confirmed)
                {
                    res = await _dapper.GetAsync<Response>("UPDATE Orders SET StatusID=@StatusID,InvoiceNo = @InvoiceNumber ,@StatusCode = 1,@ResponseText = 'Order Confirmed Successfully',ReturnRemark=@ReturnRemark Where ID=@ID; Select @StatusCode StatusCode, @ResponseText ResponseText", new { req.ID, req.StatusID, req.InvoiceNumber, ReturnRemark = req.Remark ?? string.Empty, LoginID = loginId, StatusCode = -1, ResponseText = "Failed" }, CommandType.Text);
                }
                else
                {
                    res = await _dapper.GetAsync<Response>("UPDATE Orders SET StatusID=@StatusID,@StatusCode = 1,@ResponseText = 'Return Initiated Successfully',ReturnRemark=@ReturnRemark Where ID=@ID; Select @StatusCode StatusCode, @ResponseText ResponseText", new { req.ID, req.StatusID, ReturnRemark = req.Remark ?? string.Empty, LoginID = loginId, StatusCode = -1, ResponseText = "Failed" }, CommandType.Text);
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
            string sp = @"Select o.InvoiceNo,ua.FullName,u.Email,ua.MobileNo, (ua.HouseNo + ' '+ ua.Area + ' ' + ua.Landmark + ' ' + ua.TownCity  + ' , ' + s.StateName + ' , ' + ua.Pincode) ShippingAddress,o.InvoiceNo,o.InvoiceDate,o.EntryOn OrderDate, vg.Title,o.Rate,o.MRP,o.DocketNo,o.Qty,
vp.ContactNo,vp.ShopName,vp.[Address] VendorAddress,vs.StateName VendorState,vu.Email VendorEmail,o.IGST,o.CGST,o.SGST,
stuff((    
  select ',' + aiu.AttributeValue    
  from AttributeInfo aiu    
  where aiu.GroupId = vg.Id    
  for xml path('')    
 ),1,1,'') Attributes  
from Orders o(nolock) 
inner join UserAddress ua(nolock) on ua.Id = o.ShippingAddressID
inner join VariantGroup vg(nolock) on vg.Id = o.VarriantID
inner join Users u(nolock) on u.Id = o.UserID
inner join States s(nolock) on s.Id = ua.StateId
left join VendorProfile vp(nolock) on vp.Id = vg.EntryBy
left join States vs(nolock) on vs.Id = vp.StateId
inner join Users vu(nolock) on vu.Id = vg.EntryBy
where o.ID = @Id";
            var res = new Response<OrderInvoice>();
            try
            {
                res.Result = await _dapper.GetAsync<OrderInvoice>(sp, new { Id }, CommandType.Text);
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
                string sqlQuery = @"select o.ID,u.Name as VendorName,CONVERT(varchar,otl.CreatedOn,106) CreatedOn,o.StatusID,v.Thumbnail,u.Email,v.Title,v.MRP,v.SellingCost,ua.FullName,ua.MobileNo,ua.Pincode,ua.HouseNo,ua.Area,ua.Landmark,ua.TownCity,s.StateName from Orders o inner join VariantGroup v on o.VarriantID = v.Id 
                   inner join UserAddress ua on o.ShippingAddressID = ua.Id
                   inner join States s on ua.StateID = s.Id 
                   inner join VendorProfile vp on  o.VendorID = vp.Id
                   inner join Users u on vp.UserId = u.Id 
                   inner join OrderTimeline otl  on o.ID = otl.OrderID where o.ID = @ID;";
                res.usersOrderTrakingRes = await _dapper.GetAsync<UsersOrderTrakingRes>(sqlQuery, new
                {
                    req.ID
                }, CommandType.Text);

                string Query = @"SELECT st.Id, st.StatusType,Convert(varchar(11),ot.CreatedOn,106) EntryOn FROM StatusTypes st(nolock) 
left Join OrderTimeline ot(nolock) on ot.StatusID = st.Id Where OrderID = @ID and st.IsShowTimeLine = 1 order by st.Id";
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
    }
}
