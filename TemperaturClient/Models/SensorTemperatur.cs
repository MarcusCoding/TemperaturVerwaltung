using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemperaturClient.Models
{
    public class SensorTemperatur
    {
        public int temperaturID { get; set; }

        public int sensorID { get; set; }

        public DateTime zeit { get; set; }

        public double temperatur { get; set; }

        public double maxtemperatur { get; set; }

        public int sensorNr { get; set; }

        public int serverschrank { get; set; }

        public string adresse { get; set; }

        public int herstellerNr { get; set; }

        public double maxTemperatur { get; set; }

        public int herstellerID { get; set; }

        public string herstellerName { get; set; }

    }
}
