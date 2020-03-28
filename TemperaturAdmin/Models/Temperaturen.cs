using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace TemperaturAdmin.Models
{
    public class Temperaturen
    {
        public int temperaturID { get; set; }

        public int sensorID { get; set; }

        public DateTime zeit { get; set; }
        
        public double temperatur { get; set; }

    }
}
