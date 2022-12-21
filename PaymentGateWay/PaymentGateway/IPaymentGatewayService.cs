using Entities.Models;
using Service.Models;
using System.Threading.Tasks;
using static PaymentGateWay.PaymentGateway.CashFree.Models;

namespace PaymentGateWay.PaymentGateway
{
    public interface IPaymentGatewayService
    {
        Task<ResponsePG<PaymentGatewayResponse>> GeneratePGRequestForWebAsync(PaymentGatewayRequest request);
        Task<ResponsePG<CashFreeResponseForApp>> GeneratePGRequestForAppAsync(PaymentGatewayRequest request);
        Task<ResponsePG<int>> SaveInitiatePayment(PaymentGatewayRequest request, int packageId);
        Task<ResponsePG<StatusCheckResponse>> StatusCheck(StatusCheckRequest request);
        Task<ResponsePG<PaymentGatewayRequest>> GetInitiatedPaymentDetail(int TID);
        Task<ResponsePG> updateInitiatedPayment(int TID, string status);
    }
}
