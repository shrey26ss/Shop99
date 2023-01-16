using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interface
{
    public interface INotifyService
    {
        Task<IResponse> SaveSMSEmailWhatsappNotification(SMSEmailWhatsappNotification req, int LoginID);

        Task<IResponse<IEnumerable<NotifyModel>>> GetNotify();
    }
}
