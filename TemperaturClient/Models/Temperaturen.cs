using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace TemperaturClient.Models
{
    public class Temperaturen
    {
        public int temperaturID { get; set; }

        public int sensorID { get; set; }

        public DateTime zeit { get; set; }
        
        public double temperatur { get; set; }

        public double maxtemperatur { get; set; }

        public int temperaturUeberschritten { get; set; }

        public BitmapImage image { get; set; }

    }
}
