using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace TemperaturClient.Converters
{
    public class ImageConverter : IValueConverter
    {
        public object Convert(object values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values is int)
            {
                int ueberSchritten = (int)values;

                if (ueberSchritten == 1)
                {
                    return new BitmapImage(new Uri("pack://application:,,,/Resources/schließen.png"));
                }
                else
                {
                    return new BitmapImage(new Uri("pack://application:,,,/Resources/ok.png"));
                }
            }
            else
            {
                return new BitmapImage(new Uri("pack://application:,,,/Resources/ok.png"));
            }
        }


        public object ConvertBack(object value, Type targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
