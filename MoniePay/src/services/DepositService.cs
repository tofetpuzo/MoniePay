// SPDX-License-Identifier: Apache-2.0
/*
 * Customer onboarding: creates the Identity user and the customer profile
 * atomically.
 *
 * Copyright (c) 2026, MoniePay
 */

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
