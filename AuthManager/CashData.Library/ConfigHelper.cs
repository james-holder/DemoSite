using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashData.Library
{
    public class ConfigHelper
    {
        public static decimal GetTaxRate()
        {
            string rateText = ConfigurationManager.AppSettings["taxRate"];

            bool isValidRate = Decimal.TryParse(rateText, out decimal output);

            if(isValidRate == false)
            {
                throw new ConfigurationErrorsException("The tax rate is not set up properly");
            }

            return output;
        }
    }
}
