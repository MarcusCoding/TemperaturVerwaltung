using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace TemperaturAdmin.Converters
{
    public class TempConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value != null)
            {
                if (Properties.Settings.Default.TemperaturTyp.Contains("°F"))
                {
                    //Konvertierung zu Fahrenheit
                    if(value is double)
                    {
                        double temperatur = (double)value;
                        return ((temperatur * 1.8) + 32) + " " + Properties.Settings.Default.TemperaturTyp;
                    }
                    return value;
                }
                else
                {
                    return value + " " + Properties.Settings.Default.TemperaturTyp;
                }
            }
            else
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
