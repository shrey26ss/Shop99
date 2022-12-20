using Service.Models;
using System.Threading.Tasks;
using static PaymentGateWay.PaymentGateway.CashFree.Models;

namespace PaymentGateWay.PaymentGateway
{
    public interface IPaymentGatewayService
    {
        Task<Response<PaymentGatewayResponse>> GeneratePGRequestForWebAsync(PaymentGatewayRequest request);
        Task<Response<CashFreeResponseForApp>> GeneratePGRequestForAppAsync(PaymentGatewayRequest request);
        Task<Response<int>> SaveInitiatePayment(PaymentGatewayRequest request, int packageId);
        Task<Response<StatusCheckResponse>> StatusCheck(StatusCheckRequest request);
        Task<Response<PaymentGatewayRequest>> GetInitiatedPaymentDetail(int TID);
        Task<Response> updateInitiatedPayment(int TID, string status);
    }
}
