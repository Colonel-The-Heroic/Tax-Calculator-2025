using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JosephsSimpleTaxCalculatorY2025
{
    public class FederalTaxBracket
    {
        public int BracketId;
        public int FilingStatusId;
        public decimal MinValue;
        public decimal? MaxValue;
        public decimal TaxRate;
    }
}
