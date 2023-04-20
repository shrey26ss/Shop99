using Data;
using Entities.Models;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.CartWishList;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.AppCode;

namespace WebAPI.Controllers
{
    [CanBePaused]
    [ApiExplorerSettings]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly string Connectionstring;
        private IDapperRepository _dapper;
        private readonly IPGCallback _pgService;
        public TaskController(IConnectionString connectionString, IDapperRepository dapper, IPGCallback pgService)
        {
            Connectionstring = connectionString.connectionString;
            _dapper = dapper;
            _pgService = pgService;
        }

        private IActionResult PauseTask()
        {
            var storage = new SqlServerStorage(Connectionstring);
            using (var connection = storage.GetConnection())
            {
                connection.Pause(typeof(TaskController));
            }
            return Ok("Task paused");
        }

        private IActionResult ResumeTask()
        {
            var storage = new SqlServerStorage(Connectionstring);
            using (var connection = storage.GetConnection())
            {
                connection.Resume(typeof(TaskController));
            }
            return Ok("Task Resumed");
        }

        [AllowAnonymous]
        [HttpGet("/ScheduleStatusCheck")]
        public IActionResult ScheduleStatusCheck()
        {
            try
            {
                string cronExpressionEvery_10_Sec = "0/10 * * * * *";
                string cronExpressionEvery_1_Min = "* * * * *";
                string cronExpressionEvery_30_Min = "0 */30 * * * ?";
                RecurringJob.AddOrUpdate(() => CheckPendingStatusOfOrder(), cronExpressionEvery_30_Min);
                return Ok("Scheduled");
            }
            catch (Exception ex)
            {
                return Ok("Something went wrong");
            }
        }
        [AllowAnonymous]
        [HttpPost("/CheckPendingStatusOfOrder")]
        public async Task CheckPendingStatusOfOrder()
        {
            var res = await _dapper.GetAllAsync<PaymentDetails>(@"SELECT TID,PGID,Amount,UserId,EntryOn,ModifyOn,Status,UTR,RefrenceID,DebitedWalletAmount FROM InitiatePayment (nolock) where Status = 'P'", null, System.Data.CommandType.Text);
            if(res != null && res.Count() > 0)
            {
                foreach(var item in res)
                {
                    await _pgService.TransactionStatuscheck(new RequestBase<TransactionStatusRequest> { Data = new TransactionStatusRequest
                    {
                        TID = item.TID,
                        Status = item.Status
                    } });
                }
            }
        }


        [AllowAnonymous]
        [HttpPost("/statusCheck")]
        public async Task statusCheck(int tid)
        {
            var res = await _dapper.GetAllAsync<PaymentDetails>(@"SELECT TID,PGID,Amount,UserId,EntryOn,ModifyOn,Status,UTR,RefrenceID,DebitedWalletAmount FROM InitiatePayment (nolock) where Status = 'P' and TID = @tid", new { tid}, System.Data.CommandType.Text);
            if (res != null && res.Count() > 0)
            {
                foreach (var item in res)
                {
                    await _pgService.TransactionStatuscheck(new RequestBase<TransactionStatusRequest>
                    {
                        Data = new TransactionStatusRequest
                        {
                            TID = item.TID,
                            Status = item.Status
                        }
                    });
                }
            }
        }
    }
}
