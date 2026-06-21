// SPDX-License-Identifier: Apache-2.0
/*
 * JWT access and refresh token data transfer objects.
 *
 * Copyright (c) 2026, MoniePay
 */

using Newtonsoft.Json;
namespace MoniePay.src.models
{
    public class TokenModel
    {
        [JsonProperty("token")]
        public string? Token { get; set; }
        [JsonProperty("refreshToken")]
        public string? RefreshToken { get; set; }

        public class AuthenticationResult : TokenModel
        {
            public bool? Success { get; set; }
            public IEnumerable<string>? Errors { get; set; }
        }
    }
}
