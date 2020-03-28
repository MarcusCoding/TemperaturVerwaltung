using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using TemperaturClient.Models;

namespace TemperaturClient.Converters
{
    public class ColorConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] is double && values[1] is double)
            {
                double temperatur = (double)values[0];
                double maxTemperatur = (double)values[1];

                if(temperatur > maxTemperatur)
                {
                    return Brushes.IndianRed;
                }
                else
                {
                    return DependencyProperty.UnsetValue;
                }
            }
            else
            {
                return DependencyProperty.UnsetValue;
            }      
        }


        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
