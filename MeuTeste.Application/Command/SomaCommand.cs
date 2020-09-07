using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using MediatR;
using MeuTeste.Application.Response;


namespace MeuTeste.Application.Command
{
    public class SomaCommand : IRequest<SomaResponse>
    {
        public SomaCommand(int a, int b)
        {
            Valor1 = a;
            Valor2 = b;
        }

        public int Valor1 { get; set; }
        public int Valor2 { get; set; }
    }
}
