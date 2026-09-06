using MoniePay.src.dto;

namespace MoniePay.src.services
{

    public interface IDepositService
    {
 
        Task<PaymentIntentResponse> CreatePaymentIntent(CreatePaymentIntentRequest? request, Guid authenticatedUserId);
    }

    public class DepositService
    {

    }
}
