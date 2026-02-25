using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace JosephsSimpleTaxCalculatorY2025
{
    public static class DbHelper//The helper will retrieve the connection string
    {
        public static string Conn => ConfigurationManager.ConnectionStrings["TaxInfo2025"].ConnectionString;
    }
}
