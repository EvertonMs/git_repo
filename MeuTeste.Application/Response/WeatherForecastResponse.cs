using System;
using System.Collections.Generic;
using System.Text;

namespace MeuTeste.Application.Response
{
    public class WeatherForecastResponse : Response
    {

        //public Objetos[] Property1 { get; set; }
        public DateTime date { get; set; }
        public int temperatureC { get; set; }
        public int temperatureF { get; set; }
        public string summary { get; set; }

    }

    //public class Objetos
    //{
        
    //}
}
