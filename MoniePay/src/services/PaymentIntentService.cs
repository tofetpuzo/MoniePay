using MoniePay.src.dto;

namespace MoniePay.src.services
{
    public class PaymentIntentService
    {
        private readonly CustomerService customerService;
        public PaymentIntentService(CustomerService service)
        {
            this.customerService = service;
        }

        public CreatePaymentIntentRequest createPaymentIntent(CreatePaymentIntentRequest? paymentIntent)
        {
            // check if customer exist
            ArgumentNullException.ThrowIfNull(paymentIntent);

            var customer = customerService.GetCustomer(paymentIntent.customerId);
            customer?.Wait();
            // method to check balance

            // check the currency conversion see if the rate is the same rate

            // 

        }
    }

}
