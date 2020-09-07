//using Easynvest.MGM.Application.Policy;
//using Easynvest.MGM.CrossCutting;
using MeuTeste.CrossCutting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Timeout;
using System;

namespace MeuTeste.Ioc
{
    public static class PolicesExtension
    {
        public static IServiceCollection AddPollyPolices(this IServiceCollection services, IConfiguration configuration)
        {
            var config = new CircuitBreakOptions();
            configuration.GetSection("HttpPolicySettings").Bind(config);
            config.CircuitBreakerTimeoutInSeconds = 10;
            config.DurationOfBreakInSeconds = 11;
            config.NumberOfExceptionsAllowedBeforeBreaking = 2;
            config.TotalResponseTimeoutInSeconds = 3;

            var timeoutTryPolicy = Policy.TimeoutAsync(config.CircuitBreakerTimeoutInSeconds, TimeoutStrategy.Pessimistic);

            var circuitBreakerPolicy = Policy.Handle<Exception>()
                .CircuitBreakerAsync(
                    config.NumberOfExceptionsAllowedBeforeBreaking,
                    TimeSpan.FromSeconds(config.DurationOfBreakInSeconds),
                    OnBreak,
                    OnReset
                )
                .WrapAsync(timeoutTryPolicy);

            var waitAndRetryPolicy = Policy.Handle<Exception>()
                .WaitAndRetryAsync(
                    2,
                    retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    onRetry: OnRetry
                )
                .WrapAsync(circuitBreakerPolicy);

            Policy.TimeoutAsync(config.TotalResponseTimeoutInSeconds, TimeoutStrategy.Pessimistic)
                .WrapAsync(waitAndRetryPolicy);

            services.AddPolicyRegistry();

            return services;
        }

        private static void OnBreak(Exception exception, TimeSpan timespan, Context context)
        {
            var logger = context[PolicyContextKeys.LOGGER_KEY] as ILogger;
            logger.LogWarning(exception, "CircuitBreaker: o repositório está fora!");
        }

        private static void OnReset(Context context)
        {
            var logger = context[PolicyContextKeys.LOGGER_KEY] as ILogger;
            logger.LogWarning("CircuitBreaker: o repositório está de volta!");
        }

        private static void OnRetry(Exception exception, TimeSpan timeSpan, int retries, Context context)
        {
            var logger = context[PolicyContextKeys.LOGGER_KEY] as ILogger;
            var accountId = context[PolicyContextKeys.ACCOUNT_ID_KEY] as string;

            logger.LogWarning(exception, "WaitAndRetry: Tentativa {RetryCount}, Espera: {WaitTime} seg., cliente {AccountId}", retries, timeSpan.TotalSeconds, accountId);
        }
    }
}
