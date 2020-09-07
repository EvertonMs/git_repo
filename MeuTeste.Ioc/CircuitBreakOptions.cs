using System;
using System.Collections.Generic;
using System.Text;

namespace MeuTeste.Ioc
{
    public class CircuitBreakOptions
    {
        public int CircuitBreakerTimeoutInSeconds { get; set; }
        public int DurationOfBreakInSeconds { get; set; }
        public int NumberOfExceptionsAllowedBeforeBreaking { get; set; }
        public int TotalResponseTimeoutInSeconds { get; set; }
    }
}
