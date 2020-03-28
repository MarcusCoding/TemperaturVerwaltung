using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemperaturClient.Helpers
{
    public class Converter
    {

        public int ConvertToInt(string value)
        {
            int retVal = -1;
            if(Int32.TryParse(value, out retVal))
            {
                return retVal;
            }
            else
            {
                return retVal;
            }
        }

        public double ConvertToDouble(string value)
        {
            double retVal = -1;
            if (double.TryParse(value, out retVal))
            {
                return retVal;
            }
            else
            {
                return retVal;
            }
        }

        public string ConvertISOToDate(string date)
        {
            if (!string.IsNullOrEmpty(date))
            {
                return date.Substring(6, 2) + "." + date.Substring(4, 2) + "." + date.Substring(0, 4);
            } 
            else
            {
                return String.Empty;
            }
        }

    }
}
