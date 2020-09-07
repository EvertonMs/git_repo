using System;
using System.Collections.Generic;
using System.Text;

namespace MeuTeste.Application.Response
{
    public class SomaResponse : Response
    { 
        public string myHash { get; set; }
        public int resultado { get; set; }
        public string mensagem { get; set; }
    }
}
